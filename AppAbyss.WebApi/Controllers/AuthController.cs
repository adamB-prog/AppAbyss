using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using AppAbyss.Data;
using AppAbyss.Data.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppAbyss.WebApi.Controllers;

/// <summary>
/// This controller handles the authorisation functions.
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used to manage user data.</param>
    /// <param name="logger">The logger used to log errors and information.</param>
    /// <exception cref="ArgumentNullException"> Thrown if the provided UserManager or logger instance is null.</exception>
    public AuthController(UserManager<AppUser> userManager, ILogger<AuthController> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    /// <summary>
    /// Handles the user registration process.
    /// </summary>
    /// <param name="registerDto">The <see cref="RegisterDto"/> containing user registration information.</param>
    /// <returns>
    /// It returns an HTTP 200 OK response if the registration data is valid and the registration is successful, otherwise it returns an HTTP 400 Bad Request response with the validation errors.
    /// </returns>
    /// <response code="200">Return a JWT token</response>
    /// <response code="400">Return an error message</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            this._logger.LogError("Invalid registration model");
            return BadRequest(ModelState);
        }

        var user = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                this._logger.LogInformation("User registration succeeded for {Username}", user.UserName);
                // test
                this._logger.LogInformation("Users count: {Count}", _userManager.Users.Count());
                
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            this._logger.LogWarning("User registration failed, {0} error(s) occurred", ModelState.ErrorCount);
            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "An error occurred while registering {Username}", user.UserName);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Handles the user login process.
    /// </summary>
    /// <param name="loginModel">The <see cref="LoginDto"/> containing user login credentials.</param>
    /// <returns>
    /// It returns an HTTP 200 OK response with a JWT token If the login credentials are valid, otherwise it return an HTTP 400 or 500 response.
    /// </returns>
    /// <response code="200">Return a JWT token</response>
    /// <response code="400">Return an error message</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(LoginDto loginModel)
    {
        if (!ModelState.IsValid)
        {
            this._logger.LogWarning("Login attempt with invalid model state");
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {

                var claim = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id)
                };
                foreach (var role in await _userManager.GetRolesAsync(user))
                {
                    claim.Add(new Claim(ClaimTypes.Role, role));
                }

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("verylongsecretkey"));

                var token = new JwtSecurityToken(
                    issuer: "http://www.security.org", audience: "http://www.security.org",
                    claims: claim, expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                this._logger.LogInformation("User {UserName} logged in successfully", loginModel.UserName);
                return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });

                
            }
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Error occurred while generating JWT token for user {UserName}",
                loginModel.UserName);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        this._logger.LogWarning("Unauthorized login attempt for user {UserName}",loginModel.UserName);
        return Unauthorized("Invalid username or password");
    }
}

