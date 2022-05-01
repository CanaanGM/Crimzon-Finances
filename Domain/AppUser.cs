
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Bio { get; set; }

        ICollection<Purchase> purchases { get; set; } = new List<Purchase>();
        ICollection<Transfer> transfer { get; set; } = new List<Transfer>();    
    }
}
