using System;

namespace Union.Backend.Service.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public int Sum { get; set; }
        public DateTime Date { get; set; }
        public Guid Leasing { get; set; }
    }
}
