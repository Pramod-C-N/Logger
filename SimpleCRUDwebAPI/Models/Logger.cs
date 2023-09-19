namespace SimpleCRUDwebAPI.Models
{
    public class Logger
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
