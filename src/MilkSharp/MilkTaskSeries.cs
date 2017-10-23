using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkTaskSeries
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public Uri Url { get; set; }
        public static MilkTaskSeries Parse(XElement taskSeriesElement)
        {
            var rawUrl = taskSeriesElement.Attribute("url")?.Value;
            var taskSeries = new MilkTaskSeries
            {
                Id = int.Parse(taskSeriesElement.Attribute("id").Value),
                Name = taskSeriesElement.Attribute("name").Value,
                Url = string.IsNullOrEmpty(rawUrl) ? null : new Uri(rawUrl),
            };

            return taskSeries;
        }
    }
}