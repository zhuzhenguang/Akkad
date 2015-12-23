using System;
using CommonDomain.Messaging;

namespace Akkad.Events
{
    public class UserCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}