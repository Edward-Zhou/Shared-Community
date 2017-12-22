using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SharedCommunity.Authorization;
using SharedCommunity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharedCommunity.XunitTest
{
    public class AuthorizeCoreAttributeTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthorizeCoreAttributeTests(ServiceProviderFixture serviceProviderFixture)
        {
            _serviceProviderFixture = serviceProviderFixture;
            _serviceProvider = serviceProviderFixture.ServiceProvider;
            _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        [Fact]
        public async void Anonymous()
        {
            //ActionContext actionContext = new ActionContext(,)
            //AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, filters);
            //var filter = new AuthorizeCoreRequirementFilter(new Author(), _userManager);
            //filter.OnAuthorization(filterContext);
        }
    }
}
