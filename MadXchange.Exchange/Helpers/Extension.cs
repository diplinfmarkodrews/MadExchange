using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;

namespace MadXchange.Exchange.Helpers
{
    public static class Extension
    {
        public static int Count(this XchangeDescriptor[] descriptors) => descriptors.Length - 1;

        public static int CountEndpoints(this XchangeDescriptor descriptor) => descriptor.EndPoints.Length - 1;

        public static EndPoint GetEndPoint(this EndPoint[] endPoints, XchangeOperation operation) => endPoints[(int)operation];
    }
}