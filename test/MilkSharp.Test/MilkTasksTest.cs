using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class MilkTasksTest
    {
        [Fact]
        public void GetListTestForAll()
        {
            var rawClientMock = new Mock<IMilkRawClient>();

            rawClientMock
                .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((method, parameters) =>
                {
                    Assert.Equal("rtm.tasks.getList", method);
                })
                .Returns(() => Task.FromResult(@"
                    <rsp stat=""ok"">
                        <tasks>
                            <list id=""1"">
                                <taskseries id=""2"" created=""2015-05-07T10:19:54Z"" modified=""2015-05-07T10:19:54Z""
                                    name=""Get Bananas"" source=""api"" url=""http://example.com"" location_id="""">
                                    <tags/>
                                    <participants/>
                                    <notes/>
                                    <task id=""3"" due="""" has_due_time=""0"" added=""2015-05-07T10:19:54Z""
                                        completed="""" deleted="""" priority=""N"" postponed=""0"" estimate=""""/>
                                </taskseries>
                            </list>
                        </tasks>
                    </rsp>
                    "));
            var rawClient = rawClientMock.Object;

            var milkTasks = new MilkTasks(rawClient);

            IObservable<MilkTask> taskObservable = milkTasks.GetList();
            taskObservable.Subscribe(task =>
            {
                Assert.Equal(1, task.ListId);

                MilkTaskSeries taskSeries = task.TaskSeries;
                Assert.Equal(2, taskSeries.Id);

                Assert.Equal(3, task.Id);
            });
        }
    }
}
