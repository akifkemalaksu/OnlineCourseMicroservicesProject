using FreeCourse.IdentityServer.Services.Interfaces;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FreeCourse.IdentityServer.Services
{
    public class PersistedGrantService : IPersistedGrantService
    {
        private readonly ILogger<PersistedGrantService> logger;
        private List<PersistedGrant> persistedGrants;
        private readonly string jsonFile;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public PersistedGrantService(IHostingEnvironment hostingEnvironment, ILogger<PersistedGrantService> logger)
        {
            this.logger = logger;
            jsonFile = hostingEnvironment.ContentRootPath + @"\PersistedGrants.json";
        }

        public PersistedGrant GetPersistedGrant(Func<PersistedGrant, bool> expression)
        {
            if (persistedGrants != null || ReadJson())
            {
                return persistedGrants.FirstOrDefault(expression);
            }
            return null;
        }

        public List<PersistedGrant> GetPersistedGrantList(Func<PersistedGrant, bool> expression)
        {
            if (persistedGrants != null || ReadJson())
            {
                return persistedGrants.Where(expression).ToList();
            }
            return null;
        }

        public List<PersistedGrant> GetBySubjectClientType(string subjectId, string clientId, string type)
        {
            if (persistedGrants != null || ReadJson())
            {
                return persistedGrants.Where(x => x.SubjectId == subjectId && x.ClientId == clientId && x.Type == type).ToList();
            }
            return null;
        }

        public void Add(PersistedGrant item)
        {
            if (persistedGrants != null || ReadJson())
            {
                persistedGrants.Add(item);
                SaveJson();
            }
        }

        public void Remove(PersistedGrant item)
        {
            if (persistedGrants != null || ReadJson())
            {
                persistedGrants.Remove(item);
                SaveJson();
            }
        }

        public void Remove(List<PersistedGrant> items)
        {
            if (persistedGrants != null || ReadJson())
            {
                Parallel.ForEach(items, (item) => persistedGrants.Remove(item));
                SaveJson();
            }
        }

        public void Update(PersistedGrant item)
        {
            if (persistedGrants != null || ReadJson())
            {
                var index = persistedGrants.FindIndex(x => x.Key == item.Key);
                persistedGrants[index] = item;
                SaveJson();
            }
        }

        private bool ReadJson()
        {
            bool result = true;

            if (File.Exists(jsonFile))
            {
                try
                {
                    using StreamReader file = File.OpenText(jsonFile);
                    persistedGrants = _serializer.Deserialize(file, typeof(List<PersistedGrant>)) as List<PersistedGrant>;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in reading JSON");
                    result = false;
                }
            }
            else
            {
                persistedGrants = new List<PersistedGrant>();
            }

            return result;
        }

        private bool SaveJson()
        {
            bool result = true;

            try
            {
                using StreamWriter file = File.CreateText(jsonFile);
                _serializer.Serialize(file, persistedGrants);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in saving JSON");
                result = false;
            }

            return result;
        }
    }
}
