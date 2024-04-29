using Graduation_Project.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly Graduation_Project.API.Data.ApplicationDatabaseContext _context;

        public ChatController(Graduation_Project.API.Data.ApplicationDatabaseContext context)
        {
            _context = context;
        }



        // GET: api/Chats
        [HttpGet]
        [Authorize]

        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }
            return await _context.Chats.ToListAsync();
        }

        // GET: api/Chats/5
        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<Chat>> GetChat(int id)
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }
            var chat = await _context.Chats.FindAsync(id);

            if (chat == null)
            {
                return NotFound();
            }

            return chat;
        }


        /// <summary>
        ///Get the Chat History For This Parent(User)
        /// </summary>
        [HttpGet("UserChatHistory/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Chat>>> GetUserChats(Guid userId)
        {
            var userChats = await _context.Chats
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Created)
                .ToListAsync();

            if (userChats == null || userChats.Count == 0)
            {
                return NotFound();
            }

            return userChats;
        }


        [HttpPost("send-message")]
        //[ValidateModel]
        //[Authorize]

        public async Task<ActionResult<Chat>> PostChat(ChatRequestDto chatDto)

        {
            if (_context.Chats == null)
            {
                return Problem("Entity set 'ApplicationDatabaseContext.Chats'  is null.");
            }
            var mappedChat = new Chat(chatDto.UserId)
            {
              
                UserId = chatDto.UserId,
                Message = chatDto.Message,
                SchoolId = chatDto.SchoolId
            };
            _context.Chats.Add(mappedChat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChat", new { id = mappedChat.Id }, mappedChat);
        }





    }
}
