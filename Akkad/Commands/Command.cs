using System;
using Akkad.Resources;

namespace Akkad.Commands
{
    public class Command : ICommand
    {
        public Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; private set; }
    }
}