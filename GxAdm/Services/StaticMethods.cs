using Microsoft.OData.Client;

namespace GxAdm.Services
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

    }
}
