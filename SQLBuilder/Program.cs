using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SQLBuilder
{
    //demo
    class Program
    {
        //demo of sqlbuilder
        static void Main(string[] args)
        {
            P p = new P();

            DBBase db = new DBBase();

            QueryClauses q = new QueryClauses();
            OrderByClauses o = new OrderByClauses();
            DBBase.GetQueryClauses("q0=age,eq,&q1=sex,eq,1&q2=age,lt,30&q3=sex,in,1,2,3,4&q4=id,eq,a001&orderby=sex,asc,age,desc", q, o);

            SelectClauses s = new SelectClauses();
            db.GetSelectClauses(p, s);

            JoinClauses j = new JoinClauses();
            db.GetJoinClauses(p, j);

            WhereClauses w = new WhereClauses();
            db.GetWhereClauses(p, q, w);

            db.GetOrderByClauses(p, o);

            SQLClause sql = new SQLClause(s, 
                db.GetFromClause(p),
                j, w, o);

            Trace.WriteLine(sql.ToString());
        }
    }
}
