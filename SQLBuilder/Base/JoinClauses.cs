using System;
using System.Collections.Generic;

namespace SQLBuilder
{
    class JoinClauses 
    {
        private class Comparison
        {
            public string _fromAlias { get; set; }
            public string _from {get; set;}
            public string _toAlias { get; set; }
            public string _to {get; set;}
            public Operator _operator { get; set; }

            public Comparison(string _fromAlias, string _from, string _toAlias, string _to, Operator _operator)
            {
                this._fromAlias = _fromAlias;
                this._from = _from;
                this._toAlias = _toAlias;
                this._to = _to;
                this._operator = _operator;
            }
        }

        private class JoinClause
        {
            public string _table {get; set;}
            public string _alias { get; set; }
            public JoinType _joinType { get; set; }

            public List<Comparison> _comparisons = new List<Comparison>();

            public JoinClause(string _table, string _alias, JoinType _joinType)
            {
                this._table = _table;
                this._alias = _alias;
                this._joinType = _joinType;
            }
        }

        private Dictionary<string, JoinClause> el = new Dictionary<string, JoinClause>();

        public void AddJoin(string Table, String Alias, JoinType JoinType)
        {
            if (!el.ContainsKey(Table))
            {
                JoinClause clause = new JoinClause(Table, Alias, JoinType);
                el.Add(Table, clause);
            }
        }

        public void AddComparison(string Table, string FromAlias, string From, string ToAlias, string To, Operator Comparison)
        {
            if (!el.ContainsKey(Table))
            {
                throw new ArgumentException("No table exists.");
            }

            el[Table]._comparisons.Add(new Comparison(FromAlias, From, ToAlias, To, Comparison));
        }

        public override string ToString()
        {
            string r = "";

            foreach (KeyValuePair<string, JoinClause> e in el)
            {
                r = r + e.Value._joinType.Display() + " " + e.Key + " " + e.Value._alias + " ON ";
                
                foreach (Comparison c in e.Value._comparisons)
                {
                    r = r + c._fromAlias + "." + c._from 
                        + c._operator.Display() 
                        + c._toAlias + "." + c._to 
                        + " " + LogicOperator.AND + " ";
                }

                r = r.Substring(0, r.Length - LogicOperator.AND.ToString().Length - 1);
            }
            
            return r;
        }
    }
}
