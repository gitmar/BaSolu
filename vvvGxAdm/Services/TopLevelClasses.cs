namespace GxAdm.Services
{
    public static class ServiceProviderExtensions
    {
        public static T? TryGetService<T>(this IServiceProvider provider) where T : class
        {
            if (provider == null)
            {
                Console.WriteLine("ServiceProvider is null — cannot resolve service.");
                return null;
            }

            return provider.GetService(typeof(T)) as T;
        }
    }
}
