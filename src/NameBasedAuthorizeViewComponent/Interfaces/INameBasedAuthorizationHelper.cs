namespace NameBasedAuthorizeViewComponent.Interfaces;

/// <summary>
/// This class is used by the NameBasedAuthorizeView component 
/// This class has two methods, GetRolesForComponent and GetRolesFromRoute
/// both should return a comma delimited list of roles that are allowed to access the component.
/// If a component name has been passed to the NameBasedAuthorizeView e.g. <NameBasedAuthorizeView ComponentAuthName="AdminComponent"> then
/// the GetRolesForComponent(string componentName) will be called if no parameter is passed in e.g. just <NameBasedAuthorizeView> then
///  GetRolesFromRoute(string componentRoute) will called, componentRoute will be what ever the route of the current component is 
/// </summary>
public interface INameBasedAuthorizationHelper
{
    /// <summary>
    ///  when you the NameBasedAuthorizeView component and pass the ComponentAuthName to it this
    /// methode will be used to determine what roles should have access to that component   
    /// </summary>
    /// <param name="componentName">
    /// The parameter will be equal to the ComponentAuthName passed into NameBasedAuthorizeView
    /// <NameBasedAuthorizeView ComponentAuthName="AdminComponent"> componentName = "AdminComponent"
    /// </param>
    /// <returns>comma delimited list of roles that are allowed to access the component e.g. "Admin","CEO","Everyone"</returns>
    string GetRolesForComponent(string componentName);
    
    /// <summary>
    ///  This is a fallback method when ComponentAuthName is not passed to NameBasedAuthorizeView thus
    ///  the route of the component is passed to method e.g. https://localhost:7082/counter
    ///  When Implementing this I recommend injecting Microsoft.AspNetCore.Components.NavigationManger
    /// and using .ToBaseRelativePath(String) to get the relative pass
    /// </summary>
    /// <param name="componentRoute"></param>
    /// <returns>comma delimited list of roles that are allowed to access the component e.g. "Admin","CEO","Everyone"</returns>
    
    string GetRolesFromRoute(string componentRoute);
}