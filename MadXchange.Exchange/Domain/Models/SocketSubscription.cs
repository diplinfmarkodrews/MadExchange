using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Domain.Models
{
    public interface ISocketSubscription
    {
        Guid Id { get; }
    }

    public class SocketSubscription : ISocketSubscription
    {
        public Guid Id { get; } 
        public string Topic { get; }
        public IEnumerable<string> Args { get; }

        public SocketSubscription(Guid id, string topic, IEnumerable<string> args)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Topic = topic;
            Args = args;
        }
    }
}