using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FreeCourse.IdentityServer.Services.Interfaces
{
    public interface IPersistedGrantService
    {
        PersistedGrant GetPersistedGrant(Func<PersistedGrant, bool> expression);
        List<PersistedGrant> GetPersistedGrantList(Func<PersistedGrant, bool> expression);
        void Add(PersistedGrant item);
        void Update(PersistedGrant item);
        void Remove(PersistedGrant item);
        void Remove(List<PersistedGrant> items);
    }
}
