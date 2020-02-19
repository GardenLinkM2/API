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
            throw new WorkInProgressApiException();
        }


        public async Task<QueryResults<DemandDto>> GetContactDemandById(Guid demandId)
        {
            //TODO
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<DemandDto>> GetAllContactDemands(Guid userId)
        {
            //TODO
            throw new WorkInProgressApiException();
        }

        

        public async Task<QueryResults<DemandDto>> AddContactDemand(DemandDto demand)
        {
            //TODO

            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<ContactDto>> CreateContact(Guid friendId)
        {
            //TODO
            throw new WorkInProgressApiException();
        }


        public async Task DeleteDemand(Guid demandId)
        {

            //TODO deletedemand
            throw new WorkInProgressApiException();
        }

        public async Task DeleteContact(Guid friendId)
        {
            throw new WorkInProgressApiException();
            //TODO deleteContact

        }
    }
}