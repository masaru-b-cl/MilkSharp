using System;
using System.Xml.Linq;

using Xunit;

namespace MilkSharp.Test
{
    public class MilkParserTest
    {
        [Fact]
        public void ParseTaskSeriesTest_WhenXmlElementWithFilledAttributes()
        {
            var element = XElement.Parse(@"
                <taskseries id=""1"" created=""2015-05-07T10:19:54Z"" modified=""2015-05-07T11:19:54Z""
                    name=""Get Bananas"" source=""api"" url=""http://example.com"" location_id=""2"">
                    <tags/>
                    <participants/>
                    <notes/>
                    <task id=""3"" due="""" has_due_time=""0"" added=""2015-05-07T10:19:54Z""
                        completed="""" deleted="""" priority=""N"" postponed=""0"" estimate=""""/>
                </taskseries>
                ");

            var taskSeries = MilkParser.ParseTaskSeries(element);

            Assert.Equal(1, taskSeries.Id);
            Assert.Equal("Get Bananas", taskSeries.Name);
            Assert.Equal(new Uri("http://example.com"), taskSeries.Url);
            Assert.Equal(2, taskSeries.LocationId);
            Assert.Equal(new DateTime(2015, 05, 07, 10, 19, 54, DateTimeKind.Utc).ToLocalTime(), taskSeries.Created);
            Assert.Equal(new DateTime(2015, 05, 07, 11, 19, 54, DateTimeKind.Utc).ToLocalTime(), taskSeries.Modified);
        }

    }
}
