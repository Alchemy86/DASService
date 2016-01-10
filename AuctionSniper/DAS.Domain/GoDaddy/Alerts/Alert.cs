using System;
using DAS.Domain.Enum;
using DAS.Domain.GoDaddy.Users;

namespace DAS.Domain.GoDaddy.Alerts
{
    public class Alert : IAlert
    {
        public Guid AlertId { get; set; }

        public Alert()
        {
            Type = AlertType.Win;
        }

        public string Description
        {
            get { return Type.ToName(); }
        }

        public DateTime TriggerTime { get; set; }
        public bool Processed { get; set; }
        public Guid AuctionId { get; set; }
        public bool Custom { get; set; }
        public AlertType Type { get; set; }
        public Auction Auction { get; set; }
    }
}
