using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MilkSharp
{
    public class MilkList
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool Deleted { get; set; }
        public bool Locked { get; set; }
        public bool Archived { get; set; }
        public int Position { get; set; }
        public bool Smart { get; set; }
        public string Filter { get; set; }

        public static MilkList Parse(XElement list)
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
    }
}