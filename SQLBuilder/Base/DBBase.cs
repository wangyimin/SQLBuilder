using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SQLBuilder
{
    class DBBase
    {
        /* used for where clause,
         * alias is to use as prefix of column
         * type is to use as value conversion
         */
        private class ColumnType
        {
            public string _alias { get; set; }
            public Type _type { get; set; }

            public ColumnType(string _alias, Type _type)
            {
                this._alias = _alias;
                this._type = _type;
            }
        }

        /* used for get alias for select, join and where clause */
        protected Dictionary<string, string> _alias = new Dictionary<string, string>();

        public string GetAlias<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            string val;
            if (_alias.TryGetValue(obj.GetType().Name, out val))
            {
                return val;
            }

            _alias.Add(obj.GetType().Name, obj.GetType().Name);

            return obj.GetType().Name;
        }

        /* used for where clause
         * convert value from string to correct type
         */
        public static string Convert(Type type, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return "NULL";
            }

            if ("String".Equals(type.Name))
            {
                return "'" + obj.ToString() + "'";
            }
            else if ("Int32".Equals(type.Name))
            {
                return Int32.Parse(obj.ToString()).ToString();
            }
            else if ("DateTime".Equals(type.Name))
            {
                return "TO_DATE('" + obj.ToString() + "', 'YYYY/MM/DD')";
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unsupport type.");
            }
        }
        
        /* used to analyze query string with below format
         * ex: q1=sex,eq,1&q2=age,lt,30&q3=sex,in,1,2,3,4&orderby=sex,asc,age,desc"
         * q1: a query with no duplicated number, uses & to join multiple query conditions
         * sex: a defined column name exists table
         * eq: comparsion operator such as eq(=), nq(!=), lt(<), le(<=), gt(>), ge(>=), like, in
         * 1: value
         */
        public static void GetQueryClauses(string queries, QueryClauses query, OrderByClauses order)
        {
            if (String.IsNullOrEmpty(queries) || query == null || order == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (string s in queries.Split('&'))
            {
                string[] part = s.Split('=');
                if (part.Length != 2)
                {
                    throw new ArgumentOutOfRangeException("Wrong query string.");
                }

                if (new Regex(@"q\d+", RegexOptions.IgnoreCase).Match(part[0]).Success)
                {
                    string[] o = part[1].Split(new Char[] { ',', ' ' }, 3);
                    if (o.Length != 3)
                    {
                        throw new ArgumentOutOfRangeException("Wrong query string.");
                    }

                    query.Add(part[0], o[0], EnumExtensions.Convert<Operator>(o[1]), o[2]);

                }

                if (new Regex(@"orderby", RegexOptions.IgnoreCase).Match(part[0]).Success)
                {
                    string[] o = part[1].Split(',');
                    if (o.Length % 2 != 0)
                    {
                        throw new ArgumentOutOfRangeException("Wrong orderby string.");
                    }
                    for (int i = 0; i < o.Length; i = i+2)
                    {
                        order.Add(null, o[0+i], EnumExtensions.Convert<SortType>(o[1+i]));
                    }
                }
            }
        }
        
        // Get table name by class attribute
        private string GetTableName<T>(T obj)
        {
            DBAttributes attr = (DBAttributes)obj.GetType()
                    .GetCustomAttributes<DBAttributes>(false)
                    .Where(el => el.Table.Length != 0).FirstOrDefault();

            if (attr == null)
            {
                throw new ArgumentOutOfRangeException("No table attribute exists.");
            }
            return attr.Table;
        }

        /*
         * Select clause
         * Get all property name (as column name) with attribute Showable is true
         */
        public void GetSelectClauses<T>(T obj, SelectClauses select)
        {
            if (obj == null || select == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                foreach (DBAttributes attr in 
                    prop.GetCustomAttributes<DBAttributes>(false).Where(el => el.Showable || el.JoinClass != null))
                {
                    if (attr.Showable)
                    {
                        select.Add(GetAlias(obj), prop.Name);
                    }
                    else if (attr.JoinClass != null)
                    {
                        if (!_alias.ContainsKey(attr.JoinClass))
                        {
                            GetSelectClauses(Activator.CreateInstance(
                                    Type.GetType(
                                        obj.GetType().Namespace + "." + attr.JoinClass)
                                    ), select);
                        }
                    }
                }
            }
        }

        /*
         * From clause
         * Get main table(class) name
        */
        public FromClause GetFromClause<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            return new FromClause(GetAlias<T>(obj), GetTableName<T>(obj));
        }

        /*
         * Join clause
         * Get join clause
        */
        public void GetJoinClauses<T>(T obj, JoinClauses join)
        {
            object cls = null;
            
            /*
            var properties = obj.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(DBAttributes), false));
            */
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                foreach (DBAttributes attr in 
                    prop.GetCustomAttributes<DBAttributes>(false).Where(el => el.JoinClass != null))
                {
                    cls = Activator.CreateInstance(
                                Type.GetType(
                                    obj.GetType().Namespace + "." + attr.JoinClass)
                                );

                    join.AddJoin(GetTableName(cls), GetAlias(cls), attr.JoinMethod);
                    join.AddComparison(GetTableName(cls), GetAlias(obj), prop.Name,
                            GetAlias(cls), prop.Name, attr.Comparison);
                }
            }

            if (cls != null)
            {
                GetJoinClauses(cls, join);
            }

        }

        /*
         * Where clause
         * Get where clause
        */
        public void GetWhereClauses<T>(T obj, QueryClauses query, WhereClauses where)
        {
            if (obj == null || query == null || where == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (KeyValuePair<string, QueryClauses.QueryClause> e in query.Get())
            {
                where.Add(
                    GetColumnTypeByProp(obj, e.Value._key)._alias, 
                    e.Value._key, 
                    e.Value._operator,
                    GetColumnTypeByProp(obj, e.Value._key)._type, 
                    Convert(GetColumnTypeByProp(obj, e.Value._key)._type, e.Value._value));
            }
        }

        /*
         * Orderby clause
         * Get orderby clause
        */
        public void GetOrderByClauses<T>(T obj, OrderByClauses order)
        {
            if (obj == null || order == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (KeyValuePair<string, OrderByClauses.OrderByClause> e in order.Get())
            {
                e.Value._alias = GetColumnTypeByProp(obj, e.Key)._alias;
            }
        }

        private ColumnType GetColumnTypeByProp<T>(T obj, string p)
        {
            object cls = null;

            if (obj == null || string.IsNullOrEmpty(p))
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (p.Equals(prop.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return new ColumnType(GetAlias(obj), prop.PropertyType);
                }
            }

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                foreach (DBAttributes attr in 
                    prop.GetCustomAttributes<DBAttributes>(false).Where(el => el.JoinClass != null))
                {
                    cls = Activator.CreateInstance(
                                    Type.GetType(
                                        obj.GetType().Namespace + "." + attr.JoinClass)
                                    );

                    return GetColumnTypeByProp(cls, p);
                }
            }

            throw new ArgumentOutOfRangeException("No column defined in current class.");
        }

    }
}
