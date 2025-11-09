using System.Linq.Expressions;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Claims;

using Blazored.LocalStorage;

using GxShared.GlobModels;

using GxWapi.DaModels;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;

using Newtonsoft.Json;

namespace GxAdm.Services
{
    public class MyODataContext //: DataServiceContext
    {
        private readonly Default.Container _aocontext;
        private readonly Uri _serviceRoot;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        public MyODataContext(Uri serviceRoot, string otoken, HttpClient httpClient)
        {
            Console.WriteLine("MyODataContext constructor called");
            var protocolVersion = ODataProtocolVersion.V4;
            _serviceRoot = serviceRoot;
            _httpClient = httpClient;
            _aocontext = new Default.Container(_serviceRoot, protocolVersion);
            _aocontext.Format.UseJson();
            _aocontext.Configurations.RequestPipeline.OnMessageCreating = (args) =>
            {
                var requestMessage = new HttpClientRequestMessage(args);
                //Console.WriteLine($"Odata TOKEN here {otoken}");
                if (!string.IsNullOrEmpty(otoken))
                {
                    requestMessage.SetHeader("Authorization", $"Bearer {otoken}");
                }
                // Copy any additional headers if needed
                foreach (var header in args.Headers)
                {
                    requestMessage.SetHeader(header.Key, header.Value);
                }
                Console.WriteLine($"Injecting token: {otoken}");
                Console.WriteLine($"Request URI: {args.RequestUri}");
                Console.WriteLine($"Headers: {string.Join(", ", args.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
                return requestMessage;
            };
        }
        public EntityStates? GetEntityState(object entity)
        {
            // Use the generated context's inherited method
            return _aocontext.GetEntityDescriptor(entity)?.State;
        }
        public bool IsEntityTracked(object entity)
        {
            return _aocontext.GetEntityDescriptor(entity) != null;
        }
        public IEnumerable<EntityDescriptor> GetAllTrackedEntities()
        {
            return _aocontext.Entities;
        }
        public DataServiceQuery<T> Query<T>(string entitySetName = null) where T : class
        {
            entitySetName ??= typeof(T).Name + "s";
            return _aocontext.CreateQuery<T>(entitySetName);
        }
        public async Task<List<T>> ExecuteQueryAsync<T>(DataServiceQuery<T> query) where T : class
        {
            int tst = 1;
            try
            {
                //var results = await query.ExecuteAsync();

                //tst = 2;
                //return results.ToList();
                var results = await query.ExecuteAsync();
                var list = results.ToList(); // ✅ safe after await
                return list;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("response payload"))
            {
                Console.WriteLine($"Empty or malformed {tst} response for {typeof(T).Name}: returning empty list.");
                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected {tst} error: {ex}");
                return null;
            }
        }
        public async Task<IEnumerable<T>> LoadFilteredAsync<T>(
            Expression<Func<T, bool>> filter = null,
            string entitySetName = null,
            string expand = null,
            int? top = null,
            int? skip = null
        ) where T : class
        {
            int tst = 0;
            entitySetName ??= typeof(T).Name + "s";
            var query = _aocontext.CreateQuery<T>(entitySetName) as DataServiceQuery<T>;

            // Apply $expand if needed
            if (!string.IsNullOrEmpty(expand))
            {
                query = query.AddQueryOption("$expand", expand) as DataServiceQuery<T>;
                Console.WriteLine($"current expand : {query.ToString()}");
            }
            // Apply $top and $skip
            if (top.HasValue)
                query = query.AddQueryOption("$top", top.Value) as DataServiceQuery<T>;
            if (skip.HasValue)
                query = query.AddQueryOption("$skip", skip.Value) as DataServiceQuery<T>;

            // Apply filter if specified
            if (filter != null)
                query = query.Where(filter) as DataServiceQuery<T>;

            try
            {
                //tst = 1;
                //var results = await query.ExecuteAsync();
                //tst = 2;
                ////Console.WriteLine($"OData query results: {results}");
                //tst = 3;
                //return results.ToList();
                var results = await query.ExecuteAsync();
                var list = results.ToList(); // ✅ safe after await
                return list;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error executing OData query: {tst} et {ex.InnerException.Message}");
                ////return new List<T>();
                Console.WriteLine($"Error: place {tst} — {ex.Message} / inner: {ex.InnerException?.Message}");
                throw;

            }
        }
        
        // Add CRUD methods
        public void AttachTo<T>(string entitySetName, T entity) where T : class
            => _aocontext.AttachTo(entitySetName, entity);
        public void AttachLink<T>(T source, string navigationProperty, object target) where T : class
            => _aocontext.AttachLink(source, navigationProperty, target);
        public void AddRelatedObject<TSource, TTarget>(TSource source, string navigationProperty, TTarget relatedObject)
            where TSource : class
            where TTarget : class
            => _aocontext.AddRelatedObject(source, navigationProperty, relatedObject);

        public bool MyChangeTracker()
            => _aocontext.EntityTracker.Entities.Any(e => !e.State.HasFlag(EntityStates.Unchanged));
        public void Detach<T>(T entity) where T : class
            => _aocontext.Detach(entity);
        public void AddObject<T>(string entitySetName, T entity) where T : class
            => _aocontext.AddObject(entitySetName, entity);
        public void DeleteObject<T>(T entity) where T : class
            => _aocontext.DeleteObject(entity);
        public void UpdateObject<T>(T entity) where T : class
            => _aocontext.UpdateObject(entity);
        public async Task<T> CancelEdit<T>(string entitySetName, int id, T editedEntity, Func<T, bool> replaceInParent) where T : class
        {
            // Step 1: Detach the edited entity from tracking
            _aocontext.Detach(editedEntity);

            // Step 2: Requery the fresh instance from server
            var refreshedResult = await _aocontext.ExecuteAsync<T>(
                new Uri($"{entitySetName}({id})", UriKind.Relative)
            );

            // Step 3: Try to find where to reattach it
            var refreshed = refreshedResult.SingleOrDefault();
            if (refreshed != null)
            {
                // Your caller can use this callback to update the reference graph
                // e.g. replace old Rubvar in Plngen.Rubvars collection
                if (replaceInParent != null)
                    replaceInParent(refreshed);
            }

            return refreshed;
        }
        public async Task<bool> SaveDataAsync()
        {
            foreach (var ed in _aocontext.Entities)
            {
                if (ed.State == EntityStates.Added || ed.State == EntityStates.Modified || ed.State == EntityStates.Deleted)
                {
                    Console.WriteLine($"Entity: {ed.Entity.GetType().Name}, State: {ed.State}");
                }
            }

            try
            {
                var response = await _aocontext.SaveChangesAsync();
                Console.Write($"Data saved successfully {response.Count()}");
                return response != null && response.Count() > 0;
            }
            catch (Exception ex)
            {
                // Log the actual error
                Console.WriteLine($"SaveChanges Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw to see the actual error
            }
        }
        public void EnsureDetached<T>(T entity)
        {
            var descriptor = _aocontext.GetEntityDescriptor(entity);
            if (descriptor != null)
            {
                _aocontext.Detach(entity);
            }
        }
        public DataServiceContext Context => _aocontext;
        public DataServiceCollection<T> CreateTrackedCollection<T>(
            IEnumerable<T> items,
            string entitySetName = null,
            TrackingMode trackingMode = TrackingMode.AutoChangeTracking) where T : class
        {
            entitySetName ??= typeof(T).Name + "s";
            return new DataServiceCollection<T>(
                Context,
                items,
                trackingMode,
                entitySetName,
                null,
                null
            );
        }
    }
    public interface IODataContextFactory
    {
        Task<MyODataContext> CreateAsync();
    }

    public class ODataContextFactory : IODataContextFactory
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public ODataContextFactory(ILocalStorageService localStorage, IConfiguration configuration, IHttpClientFactory hCliefactory)
        {
            _localStorage = localStorage;
            _configuration = configuration;
            _httpClient = hCliefactory.CreateClient("ODataClient");
        }

        public async Task<MyODataContext> CreateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");

            var backUrl = _configuration["BackendUrl"];
            if (!backUrl.EndsWith("/"))
            {
                backUrl += "/";
            }
            backUrl += "odata";

            var uri = new Uri(backUrl);
            //return new MyODataContext(uri, token);
            return new MyODataContext(uri, token, _httpClient);
        }
    }


    

    //public class ODataContextFactory : IODataContextFactory
    //{
    //    private readonly ILocalStorageService _localStorage;

    //    public ODataContextFactory(ILocalStorageService localStorage)
    //    {
    //        _localStorage = localStorage;
    //    }

    //    public async Task<MyODataContext> CreateAsync()
    //    {
    //        var token = await _localStorage.GetItemAsync<string>("blazToken");
    //        var uri = new Uri("https://localhost:7095/odata");
    //        return new MyODataContext(uri, token);
    //    }
    //}
}
