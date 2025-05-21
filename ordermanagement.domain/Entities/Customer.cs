namespace ordermanagement.domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }
        public CustomerSegment Segment { get; set; }

        public Customer(string name, string email, string phone, string address, CustomerSegment segment)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            RegisteredOn = DateTime.Now;
            Segment = segment;
        }
    }
}
