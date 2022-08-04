namespace UriNexus.Tests.Collections;

[TestClass]
public class UrlParameterListTester
{
    [TestClass]
    public class Count : Tester
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange
            var instance = new UrlParameterList();

            //Act
            var result = instance.Count;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenIsNotEmpty_ReturnNumberOfItems()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>(4).ToUrlParameterList();

            //Act
            var result = instance.Count;

            //Assert
            result.Should().Be(4);
        }
    }

    [TestClass]
    public class Indexer : Tester
    {
        [TestMethod]
        public void WhenThereIsNoParameterWithName_ReturnEmpty()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            //Act
            var result = instance[Fixture.Create<string>()];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneParameterWithNameButDifferentCasing_ReturnEmpty()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            //Act
            var result = instance[instance.GetRandom().Name.ToLowerInvariant()];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneParameterWithSameNameAndCasing_ReturnItsValue()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameter = instance.GetRandom();

            //Act
            var result = instance[parameter.Name];

            //Assert
            result.Should().Be(parameter.Value.ToString());
        }
    }

    [TestClass]
    public class Constructors : Tester
    {
        [TestMethod]
        public void WhenUsingDefault_InstantiateEmpty()
        {
            //Arrange

            //Act
            var result = new UrlParameterList();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenUsingParams_InstantiateWithParameters()
        {
            //Arrange
            var parameter1 = Fixture.Create<UrlParameter>();
            var parameter2 = Fixture.Create<UrlParameter>();
            var parameter3 = Fixture.Create<UrlParameter>();

            //Act
            var result = new UrlParameterList(parameter1, parameter2, parameter3);

            //Assert
            result.Should().BeEquivalentTo(new List<UrlParameter> { parameter1, parameter2, parameter3 });
        }

        [TestMethod]
        public void WhenUsingNullEnumerable_Throw()
        {
            //Arrange
            IEnumerable<UrlParameter> parameters = null!;

            //Act
            var action = () => new UrlParameterList(parameters);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
        }

        [TestMethod]
        public void WhenUsingEnumerable_InstantiateWithContents()
        {
            //Arrange
            var parameters = Fixture.CreateMany<UrlParameter>().ToList();

            //Act
            var result = new UrlParameterList(parameters);

            //Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }

    [TestClass]
    public class With_Name_Value : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            var value = Fixture.Create<object>();

            //Act
            var action = () => instance.With(name, value);

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
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            var name = Fixture.Create<string>();

            //Act
            var action = () => instance.With(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
        }

        [TestMethod]
        public void WhenAddingNameThatAlreadyExists_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            var name = instance.GetRandom().Name;
            var value = Fixture.Create<object>();

            //Act
            var action = () => instance.With(name, value);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, name));
        }

        [TestMethod]
        public void WhenAddingNewName_ReturnNewCollectionWithNameAndValue()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameter = Fixture.Create<UrlParameter>();

            //Act
            var result = instance.With(parameter.Name, parameter.Value);

            //Assert
            result.Should().BeEquivalentTo(instance.Concat(new[] { parameter }));
        }

        [TestMethod]
        public void WhenAddingNewName_DoNotModifyOriginalList()
        {
            //Arrange
            var originalInstance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var instance = originalInstance.ToUrlParameterList();
            var parameter = Fixture.Create<UrlParameter>();

            //Act
            instance.With(parameter.Name, parameter.Value);

            //Assert
            instance.Should().BeEquivalentTo(originalInstance);
        }
    }

    [TestClass]
    public class With_Params : Tester
    {
        [TestMethod]
        public void WhenOneParameterIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToArray();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
        }

        [TestMethod]
        public void WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var existingItem = instance.GetRandom();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToArray();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
        }

        [TestMethod]
        public void WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var existingItems = new List<UrlParameter> { instance.First(), instance.Last() };
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(existingItems).ToArray();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
        }

        [TestMethod]
        public void WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().ToArray();

            //Act
            var result = instance.With(parameters);

            //Assert
            result.Should().BeEquivalentTo(instance.Concat(parameters));
        }

        [TestMethod]
        public void WhenAddingNewParameters_DoNotModifyOriginalCollection()
        {
            //Arrange
            var originalInstance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var instance = originalInstance.ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().ToArray();

            //Act
            instance.With(parameters);

            //Assert
            instance.Should().BeEquivalentTo(originalInstance);
        }
    }

    [TestClass]
    public class With_Enumerable : Tester
    {
        [TestMethod]
        public void WhenParametersIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            IEnumerable<UrlParameter> parameters = null!;

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
        }

        [TestMethod]
        public void WhenOneParameterIsNull_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToList();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
        }

        [TestMethod]
        public void WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var existingItem = instance.GetRandom();
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToList();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
        }

        [TestMethod]
        public void WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var existingItems = new List<UrlParameter> { instance.First(), instance.Last() };
            var parameters = Fixture.CreateMany<UrlParameter>().Concat(existingItems).ToList();

            //Act
            var action = () => instance.With(parameters);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
        }

        [TestMethod]
        public void WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().ToList();

            //Act
            var result = instance.With(parameters);

            //Assert
            result.Should().BeEquivalentTo(instance.Concat(parameters));
        }

        [TestMethod]
        public void WhenAddingNewParameters_DoNotModifyOriginalCollection()
        {
            //Arrange
            var originalInstance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var instance = originalInstance.ToUrlParameterList();
            var parameters = Fixture.CreateMany<UrlParameter>().ToList();

            //Act
            instance.With(parameters);

            //Assert
            instance.Should().BeEquivalentTo(originalInstance);
        }
    }

    [TestClass]
    public class Without : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNameIsNullOrEmpty_Throw(string name)
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            //Act
            var action = () => instance.Without(name);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
        }

        [TestMethod]
        public void WhenThereIsNoItemWithThatNameInCollection_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();

            var name = Fixture.Create<string>();

            //Act
            var action = () => instance.Without(name);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.RemovingNonExistentParameter, name));
        }

        [TestMethod]
        public void WhenThereIsAnItemWithThatName_RemoveIt()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var item = instance.GetRandom();

            //Act
            var result = instance.Without(item.Name);

            //Assert
            result.Should().BeEquivalentTo(instance.Where(x => x != item));
        }

        [TestMethod]
        public void WhenThereIsAnItemWithThatName_DoNotModifyOriginalInstance()
        {
            //Arrange
            var originalInstance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var instance = originalInstance.ToUrlParameterList();
            var item = instance.GetRandom();

            //Act
            var result = instance.Without(item.Name);

            //Assert
            instance.Should().BeEquivalentTo(originalInstance);
        }
    }

    [TestClass]
    public class EqualsMethod : Tester
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            IEnumerable<UrlParameter> other = null!;

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsTheSameReference_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var other = instance;

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsParameterListWithDifferentContent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var other = instance.Concat(Fixture.CreateMany<UrlParameter>()).ToUrlParameterList();

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsEnumerableWithDifferentContent_ReturnFalse()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var other = instance.Concat(Fixture.CreateMany<UrlParameter>());

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsParameterListWithSameContent_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var other = instance.ToUrlParameterList();

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsEnumerableWithSameContent_ReturnTrue()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>().ToUrlParameterList();
            var other = instance.ToArray();

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
            var instance = new UrlParameterList();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenHasOnlyOneItem_ReturnSingleItemSeparatedByEqualsSign()
        {
            //Arrange
            var instance = new UrlParameterList(Fixture.Create<UrlParameter>());

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{instance.Single().Name}={instance.Single().Value}");
        }

        [TestMethod]
        public void WhenHasMultipleItems_FormatAllItemsSeparatedByAmpersand()
        {
            //Arrange
            var instance = Fixture.CreateMany<UrlParameter>(3).ToUrlParameterList();
            var items = instance.ToList();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{items[0].Name}={items[0].Value}&{items[1].Name}={items[1].Value}&{items[2].Name}={items[2].Value}");
        }
    }
}