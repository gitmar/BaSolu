// Features/Plans/RubCrudGrid.razor.cs
using GxPilo.Components.Plans;
using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
public partial class RubCrudGrid : CrudGridBase<RubvarDto>
{
    [Inject] private IPendingChangesGuard Guard { get; set; } = default!;

    public List<RubvarDto> MyDaRubs { get; set; } = new();

    public RubCrudGrid(IPendingChangesGuard guard)
        : base(guard, "Rubvars")
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

    protected override bool Validate(RubvarDto entity) => !string.IsNullOrWhiteSpace(entity.Liba);

    protected override void AddToLocalCollection(RubvarDto entity) => MyDaRubs.Insert(0, entity);
    protected override void RemoveFromLocalCollection(RubvarDto entity) => MyDaRubs.Remove(entity);
    protected override void ReplaceInLocalCollection(RubvarDto entity)
    {
        var idx = MyDaRubs.FindIndex(r => r.Rowguid == entity.Rowguid);
        if (idx >= 0) MyDaRubs[idx] = entity;
    }

    protected override void CopyDraftToGridItem(RubvarDto entity)
    {
        if (Draft is null) return;
        entity.Liba = Draft.Liba;
        entity.Abg = Draft.Abg;
        entity.Eta = Draft.Eta;
        entity.Ecmount = Draft.Ecmount;
        // ...
    }

    protected override void RollbackPendingState(RubvarDto entity, bool isNew)
    {
        var rowguid = EntityKeyHelper.GetRowguid(entity);
        if (isNew)
            RemoveFromLocalCollection(entity);

        SetRowState(rowguid, RowState.Default);
        _rowPendingOpTypeByRow.Remove(rowguid);
        _rowOpInfoByRow.Remove(rowguid);
        StateHasChanged();
    }

    protected override void FinalizeConfirmedState(RubvarDto entity, string message)
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

    protected async Task StartEdit(RubvarDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.EditPending);
        SetPendingOpType(rowguid, PendingOpType.Update);
        Draft = CloneDraft(item);
        _currentlyEditingRowguid = rowguid;
        StateHasChanged();
    }

    protected async Task StartDelete(RubvarDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.DeletePending);
        StateHasChanged();
    }

    public int NbChanges { get; set; }
    public string? Imessage { get; set; }
}
