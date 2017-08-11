
namespace SQLBuilder
{
    [DBAttributes(Table = "Child")]
    class C
    {
        public string Sex { get; set; }

        public int Age { get; set; } 

        [DBAttributes(JoinMethod = JoinType.LeftJoin, JoinClass = "Gc", Comparison = Operator.EQ)]
        public string ID { get; set; }

        [DBAttributes(Showable = true)]
        public string Title { get; set; } 

    }
}
