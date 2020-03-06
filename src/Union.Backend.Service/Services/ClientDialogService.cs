using Jose;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Union.Backend.Model;
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
        private readonly IOptions<AuthSettings> auth;
        private readonly GardenLinkContext db;
        private readonly UsersService usersService;
        public ClientDialogService(
            IOptions<AuthSettings> auth,
            GardenLinkContext gardenLinkContext,
            UsersService usersService)
        {
            this.auth = auth;
            this.db = gardenLinkContext;
            this.usersService = usersService;
        }

        private async Task<AuthUserDto> RequestUserInfo(string host, Guid id, string token)
        {
            return await HttpGetAsync<AuthUserDto>($"{host}/users/{id}", (HttpRequestHeader.Authorization, token));
        }

        private TokenDto GenerateUserToken(User user, bool isAdmin)
        {
            var header = new Dictionary<string, object>()
            {
                ["kid"] = JwtConfigForBack.Kid
            };
            var payload = new Dictionary<string, object>()
            {
                ["uuid"] = user.Id,
                ["sub"] = user.Mail,
                ["isAdmin"] = isAdmin,
                ["exp"] = DateTime.UtcNow.AddDays(JwtConfigForBack.BaseAddDays).Subtract(new DateTime(1970, 1, 1)).TotalSeconds
            };
            return new TokenDto
            {
                Token = JWT.Encode(
                    payload, 
                    Convert.FromBase64String(auth.Value.BackSecret),
                    JwtConfigForBack.JwsAlgorithm,
                    extraHeaders: header)
            };
        }

        private async Task<User> GenerateNewUser(Guid id, TokenDto token)
        {
            var authUser = await RequestUserInfo(auth.Value.Host, id, token.Token);
            var userDto = new UserDto
            {
                Id = id,
                Email = token.Sub, //Use the auth token because email is hidden in the auth endpoint
                LastName = authUser.LastName,
                FirstName = authUser.FirstName,
            };
            return (await usersService.AddUser(userDto, id)).Data.ConvertToModel();
        }

        public async Task<TokenDto> Syn(TokenDto tokenDto)
        {
            try
            {
                var accessToken = ValidateAndGetToken<TokenDto>(tokenDto.Token, auth.Value.ClientSecret);
                accessToken.Token = tokenDto.Token;
                var userId = new Guid(accessToken.Uuid);
                var user = await db.Users.GetByIdAsync(userId) ?? await GenerateNewUser(userId, accessToken);
                //return GenerateUserToken(user, accessToken.IsAdmin ?? false);
                return GenerateUserToken(user, true); //TEMP
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
