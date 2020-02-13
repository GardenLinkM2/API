using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Auth;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using static Union.Backend.Service.Auth.Utils;

namespace Union.Backend.Service.Services
{
    public class ClientDialogService
    {
        private readonly IConfiguration config;
        private readonly GardenLinkContext db;
        private readonly UsersService usersService;
        public ClientDialogService(
            IConfiguration config, 
            GardenLinkContext gardenLinkContext,
            UsersService usersService)
        {
            this.config = config;
            db = gardenLinkContext;
            this.usersService = usersService;
        }

        private TokenDto GetValidateToken(string token, string secret)
        {
            var accessToken = Jose.JWT.Decode<TokenDto>(token, Convert.FromBase64String(secret));
            accessToken.Token = token;
            return accessToken;
        }

        private async Task<AuthUserDto> GetUserInfo(string host, Guid id, string token)
        {
            return await HttpGetAsync<AuthUserDto>($"{host}/users/{id}", (HttpRequestHeader.Authorization, token));
        }

        private async Task<User> AddNewUser(Guid id, string token)
        {
            var authUser = await GetUserInfo(config.GetValue<string>("authServer:host"), id, token);
            var userDto = new UserDto
            {
                Id = id,
                UserName = authUser.Username,
                LastName = authUser.LastName,
                FirstName = authUser.FirstName
            };
            return (await usersService.AddUser(userDto, id)).Data.ConvertToModel();
        }

        public async Task<TokenDto> Syn(TokenDto tokenDto)
        {
            try
            {
                //Utils.GetValidateToken(tokenDto.Token, config.GetValue<string>("authServer:clientSecret"));
                //var verificationKey = config.GetValue<string>("authServer:clientSecret");
                var accessToken = GetValidateToken(tokenDto.Token, config.GetValue<string>("authServer:clientSecret"));
                //var tokenInfo = Utils.GetValidateToken(tokenDto.Token, verificationKey);
                ////TODO
                ////GetById(id-dans-token)
                ////true: generate token
                ////false: appeler devauthm2 avec le token en Header et /users/id-dans-token
                var userId = new Guid(accessToken.Uuid);
                var user = await usersService.GetUserById(userId) ?? await AddNewUser(userId, accessToken.Token);
                //TODO: generate token
                return new TokenDto();
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BadRequestApiException(ex.Message);
            }
        }
    }
}
