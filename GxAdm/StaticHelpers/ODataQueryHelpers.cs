using System.Linq.Expressions;

namespace GxAdm.StaticHelpers
{
    public static class ODataQueryHelper
    {
        /// <summary>
        /// Converts a nested expression into an OData $expand string.
        /// Example: o => o.Plngens.Select(p => p.Rubvars.Select(r => r.Rubfmts))
        /// becomes "Plngens($expand=Rubvars($expand=Rubfmts))"
        /// </summary>
        public static string Expand<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            return BuildExpand(exp.Body);
        }
        private static string BuildExpand(Expression expr)
        {
            switch (expr)
            {
                case MemberExpression m:
                    return m.Member.Name;
                case MethodCallExpression mc when mc.Method.Name == "Select":
                    var outer = BuildExpand(mc.Arguments[0]);
                    var inner = BuildExpand(mc.Arguments[1]);
                    return $"{outer}($expand={inner})";
                default:
                    throw new NotSupportedException($"Expression type {expr.GetType().Name} not supported for OData $expand");
            }
        }
    }
}
