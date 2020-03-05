using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MadXchange.Exchange.Types
{
    public class XchangeSocketDescriptor : IOptions
    {
        public Xchange Xchange { get;  set; }
        public string SocketUrl { get;  set; }
        public string AuthUrl { get; set; }        
        public int KeepAliveInterval { get; set; }
        public Dictionary<string, Type> TypeDescriptors { get; set; }
        public Dictionary<string, string[]> FieldDescriptors { get; set; }
        public Dictionary<string, string[]> CombinedStrings { get; set; }
        //combine multible values to get response status
        public Dictionary<string, Func<string[], bool>> Accessors { get; set; }
        public string ExpiresTime { get; set; }
        public string Signature { get; set; }
        public string ApiKey { get; set; }
        
        public Dictionary<string, SocketSubscriptionArgs> SubscriptionArgs { get;  } = new Dictionary<string, SocketSubscriptionArgs>();

        public IEnumerable<SocketSubscription> CreatePrivateSubscriptions()
        {
            var privateSubscriptions = SubscriptionArgs.Where(s => !s.Value.IsPublic).ToArray();
            var result = new SocketSubscription[privateSubscriptions.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var subscriptionArg = SubscriptionArgs.GetValueOrDefault(privateSubscriptions[i].Key);
                result[i] = new SocketSubscription(id: Guid.NewGuid(), 
                                              channel: privateSubscriptions[i].Key, 
                                                topic: subscriptionArg.Topic, 
                                                 args: subscriptionArg.Args, 
                                           returnType: subscriptionArg.ReturnType, 
                                            isPrivate: true);
            }

            return result;
        }

       

        public IEnumerable<SocketSubscription> CreatePublicSubscriptions((string, string)[] subscriptionTags)
        {
            var result = new SocketSubscription[subscriptionTags.Count()];
            for (int i = 0; i < result.Length; i++)
            {
                var subscriptionArg = SubscriptionArgs[subscriptionTags[i].Item1];
                result[i] = new SocketSubscription(id: Guid.NewGuid(), 
                                              channel: subscriptionTags[i].Item1,
                                                topic: subscriptionArg.Topic,
                                                 args: subscriptionArg.Args.ToList().Append(subscriptionTags[i].Item2),
                                           returnType: subscriptionArg.ReturnType);
            }

            return result;          
        }
    }

    public class SocketSubscriptionArgs
    {
        public bool IsPublic { get; set; }
        public string Topic { get; set; }
        public IEnumerable<string> Args { get; set; }
        public Type ReturnType { get; set; }
    }
}