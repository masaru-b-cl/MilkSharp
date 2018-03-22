using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace MilkSharp
{
    public class MilkTasks
    {
        private MilkContext context;
        private IMilkRawClient rawClient;

        internal MilkTasks(IMilkRawClient rawClient)
        {
            this.rawClient = rawClient;
        }

        public MilkTasks(MilkContext context) : this(new MilkRawClient(context))
        {
            this.context = context;
        }

        public IObservable<MilkTask> GetList()
        {
            const string method = "rtm.tasks.getList";

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            return rawClient.Invoke(method, parameters)
                .ToObservable()
                .Select(rawXml => XDocument.Parse(rawXml))
                .SelectMany(xml => xml.Descendants("list"))
                .SelectMany(listElement => listElement.Elements("taskseries")
                    .Select(taskSeriesElement =>
                        (
                            listId: int.Parse(listElement.Attribute("id").Value),
                            taskSeries: MilkParser.ParseTaskSeries(taskSeriesElement),
                            taskElements: taskSeriesElement.Elements("task")
                        )
                    )
                )
                .SelectMany(tuple => tuple.taskElements
                    .Select(taskElement =>
                        new MilkTask
                        {
                            ListId = tuple.listId,
                            TaskSeries = tuple.taskSeries,
                            Id = int.Parse(taskElement.Attribute("id").Value),
                        }
                    )
                )
                ;
        }
    }
}