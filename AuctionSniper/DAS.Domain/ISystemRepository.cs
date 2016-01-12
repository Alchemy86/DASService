using System;
using System.Collections.Generic;
using DAS.Domain.GoDaddy;
using DAS.Domain.GoDaddy.Alerts;
using DAS.Domain.GoDaddy.Users;
using DAS.Domain.Users;

namespace DAS.Domain
{
    public interface ISystemRepository
    {
        string ServiceEmail { get; }
        string ServiceEmailPassword { get; }
        string ServiceEmailPort { get; }
        string ServiceEmailSmtp { get; }
        string ServiceEmailUser { get; }
        string AlertEmail { get; }
        string BidTime { get; }

        IEnumerable<Auction> GetEndingAuctions();
        IEnumerable<Alert> GetAlerts();

        void MarkAlertAsProcesed(Guid alertId);
        void MarkAuctionAsProcess(Guid auctionId);
        void SaveGodaddyAccount(GoDaddyAccount account);
        User SaveAccount(User account);

    }
}