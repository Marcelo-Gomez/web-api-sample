using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api_sample.api.Business;
using web_api_sample.api.Models.Entities;
using web_api_sample.api.Repositories.Interfaces;
using Xunit;

namespace web_api_sample.tests.Business
{
    [Collection("Role")]
    public class RoleBusinessTest
    {
        private static Role Role => new Role{ Id = 1, Name = "TestName", Description = "TestDescription", Active = true, CreatedAt = DateTime.Now };
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new Mock<IRoleRepository>();

        #region AddAsync

        [Fact]
        [Trait("Role", "AddAsync")]
        public async Task AddAsync_NullRole_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(Role.Id));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);
            Role role = null;

            //Act
            bool result = await roleBusiness.AddAsync(role);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "AddAsync")]
        public async Task AddAsync_NameExists_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            _roleRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(Role.Id));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.AddAsync(Role);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "AddAsync")]
        public async Task AddAsync_NameNotExists_True()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(Role.Id));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.AddAsync(Role);

            //Assert
            Assert.True(result);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        [Trait("Role", "UpdateAsync")]
        public async Task UpdateAsync_NullRole_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);
            Role role = null;

            //Act
            bool result = await roleBusiness.UpdateAsync(role);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "UpdateAsync")]
        public async Task UpdateAsync_NotExists_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.UpdateAsync(Role);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "UpdateAsync")]
        public async Task UpdateAsync_Exists_True()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(true));
            _roleRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Role>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.UpdateAsync(Role);

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
        [Trait("Role", "DeleteAsync")]
        public async Task DeleteAsync_LessThanOrEqualToZero_False(int id)
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.DeleteAsync(id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "DeleteAsync")]
        public async Task DeleteAsync_NotExists_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(false));
            _roleRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);
            
            //Act
            bool result = await roleBusiness.DeleteAsync(Role.Id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "DeleteAsync")]
        public async Task DeleteAsync_Exists_False()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.ExistsAsync(It.IsAny<int>())).Returns(() => Task.FromResult(true));
            _roleRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(() => Task.FromResult(0));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.DeleteAsync(Role.Id);

            //Assert
            Assert.True(result);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        [Trait("Role", "GetAllAsync")]
        public async Task GetAllAsync_Null()
        {
            //Arrange
            List<Role> roles = null;
            _roleRepositoryMock.Setup(m => m.GetAllAsync()).Returns(() => Task.FromResult(roles));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            var result = await roleBusiness.GetAllAsync();

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        [Trait("Role", "GetAllAsync")]
        public async Task GetAllAsync_RoleList()
        {
            //Arrange
            List<Role> roles = new List<Role>() { Role, Role, Role };
            _roleRepositoryMock.Setup(m => m.GetAllAsync()).Returns(() => Task.FromResult(roles));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            var result = await roleBusiness.GetAllAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        [Trait("Role", "GetByIdAsync")]
        public async Task GetByIdAsync_Null()
        {
            //Arrange
            Role role = null;
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(role));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            var result = await roleBusiness.GetByIdAsync(Role.Id);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        [Trait("Role", "GetByIdAsync")]
        public async Task GetByIdAsync_Role()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(Role));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            var result = await roleBusiness.GetByIdAsync(Role.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().Be(Role.Id);
            result.Name.Should().NotBeNullOrEmpty();
            result.Name.Should().Be(Role.Name);
            result.Description.Should().NotBeNullOrEmpty();
            result.Description.Should().Be(Role.Description);
            result.Active.Should().Be(Role.Active);
            result.CreatedAt.Should().HaveYear(DateTime.Now.Year);
            result.CreatedAt.Should().HaveMonth(DateTime.Now.Month);
            result.CreatedAt.Should().HaveDay(DateTime.Now.Day);
            result.CreatedAt.Should().HaveHour(DateTime.Now.Hour);
            result.CreatedAt.Should().HaveMinute(DateTime.Now.Minute);
        }

        #endregion

        #region ValidateRoleNameAsync

        [Fact]
        [Trait("Role", "ValidateRoleNameAsync")]
        public async Task ValidateRoleNameAsync_RoleNotExists_False()
        {
            //Arrange
            Role role = null;
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(role));
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.ValidateRoleNameAsync(Role);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Role", "ValidateRoleNameAsync")]
        public async Task ValidateRoleNameAsync_SameRolesNames_True()
        {
            //Arrange
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(Role));
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.ValidateRoleNameAsync(Role);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Role", "ValidateRoleNameAsync")]
        public async Task ValidateRoleNameAsync_NameNotExists_True()
        {
            //Arrange
            Role role = new Role { Id = 1, Name = "TestName2" };
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(role));
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(false));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.ValidateRoleNameAsync(Role);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Role", "ValidateRoleNameAsync")]
        public async Task ValidateRoleNameAsync_NameExists_False()
        {
            //Arrange
            Role role = new Role { Id = 1, Name = "TestName2" };
            _roleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult(role));
            _roleRepositoryMock.Setup(m => m.NameExistsAsync(It.IsAny<string>())).Returns(() => Task.FromResult(true));
            RoleBusiness roleBusiness = new RoleBusiness(_roleRepositoryMock.Object);

            //Act
            bool result = await roleBusiness.ValidateRoleNameAsync(Role);

            //Assert
            Assert.False(result);
        }

        #endregion
    }
}