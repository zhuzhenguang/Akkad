using System;
using CommonDomain.Aggregates;

namespace Akkad.Domain
{
    public class UserId : IIdentity
    {
        public UserId(Guid value)
        {
            Value = value.ToString();
        }

        public string Value { get; private set; }

        protected bool Equals(UserId other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserId) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}