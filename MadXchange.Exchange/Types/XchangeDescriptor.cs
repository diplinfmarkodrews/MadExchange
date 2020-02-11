using MadXchange.Exchange.Domain.Types;

namespace MadXchange.Exchange.Types
{
    public class XchangeDescriptor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SocketUrl { get; set; }
        public string BaseUrl { get; set; }

        public EndPoint[] EndPoints { get; set; }

        public XchangeDescriptor()
        {
        }
    }
}