namespace API.DTOs
{
    public class TransferWriteDto
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime DateWasMade { get; set; }
        public string FromBank { get; set; }
        public string FromAccount { get; set; }
        public string Reciever { get; set; }
        public string RecieverAccount { get; set; }
        public string TransferType { get; set; } // CliQ, Normal Transfer, Wire , etc . . .
    }

    public class TransferReadDto : TransferWriteDto
    {
        public Guid Id { get; set; }
    }
}