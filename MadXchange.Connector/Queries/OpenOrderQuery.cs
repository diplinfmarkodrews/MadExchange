using Convey.CQRS.Queries;

namespace MadXchange.Exchange.Queries
{
    public class OpenOrderQuery<IOrder> : IQuery<IOrder>
    {

        public IOrder Data;
    }
}
