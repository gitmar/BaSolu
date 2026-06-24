// Features/Plans/PlanCrudGrid.razor.cs
using GxPilo.Components.Plans;
using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
public partial class PlanCrudGrid : CrudGridBase<PlngenDto>
{
    [Inject] private IPendingChangesGuard Guard { get; set; } = default!;

    public List<PlngenDto> MyDaPlans { get; set; } = new();

    public PlanCrudGrid(IPendingChangesGuard guard)
        : base(guard, "Plngens")
    {
    }

    protected override void SubscribeToGuard()
    {
        Guard.OnChanges += () =>
        {
            NbChanges = Guard.GetPendingChangesCount();
            StateHasChanged();
        };
    }

    protected override bool Validate(PlngenDto entity) => !string.IsNullOrWhiteSpace(entity.Liba);

    protected override void AddToLocalCollection(PlngenDto entity) => MyDaPlans.Insert(0, entity);
    protected override void RemoveFromLocalCollection(PlngenDto entity) => MyDaPlans.Remove(entity);
    protected override void ReplaceInLocalCollection(PlngenDto entity)
    {
        var idx = MyDaPlans.FindIndex(p => p.Rowguid == entity.Rowguid);
        if (idx >= 0) MyDaPlans[idx] = entity;
    }

    protected override void CopyDraftToGridItem(PlngenDto entity)
    {
        if (Draft is null) return;
        entity.Liba = Draft.Liba;
        entity.Abg = Draft.Abg;
        entity.Eta = Draft.Eta;
        // ... other fields
    }

    protected override void RollbackPendingState(PlngenDto entity, bool isNew)
    {
        var rowguid = EntityKeyHelper.GetRowguid(entity);
        if (isNew)
            RemoveFromLocalCollection(entity);

        SetRowState(rowguid, RowState.Default);
        _rowPendingOpTypeByRow.Remove(rowguid);
        _rowOpInfoByRow.Remove(rowguid);
        StateHasChanged();
    }

    protected override void FinalizeConfirmedState(PlngenDto entity, string message)
    {
        var rowguid = EntityKeyHelper.GetRowguid(entity);
        SetRowState(rowguid, RowState.Default);
        Imessage = message;
        StateHasChanged();
    }

    protected override void EndEdit()
    {
        Draft = null;
        _currentlyEditingRowguid = null;
        StateHasChanged();
    }

    protected async Task StartEdit(PlngenDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.EditPending);
        SetPendingOpType(rowguid, PendingOpType.Update);
        Draft = CloneDraft(item);
        _currentlyEditingRowguid = rowguid;
        StateHasChanged();
    }

    protected async Task StartDelete(PlngenDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.DeletePending);
        StateHasChanged();
    }

    public int NbChanges { get; set; }
    public string? Imessage { get; set; }
}