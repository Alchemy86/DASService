using System;
using ASEntityFramework;

namespace AuctionSniperWnSer
{
    public class Error
    {
        
        public void Add(string message)
        {
            try
            {
                using (var ds = new ASEntities())
                {
                    ds.EventLog.Add(new EventLog
                    {
                        CreatedDate = DateTime.Now,
                        Event = "Error",
                        Message = message
                    });
                    ds.SaveChanges();
                }
            }
            catch (Exception)
            {
                    
                throw;
            }
            
        }

    }
}
