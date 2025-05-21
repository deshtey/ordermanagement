namespace ordermanagement.domain.Entities
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }
        public CustomerSegment Segment { get; set; }

    }
}
