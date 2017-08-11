
namespace SQLBuilder
{
    public enum Operator
    {
        [DBAttributes(Display = "=")]
        EQ,
        [DBAttributes(Display = "<>")]
        NE,
        [DBAttributes(Display = "like")]
        LIKE,
        [DBAttributes(Display = ">")]
        GT,
        [DBAttributes(Display = ">=")]
        GE,
        [DBAttributes(Display = "<")]
        LT,
        [DBAttributes(Display = "<=")]
        LE,
        [DBAttributes(Display = "in")]
        IN
    }
}
