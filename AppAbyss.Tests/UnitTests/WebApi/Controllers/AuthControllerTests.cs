using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AppAbyss.Data;
using AppAbyss.Data.Dtos.Auth;
using AppAbyss.Tests.Helpers;
using AppAbyss.WebApi.Controllers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace AppAbyss.Tests.UnitTests.WebApi.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private Mock<UserManager<AppUser>> _userManagerMock;
    private Mock<ILogger<AuthController>> _loggerMock;
    private AuthController _authController;

    [SetUp]
    public void Setup()
    {
        this._userManagerMock = MockHelpers.MockUserManager<AppUser>();
        this._loggerMock = new Mock<ILogger<AuthController>>();
        this._authController =  new AuthController(_userManagerMock.Object, _loggerMock.Object);
    }
    
    [Test]
    public async Task Register_WithValidModel_ShouldReturnsOk()
    {
        var registerDto = new RegisterDto { 
            Email = "valid@email.com",
            Username = "username",
            Password = "Password123*"
        };

        this._userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        var result = await this._authController.Register(registerDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
    // todo theory lista 

    [Theory]
    [TestCase("DuplicateUserName", "Username already exists")]
    [TestCase("DuplicateEmail", "Email already exists")]
    [TestCase("PasswordRequiresDigit", "Passwords must have at least one digit ('0'-'9').")]
    [TestCase("PasswordRequiresLower", "Passwords must have at least one lowercase ('a'-'z').")]
    [TestCase("PasswordRequiresUpper", "Passwords must have at least one uppercase ('A'-'Z').")]
    [TestCase("PasswordRequiresNonAlphanumeric", "Passwords must have at least one non alphanumeric character.")]
    [TestCase("PasswordTooShort", "Passwords must be at least 8 characters.")]
    public async Task Register_ErrorHappensInCreateAsync_ShouldReturnsBadRequest(string code, string description)
    {
        var registerDto = new RegisterDto { 
            Email = "valid@email.com",
            Username = "username",
            Password = "Password123*"
        };
        
        var identityError = new IdentityError
        {
            Code = code,
            Description = description
        };  

        var exceptedValue = new SerializableError
        {
            { code, new[] { description } }
        };
        
        this._userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(identityError));
        
        var result = await this._authController.Register(registerDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            
            var objectResult = result.As<BadRequestObjectResult>();
            objectResult.StatusCode.Should().Be(400);
            objectResult.Value.Should().BeOfType<SerializableError>();
            
            var problemDetails = objectResult.Value.As<SerializableError>();
            problemDetails.Should().NotBeNull();
            
            problemDetails.Should().BeEquivalentTo(exceptedValue);
            
        
            
        }
    }
    
    [Test]
    public async Task Register_WithInvalidModel_ShouldReturnsBadRequest()
    {
        var registerDto = new RegisterDto { 
            Email = "invalidemail",
            Username = "username",
            Password = "Password"
        };
        
        
        this._authController.ModelState.AddModelError("key", "error msg");
        var result = await this._authController.Register(registerDto);
        
        
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            
            var badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Value.Should().BeOfType<SerializableError>();
        }
    }
    
    [Test]
    public async Task Register_WithValidModelAndErrorHappens_ShouldReturnsInternalServerError()
    {
        var registerDto = new RegisterDto { 
            Email = "valid@email.com",
            Username = "username",
            Password = "Password123*"
        };

        this._userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        
        var result = await this._authController.Register(registerDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<StatusCodeResult>();
            
            var statusCodeResult = result.As<StatusCodeResult>();
            statusCodeResult.StatusCode.Should().Be(500);
        }
    }
    
    [Test]
    public async Task Login_WithValidModel_ShouldReturnsOk()
    {
        var loginDto = new LoginDto { 
            UserName = "username",
            Password = "Password123*"
        };

        this._userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());
        
        this._userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        
        this._userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(new List<string>());
        
        var result = await this._authController.Login(loginDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
    
    [Test]
    [TestCase("AnyKey", "AnyErrorMsg")]
    public async Task Login_WithInvalidModel_ShouldReturnsBadRequest(string key, string errorMsg)
    {
        var loginDto = new LoginDto { 
            UserName = "username",
            Password = "Password123*"
        };
        
        var expectedValue = new SerializableError
        {
            { key, new[] { errorMsg } }
        };
        
        this._authController.ModelState.AddModelError(key, errorMsg);
        var result = await this._authController.Login(loginDto);
        
        
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
            
            var badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Value.Should().BeOfType<SerializableError>();
            
            var problemDetails = badRequestObjectResult.Value.As<SerializableError>();
            problemDetails.Should().NotBeNull();
            problemDetails.Should().HaveCount(1);
            problemDetails.Should().ContainKey(key);
            
            problemDetails.Should().BeEquivalentTo(expectedValue);
            
        }
    }
    
    [Test]
    public async Task Login_WithValidModelAndErrorHappens_ShouldReturnsInternalServerError()
    {
        var loginDto = new LoginDto { 
            UserName = "username",
            Password = "Password123*"
        };
        
        var expectedStatusCode = 500;

        this._userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        
        var result = await this._authController.Login(loginDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<StatusCodeResult>();
            
            var statusCodeResult = result.As<StatusCodeResult>();
            statusCodeResult.StatusCode.Should().Be(expectedStatusCode);
        }
    }
    
    [Test]
    public async Task Login_WithValidModelAndUserNotFound_ShouldReturnsUnauthorizedResult()
    {
        var loginDto = new LoginDto { 
            UserName = "username",
            Password = "Password123*"
        };
        
        this._userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((AppUser)null!);
        
        var result = await this._authController.Login(loginDto);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<UnauthorizedObjectResult>();
            
            var unauthorizedObjectResult = result.As<UnauthorizedObjectResult>();
            unauthorizedObjectResult.Value.Should().BeOfType<string>();
            unauthorizedObjectResult.Value.Should().Be("Invalid username or password");
            
        }
    }

    [Test]
    public async Task Login_WithValidUser_ShouldContainRolesInToken()
    {
        var loginDto = new LoginDto { 
            UserName = "username",
            Password = "Password123*"
        };
        var token = "token";
        var expiration = "expiration";
        
        var roles = new List<string> { "Admin", "User" };
        
        this._userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());
        
        this._userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        
        _userManagerMock.Setup(x=> x.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(roles);
        
        var result = await this._authController.Login(loginDto);
        
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            
            var okObjectResult = result.As<OkObjectResult>();
            var valueJson = JsonConvert.SerializeObject(okObjectResult.Value);
            var valueObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(valueJson);

            valueObj.Should().NotBeNull();
            valueObj.Should().ContainKey(token);
            valueObj.Should().ContainKey(expiration);
            valueObj[token].Should().NotBeNull();
            valueObj[expiration].Should().NotBeNull();
            
            var tokenValue = valueObj["token"].ToString();
            var expirationValue = DateTime.Parse(valueObj[expiration].ToString());
            
            tokenValue.Should().NotBeNullOrEmpty();
            expiration.Should().NotBeNullOrEmpty();
            
            expirationValue.Should().BeAfter(DateTime.Now);
            


            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(tokenValue);

            jwtSecurityToken.Should().NotBeNull();
            jwtSecurityToken.Claims.Should().Contain(x => x.Type == JwtRegisteredClaimNames.Sub);
            jwtSecurityToken.Claims.Should().Contain(x => x.Type == JwtRegisteredClaimNames.NameId);
            jwtSecurityToken.Claims.Should().Contain(x => x.Type == ClaimTypes.Role);

            jwtSecurityToken.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value)
                .Should().BeEquivalentTo(roles); 
        }
        
        
        
        
    }
    
    
    
    
}