
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FolderReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TransferReadDto> Transfers { get; set; } = new List<TransferReadDto>();
        public ICollection<PurchaseReadDto> Purchases { get; set; } = new List<PurchaseReadDto>();
        public ICollection<DeptReadDto> Depts { get; set; } = new List<DeptReadDto>();

    }

    public class FolderWriteDto
    {
        public string Name { get; set; }

    }
}
