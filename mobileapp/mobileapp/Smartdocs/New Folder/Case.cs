using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danfoss.PhotoID.Entities
{
    public class Case
    {
        public int Id { get; set; }

        public CaseStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ForwardedDate { get; set; }

        public string ReceiverEmail { get; set; }

        public int UserId { get; set; }

        public Guid Token { get; set; }

        public CaseMedia Media { get; set; }
    }
}
