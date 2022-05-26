using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public string? TransactionId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime DateWasMade { get; set; }
        public string FromBank { get; set; }
        public string FromAccount { get; set; }
        public string Reciever { get; set; }
        public string RecieverAccount { get; set; }
        public string TransferType { get; set; } // CliQ, Normal Transfer, Wire , etc . . .

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public Guid FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}
