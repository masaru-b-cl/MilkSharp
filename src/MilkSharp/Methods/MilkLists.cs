using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace MilkSharp
{
    public class MilkLists
    {
        private MilkContext context;
        private IMilkRawClient rawClient;

        internal MilkLists(IMilkRawClient rawClient)
        {
            this.rawClient = rawClient;
        }

        public MilkLists(MilkContext context) : this(new MilkRawClient(context))
        {
            this.context = context;
        }

        public IObservable<MilkList> GetList()
        {
            const string method = "rtm.lists.getList";

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            var task = rawClient.Invoke(method, parameters);

            return task.ToObservable()
                .Select(rawRsp => XElement.Parse(rawRsp))
                .Select(xmlRsp => xmlRsp.Descendants("list"))
                .SelectMany(_ => _)
                .Select(e => MilkParser.ParseList(e));
        }
    }
}