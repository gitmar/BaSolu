namespace GxTie.ClieModels
{
    public enum RowState
    {
        Normal,
        EditPending,
        DeletePending,
        AddPending,  // 🔥 NEW for Add
        Locked
    }
    public enum GridMode
    {
        TiersAdding,
        TiersEditing,
        AflsAdding,
        AflsEditing,
        None
    }
    public enum EntityLevel { Plan, Rub, Fmt, Atr }
}
