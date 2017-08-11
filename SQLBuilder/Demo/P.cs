
namespace SQLBuilder
{
    [DBAttributes(Table="Parent")]
    class P
    {
        [DBAttributes(Type = "PK")]
        public string Cd { get; set; }

        [DBAttributes(Type = "MAX")]
        public string Eda { get; set; }
  
        [DBAttributes(Showable = true)]
        public string Name { get; set; }

        [DBAttributes(JoinMethod = JoinType.LeftJoin, JoinClass = "C", Comparison = Operator.EQ)]
        public string Sex { get; set; }

        [DBAttributes(JoinMethod = JoinType.LeftJoin, JoinClass = "C", Comparison = Operator.EQ)]
        public int Age { get; set; } 
    }
}
