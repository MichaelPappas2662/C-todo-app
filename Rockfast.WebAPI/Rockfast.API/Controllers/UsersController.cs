using Microsoft.AspNetCore.Mvc;
using Rockfast.ApiDatabase;
using Rockfast.ApiDatabase.DomainModels;
using Rockfast.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Rockfast.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public UsersController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      
        [HttpPost]
        public async Task<ActionResult<UserVM>> Post([FromBody] UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = model.Username
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            model.Id = user.Id;
            return CreatedAtAction(nameof(Get), new { id = user.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserVM>> Get(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userVM = new UserVM
            {
                Id = user.Id,
                Username = user.Username
            };

            return Ok(userVM);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserVM>>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Select(user => new UserVM
                {
                    Id = user.Id,
                    Username = user.Username
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
