using Akkad.Commands;
using Akkad.Domain;
using Akkad.Resources;
using Autofac;
using CommonDomain.Persistence;
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

            var userId = userResource.Create("Zhu");

            var userFromEventStore = Scope.Resolve<IRepository>().GetById<User>(userId);
            Assert.Equal(userId, userFromEventStore.Id);
            Assert.Equal("Zhu", userFromEventStore.Name);
        }
    }
}