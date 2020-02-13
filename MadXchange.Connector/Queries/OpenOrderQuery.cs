using Convey.CQRS.Queries;

namespace MadXchange.Connector.Queries
{
    public class OpenOrderQuery<IOrder> : IQuery<IOrder>
    {
        public IOrder Data;
    }
}