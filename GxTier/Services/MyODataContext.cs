using System;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Claims;

using Blazored.LocalStorage;

using GxTie.Services;

using GxShared.Sess;

using GxWapi.DaModels;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;

using Newtonsoft.Json;

namespace GxTie.Services
{
    public interface IMyODataContext
    {
        //DataServiceContext Context { get; }
        Uri ServiceRoot { get; }

        // Tracking & State
        bool IsEntityTracked(object entity);
        EntityStates? GetEntityState(object entity);
        IEnumerable<EntityDescriptor> GetAllTrackedEntities();
        void Detach(object entity);
        void SetEntityState(object entity, EntityStates state);

        // Queries
        DataServiceQuery<T> Query<T>(string entitySetName = null) where T : class;
        Task<List<T>> ExecuteQueryAsync<T>(DataServiceQuery<T> query) where T : class;
        Task<T> GetByKeyAsync<T>(int key, string entitySetName, string expand = null) where T : class;
        Task<IEnumerable<T>> LoadFilteredAsync<T>(
            Expression<Func<T, bool>> filter = null,
            string entitySetName = null,
            string expand = null,
            int? top = null,
            int? skip = null) where T : class;

        // CRUD Operations (OData Client - LOADING/TRACKING ONLY)
        void AttachTo<T>(string entitySetName, T entity) where T : class;
        void AddObject<T>(string entitySetName, T entity) where T : class;
        void UpdateObject<T>(T entity) where T : class;
        void DeleteObject<T>(T entity) where T : class;
        void AttachLink<T>(T source, string navigationProperty, object target) where T : class;
        void AddRelatedObject<TSource, TTarget>(TSource source, string navigationProperty, TTarget relatedObject)
            where TSource : class where TTarget : class;
        // NEW: For PendingChangesGuard integration
        IEnumerable<EntityDescriptor> GetPendingChanges();  // ← Native OData type
        //IEnumerable<EntityEntry> GetPendingChanges();
        //void ClearPendingChanges();

        // Collections
        DataServiceCollection<T> CreateTrackedCollection<T>(
            IEnumerable<T> items, string entitySetName = null,
            TrackingMode trackingMode = TrackingMode.AutoChangeTracking) where T : class;
    }
    public class MyODataContext : IMyODataContext
    {
        private readonly Default.Container _aocontext;
        private readonly Uri _serviceRoot;
        private readonly HttpClient _httpClient;

        public MyODataContext(Uri serviceRoot, HttpClient httpClient)
        {
            _serviceRoot = serviceRoot;
            _httpClient = httpClient;

            // Use the Uri + ProtocolVersion constructor (your generated proxy doesn’t support HttpClient overload)
            _aocontext = new Default.Container(_serviceRoot, ODataProtocolVersion.V4);

            // Attach only Authorization header here
            _aocontext.BuildingRequest += (sender, e) =>
            {
                if (_httpClient.DefaultRequestHeaders.Authorization != null)
                {
                    e.Headers["Authorization"] = _httpClient.DefaultRequestHeaders.Authorization.ToString();
                }
                // ✅ Correct Accept header
                e.Headers["Accept"] = "application/json";
            };
            // Let OData manage Accept header
            _aocontext.Format.UseJson();
        }

        public Default.Container Context => _aocontext;
        public Uri ServiceRoot => _serviceRoot;

        // Entity state helpers
        public EntityStates? GetEntityState(object entity) => _aocontext.GetEntityDescriptor(entity)?.State;
        public bool IsEntityTracked(object entity) => _aocontext.GetEntityDescriptor(entity) != null;
        public IEnumerable<EntityDescriptor> GetAllTrackedEntities() => _aocontext.Entities;

        // Query helpers
        public DataServiceQuery<T> Query<T>(string entitySetName = null) where T : class
        {
            entitySetName ??= typeof(T).Name + "s";
            return _aocontext.CreateQuery<T>(entitySetName);
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(DataServiceQuery<T> query) where T : class
        {
            try
            {
                var results = await query.ExecuteAsync();
                return results.ToList();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("response payload"))
            {
                Console.WriteLine($"Empty response for {typeof(T).Name}: returning empty list.");
                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Query error: {ex}");
                return new List<T>();
            }
        }

        public async Task<T?> GetByKeyAsync<T>(int key, string entitySetName, string? expand = null) where T : class
        {
            var query = _aocontext.CreateQuery<T>($"{entitySetName}({key})");
            if (!string.IsNullOrEmpty(expand))
                query = query.AddQueryOption("$expand", expand);

            try
            {
                return (await query.ExecuteAsync()).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByKey failed: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<T>> LoadFilteredAsync<T>(
            Expression<Func<T, bool>> filter = null,
            string entitySetName = null,
            string expand = null,
            int? top = null,
            int? skip = null) where T : class
        {
            entitySetName ??= typeof(T).Name + "s";
            var query = _aocontext.CreateQuery<T>(entitySetName) as DataServiceQuery<T>;

            if (!string.IsNullOrEmpty(expand)) query = query.AddQueryOption("$expand", expand) as DataServiceQuery<T>;
            if (top.HasValue) query = query.AddQueryOption("$top", top.Value) as DataServiceQuery<T>;
            if (skip.HasValue) query = query.AddQueryOption("$skip", skip.Value) as DataServiceQuery<T>;
            if (filter != null) query = query.Where(filter) as DataServiceQuery<T>;

            try
            {
                var results = await query.ExecuteAsync();
                return results.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadFiltered error: {ex.Message}");
                return Enumerable.Empty<T>();
            }
        }

        // CRUD helpers
        public void AttachTo<T>(string entitySetName, T entity) where T : class => _aocontext.AttachTo(entitySetName, entity);
        public void AddObject<T>(string entitySetName, T entity) where T : class => _aocontext.AddObject(entitySetName, entity);
        public void UpdateObject<T>(T entity) where T : class => _aocontext.UpdateObject(entity);
        public void DeleteObject<T>(T entity) where T : class => _aocontext.DeleteObject(entity);
        public void Detach(object entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _aocontext.Detach(entity);
        }
        public void AttachLink<T>(T source, string navigationProperty, object target) where T : class
            => _aocontext.AttachLink(source, navigationProperty, target);
        public void AddRelatedObject<TSource, TTarget>(TSource source, string navigationProperty, TTarget relatedObject)
            where TSource : class where TTarget : class
            => _aocontext.AddRelatedObject(source, navigationProperty, relatedObject);

        // Safe attach/update
        public void SafeAttachOrUpdate<T>(T entity, string entitySetName, bool isAddPln) where T : class
        {
            if (IsEntityTracked(entity))
            {
                UpdateObject(entity);
            }
            else if (isAddPln)
            {
                AddObject(entitySetName, entity);
            }
            else
            {
                _aocontext.AttachTo(entitySetName, entity);
                UpdateObject(entity);
            }
        }

        public void SafeAttachOrUpdate<T>(T entity, string entitySetName) where T : class
        {
            var descriptor = _aocontext.GetEntityDescriptor(entity);
            bool isAddState = descriptor?.State == EntityStates.Added;
            SafeAttachOrUpdate(entity, entitySetName, isAddState);
        }

        // Pending changes guard
        public IEnumerable<EntityDescriptor> GetPendingChanges() =>
            _aocontext.Entities.Where(e => e.State == EntityStates.Added ||
                                           e.State == EntityStates.Modified ||
                                           e.State == EntityStates.Deleted);

        public void ClearPendingChanges()
        {
            var pending = _aocontext.Entities.Where(e => e.State != EntityStates.Unchanged).ToList();
            foreach (var entity in pending)
            {
                _aocontext.Detach(entity.Entity);
            }
            Console.WriteLine($"Cleared {pending.Count} pending changes");
        }

        public void SetEntityState(object entity, EntityStates state)
        {
            var descriptor = _aocontext.GetEntityDescriptor(entity);
            if (descriptor != null)
            {
                Console.WriteLine($"State change {descriptor.State} → {state} for {entity}");
            }
        }

        // Collections
        public DataServiceCollection<T> CreateTrackedCollection<T>(
            IEnumerable<T> items,
            string entitySetName = null,
            TrackingMode trackingMode = TrackingMode.AutoChangeTracking) where T : class
        {
            entitySetName ??= typeof(T).Name + "s";
            return new DataServiceCollection<T>(_aocontext, items, trackingMode, entitySetName, null, null);
        }
    }
    public interface IODataContextFactory
    {
        Task<MyODataContext> CreateAsync();
    }
    public class ODataContextFactory : IODataContextFactory
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        public ODataContextFactory(
            ILocalStorageService localStorage,
            IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MyODataContext> CreateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");

            // Use the named ODataClient (already has /odata base address)
            var client = _httpClientFactory.CreateClient("ODataClient");

            // Attach bearer token
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            // Return context bound to this client
            return new MyODataContext(client.BaseAddress, client);
        }
    }
}
    //public class ODataContextFactory : IODataContextFactory
    //{
    //    private readonly ILocalStorageService _localStorage;
    //    private readonly IConfiguration _configuration;
    //    private readonly IHttpClientFactory _httpClientFactory;

    //    public ODataContextFactory(
    //        ILocalStorageService localStorage,
    //        IConfiguration configuration,
    //        IHttpClientFactory httpClientFactory)
    //    {
    //        _localStorage = localStorage;
    //        _configuration = configuration;
    //        _httpClientFactory = httpClientFactory;
    //    }

    //    public async Task<MyODataContext> CreateAsync()
    //    {
    //        var token = await _localStorage.GetItemAsync<string>("blazToken");

    //        var backUrl = _configuration["BackendUrl"] ?? "https://localhost:7095";
    //        if (!backUrl.EndsWith("/")) backUrl += "/";
    //        backUrl += "odata";

    //        var uri = new Uri(backUrl);

    //        // Create HttpClient from factory
    //        var client = _httpClientFactory.CreateClient("ODataClient");

    //        // 🔒 Attach bearer token
    //        if (!string.IsNullOrEmpty(token))
    //        {
    //            client.DefaultRequestHeaders.Authorization =
    //                new AuthenticationHeaderValue("Bearer", token);
    //        }

    //        // 📦 Enforce OData headers
    //        //client.DefaultRequestHeaders.Accept.Clear();
    //        //client.DefaultRequestHeaders.Accept.Add(
    //        //    new MediaTypeWithQualityHeaderValue("application/json")
    //        //    {
    //        //        Parameters = { new NameValueHeaderValue("odata.metadata", "minimal") }
    //        //    });

    //        // Return context bound to this client
    //        return new MyODataContext(uri, client);
    //    }
    //    public class MyODataContext : IMyODataContext
    //    {
    //        private readonly Default.Container _aocontext;
    //        private readonly Uri _serviceRoot;
    //        private readonly HttpClient _httpClient;

//        public MyODataContext(Uri serviceRoot, HttpClient httpClient)
//        {
//            _serviceRoot = serviceRoot;
//            _httpClient = httpClient;
//            _aocontext = new Default.Container(_serviceRoot, ODataProtocolVersion.V4);
//            // Instantiate OData container with HttpClient
//            //_aocontext = new Default.Container(_serviceRoot, _httpClient);
//            _aocontext.BuildingRequest += (sender, e) =>
//            {
//                e.Headers.Add("Authorization", $"Bearer {_httpClient.DefaultRequestHeaders.Authorization.Parameter}");
//                e.Headers.Add("Accept", "application/json;odata.metadata=minimal");
//            };
//            // Ensure JSON format
//            _aocontext.Format.UseJson();
//        }

//        public Default.Container Context => _aocontext;

//        //private void ConfigureRequestPipeline(string otoken)
//        //{
//        //    _aocontext.Configurations.RequestPipeline.OnMessageCreating = (args) =>
//        //    {
//        //        var requestMessage = new HttpClientRequestMessage(args);
//        //        if (!string.IsNullOrEmpty(otoken))
//        //        {
//        //            requestMessage.SetHeader("Authorization", $"Bearer {otoken}");
//        //        }
//        //        foreach (var header in args.Headers)
//        //        {
//        //            requestMessage.SetHeader(header.Key, header.Value);
//        //        }
//        //        Console.WriteLine($"OData GET request: {args.RequestUri}");
//        //        Console.WriteLine($"{args.Method} {args.RequestUri}");
//        //        foreach (var h in args.Headers)
//        //            Console.WriteLine($"{h.Key}: {h.Value}");
//        //        return requestMessage;
//        //    };
//        //}

//        // INTERFACE IMPLEMENTATION - UNCHANGED (Loading/Tracking)
//        //public DataServiceContext Context => _aocontext;
//        public Uri ServiceRoot => _serviceRoot;

//        public EntityStates? GetEntityState(object entity) => _aocontext.GetEntityDescriptor(entity)?.State;
//        public bool IsEntityTracked(object entity) => _aocontext.GetEntityDescriptor(entity) != null;
//        public IEnumerable<EntityDescriptor> GetAllTrackedEntities() => _aocontext.Entities;

//        public DataServiceQuery<T> Query<T>(string entitySetName = null) where T : class
//        {
//            entitySetName ??= typeof(T).Name + "s";
//            return _aocontext.CreateQuery<T>(entitySetName);
//        }

//        public async Task<List<T>> ExecuteQueryAsync<T>(DataServiceQuery<T> query) where T : class
//        {
//            try
//            {
//                var results = await query.ExecuteAsync();
//                return results.ToList();
//            }
//            catch (InvalidOperationException ex) when (ex.Message.Contains("response payload"))
//            {
//                Console.WriteLine($"Empty response for {typeof(T).Name}: returning empty list.");
//                return new List<T>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Query error: {ex}");
//                return new List<T>();
//            }
//        }

//        public async Task<T?> GetByKeyAsync<T>(
//    int key,
//    string entitySetName,
//    string? expand = null
//) where T : class
//        {
//            var query = _aocontext.CreateQuery<T>($"{entitySetName}({key})");

//            if (!string.IsNullOrEmpty(expand))
//                query = query.AddQueryOption("$expand", expand);

//            try
//            {
//                return (await query.ExecuteAsync()).SingleOrDefault();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"GetByKey failed: {ex.Message}");
//                return null;
//            }
//        }
//     public async Task<IEnumerable<T>> LoadFilteredAsync<T>(
//            Expression<Func<T, bool>> filter = null, string entitySetName = null,
//            string expand = null, int? top = null, int? skip = null) where T : class
//        {
//            entitySetName ??= typeof(T).Name + "s";
//            var query = _aocontext.CreateQuery<T>(entitySetName) as DataServiceQuery<T>;

//            if (!string.IsNullOrEmpty(expand)) query = query.AddQueryOption("$expand", expand) as DataServiceQuery<T>;
//            if (top.HasValue) query = query.AddQueryOption("$top", top.Value) as DataServiceQuery<T>;
//            if (skip.HasValue) query = query.AddQueryOption("$skip", skip.Value) as DataServiceQuery<T>;
//            if (filter != null) query = query.Where(filter) as DataServiceQuery<T>;

//            try
//            {
//                var results = await query.ExecuteAsync();
//                return results.ToList();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"LoadFiltered error: {ex.Message}");
//                return Enumerable.Empty<T>();
//            }
//        }

//        // CRUD - OData Client (Tracking only)
//        public void AttachTo<T>(string entitySetName, T entity) where T : class => _aocontext.AttachTo(entitySetName, entity);
//        public void AddObject<T>(string entitySetName, T entity) where T : class => _aocontext.AddObject(entitySetName, entity);
//        public void UpdateObject<T>(T entity) where T : class => _aocontext.UpdateObject(entity);
//        public void DeleteObject<T>(T entity) where T : class => _aocontext.DeleteObject(entity);
//        // ✅ NEW: Non-generic wrapper for interface
//        public void Detach(object entity)
//        {
//            ArgumentNullException.ThrowIfNull(entity);
//            _aocontext.Detach(entity);
//        }
//        public void AttachLink<T>(T source, string navigationProperty, object target) where T : class
//            => _aocontext.AttachLink(source, navigationProperty, target);
//        public void AddRelatedObject<TSource, TTarget>(TSource source, string navigationProperty, TTarget relatedObject)
//            where TSource : class where TTarget : class
//            => _aocontext.AddRelatedObject(source, navigationProperty, relatedObject);
//        // 🔥 NEW: Safe Attach/Update with isAddPln flag
//        public void SafeAttachOrUpdate<T>(T entity, string entitySetName, bool isAddPln) where T : class
//        {
//            if (IsEntityTracked(entity))
//            {
//                UpdateObject(entity);  // Already tracked → Update
//            }
//            else if (isAddPln)
//            {
//                AddObject(entitySetName, entity);  // New row → Add
//            }
//            else
//            {
//                // Existing row → Attach then Update
//                _aocontext.AttachTo(entitySetName, entity);
//                UpdateObject(entity);
//            }
//        }
//        // Overload for convenience (auto-detects add state)
//        public void SafeAttachOrUpdate<T>(T entity, string entitySetName) where T : class
//        {
//            // Detect if this entity is in Added state or flagged as new
//            var descriptor = _aocontext.GetEntityDescriptor(entity);
//            bool isAddState = descriptor?.State == EntityStates.Added;
//            SafeAttachOrUpdate(entity, entitySetName, isAddState);
//        }
//        // 🔥 NEW: PendingChangesGuard Integration
//        public IEnumerable<EntityDescriptor> GetPendingChanges()
//        {
//            return _aocontext.Entities
//                .Where(e => e.State == EntityStates.Added ||
//                           e.State == EntityStates.Modified ||
//                           e.State == EntityStates.Deleted);
//        }
//        public void ClearPendingChanges()
//        {
//            var pending = _aocontext.Entities
//                .Where(e => e.State != EntityStates.Unchanged)
//                .ToList();

//            foreach (var entity in pending)
//            {
//                _aocontext.Detach(entity.Entity);
//            }
//            Console.WriteLine($"Cleared {pending.Count} pending changes");
//        }

//        public void SetEntityState(object entity, EntityStates state)
//        {
//            var descriptor = _aocontext.GetEntityDescriptor(entity);
//            if (descriptor != null)
//            {
//                // OData Client doesn't directly support state setting
//                // PendingChangesGuard handles this via Detach/ClearPendingChanges
//                Console.WriteLine($"State change {descriptor.State} → {state} for {entity}");
//            }
//        }

//        // Collections
//        public DataServiceCollection<T> CreateTrackedCollection<T>(
//            IEnumerable<T> items, string entitySetName = null,
//            TrackingMode trackingMode = TrackingMode.AutoChangeTracking) where T : class
//        {
//            entitySetName ??= typeof(T).Name + "s";
//            return new DataServiceCollection<T>(_aocontext, items, trackingMode, entitySetName, null, null);
//        }
//    }
//}

//public class ODataContextFactory : IODataContextFactory
//{
//    private readonly ILocalStorageService _localStorage;
//    private readonly IConfiguration _configuration;
//    private readonly IHttpClientFactory _httpClientFactory;  // 🔥 ADDED
//    public ODataContextFactory(
//        ILocalStorageService localStorage,
//        IConfiguration configuration,
//        IHttpClientFactory httpClientFactory)  // 🔥 INJECTED
//    {
//        _localStorage = localStorage;
//        _configuration = configuration;
//        _httpClientFactory = httpClientFactory;
//    }
//    public async Task<MyODataContext> CreateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("blazToken");
//        var backUrl = _configuration["BackendUrl"] ?? "https://localhost:7095";
//        if (!backUrl.EndsWith("/")) backUrl += "/";
//        backUrl += "odata";
//        var uri = new Uri(backUrl);
//        return new MyODataContext(uri, token, _httpClientFactory);  // 🔥 PASS HttpClientFactory
//    }
//}
//    public class ODataContextFactory : IODataContextFactory
//{
//    private readonly ILocalStorageService _localStorage;
//    private readonly IConfiguration _configuration;

//    public ODataContextFactory(ILocalStorageService localStorage, IConfiguration configuration)
//    {
//        _localStorage = localStorage;
//        _configuration = configuration;
//    }

//    public async Task<MyODataContext> CreateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("blazToken");

//        var backUrl = _configuration["BackendUrl"];
//        if (!backUrl.EndsWith("/"))
//        {
//            backUrl += "/";
//        }
//        backUrl += "odata";

//        var uri = new Uri(backUrl);
//        return new MyODataContext(uri, token);
//    }
//}



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
////public class MyODataContext //: DataServiceContext
////{
////    private readonly Default.Container _aocontext;
////    private readonly Uri _serviceRoot;
////    private readonly ILocalStorageService _localStorage;
////    private readonly HttpClient _httpClient; 
////    public MyODataContext(Uri serviceRoot, string otoken)
////    {
////        Console.WriteLine("MyODataContext constructor called");
////        var protocolVersion = ODataProtocolVersion.V4;
////        _serviceRoot = serviceRoot;
////        _aocontext = new Default.Container(_serviceRoot, protocolVersion);
////        // ✅ CRITICAL FIX - Add this line!
////        // ✅ FIX: Use PreserveChanges instead of OverwriteChanges
////        //_aocontext.MergeOption = MergeOption.PreserveChanges;  // ← This works!
////        _aocontext.Format.UseJson();
////        _aocontext.Configurations.RequestPipeline.OnMessageCreating = (args) =>
////        {
////            var requestMessage = new HttpClientRequestMessage(args);
////            //Console.WriteLine($"Odata TOKEN here {otoken}");
////            if (!string.IsNullOrEmpty(otoken))
////            {
////                requestMessage.SetHeader("Authorization", $"Bearer {otoken}");
////            }
////            // Copy any additional headers if needed
////            foreach (var header in args.Headers)
////            {
////                requestMessage.SetHeader(header.Key, header.Value);
////            }
////            Console.WriteLine($"Injecting token: {otoken}");
////            Console.WriteLine($"Request URI: {args.RequestUri}");
////            Console.WriteLine($"Headers: {string.Join(", ", args.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
////            return requestMessage;
////        };
////    }
////    public EntityStates? GetEntityState(object entity)
////    {
////        // Use the generated context's inherited method
////        return _aocontext.GetEntityDescriptor(entity)?.State;
////    }
////    public bool IsEntityTracked(object entity)
////    {
////        return _aocontext.GetEntityDescriptor(entity) != null;
////    }
////    public IEnumerable<EntityDescriptor> GetAllTrackedEntities()
////    {
////        return _aocontext.Entities;
////    }
////    public DataServiceQuery<T> Query<T>(string entitySetName = null) where T : class
////    {
////        entitySetName ??= typeof(T).Name + "s";
////        return _aocontext.CreateQuery<T>(entitySetName);
////    }
////    public async Task<List<T>> ExecuteQueryAsync<T>(DataServiceQuery<T> query) where T : class
////    {
////        int tst = 1;
////        try
////        {
////            var results = await query.ExecuteAsync();
////            tst = 2;
////            return results.ToList();
////        }
////        catch (InvalidOperationException ex) when (ex.Message.Contains("response payload"))
////        {
////            Console.WriteLine($"Empty or malformed {tst} response for {typeof(T).Name}: returning empty list.");
////            return new List<T>();
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"Unexpected {tst} error: {ex}");
////            return null;
////        }
////    }
////    public async Task<T> GetByKeyAsync<T>(int key, string entitySetName, string expand) where T : class
////    {
////        var keyQuery = _aocontext.CreateQuery<T>($"{entitySetName}({key})");
////        if (!string.IsNullOrEmpty(expand))
////            keyQuery = keyQuery.AddQueryOption("$expand", expand);

////        Console.WriteLine($"Request URI: {keyQuery.RequestUri.OriginalString}");

////        try
////        {
////            // ✅ SINGLE ENUMERATION ONLY
////            var results = await keyQuery.ExecuteAsync();
////            var enumerator = results.GetEnumerator();
////            var hasResult = enumerator.MoveNext();

////            if (hasResult)
////            {
////                var entity = enumerator.Current as T;
////                Console.WriteLine("✅ SINGLE entity loaded");

////                // ✅ Initialize collections AFTER single enumeration
////                if (entity is Gxorga orga)
////                {
////                    if (orga.Gsgfixes == null) orga.Gsgfixes = new();
////                    if (orga.Gsglnes == null) orga.Gsglnes = new();

////                    Console.WriteLine($"nb gfixes: {orga.Gsgfixes.Count}");
////                }

////                return entity;
////            }

////            return null;
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"❌ Key query failed: {ex.Message}");
////            return null;
////        }
////    }


////    //public async Task<T> GetByKeyAsync<T>(int key, string entitySetName = null, string expand = null) where T : class
////    //{
////    //    entitySetName ??= typeof(T).Name + "s";

////    //    // ✅ KEY-BASED: Generates /Gxorgas(10001)
////    //    var keyQuery = _aocontext.CreateQuery<T>($"{entitySetName}({key})");

////    //    if (!string.IsNullOrEmpty(expand))
////    //        keyQuery = keyQuery.AddQueryOption("$expand", expand);

////    //    try
////    //    {
////    //        var result = await keyQuery.ExecuteAsync();
////    //        return result.Cast<T>().FirstOrDefault();
////    //    }
////    //    catch (Exception ex)
////    //    {
////    //        Console.WriteLine($"Key query failed for {entitySetName}({key}): {ex.Message}");
////    //        return null;
////    //    }
////    //}
////    public async Task<IEnumerable<T>> LoadFilteredAsync<T>(
////        Expression<Func<T, bool>> filter = null,
////        string entitySetName = null,
////        string expand = null,
////        int? top = null,
////        int? skip = null
////    ) where T : class
////    {
////        int tst = 0;
////        entitySetName ??= typeof(T).Name + "s";
////        var query = _aocontext.CreateQuery<T>(entitySetName) as DataServiceQuery<T>;

////        // Apply $expand if needed
////        if (!string.IsNullOrEmpty(expand))
////        {
////            query = query.AddQueryOption("$expand", expand) as DataServiceQuery<T>;
////            Console.WriteLine($"current expand : {query.ToString()}");
////        }
////        // Apply $top and $skip
////        if (top.HasValue)
////            query = query.AddQueryOption("$top", top.Value) as DataServiceQuery<T>;
////        if (skip.HasValue)
////            query = query.AddQueryOption("$skip", skip.Value) as DataServiceQuery<T>;

////        // Apply filter if specified
////        if (filter != null)
////            query = query.Where(filter) as DataServiceQuery<T>;

////        try
////        {
////            tst = 1;
////            var results = await query.ExecuteAsync();
////            tst = 2;
////            //Console.WriteLine($"OData query results: {results}");
////            tst = 3;
////            return results.ToList();
////        }
////        catch (Exception ex)
////        {
////            //Console.WriteLine($"Error executing OData query: {tst} et {ex.InnerException.Message}");
////            ////return new List<T>();
////            Console.WriteLine($"Error: place {tst} — {ex.Message} / inner: {ex.InnerException?.Message}");
////            throw;

////        }
////    }

////    // Add CRUD methods
////    public void AttachTo<T>(string entitySetName, T entity) where T : class
////        => _aocontext.AttachTo(entitySetName, entity);
////    public void AttachLink<T>(T source, string navigationProperty, object target) where T : class
////        => _aocontext.AttachLink(source, navigationProperty, target);
////    public void AddRelatedObject<TSource, TTarget>(TSource source, string navigationProperty, TTarget relatedObject)
////        where TSource : class
////        where TTarget : class
////        => _aocontext.AddRelatedObject(source, navigationProperty, relatedObject);

////    public bool MyChangeTracker()
////        => _aocontext.EntityTracker.Entities.Any(e => !e.State.HasFlag(EntityStates.Unchanged));
////    public void Detach<T>(T entity) where T : class
////        => _aocontext.Detach(entity);
////    public void AddObject<T>(string entitySetName, T entity) where T : class
////        => _aocontext.AddObject(entitySetName, entity);
////    public void DeleteObject<T>(T entity) where T : class
////        => _aocontext.DeleteObject(entity);
////    public void UpdateObject<T>(T entity) where T : class
////        => _aocontext.UpdateObject(entity);
////    public async Task<T> CancelEdit<T>(string entitySetName, int id, T editedEntity, Func<T, bool> replaceInParent) where T : class
////    {
////        // Step 1: Detach the edited entity from tracking
////        _aocontext.Detach(editedEntity);

////        // Step 2: Requery the fresh instance from server
////        var refreshedResult = await _aocontext.ExecuteAsync<T>(
////            new Uri($"{entitySetName}({id})", UriKind.Relative)
////        );

////        // Step 3: Try to find where to reattach it
////        var refreshed = refreshedResult.SingleOrDefault();
////        if (refreshed != null)
////        {
////            // Your caller can use this callback to update the reference graph
////            // e.g. replace old Rubvar in Plngen.Rubvars collection
////            if (replaceInParent != null)
////                replaceInParent(refreshed);
////        }

////        return refreshed;
////    }
////    public async Task<bool> SaveDataAsync()
////    {
////        foreach (var ed in _aocontext.Entities)
////        {
////            if (ed.State == EntityStates.Added || ed.State == EntityStates.Modified || ed.State == EntityStates.Deleted)
////            {
////                Console.WriteLine($"Entity: {ed.Entity.GetType().Name}, State: {ed.State}");
////            }
////        }

////        try
////        {
////            var response = await _aocontext.SaveChangesAsync();
////            Console.Write($"Data saved successfully {response.Count()}");
////            return response != null && response.Count() > 0;
////        }
////        catch (Exception ex)
////        {
////            // Log the actual error
////            Console.WriteLine($"SaveChanges Error: {ex.Message}");
////            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
////            if (ex.InnerException != null)
////            {
////                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
////            }
////            throw; // Re-throw to see the actual error
////        }
////    }
////    public void EnsureDetached<T>(T entity)
////    {
////        var descriptor = _aocontext.GetEntityDescriptor(entity);
////        if (descriptor != null)
////        {
////            _aocontext.Detach(entity);
////        }
////    }
////    public DataServiceContext Context => _aocontext;
////    public DataServiceCollection<T> CreateTrackedCollection<T>(
////        IEnumerable<T> items,
////        string entitySetName = null,
////        TrackingMode trackingMode = TrackingMode.AutoChangeTracking) where T : class
////    {
////        entitySetName ??= typeof(T).Name + "s";
////        return new DataServiceCollection<T>(
////            Context,
////            items,
////            trackingMode,
////            entitySetName,
////            null,
////            null
////        );
////    }
////}

