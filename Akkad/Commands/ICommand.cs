using System;

namespace Akkad.Commands
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}