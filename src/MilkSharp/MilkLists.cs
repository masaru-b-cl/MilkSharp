using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkLists
    {
        private MilkContext context;
        private IMilkCoreClient milkCoreClient;

        public MilkLists(MilkContext context) : this(new MilkCoreClient(context))
        {
            this.context = context;
        }

        public MilkLists(IMilkCoreClient milkCoreClient)
        {
            this.milkCoreClient = milkCoreClient;
        }

        public IObservable<MilkList> GetList()
        {
            const string method = "rtm.lists.getList";

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            var task = milkCoreClient.InvokeNew(method, parameters);

            return task.ToObservable()
                .Select(rawRsp => XElement.Parse(rawRsp))
                .Select(xmlRsp => xmlRsp.Descendants("list"))
                .SelectMany(_ => _)
                .Select(e => MilkList.Parse(e));
        }
    }
}