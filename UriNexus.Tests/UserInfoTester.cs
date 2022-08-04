namespace UriNexus.Tests;

[TestClass]
public class UserInfoTester
{
    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void Always_FormatUserNamePasswordSeparated()
        {
            //Arrange
            var instance = Fixture.Create<UserInfo>();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{instance.Name}:{instance.Password}");
        }
    }
}