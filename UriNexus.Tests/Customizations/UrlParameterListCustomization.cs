namespace UriNexus.Tests.Customizations;

[AutoCustomization]
public class UrlParameterListCustomization : CustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(UrlParameterList)];

    protected override IDummyBuilder BuildMe(IDummy dummy, Type type) => dummy.Build<UrlParameterList>().FromFactory(() => dummy.CreateMany<UrlParameter>().ToUrlParameterList());
}