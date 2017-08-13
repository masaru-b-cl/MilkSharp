using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkTestEchoResponse : Dictionary<string, string>
    {
        public static MilkTestEchoResponse Parse(string rawRsp)
        {
            var element = XElement.Parse(rawRsp);
            MilkTestEchoResponse rsp= new MilkTestEchoResponse();
            foreach (var child in element.Elements())
            {
                rsp[child.Name.LocalName] = child.Value;
            }
            return rsp;
        }
    }
}