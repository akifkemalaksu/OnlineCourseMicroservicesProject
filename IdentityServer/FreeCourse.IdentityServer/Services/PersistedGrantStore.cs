using FreeCourse.IdentityServer.Services.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly ILogger<PersistedGrantStore> _logger;
        private readonly IPersistedGrantService _persistedGrantService;

        public PersistedGrantStore(ILogger<PersistedGrantStore> logger, IPersistedGrantService persistedGrantService)
        {
            _logger = logger;
            _persistedGrantService = persistedGrantService;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var persistedGrants = _persistedGrantService.GetPersistedGrantList(x =>
                x.ClientId == (filter.ClientId ?? x.ClientId) &&
                x.SessionId == (filter.SessionId ?? x.SessionId) &&
                x.SubjectId == (filter.SubjectId ?? x.SubjectId) &&
                x.Type == (filter.Type ?? x.Type));
            //todo seq log entegresi
            _logger.LogDebug($"{persistedGrants.Count} persisted grants found.");

            return Task.FromResult(persistedGrants.AsEnumerable());
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = _persistedGrantService.GetPersistedGrant(x => x.Key == key);

            _logger.LogDebug($"{key} found in database: {persistedGrant != null}");

            return Task.FromResult(persistedGrant);
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            var persistedGrants = _persistedGrantService.GetPersistedGrantList(x =>
                x.ClientId == (filter.ClientId ?? x.ClientId) &&
                x.SessionId == (filter.SessionId ?? x.SessionId) &&
                x.SubjectId == (filter.SubjectId ?? x.SubjectId) &&
                x.Type == (filter.Type ?? x.Type));

            _logger.LogDebug($"removing {persistedGrants.Count} persisted grants from database.");

            _persistedGrantService.Remove(persistedGrants);

            return Task.FromResult(0);
        }

        public Task RemoveAsync(string key)
        {
            var persistedGrant = _persistedGrantService.GetPersistedGrant(x => x.Key == key);
            if (persistedGrant != null)
            {
                _logger.LogDebug($"removing {key} persisted grant from database");

                _persistedGrantService.Remove(persistedGrant);
            }
            else
                _logger.LogDebug($"no {key} persisted grant found in database");

            return Task.FromResult(0);
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            var existing = _persistedGrantService.GetPersistedGrant(x => x.Key == grant.Key);

            if (existing == null)
            {
                _logger.LogDebug($"{grant.Key} not found in database");
                _persistedGrantService.Add(grant);
            }
            else
            {
                _logger.LogDebug($"{grant.Key} found in database");
                _persistedGrantService.Update(grant);
            }

            return Task.FromResult(0);
        }
    }
}
