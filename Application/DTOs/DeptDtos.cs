

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeptReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double AmountRemaining { get; set; }
        public DateTime DateDeptWasMade { get; set; }
        public DateTime DatePaidOff { get; set; }
        public bool PaidOff { get; set; }
        public string Deptor { get; set; }
        public ICollection<TransferReadDto> Payments { get; set; }
    }

    public class DeptWriteDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double AmountRemaining { get; set; }
        public DateTime DateDeptWasMade { get; set; }
        public DateTime DatePaidOff { get; set; }
        public bool PaidOff { get; set; } = false;
        public string Deptor { get; set; }
        public ICollection<TransferReadDto>? Payments { get; set; }

    }
}
