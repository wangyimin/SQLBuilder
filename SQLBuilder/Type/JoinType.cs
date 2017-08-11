
namespace SQLBuilder
{
    public enum JoinType
    {
        [DBAttributes(Display="INNER JOIN")]
        InnerJoin,
        [DBAttributes(Display = "OUTER JOIN")]
        OuterJoin,
        [DBAttributes(Display = "LEFT JOIN")]
        LeftJoin,
        [DBAttributes(Display = "RIGHT JOIN")]
        RightJoin,
    }
}
