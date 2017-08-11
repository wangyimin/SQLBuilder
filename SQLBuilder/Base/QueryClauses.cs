using System.Collections.Generic;

namespace SQLBuilder
{
    class QueryClauses
    {
        public class QueryClause
        {
            public string _key { get; set; }
            public Operator _operator { get; set; }
            public object _value { get; set; }

            public QueryClause(string _key, Operator _operator, object _value)
            {
                this._key = _key;
                this._operator = _operator;
                this._value = _value;
            }
        }

        private Dictionary<string, QueryClause> el = new Dictionary<string, QueryClause>();

        public void Add(string Q, string Key, Operator Operator, object Value)
        {
            if (!el.ContainsKey(Q))
            {
                QueryClause clause = new QueryClause(Key, Operator, Value);
                el.Add(Q, clause);
            }
        }

        public Dictionary<string, QueryClause> Get()
        {
            return el;
        }

        public override string ToString()
        {
            string r = "";

            foreach (KeyValuePair<string, QueryClause> e in el)
            {
                r = r + e.Value._key + " "
                      + e.Value._operator.Display() + " " + e.Value._value.ToString()
                      + " " + LogicOperator.AND + " ";
            }

            r = r.Substring(0, r.Length - LogicOperator.AND.ToString().Length - 2);

            return r;
        }
    }
}
