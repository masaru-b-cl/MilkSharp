namespace MilkSharp
{
    public class MilkContext
    {
        public string ApiKey { get; private set; }
        public string SharedSecret { get; private set; }
        public MilkAuthToken AuthToken { get; set; }

        public MilkContext(string apiKey, string sharedSecret)
        {
            this.ApiKey = apiKey;
            this.SharedSecret = sharedSecret;
        }
    }
}