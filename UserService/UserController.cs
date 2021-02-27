using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace UserService
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NpgsqlConnection _connection;
        
        public UserController(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> Get(int userId)
        {
            const string sql = @"select * from users where Id = @userId";

            var user = await _connection.QueryFirstOrDefaultAsync<User>(sql, new { userId });
            if (user is null)
                return NotFound();
            
            return user;
        }

        [HttpGet]
        public async Task<ActionResult<User[]>> Get()
        {
            const string sql = @"select * from users";

            var users = await _connection.QueryAsync<User>(sql);
            return users.ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(User user)
        {
            const string sql = @"
                insert into Users (email, last_name, first_name)
                values (@email, @lastName, @firstName) returning id";
            
            var userId = await _connection.QuerySingleAsync<int>(sql, user);
            return CreatedAtAction(nameof(Get), new {id = userId}, userId);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(int userId, User user)
        {
            const string sql = @"
                update Users
                set
                    email = @email,
                    lastName = @lastName,
                    firstName = @firstName
                where id = @userId";
            
            var rowsAffected = await _connection.ExecuteAsync(sql, new { id = userId, user = user });
            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
        }
        
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            const string sql = @"delete from Users where id = @userId";
            
            var rowsAffected = await _connection.ExecuteAsync(sql, new { userId });
            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
        }
    }
}