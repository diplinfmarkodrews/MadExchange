using System;

namespace MadXchange.Common.Messages
{
    public interface IMessage
    {
        public Guid Id { get; }
    }
}