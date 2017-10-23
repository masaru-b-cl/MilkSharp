using System;
using System.Collections.Generic;

namespace MilkSharp
{
    public class MilkTask
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string Name
        {
            get => TaskSeries?.Name;
            set
            {
                if (TaskSeries != null)
                {
                    TaskSeries.Name = value;
                }
            }
        }
        public MilkTaskSeries TaskSeries { get; set; }
        public Uri Url
        {
            get => TaskSeries?.Url;
            set
            {
                if (TaskSeries != null)
                {
                    TaskSeries.Url = value;
                }
            }
        }

    }
}