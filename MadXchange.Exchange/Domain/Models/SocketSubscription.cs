using System;
using System.Collections.Generic;
using System.Linq;
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
        public Guid Id { get; set; } 
        //channel is my definition
        public string Channel { get; }
        //topic is exchange definition of
        public string Topic { get; }
        public IEnumerable<string> Args { get; } 
        public bool IsSubscribed { get; set; }
        public Type ReturnType { get; }
        public Type ReturnArrayType { get; }
     

        public SocketSubscription(Guid id, string channel, string topic, IEnumerable<string> args, Type returnType, bool isPrivate = false)
        {

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Channel = channel;
            var topicBuilder = new StringBuilder().Append(topic);
                                                  
            if (args != null && args.Count() > 0)
                topicBuilder.Append('.').AppendJoin('.', args);
                                      
            Topic = topicBuilder.ToString();
            ReturnType = returnType;
            ReturnArrayType = returnType.MakeArrayType();
            Args = args;    
            
        } 
        
      
    }
}