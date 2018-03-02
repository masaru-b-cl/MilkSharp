using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MilkSharp
{
    internal interface IMilkSignatureGenerator
    {
        string Generate(IDictionary<string, string> postParameters);
    }

    public class MilkSignatureGenerator : IMilkSignatureGenerator
    {
        private MilkContext context;

        public MilkSignatureGenerator(MilkContext context)
        {
            this.context = context;
        }

        public string Generate(IDictionary<string, string> postParameters)
        {
            var orderedFlattenedParameters = string.Concat(
                new SortedDictionary<string, string>(postParameters)
                .Select(pair => $"{pair.Key}{pair.Value}")
                .ToArray());

            var signatureSource = context.SharedSecret + orderedFlattenedParameters;

            using (var md5 = MD5.Create())
            {
                byte[] md5HashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureSource));
                return BitConverter.ToString(md5HashedBytes).ToLower().Replace("-", "");
            }
        }
    }
}