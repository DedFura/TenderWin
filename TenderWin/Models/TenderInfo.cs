using System;
using System.Collections.Generic;

namespace TenderWin.Models
{
    public class TenderInfo
    {
        public int Id { get; set; }
        public string TradeName { get; set; }
        public string TradeStateName { get; set; }
        public string CustomerFullName { get; set; }
        public decimal InitialPrice { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime FillingApplicationEndDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<LotInfo> Lots { get; set; }
        public List<Documentation> Documentation { get; set; }
    }
}
