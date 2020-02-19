using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class DemandDto
    {
        public Guid Id { get; set; }
        public string FirstMessage { get; set; }
        public ContactStatus Status { get; set; }
    }
}
