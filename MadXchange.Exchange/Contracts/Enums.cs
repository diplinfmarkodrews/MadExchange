using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{
    [DataContract]
    public enum OrderStatus
    {
        UNKNOWN = 0,

        [EnumMember]
        Canceled = 1,

        [EnumMember]
        Expired = 2,

        [EnumMember]
        Filled = 3,

        [EnumMember]
        New = 4,

        [EnumMember]
        PartiallyCanceled = 5,

        [EnumMember]
        PartiallyFilled = 6,

        [EnumMember]
        PendingCancel = 7,

        [EnumMember]
        PendingNew = 8,

        [EnumMember]
        PendingReplace = 9,

        [EnumMember]
        Rejected = 10,

        [EnumMember]
        Replaced = 11,

        [EnumMember]
        Stopped = 12
    }

    [DataContract]
    public enum OrderSide
    {
        Unknown = 0,

        [EnumMember]
        Long = 1,

        [EnumMember]
        Short = 2
    }

    [DataContract]
    public enum OrderType
    {
        Unknown = 0,

        [EnumMember]
        Limit = 1, //these 2 needed for binance only

        [EnumMember]
        Market = 2, //

        [EnumMember]
        Stop = 3,

        [EnumMember]
        StopLimit = 4,

        [EnumMember]
        MarketIfTouched = 5,

        [EnumMember]
        LimitIfTouched = 6,

        [EnumMember]
        MarketWithLeftOverAsLimit = 7,

        [EnumMember]
        Pegged = 8
    }

    [DataContract]
    public enum TimeInForce
    {
        Unknown = 0,

        [EnumMember]
        GoodTillCancel = 1, //0 is standard

        [EnumMember]
        ImmediateOrCancel = 2,

        [EnumMember]
        FillOrKill = 3,

        [EnumMember]
        PostOnly = 4
    }
    [DataContract]
    public enum SocketMethod
    {
        UnSpecified = 0,        
        [DataMember]
        Ping = 1,
        [DataMember]
        Auth = 2,
        [DataMember]
        Subscribe = 3,
        [DataMember]
        Unsubscribe = 4

    }

    public enum ExecInst
    {
        Unknown = 0,
        ParticipateDoNotInitiate = 1,
        AllOrNone = 2,
        MarkPrice = 3,
        IndexPrice = 3,
        LastPrice = 5,
        Close = 6,
        ReduceOnly = 7,
        Fixed = 8,
    }
}