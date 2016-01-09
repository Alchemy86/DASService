using System.ComponentModel;

namespace DAS.Domain.GoDaddy.Alerts
{
    public interface IAlert
    {
        System.Guid AlertId { get; set; }
        string Description { get; }
        System.DateTime TriggerTime { get; set; }
        bool Processed { get; set; }
        System.Guid AuctionId { get; set; }
        bool Custom { get; set; }
        AlertType Type { get; set; }
    }

    public enum AlertType
    {
        [Description("Win Alert")]
        Win,
        [Description("1 Hour Alert")]
        Reminder1Hour,
        [Description("12 Hour Alert")]
        Reminder12Hours
    }
}