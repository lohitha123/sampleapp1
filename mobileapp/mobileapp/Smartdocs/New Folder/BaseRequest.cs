namespace Danfoss.PhotoID.DTO
{
    public abstract class BaseRequest
    {
        public Entities.User RequestingUser { get; set; }
    }
}
