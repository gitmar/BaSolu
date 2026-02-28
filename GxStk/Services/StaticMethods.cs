using Microsoft.OData.Client;

namespace GxStk.Services
{
    public static class StaticMethods
    {
        public static void MarkModified<T>(this DataServiceContext ctx, T entity, string setName)
        {
            // Use ReferenceEquals to ensure we're checking the same object instance
            if (!ctx.Entities.Any(e => ReferenceEquals(e.Entity, entity)))
            {
                ctx.AttachTo(setName, entity);
            }
            ctx.UpdateObject(entity);
        }
        public static bool IsNew<T>(
                MyODataContext context,
                T entity,
                Func<T, int> keySelector)
                where T : class
        {
            // Check OData tracking state first
            var descriptor = context.Context.GetEntityDescriptor(entity);
            if (descriptor != null)
            {
                if (descriptor.State == EntityStates.Added)
                    return true;   // definitely new
                if (descriptor.State == EntityStates.Unchanged ||
                    descriptor.State == EntityStates.Modified)
                    return false;  // already persisted
            }

            // Fallback: check int key value
            var keyValue = keySelector(entity);
            return keyValue == 0; // default int means not saved
        }
        //public static class ODataEntityHelper
        //{

        //}
    }
}
