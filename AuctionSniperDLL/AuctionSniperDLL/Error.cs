using System;
using ASEntityFramework;

namespace AuctionSniperDLL
{
    class Error
    {
        public void Add(string message, string type = "Error")
        {
            try
            {
                using (var ds = new ASEntities())
                {
                    ds.EventLog.Add(new EventLog
                    {
                        CreatedDate = DateTime.Now,
                        Event = type,
                        Message = message
                    });
                    ds.SaveChanges();
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
