using System.Threading.Tasks;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;

namespace Union.Backend.Service.Services
{
    public class ClientDialogService
    {
        public async Task<bool> Syn(TokenDto tokenDto)
        {
            throw new WorkInProgressApiException();
        }
    }
}
