using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Models
{
    public interface ISocketSubscription  
    {
        Guid Id { get; }
    }

    public sealed class SocketSubscription : ISocketSubscription
    {
        //given Id to directly address subscription, or random
        public Guid Id { get; } 
        //channel is my definition
        public string Channel { get; }
        //topic is exchange definition of
        public string Topic { get; set; }
        public IEnumerable<string> Args { get; set; }
        public bool IsSubscribed { get; set; }
        public Type ReturnType { get; internal set; }

        public SocketSubscription(Guid id, string channel, string topic, List<string> args)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Channel = channel;
            var topicBuilder = new StringBuilder().Append(topic).Append('.').AppendJoin('.', args);
            Topic = topicBuilder.ToString();
            Args = args;

                           
        }
        public void SetupReturnType(Type type)
            => ReturnType = type;

      
    }
}