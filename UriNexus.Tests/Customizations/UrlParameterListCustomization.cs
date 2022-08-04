namespace UriNexus.Tests.Customizations;

[AutoCustomization]
public class UrlParameterListCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<UrlParameterList>(x => x.FromFactory(() => fixture.CreateMany<UrlParameter>().ToUrlParameterList()));
    }
}