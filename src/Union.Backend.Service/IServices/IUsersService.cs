using System.Threading.Tasks;
using Union.Backend.Model.Models;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.IServices
{
    public interface IUsersService
    {
        public Task<UsersQueryResults> All();
        public Task<UsersQueryResults> Add(User user);
    }
}
