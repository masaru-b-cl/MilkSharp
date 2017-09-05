using System.Collections.Generic;

namespace MilkSharp
{
    public class MilkFailureResponse
    {
        public MilkFailureResponse(string code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
        }

        public string Code { get; private set; }
        public string Msg { get; private set; }
    }
}