using System.Collections.Generic;
using Union.Backend.Service.Dtos;

namespace Union.Backend.Service.Results
{
    public class PaymentsQueryResults : QueryResults<List<PaymentDto>>
    {
    }

    public class PaymentQueryResults : QueryResults<PaymentDto>
    {

    }
}
