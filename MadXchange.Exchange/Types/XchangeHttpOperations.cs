namespace MadXchange.Exchange.Domain.Types
{
    public enum XchangeHttpOperation
    {
        Unknown = 0,
        GetInstrument = 1,
        GetMargin = 2,
        GetOrder = 3,
        GetPosition = 4,
        GetPositionList = 5,
        GetLeverage = 6,
        PostLeverage = 7,
        PostPlaceOrder = 8,
        PostUpdateOrder = 9,
        CancelAllOrders = 10,
        CancelOrder = 11,
    }

    public class EndPointParameter
    {
    }
}