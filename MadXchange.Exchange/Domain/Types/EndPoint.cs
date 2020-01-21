using RestSharp;
using System;

namespace MadXchange.Exchange.Types
{
    public interface IEndPoint : IComparable
    {
        
    }
    public class EndPoint<T> : IEndPoint
    {
        public string Name { get; set; }
        public string Url { get; set; }        
        public Parameter[] Parameter { get; set; } 
        public Parameter[] Result { get; set; }

        public int CompareTo(object obj)
        {
            return Name.CompareTo(obj);
        }
    }
    public class Parameter 
    {
        
        public NameValuePair Param { get; set; }
        public Type Type { get; set; }
        public bool IsRequired { get; set; }
    }
}