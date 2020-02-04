using System;

namespace MadXchange.Exchange.Domain.Types
{
    public interface IEndPoint : IComparable
    {
        
    }
    public sealed class EndPoint: IEndPoint
    {
        public string Name { get; set; }
        public string Url { get; set; }        
        public Parameter[] Parameter { get; set; } 
        public string Result { get; set; }

        public int CompareTo(object obj)
        {
            return Name.CompareTo(obj);
        }
        
    }
    public sealed class Parameter 
    {
        
        public ValueTuple<string, string> Param { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
   
}