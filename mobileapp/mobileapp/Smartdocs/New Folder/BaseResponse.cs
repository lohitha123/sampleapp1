using System;
using Danfoss.PhotoID.Exceptions;

namespace Danfoss.PhotoID.DTO
{
    public abstract class BaseResponse
    {
        public bool HasError { get { return Error != null; } }

        public InternalException Error { get; set; }
    }
}
