using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<Dept> Depts { get; set; } = new List<Dept>();
    }
}
