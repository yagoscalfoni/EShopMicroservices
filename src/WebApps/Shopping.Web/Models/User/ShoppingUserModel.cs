namespace Shopping.Web.Models.User
{
    public class ShoppingUserModel
    {
        public Guid UserId { get; set; }           
        public string Name { get; set; }           
        public string Email { get; set; }          
        public string PhoneNumber { get; set; }    
        public string Address { get; set; }        
        public DateTime DateOfBirth { get; set; }  
        public string Gender { get; set; }         
        public string ProfileImageUrl { get; set; }
        public DateTime DateJoined { get; set; }   
        public bool IsEmailVerified { get; set; }
    }

    public record GetUserResponse(ShoppingUserModel User);
}
