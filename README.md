# NameBasedAuthorizeViewComponent
NameBasedAuthorizeView is an alternative to Microsoft's AuthorizeView. It allows for even more control over what roles can access a component.

### Before We begin
I assume that you already have a basic understanding of authentication and authorization in dotnet and blazor when using this nuget if not I recommend reading the [microsoft docs](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0\).
I also recommend reading the documentation on Microsoft's [AuthorizeView component](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#authorizeview-component) which this component is based off of.  

If you dont know what Authentication provider to use I recommend [okta](https://developer.okta.com/). \
[Here is good](https://www.youtube.com/watch?v=GilJ29cPJAs&ab_channel=OktaDev) video is good for showing how to set up okta in blazor project. \
Around 13 mins into [this](https://youtu.be/Cej_u3fb9rI?t=783) video it shows you how to get role claim from okta

## Installation

PMC:

     Install-Package Ziemcd.Authorization.NameBasedAuthorizeViewComponent 

.NET CLI:

    dotnet add package iemcd.Authorization.NameBasedAuthorizeViewComponent

## Usage


1. The first thing you need to do is add AddAuthentication to your project you do this in program.cs

        builder.Services.AddAuthentication(
        //You need set up your Authentication provider here
        );
In order to fully use NameBasedAuthorize component you should have roles claim as part of your authentication provider \
right note on cascading on app.razor\

2. You also need to add NameBasedAuthorizeViewComponent.Components to _Imports.cs

        @using NameBasedAuthorizeViewComponent.Components

3. Next thing you want to do is implement instance INameBasedAuthorizationHelper\
INameBasedAuthorizationHelper.cs

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
    Here is my Example

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
In my example I create a fake table and I query the name of a component against that fake table

4. Now you need Add NameBasedAuthorizeComponent at startup passing in the instance of INameBaseAuthorizationHelper you created

       builder.Services.AddNameBasedAuthorizeComponent<NameBasedAuthorizationHelper>();

5. After all these steps your ready to use NameBasedAuthorizeViewComponent, Use it like so

        <NameBasedAuthorizeView ComponentAuthName="AdminComponent">
           <Authorized>
            <div class="card text-white bg-success  mb-3" style="max-width: 18rem;">
               <div class="card-header">Authorized</div>
               <div class="card-body">
                  <p class="card-text">You are Authorized to view Admin Component</p>
               </div>
          </div>
          </Authorized>
          <NotAuthorized>
          <div class="card text-white bg-danger  mb-3" style="max-width: 18rem;">
             <div class="card-header">UnAuthorized</div>
                <div class="card-body">
                   <p class="card-text">You are Not Authorized to view Admin Component</p>
                </div>
             </div>
          </NotAuthorized>
        </NameBasedAuthorizeView>
    Because of our implementation in step 3 only users with the admin will see whats in the &lt;Authorized&gt; section if the user does not have admin role they will see whats in  &lt;/NotAuthorized>

### Help and improvements 
If you need feel free to email me at [ziemcd@gmail.com](mailto:ziemcd@gmail.com)
If you think you can improve the nuget you can fork or create pull request
    