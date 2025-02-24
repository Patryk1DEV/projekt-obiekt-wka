namespace EmployeeManagementSystem
{
    public class Event
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
    }
}
