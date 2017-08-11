using System;
using System.Collections.Generic;

namespace SQLBuilder
{
    class WhereClauses 
    {
        private class WhereClause
        {
            public string _alias {get; set;}
            public string _key {get; set;}
            public Operator _operator { get; set; }
            public Type _type { get; set; }
            public object _value { get; set; }

            public WhereClause(string _alias, string _key, Operator _operator, Type _type, object _value)
            {
                this._alias = _alias;
                this._key = _key;
                this._operator = _operator;
                this._type = _type;
                this._value = _value;
            }
        }

        private List<WhereClause> lst = new List<WhereClause>();

        public void Add(string Alias, string Key, Operator Operator, Type Type, object Value)
        {
            lst.Add(new WhereClause(Alias, Key, Operator, Type, Value));
        }

        public override string ToString()
        {
            string r = "";

            foreach (WhereClause el in lst)
            {
                r = r + el._alias + "." + el._key + " "
                    + el._operator.Display(el._type, el._value.ToString())
                    + " " + LogicOperator.AND + " ";
       
            }
            
            if (r.Length != 0)
            {
                r = "WHERE " + r.Substring(0, r.Length - LogicOperator.AND.ToString().Length - 2) + " ";
            }

            return r;
        }
    }
}
