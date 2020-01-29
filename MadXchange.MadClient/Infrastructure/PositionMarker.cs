using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.MadClient.Infrastructure
{
    public interface IPositionMarker 
    {
        IPosition Position { get; set; }
        void Set(IPosition position);
        void ReduceToPosition(decimal posToBe);
        public void SetMax(decimal amount);
    }
    public class PositionMarker : IPositionMarker
    {
        public IPosition Position { get; set; }
        public decimal MaxPosQty { get; private set; }
        private decimal PosToBe;

        public PositionMarker()
        {
            Position = new Position();            
            MaxPosQty = 0.0M;
            PosToBe = -1;
        }
        public void ReduceToPosition(decimal posToBe)
        {
            PosToBe = posToBe;
        }
        public decimal GetPosToBe()
        {
            return PosToBe;
        }

        public void Set(IPosition position)
        {
            if (Math.Abs(position.CurrentQty.Value) > Math.Abs(Position.CurrentQty.Value) || Math.Sign(position.CurrentQty.Value) != Math.Sign(Position.CurrentQty.Value))
            {
                MaxPosQty = position.CurrentQty.Value;
            }
            Position = position;
        }
        public void SetMax(decimal amount)
        {
            MaxPosQty = amount;
        }
    }
}
