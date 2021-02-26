using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserService
{
    public class User
    {
        [BindNever]
        public int Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}