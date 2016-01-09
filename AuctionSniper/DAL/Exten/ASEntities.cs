using DAS.Domain;

namespace DAL
{
    // ReSharper disable once InconsistentNaming
    public partial class ASEntities : IUnitOfWork
    {
        public void Save()
        {
            SaveChanges();
        }
    }
}
