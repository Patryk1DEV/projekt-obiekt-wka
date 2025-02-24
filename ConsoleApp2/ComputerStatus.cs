namespace EmployeeManagementSystem
{
    public class ComputerStatus
    {
        public int EmployeeId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime OnlineSince { get; set; } = DateTime.MinValue;
    }
}
