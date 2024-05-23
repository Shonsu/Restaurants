using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constans;

namespace Restaurants.Application.Tests
{
    public class CurrentUserTest
    {
        // TestMethod_Scenario_ExpectedResult
        [Theory]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        public void IsInRole_WithMathingRole_ShouldReturnTrue(string roleName)
        {
            // arrange
            var currentUser = new CurrentUser(
                "1",
                "test@test.com",
                [UserRoles.Admin, UserRoles.User],
                null,
                null
            );
            // act
            bool IsInRole = currentUser.IsInRole(roleName);
            // assert
            IsInRole.Should().BeTrue();
        }

        [Fact]
        public void IsInRole_WithNoMathingRole_ShouldReturnFalse()
        {
            // arrange
            var testedOwnerRole = UserRoles.Owner;
            var currentUser = new CurrentUser(
                "1",
                "test@test.com",
                [UserRoles.Admin, UserRoles.User],
                null,
                null
            );
            // act
            bool IsInRole = currentUser.IsInRole(testedOwnerRole);
            // assert
            IsInRole.Should().BeFalse();
        }
        
        [Fact]
        public void IsInRole_WithNoMathingRoleCase_ShouldReturnFalse()
        {
            // arrange
            var testedOwnerRole = UserRoles.Owner.ToLower();
            var currentUser = new CurrentUser(
                "1",
                "test@test.com",
                [UserRoles.Admin, UserRoles.User],
                null,
                null
            );
            // act
            bool IsInRole = currentUser.IsInRole(testedOwnerRole);
            // assert
            IsInRole.Should().BeFalse();
        }
    }
}
