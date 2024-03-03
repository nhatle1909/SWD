using System.ComponentModel.DataAnnotations;

namespace Repositories.ModelView
{
    public class RequestView
    {
        public required string RequestID { get; set; }
        public required string AccountID { get; set; }
        public required string RequestStatus { get; set; }
        public required int TotalPrice { get; set; }
    }
}
