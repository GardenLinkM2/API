using System;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;


namespace Union.Backend.Service.Services
{
    public class FriendsService
    {
        private readonly GardenLinkContext db;
        public FriendsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }


        public async Task<QueryResults<DemandDto>> GetAllFriends(Guid demandId)
        {
            //TODO
            return null;
        }

        public async Task<QueryResults<DemandDto>> GetContactDemandFromUserId(Guid friendId)
        {
            //TODO
            return null;
        }

        public async Task<QueryResults<DemandDto>> GetContactFromUserId(Guid friendId)
        {
            //TODO
            return null;
        }

        public async Task<QueryResults<DemandDto>> GetContactDemand(Guid demandId)
        {
            //TODO
            return null;
        }

        public async Task<QueryResults<DemandDto>> AddContactDemand(DemandDto demand)
        {
            //TODO

            return null;
        }

        public async Task<QueryResults<ContactDto>> CreateContact(Guid friendId)
        {
            //TODO
            var result = await GetContactDemandFromUserId(friendId) ?? throw new NotFoundApiException();
            DemandDto demande = result.Data;
            //create Contact dto from demande
            return null;
        }


        public async Task DeleteDemand(Guid friendId)
        {
            var result = await GetContactDemandFromUserId(friendId) ?? throw new NotFoundApiException();
            DemandDto demande = result.Data;
            //TODO deletedemand

        }

        public async Task DeleteContact(Guid friendId)
        {
            DemandDto demande = GetContactFromUserId(friendId).Result.Data; //Pas sur de ça
            //TODO deleteContact

        }
    }
}