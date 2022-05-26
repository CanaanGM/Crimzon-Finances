using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PaymentReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime DateMade { get; set; }

        public DeptReadDto Dept { get; set; }
        public UserDto User { get; set; }


    }

    public class PaymentWriteDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime DateMade { get; set; }


    }
}
