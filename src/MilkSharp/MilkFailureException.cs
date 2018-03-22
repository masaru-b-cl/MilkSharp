using System;

namespace MilkSharp
{
    public class MilkFailureException : Exception
    {
        public string Code { get; private set; }
        public string Msg { get; private set; }

        public MilkFailureException(string code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        public override string Message => $"API call is failed | code: {Code}, msg: {Msg}";
    }
}