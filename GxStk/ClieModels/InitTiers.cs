namespace GxWapi.DaModels
{
    // Shared interface for grid helpers
    public interface IBaseTier
    {
        int Id { get; set; }
        int? Statut { get; set; }
        string? Rpays { get; set; }
        string? Xmatri { get; set; }
    }
    // Extend OData-generated Tiersp
    public partial class Tiersp : IBaseTier
    {
        // No need to redeclare properties if they already exist in the generated class.
        // The compiler will see that Tiersp implements IBaseTier because the generated
        // class already has Statut, Rpays, Id, Xmatri.
    }

    // Extend OData-generated Tiewel
    public partial class Tiewel : IBaseTier
    {
        // Same idea: just declare the interface implementation.
    }
    // Extend OData-generated Tieafl
    public partial class Tieafl : IBaseTier
    {
        // Same idea: just declare the interface implementation.
    }
    // Extend OData-generated Tiwafl
    public partial class Tiwafl : IBaseTier
    {
        // Same idea: just declare the interface implementation.
    }
}
