using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data.Abstract;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepository,
         IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {

            if (userForRegisterDto == null) return BadRequest(new { message = "Invalid request body" });

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _authRepository.UserExists(userForRegisterDto.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            //Create the user

            var user = await _authRepository.RegisterUser(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            if (userForLoginDto == null) return BadRequest(new { message = "Invalid request body" });

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var userFromRepo = await _authRepository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null) return Unauthorized();

            //generate claims for the token
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username.ToLower())
            };

            //Generate the key to be used
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("appSettings:Token").Value));

            //Generate the signing credentials with security algorithm and the key..
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Generate the security token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            //Generate jwt token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //Generate the jwt token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Write the token as a response to the client
            return Ok(new { token = tokenHandler.WriteToken(token) });

        }
    }
}