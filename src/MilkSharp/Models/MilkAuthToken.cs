using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkAuthToken
    {
        public MilkAuthToken()
        {
        }

        public MilkAuthToken(string token, MilkPerms perms)
        {
            Token = token;
            Perms = perms;
        }

        public static MilkAuthToken Parse(string rawRsp)
        {
            var rspXml = XElement.Parse(rawRsp);
            var auth = rspXml.Element("auth");
            return new MilkAuthToken(
                auth.Element("token").Value,
                MilkPerms.FromValue(auth.Element("perms").Value)
                );

        }

        public string Token { get; }
        public MilkPerms Perms { get; }
    }
}