using System;

namespace Union.Backend.Service.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public int Sum { get; set; }
        public int State { get; set; }
        public LeasingDto Leasing { get; set; }
        public UserDto Payer { get; set; }
        public UserDto Collector { get; set; }
    }
}
