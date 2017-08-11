
namespace SQLBuilder
{
    class SQLClause
    {
        public SelectClauses _select { get; set; }
        public object _from { get; set; }
        public JoinClauses _join { get; set; }
        public WhereClauses _where { get; set; }
        public OrderByClauses _orderby { get; set; }

        public SQLClause(SelectClauses _select,
            object _from,
            JoinClauses _join,
            WhereClauses _where,
            OrderByClauses _orderby)
        {
            this._select = _select;
            this._from = _from;
            this._join = _join;
            this._where = _where;
            this._orderby = _orderby;
        }

        public override string ToString()
        {
            return
                (_select == null ? "" : _select.ToString()) +
                (_from == null ? "" : _from.ToString()) +
                (_join == null ? "" : _join.ToString()) +
                (_where == null ? "" : _where.ToString()) +
                (_orderby == null ? "" : _orderby.ToString());
        }
    }
}
