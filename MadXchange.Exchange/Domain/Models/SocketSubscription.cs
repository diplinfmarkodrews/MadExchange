using ServiceStack;
using System;

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
        public bool IsSubscribed { get; set; }
        
        public ObjectDictionary Request { get; internal set; }
        public Type ReturnType { get; internal set; }
        

        public SocketSubscription(Guid id, string channel, string topic, ObjectDictionary requestDict, Type returnType)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Channel = channel;
            Request = requestDict;
            ReturnType = returnType;
            Topic = topic;
        }
        
     

        
    }
}