using System.Collections.Generic;

namespace SQLBuilder
{
    class OrderByClauses
    {
        public class OrderByClause
        {
            public string _alias { get; set; }
            public string _column { get; set; }
            public SortType _sort { get; set; }

            public OrderByClause(string _alias, string _column, SortType _sort)
            {
                this._alias = _alias;
                this._column = _column;
                this._sort = _sort;
            }
        }

        private Dictionary<string, OrderByClause> el = new Dictionary<string, OrderByClause>();

        public void Add(string Alias, string Column, SortType Sort)
        {
            if (!el.ContainsKey(Column))
            {
                OrderByClause clause = new OrderByClause(Alias, Column, Sort);
                el.Add(Column, clause);
            }
        }

        public Dictionary<string, OrderByClause> Get()
        {
            return el;
        }


        public override string ToString()
        {
            string r = "";

            foreach (KeyValuePair<string, OrderByClause> e in el)
            {
                r = r + e.Value._alias + "." + e.Value._column
                    + " " + e.Value._sort.ToString() + ", ";
            }

            if (r.Length != 0)
            {
                r = "ORDER BY " + r.Substring(0, r.Length - 2) + " ";
            }

            return r;
        }
    }
}
