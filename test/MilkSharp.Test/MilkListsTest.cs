using System;
using System.Collections.Generic;
using System.Text;

using System.Reactive.Linq;
using Xunit;
using Moq;
using System.Threading.Tasks;

namespace MilkSharp.Test
{
    public class MilkListsTest
    {
        [Fact]
        public void GetListTestForAllList()
        {
            var milkCoreClientMock = new Mock<IMilkCoreClient>();

            milkCoreClientMock
                .Setup(client => client.Invoke(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()))
                .Callback<string, IDictionary<string, string>>((method, parameters) =>
                {
                    Assert.Equal("rtm.lists.getList", method);
                })
                .Returns(() => Task.FromResult<(string, MilkFailureResponse)>((
                    @"
                        <rsp stat=""ok"">
                            <lists>
                              <list id=""100653"" name=""Inbox""
                                   deleted=""0"" locked=""1"" archived=""0"" position=""-1"" smart=""0"" />
                            </lists>
                        </rsp>
                        ",
                    null)));
            var milkCoreClient = milkCoreClientMock.Object;

            var lists = new MilkLists(milkCoreClient);

            IObservable<MilkList> listObservable = lists.GetList();
            listObservable.Subscribe(
                list =>
                {
                    Assert.Equal(100653, list.Id);
                    Assert.Equal("Inbox", list.Name);
                });
        }
    }
}
