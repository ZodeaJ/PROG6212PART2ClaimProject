using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212POE.Models
{
    public enum ClaimStatus
    {
        Submitted,     // lecturer submits
        Rejected,      // rejected by coordinator or manager
        Forwarded,     // forwarded by coordinator
        Approved       // final approval by manager
    }

    public class Claim
    {
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "Please select a lecturer.")]
        public int? LecturerId { get; set; } // foreign key

        public Lecturer? Lecturer { get; set; }

        [Required, StringLength(50)]
        public string Month { get; set; }

        [Required(ErrorMessage = "Hours worked field is required.")]
        public int? HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HourlyRate { get; set; }

        [NotMapped]
        public decimal Amount => (HoursWorked ?? 0) * (HourlyRate ?? 0);

        [Required, StringLength(200)]
        public string Description { get; set; }

        public string? SupportingDocument { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Feedback> FeedbackMessages { get; set; } = new List<Feedback>();
    }
}