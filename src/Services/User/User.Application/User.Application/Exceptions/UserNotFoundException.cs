using BuildingBlocks.Exceptions;

namespace User.Application.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid id) : base("User", id)
        {
        }
    }
}
