using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danfoss.PhotoID.DTO
{
    public class PostCaseResponse : BaseResponse
    {
        public Entities.Case Case { get; set; }
    }
}
