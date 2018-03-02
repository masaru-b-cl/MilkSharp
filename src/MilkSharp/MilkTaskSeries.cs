using System;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkTaskSeries
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public Uri Url { get; set; }
        public int? LocationId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public static MilkTaskSeries Parse(XElement taskSeriesElement)
        {
            var rawUrl = taskSeriesElement.Attribute("url")?.Value;
            var locationIdValue = taskSeriesElement.Attribute("location_id").Value;
            var taskSeries = new MilkTaskSeries
            {
                Id = int.Parse(taskSeriesElement.Attribute("id").Value),
                Name = taskSeriesElement.Attribute("name").Value,
                Url = string.IsNullOrEmpty(rawUrl) ? null : new Uri(rawUrl),
                LocationId = string.IsNullOrEmpty(locationIdValue) ? (int?)null : int.Parse(locationIdValue),
                Created = DateTime.Parse(taskSeriesElement.Attribute("created").Value),
                Modified = DateTime.Parse(taskSeriesElement.Attribute("modified").Value),
            };

            return taskSeries;
        }
    }
}