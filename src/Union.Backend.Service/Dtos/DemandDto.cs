using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class DemandDto
    {
        public string FirstMessage { get; set; }
        public Status Status { get; set; }
    }
}
