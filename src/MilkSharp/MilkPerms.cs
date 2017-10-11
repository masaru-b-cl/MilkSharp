namespace MilkSharp
{
    public class MilkPerms
    {
        public static readonly MilkPerms Read = new MilkPerms("read");
        public static readonly MilkPerms Write = new MilkPerms("write");
        public static readonly MilkPerms Delete = new MilkPerms("delete");

        private readonly string value;

        private MilkPerms(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

    }
}