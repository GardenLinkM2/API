using System;

namespace Union.Backend.Service.Dtos
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public Guid OfGarden { get; set; }
    }
}
