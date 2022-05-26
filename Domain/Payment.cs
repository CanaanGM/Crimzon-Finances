using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public Guid DeptId { get; set; }
        public Dept Dept { get; set; }

    }
}
