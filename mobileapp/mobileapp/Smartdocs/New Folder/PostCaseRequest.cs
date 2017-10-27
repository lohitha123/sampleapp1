using System;

namespace Danfoss.PhotoID.DTO
{
    public class PostCaseRequest : BaseRequest
    {
        public string Username { get; set; }

        public string Description { get; set; }

        public byte[] Content { get; set; }

        public bool IsVideo { get; set; }

        public Guid Token { get; set; }
    }
}
