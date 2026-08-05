using GxFormula.ForaBizz;

namespace GxTie.Services.Calculation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCalculationPipeline(this IServiceCollection services)
        {
            // Core engine (singleton, stateless)
            services.AddSingleton<FormulaEngine>();

            // Parser (singleton, stateless)
            services.AddSingleton<IProgramLineParser, ProgramLineParser>();

            // Internal calculators (scoped)
            services.AddScoped<ISaieCalculator, SaieCalculator>();
            services.AddScoped<IProgramCalculator, ProgramCalculator>();

            // Session factory removed – session creation now lives in ISaieCalculator.InitializeAsync.
            // If you still need ISaieSessionFactory for legacy code, keep it as a thin wrapper
            // over ISaieCalculator, but do not use it in new workflows.
            // services.AddSingleton<ISaieSessionFactory, SaieSessionFactory>();

            // Persistence (scoped)
            services.AddScoped<ICalculationPersistence, CalculationPersistence>();

            // Public calculation service (facade over calculators)
            services.AddScoped<ICalculationService, CalculationService>();

            // Workflow and higher-level services (orchestration + persistence)
            services.AddScoped<ICalculationWorkflow, CalculationWorkflow>();
            services.AddScoped<ISaieWorkflowService, SaieWorkflowService>();

            return services;
        }
    }
    //public static class ServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddCalculationPipeline(this IServiceCollection services)
    //    {
    //        // Core engine (singleton, stateless)
    //        //services.AddSingleton<FormulaEngine>();

    //        // Parser (singleton, stateless)
    //        services.AddSingleton<IProgramLineParser, ProgramLineParser>();

    //        // Internal calculators (scoped)
    //        services.AddScoped<ISaieCalculator, SaieCalculator>();
    //        services.AddScoped<IProgramCalculator, ProgramCalculator>();

    //        // Session factory (singleton)
    //        services.AddSingleton<ISaieSessionFactory, SaieSessionFactory>();

    //        // Persistence (scoped)
    //        services.AddScoped<ICalculationPersistence, CalculationPersistence>();

    //        // Public calculation service (facade)
    //        services.AddScoped<ICalculationService, CalculationService>();

    //        // Workflow and higher-level services
    //        services.AddScoped<ICalculationWorkflow, CalculationWorkflow>();
    //        services.AddScoped<ISaieWorkflowService, SaieWorkflowService>();

    //        return services;
    //    }
    //}
}
