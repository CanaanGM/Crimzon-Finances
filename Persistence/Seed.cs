using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class Seed
    {
        public static async Task SeedData(
                DataContext context //,
                //UserManager<AppUser> userManager
            )
        {
            await Task.Run(async () => await CreateIfEmpty(
                context //, 
                //userManager
                ));
            await context.SaveChangesAsync();

        }

        private static async Task CreateIfEmpty(
            DataContext context
            //, UserManager<AppUser> userManager
            )
        {
            if (!context.Purchases.Any())
                await CreatePurchases(context);
            if (!context.Transfers.Any())
                await CreateTransfers(context);
            //if (!userManager.Users.Any())
            //    await CreateUser(userManager);
        }

        private static async Task CreateTransfers(DataContext context)
        {
            var transfers = new List<Transfer>
                {
                    new Transfer
                    {
                        Id = new Guid(),
                        Amount = 0.0,
                        Name = "Test",
                        Reciever = "Test",
                        FromBank = "Test",
                        FromAccount = "Test",
                        Description= "Test",
                        DateWasMade = DateTime.UtcNow,
                        TransferType = "Test",
                        RecieverAccount= "Test",
                    }
                };
            await context.Transfers.AddRangeAsync(transfers);
        }

        private static async Task CreatePurchases(DataContext context)
        {
            var purchases = new List<Purchase>
                {
                    new Purchase
                    {
                        Id = new Guid(),
                        Name = "Test",
                        Category = "Test",
                        Description = "Test",
                        PaymentMethod = "Test",
                        Price = 0.0,
                        PriceInDollar = 0.0,
                        PurchaseDate = DateTime.UtcNow,
                        Reccuring = "Test",
                        Seller = "Test"
                    }
                };
            await context.Purchases.AddRangeAsync(purchases);
        }
    }
}
