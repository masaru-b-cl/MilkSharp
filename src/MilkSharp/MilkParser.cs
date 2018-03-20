using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public static class MilkParser
    {
        public static MilkAuthToken ParseAuthToken(string rawRsp)
        {
            var rspXml = XElement.Parse(rawRsp);
            var auth = rspXml.Element("auth");
            return new MilkAuthToken(
                auth.Element("token").Value,
                MilkPerms.FromValue(auth.Element("perms").Value)
                );
        }

        public static IDictionary<string, string> ParseEchoResponse(string rawRsp)
        {
            var element = XElement.Parse(rawRsp);
            var echoResponse = new Dictionary<string, string>();
            foreach (var child in element.Elements())
            {
                echoResponse[child.Name.LocalName] = child.Value;
            }
            return echoResponse;
        }

        public static MilkList ParseList(XElement list)
        {
            MilkList milkList = new MilkList
            {
                Id = int.Parse(list.Attribute("id").Value),
                Name = list.Attribute("name").Value,
                Deleted = list.Attribute("deleted").Value == "1",
                Locked = list.Attribute("locked").Value == "1",
                Archived = list.Attribute("archived").Value == "1",
                Position = int.Parse(list.Attribute("position").Value),
                Smart = list.Attribute("smart").Value == "1"
            };
            milkList.Filter = milkList.Smart ? list.Element("filter").Value : null;
            return milkList;
        }

        public static MilkTaskSeries ParseTaskSeries(XElement taskSeriesElement)
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
