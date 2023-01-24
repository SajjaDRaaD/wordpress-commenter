using DataAccess.Models.User;

namespace DataAccess.Repository.Contracts
{
    public interface IWebsiteRepository
    {

        Task Create(Websites website);
        Task Delete(int Id);
        Task Edit(Websites website);
        Task<List<Websites>> GetAll();
        Task<Websites> Get(int Id);
        bool CheckDuplicateWebsite(string url);

    }
}
