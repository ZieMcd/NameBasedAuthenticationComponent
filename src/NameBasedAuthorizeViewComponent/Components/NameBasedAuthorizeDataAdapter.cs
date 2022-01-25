using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using NameBasedAuthorizeViewComponent.Interfaces;

namespace NameBasedAuthorizeViewComponent.Components;

public class NameBasedAuthorizeDataAdapter : IAuthorizeData
{
    private readonly INameBasedAuthorizationHelper _nameBasedAuthorizationHelper;
    private readonly NavigationManager _navigationManager;
    private readonly NameBasedAuthorizeView _component;

    public NameBasedAuthorizeDataAdapter(INameBasedAuthorizationHelper nameBasedAuthorizationHelper, NavigationManager navigationManager, NameBasedAuthorizeView component)
    {
        _nameBasedAuthorizationHelper = nameBasedAuthorizationHelper;
        _navigationManager = navigationManager;
        _component = component;
    }
    
    public string? Roles
    {
        get => GetRoles();
        set => throw new NotSupportedException();
    }

    public string? Policy { get; set; }

    public string? AuthenticationSchemes { get; set; }
    
    private string GetRoles()
    {
        return _component.ComponentAuthName != null
            ? _nameBasedAuthorizationHelper.GetRolesForComponent(_component.ComponentAuthName)
            : _nameBasedAuthorizationHelper.GetRolesFromRoute(_navigationManager.Uri);
    }
}