namespace MilkSharp.Core
{
    public class MilkContext
    {
        internal string ApiKey { get; private set; }
        internal string SharedSecret { get; private set; }
        public MilkAuthToken AuthToken { get; set; }

        public MilkContext(string apiKey, string sharedSecret)
        {
            this.ApiKey = apiKey;
            this.SharedSecret = sharedSecret;
        }
    }
}