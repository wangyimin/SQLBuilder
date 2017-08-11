
namespace SQLBuilder
{
    [DBAttributes(Table = "Grandchild")]
    class Gc
    {
        public string ID { get; set; }

        [DBAttributes(Showable = true)]
        public string ZName { get; set; } 

    }
}
