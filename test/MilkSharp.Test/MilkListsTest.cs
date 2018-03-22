using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Reactive.Linq;

using Xunit;
using Moq;

namespace MilkSharp.Test
{
    public class MilkListsTest
    {
        [Fact]
        public void GetListTestForAllList()
        {
            var rawClientMock = new Mock<IMilkRawClient>();

            rawClientMock
                .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((method, parameters) =>
                {
                    Assert.Equal("rtm.lists.getList", method);
                })
                .Returns(() => Task.FromResult(@"
                    <rsp stat=""ok"">
                        <lists>
                            <list id=""100653"" name=""Inbox""
                                deleted=""1"" locked=""1"" archived=""1"" position=""-1"" smart=""1"">
                                <filter>(priority:1)</filter>
                            </list>
                        </lists>
                    </rsp>
                    "));
            var rawClient = rawClientMock.Object;

            var lists = new MilkClient(rawClient).Lists;

            IObservable<MilkList> listObservable = lists.GetList();
            listObservable.Subscribe(
                list =>
                {
                    Assert.Equal(100653, list.Id);
                    Assert.Equal("Inbox", list.Name);
                    Assert.True(list.Deleted);
                    Assert.True(list.Locked);
                    Assert.True(list.Archived);
                    Assert.Equal(-1, list.Position);
                    Assert.True(list.Smart);
                    Assert.Equal("(priority:1)", list.Filter);
                });
        }
    }
}
