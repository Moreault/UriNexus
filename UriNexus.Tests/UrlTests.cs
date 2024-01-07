namespace UriNexus.Tests;

[TestClass]
public class UrlTests : RecordTester<Url>
{
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Scheme_WhenValueIsNullOrEmpty_SetToEmpty(string value)
    {
        //Arrange

        //Act
        var result = new Url { Scheme = value };

        //Assert
        result.Scheme.Should().BeEmpty();
    }

    [TestMethod]
    public void Scheme_WhenValueIsHttpWithTwoDotsAndStuff_SetToHttp()
    {
        //Arrange
        var value = "http://";

        //Act
        var result = new Url { Scheme = value };

        //Assert
        result.Scheme.Should().Be("http");
    }

    [TestMethod]
    public void Scheme_WhenValueIsHttp_SetToHttp()
    {
        //Arrange
        var value = "http";

        //Act
        var result = new Url { Scheme = value };

        //Assert
        result.Scheme.Should().Be("http");
    }

    [TestMethod]
    public void UserInfo_WhenValueIsNull_SetToNull()
    {
        //Arrange
        UserInfo value = null!;

        //Act
        var result = new Url { UserInfo = value };

        //Assert
        result.UserInfo.Should().BeNull();
    }

    [TestMethod]
    public void UserInfo_WhenValueIsNotNull_SetToValue()
    {
        //Arrange
        var value = Fixture.Create<UserInfo>();

        //Act
        var result = new Url { UserInfo = value };

        //Assert
        result.UserInfo.Should().Be(value);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Host_WhenIsNullOrEmpty_SetToEmpty(string value)
    {
        //Arrange

        //Act
        var result = new Url { Host = value };

        //Assert
        result.Host.Should().BeEmpty();
    }

    [TestMethod]
    public void Host_WhenHasSlashesBeforeAndAfter_TrimSlashes()
    {
        //Arrange
        var value = "//www.sayabec.ing/";

        //Act
        var result = new Url { Host = value };

        //Assert
        result.Host.Should().Be("www.sayabec.ing");
    }

    [TestMethod]
    public void Host_WhenHasNoSlash_SetValueAsIs()
    {
        //Arrange
        var value = "www.sayabec.ing";

        //Act
        var result = new Url { Host = value };

        //Assert
        result.Host.Should().Be("www.sayabec.ing");
    }

    [TestMethod]
    public void Port_WhenValueIsNull_SetToNull()
    {
        //Arrange
        int? value = null!;

        //Act
        var result = new Url { Port = value };

        //Assert
        result.Port.Should().BeNull();
    }

    [TestMethod]
    public void Port_WhenValueIsNotNull_SetToValue()
    {
        //Arrange
        var value = Fixture.Create<int>();

        //Act
        var result = new Url { Port = value };

        //Assert
        result.Port.Should().Be(value);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Fragment_WhenIsNullOrEmpty_SetToEmpty(string value)
    {
        //Arrange

        //Act
        var result = new Url { Fragment = value };

        //Assert
        result.Fragment.Should().BeEmpty();
    }

    [TestMethod]
    public void Fragment_WhenContainsHashtag_TrimItOut()
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
    public void Fragment_WhenContainsNoHashtag_SetAsIs()
    {
        //Arrange
        var value = Fixture.Create<string>();

        //Act
        var result = new Url { Fragment = value };

        //Assert
        result.Fragment.Should().Be(value);
    }

    [TestMethod]
    public void Path_WhenValueIsNull_SetToEmpty()
    {
        //Arrange
        IReadOnlyList<string> value = null!;

        //Act
        var result = new Url { Path = value };

        //Assert
        result.Path.Should().BeEmpty();
    }

    [TestMethod]
    public void Path_WhenValueIsNotNull_SetToValue()
    {
        //Arrange
        var value = Fixture.CreateMany<string>().ToList();

        //Act
        var result = new Url { Path = value };

        //Assert
        result.Path.Should().BeEquivalentTo(value);
    }

    [TestMethod]
    public void Path_WhenOriginalPathCollectionIsModified_DoNotModifyUrl()
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

    [TestMethod]
    public void Parameters_WhenValueIsNull_SetToEmpty()
    {
        //Arrange
        UrlParameterList value = null!;

        //Act
        var result = new Url { Parameters = value };

        //Assert
        result.Parameters.Should().BeEmpty();
    }

    [TestMethod]
    public void Parameters_WhenValueIsNotNull_SetToValue()
    {
        //Arrange
        var value = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

        //Act
        var result = new Url { Parameters = value };

        //Assert
        result.Parameters.Should().BeEquivalentTo(value);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void WithParameter_WhenNameIsNullOrEmpty_Throw(string name)
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
    public void WithParameter_WhenValueIsNullOrEmpty_Throw(string value)
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
    public void WithParameter_WhenParameterWithNameAlreadyExists_Throw()
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
    public void WithParameter_WhenNameIsUnique_AddParameter()
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
    public void WithParameter_WhenNameIsUnique_DoNotModifyOriginalInstance()
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

    [TestMethod]
    public void WithParametersParams_WhenOneParameterIsNull_Throw()
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
    public void WithParametersParams_WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
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
    public void WithParametersParams_WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
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
    public void WithParametersParams_WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
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
    public void WithParametersParams_WhenAddingNewParameters_DoNotModifyOriginalCollection()
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

    [TestMethod]
    public void WithParametersEnumerable_WhenParametersIsNull_Throw()
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
    public void WithParametersEnumerable_WhenOneParameterIsNull_Throw()
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
    public void WithParametersEnumerable_WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
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
    public void WithParametersEnumerable_WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
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
    public void WithParametersEnumerable_WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
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
    public void WithParametersEnumerable_WhenAddingNewParameters_DoNotModifyOriginalCollection()
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

    [TestClass]
    public class AppendPath_Params : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void AppendPathParams_WhenContainsOneNullOrEmptySegment_Throw(string segment)
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
        public void AppendPathParams_WhenPathIsEmpty_ReturnUnmodifiedUrl()
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
        public void AppendPathParams_WhenPathIsNotEmpty_AppendToPath()
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

    [TestMethod]
    public void AppendPathEnumerable_WhenPathIsNull_Throw()
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
    public void AppendPathEnumerable_WhenContainsOneNullOrEmptySegment_Throw(string segment)
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
    public void AppendPathEnumerable_WhenPathIsEmpty_ReturnUnmodifiedUrl()
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
    public void AppendPathEnumerable_WhenPathIsNotEmpty_AppendToPath()
    {
        //Arrange
        var instance = Fixture.Create<Url>();
        var path = Fixture.CreateMany<string>().ToReadOnlyList();

        //Act
        var result = instance.AppendPath(path);

        //Assert
        result.Path.Should().BeEquivalentTo(instance.Path.Concat(path));
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var instance = new Url();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsScheme_OnlyReturnScheme()
    {
        //Arrange
        var instance = new Url { Scheme = "https" };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("https://");
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsUserInfo_OnlyReturnUserInfo()
    {
        //Arrange
        var instance = new Url { UserInfo = Fixture.Create<UserInfo>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.UserInfo}@");
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsHost_OnlyReturnHost()
    {
        //Arrange
        var instance = new Url { Host = Fixture.Create<string>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Host}/");
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsPort_OnlyReturnEmpty()
    {
        //Arrange
        var instance = new Url { Port = Fixture.Create<int>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsPath_OnlyReturnPath()
    {
        //Arrange
        var instance = new Url { Path = Fixture.CreateMany<string>().ToReadOnlyList() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be(string.Join('/', instance.Path));
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsParameters_OnlyReturnParameters()
    {
        //Arrange
        var instance = new Url { Parameters = Fixture.Create<UrlParameterList>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be(instance.Parameters.ToString());
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsFragment_OnlyReturnFragment()
    {
        //Arrange
        var instance = new Url { Fragment = Fixture.Create<string>() };

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be(instance.Fragment);
    }

    [TestMethod]
    public void ToString_WhenOnlyContainsRootParts_OnlyReturnRoot()
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
    public void ToString_WhenOnlyContainsSchemeAndHost_OnlyReturnThose()
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
    public void ToString_WhenOnlyContainsRootAndPath_Concatenate()
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
    public void ToString_WhenYouAreSociopathWhoAddsExtraSlashesInPaths_IgnoreExtraSlashes()
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
    public void ToString_WhenHasParametersAfterPath_Concatenate()
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
    public void ToString_WhenIsFullPath_Concatenate()
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

    [TestMethod]
    public void ImplicitStringConverter_WhenUrlIsNull_ReturnEmptyString()
    {
        //Arrange
        Url instance = null!;

        //Act
        string result = instance;

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ImplicitStringConverter_WhenUrlIsNotNull_UseToString()
    {
        //Arrange
        var instance = Fixture.Create<Url>();

        //Act
        string result = instance;

        //Assert
        result.Should().Be(instance.ToString());
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<Url>(Fixture);
}