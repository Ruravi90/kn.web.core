using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kn.web.core.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using kn.web.core.JWTAuthentication.Models.Authentication;
using kn.web.core.JWTAuthentication.Response.Authentication;
using Microsoft.Extensions.Configuration;

namespace kn.web.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly EFContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(EFContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        [AdminAuthorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            Claim idUser = claimsIdentity.Claims.Where(c => c.Type == "Id").FirstOrDefault();
            Claim isAdmin = claimsIdentity.Claims.Where(c => c.Type == "IsAdmin").FirstOrDefault();

            if (id != user.Id)
            {
                return BadRequest();
            }

            if (user.Id != Int32.Parse(idUser.Value) && isAdmin.Value != "1") {
                return Unauthorized();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [AdminAuthorize]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.Where(u => u.UserName == model.Username).FirstOrDefaultAsync();

            if (user != null && BC.Verify(model.Password, user.Password))
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //create claims details based on the user information
                var authClaims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Secret"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("CorporateName", user.CorporateName),
                    new Claim("UserName", user.UserName),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),
                };


                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(24),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user,
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        [AdminAuthorize]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;

            var userExists = await _context.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync();
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            try
            {
                //Registro del usuario en el magement
                await _context.Users.AddAsync(new User()
                {
                    CorporateName = model.CorporateName,
                    UserName = model.UserName,
                    Password = BC.HashPassword(model.Password),
                    IsAdmin = model.IsAdmin
                });

                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again.",
                    Details = e.StackTrace
                });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
