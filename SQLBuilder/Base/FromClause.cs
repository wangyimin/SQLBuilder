using System;
using System.Collections.Generic;

namespace SQLBuilder
{
    class FromClause 
    {
        public string _alias { get; set; }
        public object _table { get; set; }

        public FromClause() { }

        public FromClause(string _alias, object _table)
        {
            this._alias = _alias;
            this._table = _table;
        }

        public override string ToString()
        {
            return "FROM " + 
                (_table is string ? _table.ToString() : "(" + _table.ToString() + ")") +
                " " + _alias + " ";
        }
    }
}
