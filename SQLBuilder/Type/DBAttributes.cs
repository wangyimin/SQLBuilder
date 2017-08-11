using System;

namespace SQLBuilder
{
    class DBAttributes : Attribute 
    {
        // physical table name
        public string Table { get; set; }
        // show in select clause or not
        public bool Showable { get; set; }

        public string Type { get; set; }
        
        public JoinType JoinMethod { get; set; }
        public string JoinClass { get; set; }
        public Operator Comparison { get; set; }

        public string Display { get; set; }
    }
}
