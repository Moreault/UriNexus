using ToolBX.Collections.ReadOnly;

namespace UriNexus.Tests;

[TestClass]
public class UrlTester
{
    [TestClass]
    public class Scheme : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenValueIsNullOrEmpty_SetToEmpty(string value)
        {
            //Arrange

            //Act
            var result = new Url { Scheme = value };

            //Assert
            result.Scheme.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenValueIsHttpWithTwoDotsAndStuff_SetToHttp()
        {
            //Arrange
            var value = "http://";

            //Act
            var result = new Url { Scheme = value };

            //Assert
            result.Scheme.Should().Be("http");
        }

        [TestMethod]
        public void WhenValueIsHttp_SetToHttp()
        {
            //Arrange
            var value = "http";

            //Act
            var result = new Url { Scheme = value };

            //Assert
            result.Scheme.Should().Be("http");
        }
    }

    [TestClass]
    public class UserInfoProperty : Tester
    {
        [TestMethod]
        public void WhenValueIsNull_SetToNull()
        {
            //Arrange
            UserInfo value = null!;

            //Act
            var result = new Url { UserInfo = value };

            //Assert
            result.UserInfo.Should().BeNull();
        }

        [TestMethod]
        public void WhenValueIsNotNull_SetToValue()
        {
            //Arrange
            var value = Fixture.Create<UserInfo>();

            //Act
            var result = new Url { UserInfo = value };

            //Assert
            result.UserInfo.Should().Be(value);
        }
    }

    [TestClass]
    public class Host : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenIsNullOrEmpty_SetToEmpty(string value)
        {
            //Arrange

            //Act
            var result = new Url { Host = value };

            //Assert
            result.Host.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenHasSlashesBeforeAndAfter_TrimSlashes()
        {
            //Arrange
            var value = "//www.sayabec.ing/";

            //Act
            var result = new Url { Host = value };

            //Assert
            result.Host.Should().Be("www.sayabec.ing");
        }

        [TestMethod]
        public void WhenHasNoSlash_SetValueAsIs()
        {
            //Arrange
            var value = "www.sayabec.ing";

            //Act
            var result = new Url { Host = value };

            //Assert
            result.Host.Should().Be("www.sayabec.ing");
        }
    }

    [TestClass]
    public class Port : Tester
    {
        [TestMethod]
        public void WhenValueIsNull_SetToNull()
        {
            //Arrange
            int? value = null!;

            //Act
            var result = new Url { Port = value };

            //Assert
            result.Port.Should().BeNull();
        }

        [TestMethod]
        public void WhenValueIsNotNull_SetToValue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            var result = new Url { Port = value };

            //Assert
            result.Port.Should().Be(value);
        }
    }

    [TestClass]
    public class Fragment : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenIsNullOrEmpty_SetToEmpty(string value)
        {
            //Arrange

            //Act
            var result = new Url { Fragment = value };

            //Assert
            result.Fragment.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsHashtag_TrimItOut()
        {
            //Arrange
            var valueWithoutHashtag = Fixture.Create<string>();
            var value = $"#{valueWithoutHashtag}";

            //Act
            var result = new Url { Fragment = value };

            //Assert
            result.Fragment.Should().Be(valueWithoutHashtag);
        }

        [TestMethod]
        public void WhenContainsNoHashtag_SetAsIs()
        {
            //Arrange
            var value = Fixture.Create<string>();

            //Act
            var result = new Url { Fragment = value };

            //Assert
            result.Fragment.Should().Be(value);
        }
    }

    [TestClass]
    public class Path : Tester
    {
        [TestMethod]
        public void WhenValueIsNull_SetToEmpty()
        {
            //Arrange
            IReadOnlyList<string> value = null!;

            //Act
            var result = new Url { Path = value };

            //Assert
            result.Path.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenValueIsNotNull_SetToValue()
        {
            //Arrange
            var value = Fixture.CreateMany<string>().ToList();

            //Act
            var result = new Url { Path = value };

            //Assert
            result.Path.Should().BeEquivalentTo(value);
        }

        [TestMethod]
        public void WhenOriginalPathCollectionIsModified_DoNotModifyUrl()
        {
            //Arrange
            var value = Fixture.CreateMany<string>().ToList();
            var original = value.ToList();

            //Act
            var result = new Url { Path = value };

            //Assert
            value.Add(Fixture.Create<string>());
            result.Path.Should().BeEquivalentTo(original);
        }
    }

    [TestClass]
    public class Parameters : Tester
    {
        [TestMethod]
        public void WhenValueIsNull_SetToEmpty()
        {
            //Arrange
            UrlParameterList value = null!;

            //Act
            var result = new Url { Parameters = value };

            //Assert
            result.Parameters.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenValueIsNotNull_SetToValue()
        {
            //Arrange
            var value = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            //Act
            var result = new Url { Parameters = value };

            //Assert
            result.Parameters.Should().BeEquivalentTo(value);
        }
    }

    [TestClass]
    public class WithParameter : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange
            var instance = Fixture.Create<Url>();

            var value = Fixture.Create<object>();

            //Act
            var action = () => instance.WithParameter(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Name));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenValueIsNullOrEmpty_Throw(string value)
        {
            //Arrange
            var instance = Fixture.Create<Url>();

            var name = Fixture.Create<string>();

            //Act
            var action = () => instance.WithParameter(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
        }

        [TestMethod]
        public void WhenParameterWithNameAlreadyExists_Throw()
        {
            //Arrange
            var existingParameter = Fixture.Create<UrlParameter>();

            var instance = Fixture.Create<Url>() with { Parameters = new UrlParameterList(existingParameter) };

            //Act
            var action = () => instance.WithParameter(existingParameter.Name, Fixture.Create<object>());

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingParameter.Name));
        }

        [TestMethod]
        public void WhenNameIsUnique_AddParameter()
        {
            //Arrange
            var instance = Fixture.Create<Url>();

            var parameter = Fixture.Create<UrlParameter>();

            //Act
            var result = instance.WithParameter(parameter.Name, parameter.Value);

            //Assert
            result.Parameters.Should().BeEquivalentTo(instance.Parameters.Concat(new[] { parameter }));
        }

        [TestMethod]
        public void WhenNameIsUnique_DoNotModifyOriginalInstance()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var original = instance with { };

            var parameter = Fixture.Create<UrlParameter>();

            //Act
            instance.WithParameter(parameter.Name, parameter.Value);

            //Assert
            instance.Should().Be(original);
        }
    }

    [TestClass]
    public class WithParameters_Params : Tester
    {
        [TestMethod]
        public void WhenOneParameterIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToArray();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
        }

        [TestMethod]
        public void WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var existingItem = instance.Parameters.GetRandom();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToArray();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
        }

        [TestMethod]
        public void WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var existingItems = new List<UrlParameter> { instance.Parameters.First(), instance.Parameters.Last() };
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(existingItems).ToArray();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
        }

        [TestMethod]
        public void WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var parameters = Fixture.CreateMany<UrlParameter>().ToArray();

            //Act
            var result = instance.WithParameters(parameters);

            //Assert
            result.Parameters.Should().BeEquivalentTo(instance.Parameters.Concat(parameters));
        }

        [TestMethod]
        public void WhenAddingNewParameters_DoNotModifyOriginalCollection()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var original = instance with { };
            var parameters = Fixture.CreateMany<UrlParameter>().ToArray();

            //Act
            instance.WithParameters(parameters);

            //Assert
            instance.Should().BeEquivalentTo(original);
        }
    }

    [TestClass]
    public class WithParameters_Enumerable : Tester
    {
        [TestMethod]
        public void WhenParametersIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            IEnumerable<UrlParameter> parameters = null!;

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
        }

        [TestMethod]
        public void WhenOneParameterIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToList();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
        }

        [TestMethod]
        public void WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var existingItem = instance.Parameters.GetRandom();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToList();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
        }

        [TestMethod]
        public void WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var existingItems = new List<UrlParameter> { instance.Parameters.First(), instance.Parameters.Last() };
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(existingItems).ToList();

            //Act
            var action = () => instance.WithParameters(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
        }

        [TestMethod]
        public void WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var parameters = Fixture.CreateMany<UrlParameter>().ToList();

            //Act
            var result = instance.WithParameters(parameters);

            //Assert
            result.Parameters.Should().BeEquivalentTo(instance.Parameters.Concat(parameters));
        }

        [TestMethod]
        public void WhenAddingNewParameters_DoNotModifyOriginalCollection()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var original = instance with { };
            var parameters = Fixture.CreateMany<UrlParameter>().ToList();

            //Act
            instance.WithParameters(parameters);

            //Assert
            instance.Should().BeEquivalentTo(original);
        }
    }

    [TestClass]
    public class AppendPath_Params : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenContainsOneNullOrEmptySegment_Throw(string segment)
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = Fixture.CreateMany<string>().Concat(new[] { segment }).ToArray();

            //Act
            var action = () => instance.AppendPath(path);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingEmptyPathSegment);
        }

        [TestMethod]
        public void WhenPathIsEmpty_ReturnUnmodifiedUrl()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = Array.Empty<string>();

            //Act
            var result = instance.AppendPath(path);

            //Assert
            result.Should().Be(instance);
        }

        [TestMethod]
        public void WhenPathIsNotEmpty_AppendToPath()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = Fixture.CreateMany<string>().ToArray();

            //Act
            var result = instance.AppendPath(path);

            //Assert
            result.Path.Should().BeEquivalentTo(instance.Path.Concat(path));
        }

    }

    [TestClass]
    public class AppendPath_Enumerable : Tester
    {
        [TestMethod]
        public void WhenPathIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            IEnumerable<string> path = null!;

            //Act
            var action = () => instance.AppendPath(path);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(path));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenContainsOneNullOrEmptySegment_Throw(string segment)
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = Fixture.CreateMany<string>().Concat(new[] { segment }).ToReadOnlyList();

            //Act
            var action = () => instance.AppendPath(path);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingEmptyPathSegment);
        }

        [TestMethod]
        public void WhenPathIsEmpty_ReturnUnmodifiedUrl()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = new List<string>();

            //Act
            var result = instance.AppendPath(path);

            //Assert
            result.Should().Be(instance);
        }

        [TestMethod]
        public void WhenPathIsNotEmpty_AppendToPath()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var path = Fixture.CreateMany<string>().ToReadOnlyList();

            //Act
            var result = instance.AppendPath(path);

            //Assert
            result.Path.Should().BeEquivalentTo(instance.Path.Concat(path));
        }
    }

    [TestClass]
    public class EqualsMethod : Tester
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            Url other = null!;

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance;

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSchemeIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with { Scheme = Fixture.Create<string>() };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenUserInfoIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with { UserInfo = Fixture.Create<UserInfo>() };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenHostIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with { Host = Fixture.Create<string>() };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPortIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with { Port = Fixture.Create<int>() };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenFragmentIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with { Fragment = Fixture.Create<string>() };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenParametersAreDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance.WithParameters(Fixture.Create<UrlParameter>());

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenPathIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance.AppendPath(Fixture.Create<string>());

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenReferencesAreDifferentButValuesAreTheSame_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.Create<Url>();
            var other = instance with
            {
                Parameters = instance.Parameters.ToUrlParameterList(),
                Path = instance.Path.ToReadOnlyList(),
            };

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmpty()
        {
            //Arrange
            var instance = new Url();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyContainsScheme_OnlyReturnScheme()
        {
            //Arrange
            var instance = new Url { Scheme = "https" };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://");
        }

        [TestMethod]
        public void WhenOnlyContainsUserInfo_OnlyReturnUserInfo()
        {
            //Arrange
            var instance = new Url { UserInfo = Fixture.Create<UserInfo>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{instance.UserInfo}@");
        }

        [TestMethod]
        public void WhenOnlyContainsHost_OnlyReturnHost()
        {
            //Arrange
            var instance = new Url { Host = Fixture.Create<string>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{instance.Host}/");
        }

        [TestMethod]
        public void WhenOnlyContainsPort_OnlyReturnEmpty()
        {
            //Arrange
            var instance = new Url { Port = Fixture.Create<int>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyContainsPath_OnlyReturnPath()
        {
            //Arrange
            var instance = new Url { Path = Fixture.CreateMany<string>().ToReadOnlyList() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be(string.Join('/', instance.Path));
        }

        [TestMethod]
        public void WhenOnlyContainsParameters_OnlyReturnParameters()
        {
            //Arrange
            var instance = new Url { Parameters = Fixture.Create<UrlParameterList>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be(instance.Parameters.ToString());
        }

        [TestMethod]
        public void WhenOnlyContainsFragment_OnlyReturnFragment()
        {
            //Arrange
            var instance = new Url { Fragment = Fixture.Create<string>() };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be(instance.Fragment);
        }

        [TestMethod]
        public void WhenOnlyContainsRootParts_OnlyReturnRoot()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                UserInfo = new UserInfo("seb", "engineer"),
                Host = "www.sayabec.ing",
                Port = 8080
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://seb:engineer@www.sayabec.ing:8080/");
        }

        [TestMethod]
        public void WhenOnlyContainsSchemeAndHost_OnlyReturnThose()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                Host = "www.sayabec.ing",
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://www.sayabec.ing/");
        }

        [TestMethod]
        public void WhenOnlyContainsRootAndPath_Concatenate()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                UserInfo = new UserInfo("seb", "engineer"),
                Host = "www.sayabec.ing",
                Port = 8080,
                Path = new[] { "api/v1", "place", "alexandra" }.ToReadOnlyList(),
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://seb:engineer@www.sayabec.ing:8080/api/v1/place/alexandra");
        }

        [TestMethod]
        public void WhenYouAreSociopathWhoAddsExtraSlashesInPaths_IgnoreExtraSlashes()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                UserInfo = new UserInfo("seb", "engineer"),
                Host = "www.sayabec.ing",
                Port = 8080,
                Path = new[] { "/api/v1/", "/place/", "/alexandra/" }.ToReadOnlyList(),
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://seb:engineer@www.sayabec.ing:8080/api/v1/place/alexandra");
        }

        [TestMethod]
        public void WhenHasParametersAfterPath_Concatenate()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                UserInfo = new UserInfo("seb", "engineer"),
                Host = "www.sayabec.ing",
                Port = 8080,
                Path = new[] { "api/v1", "place", "alexandra" }.ToReadOnlyList(),
                Parameters = new UrlParameterList(new UrlParameter("age", 90), new UrlParameter("relationshipstatus", "single"))
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://seb:engineer@www.sayabec.ing:8080/api/v1/place/alexandra?age=90&relationshipstatus=single");
        }

        [TestMethod]
        public void WhenIsFullPath_Concatenate()
        {
            //Arrange
            var instance = new Url
            {
                Scheme = "https",
                UserInfo = new UserInfo("seb", "engineer"),
                Host = "www.sayabec.ing",
                Port = 8080,
                Path = new[] { "api/v1", "place", "alexandra" }.ToReadOnlyList(),
                Parameters = new UrlParameterList(new UrlParameter("age", 90), new UrlParameter("relationshipstatus", "single")),
                Fragment = "gertrude"
            };

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("https://seb:engineer@www.sayabec.ing:8080/api/v1/place/alexandra?age=90&relationshipstatus=single#gertrude");
        }
    }
}