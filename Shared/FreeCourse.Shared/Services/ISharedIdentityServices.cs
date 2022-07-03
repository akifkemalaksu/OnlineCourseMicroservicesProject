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
}