using System;
using Akkad.Events;
using CommonDomain.Aggregates;

namespace Akkad.Domain
{
    public class User : AggregateBase
    {
        public User()
        {
        }

        public User(Guid id, string name) : this()
        {
            RaiseEvent(new UserCreatedEvent
            {
                Id = id,
                Name = name
            });
        }

        public string Name { get; set; }
        public UserId Id { get; set; }

        public void Apply(UserCreatedEvent @event)
        {
            Id = new UserId(@event.Id);
            Name = @event.Name;
        }

        public override IIdentity GetId()
        {
            return Id;
        }
    }
}