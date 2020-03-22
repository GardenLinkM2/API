using System;

namespace Union.Backend.Service.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public int Sum { get; set; }
        public double Date { get; set; }
        public Guid Leasing { get; set; }
    }
}
