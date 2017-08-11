using System.Collections.Generic;

namespace SQLBuilder
{
    class SelectClauses 
    {
        private class SelectClause
        {
            public string _alias {get; set;}
            public string _column {get; set;}

            public SelectClause(string _alias, string _column)
            {
                this._alias = _alias;
                this._column = _column;
            }
        }

        private List<SelectClause> lst = new List<SelectClause>();

        public void Add(string Alias, string Column)
        {
            lst.Add(new SelectClause(Alias, Column));
        }

        public override string ToString()
        {
            string r = "";

            foreach (SelectClause el in lst)
            {
                r = r + el._alias + "." + el._column + ", ";
            }
            
            if (r.Length != 0)
            {
                r = "SELECT " + r.Substring(0, r.Length - 2) + " ";
            }

            return r;
        }
    }
}
