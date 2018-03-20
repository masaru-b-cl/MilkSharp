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
    }
}