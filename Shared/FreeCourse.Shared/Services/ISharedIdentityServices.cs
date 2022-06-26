using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCourse.Shared.Services
{
    public interface ISharedIdentityServices
    {
        public string GetUserId { get; }
    }

    public class SharedIdentityServices : ISharedIdentityServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}