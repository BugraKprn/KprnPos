using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Layer.Dtos.Payment
{
    public class PaymentRequestDto
    {
        public int TableId { get; set; }
        public List<PaymentDto> Payments { get; set; }
    }
}
