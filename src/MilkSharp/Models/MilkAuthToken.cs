namespace MilkSharp
{
    public class MilkAuthToken
    {
        public string Token { get; }
        public MilkPerms Perms { get; }

        public MilkAuthToken(string token, MilkPerms perms)
        {
            Token = token;
            Perms = perms;
        }
    }
}