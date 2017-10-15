using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkList
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public static MilkList Parse(XElement list)
        {
            return new MilkList
            {
                Id = int.Parse(list.Attribute("id").Value),
                Name = list.Attribute("name").Value,
            };
        }
    }
}