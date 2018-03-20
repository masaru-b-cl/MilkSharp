namespace MilkSharp
{
    public class MilkList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public bool Locked { get; set; }
        public bool Archived { get; set; }
        public int Position { get; set; }
        public bool Smart { get; set; }
        public string Filter { get; set; }
    }
}