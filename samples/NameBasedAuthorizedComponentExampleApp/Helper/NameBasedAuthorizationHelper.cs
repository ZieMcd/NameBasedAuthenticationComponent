using Microsoft.AspNetCore.Components;
using NameBasedAuthorizeViewComponent.Interfaces;

namespace NameBasedAuthorizedComponentExampleApp.Helper;

public class NameBasedAuthorizationHelper : INameBasedAuthorizationHelper
{
    private readonly List<NavRoleItem> _fakeRoleNavTable;
    private readonly NavigationManager _navigationManager;
    public NameBasedAuthorizationHelper(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _fakeRoleNavTable = new List<NavRoleItem>
        {
            new("Admin", "AdminComponent"),
            new("Everyone", "EveryoneComponent"),
        };
    }

    //Roles have have to be in a comma seperated string to work properly with AuthorizeViewCore
    public string GetRolesForComponent(string componentName)
    {
        var roles = _fakeRoleNavTable.Where(item => item.UrlOrName.Equals(componentName)).Select(item => item.Role);
        return string.Join(", ", roles);
    }

    public string GetRolesFromRoute(string componentRoute)
    {
        var relativeUrl = _navigationManager.ToBaseRelativePath(componentRoute);
        var roles = _fakeRoleNavTable.Where(item => item.UrlOrName.Equals(relativeUrl)).Select(item => item.Role);
        if (!roles.Any())
            return null;
        return string.Join(", ", roles);
    }
}

public readonly record struct NavRoleItem(string Role, string UrlOrName);