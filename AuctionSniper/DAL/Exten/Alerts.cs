
using System;
using DAS.Domain.Enum;
using DAS.Domain.GoDaddy.Alerts;

namespace DAL
{
    public partial class Alerts
    {
        public Alert ToDomainObject()
        {
            return new Alert()
            {
                AlertId = AlertID,
                AuctionId = AuctionID,
                Custom = Custom,
                Processed = Processed,
                TriggerTime = TriggerTime,
                Type = (AlertType)Enum.Parse(typeof(AlertType), AlertType)
            };
        }

        public void FromDomainObject(Alert alert)
        {
            AlertID = alert.AlertId;
            AuctionID = alert.AuctionId;
            Custom = alert.Custom;
            Processed = alert.Processed;
            TriggerTime = alert.TriggerTime;
            AlertType = alert.Type.ToName();
            Description = alert.Description;
        }
    }
}
