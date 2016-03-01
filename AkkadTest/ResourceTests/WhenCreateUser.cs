using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akkad.Commands;
using Akkad.Domain;
using Akkad.Resources;
using Autofac;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using Xunit;

namespace AkkadTest.ResourceTests
{
    public class WhenCreateUser : TestBase
    {
        [Fact]
        public void should_raise_user_created_event()
        {
            var commandBus = Scope.Resolve<ICommandBus>();
            var userResource = new UserResource(commandBus);

            var userId = userResource.Create("Zhu").Result;

            User userFromEventStore = null;
            WaitHelper.WaitUntil(() =>
            {
                userFromEventStore = Scope.Resolve<IRepository>().GetById<User>(userId);
                return userFromEventStore != null;
            });
            Assert.Equal(userId, userFromEventStore.Id);
            Assert.Equal("Zhu", userFromEventStore.Name);
        }

        [Fact]
        public void should_run_with_multi_threads()
        {
            var commandBus = Scope.Resolve<ICommandBus>();
            var userResource = new UserResource(commandBus);

            var userId = userResource.Create("Zhu").Result;

            var prepareList = PrepareList();
            Assert.Equal(100000, prepareList.Count);
            prepareList.AsParallel().ForAll(el =>
            {
                var task = ReadEventAsync(userId);
                Console.WriteLine(el);
            });
        }

        private IList<int> PrepareList()
        {
            var i = 0;
            var list = new List<int>();
            while (i < 100000)
            {
                list.Add(i);
                i++;
            }
            return list;
        }

        private async Task<StreamEventsSlice> ReadEventAsync(UserId userId)
        {
            var eventStoreConnection = Scope.Resolve<IEventStoreConnection>();
            var streamSlice =
                await
                    eventStoreConnection.ReadStreamEventsForwardAsync(string.Format("User-{0}", userId), 0, 100, false);
            return streamSlice;
        }
    }
}