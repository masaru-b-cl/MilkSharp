using System.Collections.Generic;

namespace MilkSharp
{
    public class MilkFailureResponse
    {
        public MilkFailureResponse(MilkHttpResponseMessage responseMessage)
        {
            this.ResponseMessage = responseMessage;
        }

        public MilkFailureResponse(string code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
        }

        public string Code { get; private set; }
        public string Msg { get; private set; }
        public MilkHttpResponseMessage ResponseMessage { get; private set; }
    }
}