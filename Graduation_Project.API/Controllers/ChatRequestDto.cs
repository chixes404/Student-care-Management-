namespace Graduation_Project.API.Controllers
{
    public class ChatRequestDto
    {

      public  Guid UserId { get; set; }

        public string Message {  get; set; }

        public int SchoolId { get; set; }

    }
}