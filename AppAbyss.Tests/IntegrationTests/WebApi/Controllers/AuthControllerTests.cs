using System.Net;
using System.Text;
using AppAbyss.Data.Dtos.Auth;
using AppAbyss.Tests.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;

namespace AppAbyss.Tests.IntegrationTests.WebApi.Controllers;


public class AuthControllerTests : IDisposable
{
    private IntegrationTestWebApplicationFactory _factory;
    private HttpClient _client;
    
    // TODO Login

    [OneTimeSetUp]
    public void OneTimeSetup() => _factory = new IntegrationTestWebApplicationFactory();

    [SetUp]
    public void Setup() => _client = _factory.CreateClient();

    public void Dispose() => _factory?.Dispose();

    [Test]
    public async Task Register_WithValidModel_ShouldReturnsOk()
    {
        var registerDto = new RegisterDto
        {
            Email = "valid@email.com",
            Username = "validUser",
            Password = "StrongPassword123!"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");


        var response = await _client.PostAsync("/api/auth/register", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();

            responseContent.Should()
                .NotContain("DuplicateUserName")
                .And.NotContain("DuplicateEmail")
                .And.NotContain("PasswordRequiresUniqueChars")
                .And.NotContain("PasswordRequiresNonAlphanumeric")
                .And.NotContain("PasswordRequiresDigit")
                .And.NotContain("PasswordRequiresUpper")
                .And.NotContain("PasswordRequiresLower");
        }
    }

    [Test]
    public async Task Register_WithTooShortUsername_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = "ab",
            Password = "StrongPassword123!"
        };
        var content = new StringContent(
            JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"
            );
        
        var response = await _client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Username must be a string with a minimum length of 3 and a maximum length of 256.");
        }

    }

    [Test]
    public async Task Register_WithTooLongUsername_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = string.Empty.PadLeft(257, 'a'),
            Password = "StrongPassword123!"
        };
        var content = new StringContent(
            JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"
        );
        
        var response = await _client.PostAsync("/api/auth/register", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Username must be a string with a minimum length of 3 and a maximum length of 256.");
        }
    }
    
    [Test]
    public async Task Register_WithTooShortPassword_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example2.com",
            Username = "username1",
            Password = "passwrd"
        };
        var content = new StringContent(
            JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"
        );
        
        var response = await _client.PostAsync("/api/auth/register", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Password must be a string with a minimum length of 8 and a maximum length of 256.");
        }
    }

    [Test]
    public async Task Register_WithTooLongPassword_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = "username1",
            Password = string.Empty.PadLeft(257, 'a')
        };
        
        var content = new StringContent(
            JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"
        );
        
        var response = await _client.PostAsync("/api/auth/register", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Password must be a string with a minimum length of 8 and a maximum length of 256.");
        }

    }
    
    [Test]
    public async Task Register_WithInvalidEmail_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "invalidemail",
            Username = "username1",
            Password = "StrongPassword123!"
        };
        
        var content = new StringContent(
            JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"
        );
        
        var response = await _client.PostAsync("/api/auth/register", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The Email field is not a valid e-mail address.");
        }
    }

    /*
        7db teszt kell:
        DuplicateUserName-
        DuplicateEmail-
        PasswordRequiresUniqueChars?????
        PasswordRequiresNonAlphanumeric-
        PasswordRequiresDigit-
        PasswordRequiresLower-
        PasswordRequiresUpper-
     */
    [Test]
    public async Task Register_WithExistingUsername_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = "testUserExists",
            Password = "StrongPassword123!"
        };
        
        var anotherRegisterDtoWithSameUsername = new RegisterDto
        {
            Email = "test@example2.com",
            Username = "testUserExists",
            Password = "StrongPassword123!"
        };
        
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        var anotherContent = new StringContent(JsonConvert.SerializeObject(anotherRegisterDtoWithSameUsername), Encoding.UTF8, "application/json");
        
        await _client.PostAsync("/api/auth/register", content);
        var duplicateResponse = await _client.PostAsync("/api/auth/register", anotherContent);
        
        using (new AssertionScope())
        {
            duplicateResponse.Should().NotBeNull();
            duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await duplicateResponse.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("DuplicateUserName");
        }
    }

    [Test]
    public async Task Register_WithExistingEmail_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = "testUserExists",
            Password = "StrongPassword123!"
        };
        
        var anotherRegisterDtoWithSameEmail = new RegisterDto
        {
            Email = "test@example.com",
            Username = "testUserExists2",
            Password = "StrongPassword123!"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        var anotherContent = new StringContent(JsonConvert.SerializeObject(anotherRegisterDtoWithSameEmail), Encoding.UTF8, "application/json");
        
        await this._client.PostAsync("/api/auth/register", content);
        var duplicateResponse = await this._client.PostAsync("/api/auth/register", anotherContent);

        
        using (new AssertionScope())
        {
            duplicateResponse.Should().NotBeNull();
            duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await duplicateResponse.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("DuplicateEmail");
        }
    }

    [Test]
    public async Task Register_WithPasswordThatDoesNotContainNonAlphanumeric_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Username = "username1",
            Password = "StrongPassword"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("PasswordRequiresNonAlphanumeric");

        }
    }

    [Test]
    public async Task Register_WithPasswordThatDoesNotContainDigit_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@exampleUniqueChars.com",
            Username = "UniqueChars",
            Password = "StrongPassword*"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("PasswordRequiresDigit");
        }
    }
    
    [Test]
    public async Task Register_WithPasswordThatDoesNotContainLowerCharacter_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@exampleLowerCharacter.com",
            Username = "LowerCharacter",
            Password = "PASSWORD"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("PasswordRequiresLower");
            
            
        }
    }

    [Test]
    public async Task Register_WithPasswordThatDoesNotContainUpperCharacter_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "test@exampleUpperCharacter.com",
            Username = "UpperCharacter",
            Password = "password"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("PasswordRequiresUpper");
            
            
        }
    }

    [Test]
    public async Task Register_WithoutEmail_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Username = "WithoutEmail",
            Password = "WithoutEmail"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Email is required");
            
            
        }
    }

    [Test]
    public async Task Register_WithoutUsername_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Email = "WithoutUsername@example.com",
            Password = "WithoutUser"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Username is required");
        }
    }

    [Test]
    public async Task Register_WithoutPassword_ShouldReturnBadRequest()
    {
        var registerDto = new RegisterDto
        {
            Username = "WithoutPassword",
            Email = "WithoutPassword@example.com"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

        var response = await this._client.PostAsync("/api/auth/register", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Password is required");
        }
    }

    [Test]
    public async Task Login_WithoutPassword_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            UserName = "WithoutPassword"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Password is required");
            
        }
    }

    [Test]
    public async Task Login_WithoutUsername_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            Password = "WithoutUsername"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Username is required");
            
        }
    }

    [Test]
    public async Task Login_WithTooShortUsername_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            UserName = "ab",
            Password = "WithoutUsername"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field UserName must be a string with a minimum length of 3 and a maximum length of 256.");
            
        }
    }
    
    [Test]
    public async Task Login_WithTooLongUsername_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            UserName = string.Empty.PadLeft(257, 'a'),
            Password = "WithoutUsername"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field UserName must be a string with a minimum length of 3 and a maximum length of 256.");
            
        }
    }
    
    [Test]
    public async Task Login_WithTooShortPassword_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            UserName = "WithoutPassword",
            Password = "passwrd"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Password must be a string with a minimum length of 8 and a maximum length of 256.");
            
        }
    }
    
    [Test]
    public async Task Login_WithTooLongPassword_ShouldReturnBadRequest()
    {
        var loginDto = new LoginDto()
        {
            UserName = "WithTooLongPassword",
            Password = string.Empty.PadLeft(257, 'a')
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("The field Password must be a string with a minimum length of 8 and a maximum length of 256.");
            
        }
    }
    
    [Test]
    public async Task Login_WithUnregisteredUser_ShouldReturnUnauthorized()
    {
        var loginDto = new LoginDto()
        {
            UserName = "UnregisteredUser",
            Password = "WithoutPassword"
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var response = await this._client.PostAsync("/api/auth/login", content);
        
        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Invalid username or password");
            
        }
    }

    [Test]
    public async Task Login_WithRegisteredUserAndWrongPassword_ShouldReturnUnauthorized()
    {
        var registerDto = new RegisterDto
        {
            Email = "newreg@gmail.com",
            Username = "RegisteredUser",
            Password = "RegisteredUser123!"
        };
        
        var loginDto = new LoginDto()
        {
            UserName = "RegisteredUser",
            Password = "WrongPassword"
        };
        
        var registerContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        var loginContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var registerResponse = await this._client.PostAsync("/api/auth/register", registerContent);
        var loginResponse = await this._client.PostAsync("/api/auth/login", loginContent);
        
        using (new AssertionScope())
        {
            registerResponse.Should().NotBeNull();
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            loginResponse.Should().NotBeNull();
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            
            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("Invalid username or password");
            
        }
    }

    [Test]
    public async Task Login_WithRegisteredUserAndCorrectPassword_ShouldReturnOk()
    {
        var registerDto = new RegisterDto
        {
            Email = "CorrectPassword@mail.com",
            Username = "CorrectPassword123",
            Password = "CorrectPassword123!"
        };
        
        var loginDto = new LoginDto()
        {
            UserName = "CorrectPassword123",
            Password = "CorrectPassword123!"
        };
        
        var registerContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        var loginContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        
        var registerResponse = await this._client.PostAsync("/api/auth/register", registerContent);
        var loginResponse = await this._client.PostAsync("/api/auth/login", loginContent);
        
        using (new AssertionScope())
        {
            registerResponse.Should().NotBeNull();
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            loginResponse.Should().NotBeNull();
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            
            responseContent.Should()
                .Contain("token")
                .And.Contain("expiration");
            
        }
    }
}