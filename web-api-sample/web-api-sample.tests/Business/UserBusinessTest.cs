using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api_sample.api.Business;
using web_api_sample.api.Models.Entities;
using web_api_sample.api.Repositories.Interfaces;
using web_api_sample.api.Services.Interfaces;
using Xunit;

namespace web_api_sample.tests.Business
{
    [Collection("User")]
    public class UserBusinessTest
    {
        private static User User => new User { Id = 1, Name = "TestName", Username = "TestUsername", Password = "TestPassword", Email = "TestEmail", Active = true, CreatedAt = DateTime.Now, Roles = new List<Role>() { new Role() { Id = 1, Name = "TestName", Description = "TestDescription", Active = true, CreatedAt = DateTime.Now } } };
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new Mock<IRoleRepository>();
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();

        #region AddAsync

        [Fact]
        [Trait("User", "AddAsync")]
        public async Task AddAsync_NullUser_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.AddAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);
            User user = null;

            //Act
            bool result = await userBusiness.AddAsync(user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "AddAsync")]
        public async Task AddAsync_UsernameExists_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            _userRepositoryMock.Setup(m => m.AddAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.AddAsync(User);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "AddAsync")]
        public async Task AddAsync_UsernameNotExists_True()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.AddAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.AddAsync(User);

            //Assert
            Assert.True(result);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        [Trait("User", "UpdateAsync")]
        public async Task UpdateAsync_NullUser_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);
            User user = null;

            //Act
            bool result = await userBusiness.UpdateAsync(user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "UpdateAsync")]
        public async Task UpdateAsync_NotExists_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.UpdateAsync(User);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "UpdateAsync")]
        public async Task UpdateAsync_Exists_True()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(true));
            _userRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).Returns(() => Task.FromResult(User.Id));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.UpdateAsync(User);

            //Assert
            Assert.True(result);
        }

        #endregion

        #region DeleteAsync

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [Trait("User", "DeleteAsync")]
        public async Task DeleteAsync_LessThanOrEqualToZero_False(int id)
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.DeleteUserRolesAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            _userRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.DeleteAsync(id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "DeleteAsync")]
        public async Task DeleteAsync_NotExists_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _userRepositoryMock.Setup(m => m.DeleteUserRolesAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            _userRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.DeleteAsync(User.Id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "DeleteAsync")]
        public async Task DeleteAsync_Exists_False()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(true));
            _userRepositoryMock.Setup(m => m.DeleteUserRolesAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            _userRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.DeleteAsync(User.Id);

            //Assert
            Assert.True(result);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        [Trait("User", "GetAllAsync")]
        public async Task GetAllAsync_Null()
        {
            //Arrange
            List<User> users = null;
            _userRepositoryMock.Setup(m => m.GetAllAsync()).Returns(() => Task.FromResult(users));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.GetAllAsync();

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        [Trait("User", "GetAllAsync")]
        public async Task GetAllAsync_UserList()
        {
            //Arrange
            List<User> users = new List<User>() { User, User, User };
            _userRepositoryMock.Setup(m => m.GetAllAsync()).Returns(() => Task.FromResult(users));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.GetAllAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        [Trait("User", "GetByIdAsync")]
        public async Task GetByIdAsync_Null()
        {
            //Arrange
            User user = null;
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(user));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.GetByIdAsync(User.Id);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        [Trait("User", "GetByIdAsync")]
        public async Task GetByIdAsync_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(User));
            _roleRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(User.Roles));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.GetByIdAsync(User.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().Be(User.Id);
            result.Name.Should().NotBeNullOrEmpty();
            result.Name.Should().Be(User.Name);
            result.Username.Should().NotBeNullOrEmpty();
            result.Username.Should().Be(User.Username);
            result.Password.Should().NotBeNullOrEmpty();
            result.Password.Should().Be(User.Password);
            result.Email.Should().NotBeNullOrEmpty();
            result.Email.Should().Be(User.Email);
            result.Active.Should().Be(User.Active);
            result.Roles.Should().NotBeNull();
            result.CreatedAt.Should().HaveYear(DateTime.Now.Year);
            result.CreatedAt.Should().HaveMonth(DateTime.Now.Month);
            result.CreatedAt.Should().HaveDay(DateTime.Now.Day);
            result.CreatedAt.Should().HaveHour(DateTime.Now.Hour);
            result.CreatedAt.Should().HaveMinute(DateTime.Now.Minute);
        }

        #endregion

        #region LoginAsync

        [Fact]
        [Trait("User", "LoginAsync")]
        public async Task LoginAsync_Null()
        {
            //Arrange
            User user = null;
            List<Role> roles = null;
            _userRepositoryMock.Setup(m => m.GetByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult(user));
            _roleRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(roles));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.LoginAsync(User.Username, User.Password);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        [Trait("User", "LoginAsync")]
        public async Task LoginAsync_User()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.GetByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult(User));
            _roleRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(User.Roles));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            var result = await userBusiness.LoginAsync(User.Username, User.Password);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().Be(User.Id);
            result.Name.Should().NotBeNullOrEmpty();
            result.Name.Should().Be(User.Name);
            result.Username.Should().NotBeNullOrEmpty();
            result.Username.Should().Be(User.Username);
            result.Password.Should().NotBeNullOrEmpty();
            result.Password.Should().Be(User.Password);
            result.Email.Should().NotBeNullOrEmpty();
            result.Email.Should().Be(User.Email);
            result.Active.Should().Be(User.Active);
            result.Roles.Should().NotBeNull();
            result.CreatedAt.Should().HaveYear(DateTime.Now.Year);
            result.CreatedAt.Should().HaveMonth(DateTime.Now.Month);
            result.CreatedAt.Should().HaveDay(DateTime.Now.Day);
            result.CreatedAt.Should().HaveHour(DateTime.Now.Hour);
            result.CreatedAt.Should().HaveMinute(DateTime.Now.Minute);
        }

        #endregion

        #region ValidateUsernameAsync

        [Fact]
        [Trait("User", "ValidateUsernameAsync")]
        public async Task ValidateUsernameAsync_UserNotExists_False()
        {
            //Arrange
            User user = null;
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(user));
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.ValidateUsernameAsync(User);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("User", "ValidateUsernameAsync")]
        public async Task ValidateUsernameAsync_SameUsernames_True()
        {
            //Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(User));
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.ValidateUsernameAsync(User);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("User", "ValidateUsernameAsync")]
        public async Task ValidateUsernameAsync_UsernameNotExists_True()
        {
            //Arrange
            User user = new User { Id = 1, Username = "TestUsername2" };
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(user));
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.ValidateUsernameAsync(User);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("User", "ValidateUsernameAsync")]
        public async Task ValidateUsernameAsync_UsernameExists_False()
        {
            //Arrange
            User user = new User { Id = 1, Username = "TestUsername2" };
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(user));
            _userRepositoryMock.Setup(m => m.UsernameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            UserBusiness userBusiness = new UserBusiness(_userRepositoryMock.Object, _roleRepositoryMock.Object, _emailServiceMock.Object);

            //Act
            bool result = await userBusiness.ValidateUsernameAsync(User);

            //Assert
            Assert.False(result);
        }

        #endregion
    }
}