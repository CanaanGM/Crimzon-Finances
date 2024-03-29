﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Dept
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double AmountRemaining { get; set; }
        public DateTime DateDeptWasMade { get; set; }
        public DateTime? DatePaidOff { get; set; }
        public bool PaidOff { get; set; } = false;
        public string Deptor { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }


        //public ICollection<Transfer> Payments { get; set; } = new List<Transfer>();

        //TODO: if the payments amount is == or > dept amount 
        // auto close it, or mark it as payed.
        //TODO: use transaction instead of payments
    }
}
