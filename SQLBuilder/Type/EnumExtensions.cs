using System;
using System.Linq;
using System.Reflection;

namespace SQLBuilder
{
    public static class EnumExtensions
    {
        public static string Display(this Enum e)
        {
            return e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DBAttributes>()
                            .Display;
        }

        public static T Convert<T>(string v)
        {
            if(String.IsNullOrEmpty(v))
            {
                throw new ArgumentNullException("Null parameter.");
            }

            foreach (T el in Enum.GetValues(typeof(T)))
            {
                if (v.Equals(el.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return el;
                }
            }

            throw new ArgumentOutOfRangeException("Wrong value.");
        }

        public static string Display(this Operator e, Type type, string val)
        {
            if (type == null || val == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            string r = val;

            if ("NULL".Equals(val, StringComparison.OrdinalIgnoreCase))
            {
                if (e == Operator.EQ)
                {
                    return "IS NULL";
                }
                else if (e == Operator.NE) 
                {
                    return "NOT IS NULL";
                }
                else
                {
                    throw new InvalidOperationException("Wrong operator for NULL value.");
                }
            }

            if (e == Operator.IN)
            {
                r = "";
                foreach (string s in val.Substring(1, val.Length - 2).Split(','))
                {
                    r = r + s + "',";
                }
                if (r.Length == 0)
                {
                    throw new ArgumentOutOfRangeException("Bad parameter for operator[IN].");
                }

                r = "('" + r.Substring(0, r.Length - 1) + ")";
            }

            return Display(e) + " " + r;
        }
    }
}
