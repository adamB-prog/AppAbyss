using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AppAbyss.Tests.Helpers;

/// <summary>
/// This class contains helper methods for mocking asp.net core WebAPI.
/// </summary>
public static class MockHelpers
    {
        /// <summary>
        /// Creates a mock of the <see cref="UserManager{TUser}"/> class for testing.
        /// </summary>
        /// <typeparam name="TUser">The type of the user object, must be a class that can be used with UserManager.</typeparam>
        /// <returns>A mocked instance of <see cref="UserManager{TUser}"/> class with default user and password validators added.</returns>
        public static Mock<UserManager<TUser>> GetMockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        /// <summary>
        /// Creates a mock of the <see cref="SignInManager{TUser}"/> class for testing.
        /// </summary>
        /// <typeparam name="TUser">The type of the user object, must be a class that can be used with SignInManager.</typeparam>
        /// <returns>A mocked instance of <see cref="SignInManager{TUser}"/> class</returns>
        public static Mock<SignInManager<TUser>> GetMockSignInManager<TUser>() where TUser : class
        {
            var mockUsrMgr = MockUserManager<TUser>();
            var ctxAccessor = new Mock<IHttpContextAccessor>();
            var mockClaimsPrinFact = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            var mockOpts = new Mock<IOptions<IdentityOptions>>();
            var mockLogger = new Mock<ILogger<SignInManager<TUser>>>();

            return new Mock<SignInManager<TUser>>(mockUsrMgr.Object, ctxAccessor.Object, mockClaimsPrinFact.Object, mockOpts.Object, mockLogger.Object, null, null);
        }
        
        /// <summary>
        /// Creates a mock of the <see cref="UserManager{TUser}"/> class for testing.
        /// </summary>
        /// <typeparam name="TUser">The type parameter for the user, must be a class.</typeparam>
        /// <returns>A mocked instance of <see cref="UserManager{TUser}"/>.</returns>
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        /// <summary>
        /// Creates a mock of the <see cref="RoleManager{TRole}"/> class for testing.
        /// </summary>
        /// <param name="store">An optional <see cref="IRoleStore{TRole}"/> instance to use. If not provided, a mock store will be created.</param>
        /// <typeparam name="TRole">The type parameter for the role, must be a class.</typeparam>
        /// <returns>A mocked instance of <see cref="RoleManager{TRole}"/>.</returns>
        public static Mock<RoleManager<TRole>> GetMockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new Mock<RoleManager<TRole>>(store, roles, MockLookupNormalizer(),
                new IdentityErrorDescriber(), null);
        }

        /// <summary>
        /// Creates an instance of UserManager for unit testing.
        /// </summary>
        /// <param name="store">An optional <see cref="IUserStore{TUser}"/> to use. If not provided, a mock user store is created.</param>
        /// <typeparam name="TUser">The type of user object, which must be a class.</typeparam>
        /// <returns>An instance of <see cref="UserManager{TUser}"/> configured for testing, with mock dependencies set up.</returns>
        public static UserManager<TUser> GetTestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, MockLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }

        /// <summary>
        /// Creates an instance of RoleManager for unit testing.
        /// </summary>
        /// <param name="store">An optional <see cref="IRoleStore{TRole}"/> to use. If not provided, a mock role store is created.</param>
        /// <typeparam name="TRole">The type of the role object, which must be a class.</typeparam>
        /// <returns>An instance of <see cref="RoleManager{TRole}"/> configured for testing, with mock dependencies such as role validators and identity error describers.</returns>
        public static RoleManager<TRole> GetTestRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new RoleManager<TRole>(store, roles,
                MockLookupNormalizer(),
                new IdentityErrorDescriber(),
                null);
        }

        /// <summary>
        /// Creates a mocked implementation of <see cref="ILookupNormalizer"/> for unit testing purposes. This mock normalizer provides a 
        /// basic implementation of the normalization process typically used in identity systems.
        /// </summary>
        /// <returns>A mocked instance of <see cref="ILookupNormalizer"/> that normalizes strings by converting them to Base64 and upper casing.</returns>
        public static ILookupNormalizer MockLookupNormalizer()
        {
            string NormalizerFunc(string? input) => input == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).ToUpperInvariant();

            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(n => n.NormalizeName(It.IsAny<string>())).Returns(NormalizerFunc);
            lookupNormalizer.Setup(n => n.NormalizeEmail(It.IsAny<string>())).Returns(NormalizerFunc);

            return lookupNormalizer.Object;
        }
    }