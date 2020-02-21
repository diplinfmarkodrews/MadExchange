namespace MadXchange.Exchange.Domain.Types
{
    public enum XchangeHttpOperation
    {
        Unknown = 0,
        GetInstrument = 1,
        GetInstrumentList = 2,
        GetOrderBookL2 = 3,
        GetOrderBook = 4,
        GetMargin = 5,
        GetMarginList = 6,
        GetOrder = 7,
        GetOrderList = 8,
        GetPosition = 9,
        GetPositionList = 10,
        GetLeverage = 11,
        PostLeverage = 12,
        PostPlaceOrder = 13,
        PostUpdateOrder = 14,
        PostCancelAllOrder = 15,
        PostCancelOrder = 16,
        GetWalletFund = 17,
        GetWalletWithdraw = 18,
        
    }

    public class EndPointParameter
    {
    }
}