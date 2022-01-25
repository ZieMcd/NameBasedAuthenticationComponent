using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NameBasedAuthorizeViewComponent.Interfaces;

namespace NameBasedAuthorizeViewComponent.Components;

public class NameBasedAuthorizeView : AuthorizeViewCore
{
    private readonly IAuthorizeData[] selfAsAuthorizeData;
    [Parameter] public string? ComponentAuthName { get; set; }

    public NameBasedAuthorizeView(INameBasedAuthorizationHelper nameBasedAuthorizationHelper, NavigationManager navigationManager)
    {
        selfAsAuthorizeData = new[] { new NameBasedAuthorizeDataAdapter(nameBasedAuthorizationHelper, navigationManager, this) };
    }
    
    protected override IAuthorizeData[] GetAuthorizeData()
        => selfAsAuthorizeData;
}