// Features/Plans/FmtCrudGrid.razor.cs
using GxPilo.Components.Plans;
using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
public partial class FmtCrudGrid : CrudGridBase<RubfmtDto>
{
    [Inject] private IPendingChangesGuard Guard { get; set; } = default!;

    public List<RubfmtDto> MyDaFmts { get; set; } = new();

    public FmtCrudGrid(IPendingChangesGuard guard)
            : base(guard, "Rubfmts")
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

    protected override bool Validate(RubfmtDto entity) => !string.IsNullOrWhiteSpace(entity.Liba);

    protected override void AddToLocalCollection(RubfmtDto entity) => MyDaFmts.Insert(0, entity);
    protected override void RemoveFromLocalCollection(RubfmtDto entity) => MyDaFmts.Remove(entity);
    protected override void ReplaceInLocalCollection(RubfmtDto entity)
    {
        var idx = MyDaFmts.FindIndex(r => r.Rowguid == entity.Rowguid);
        if (idx >= 0) MyDaFmts[idx] = entity;
    }

    protected override void CopyDraftToGridItem(RubfmtDto entity)
    {
        if (Draft is null) return;
        entity.Liba = Draft.Liba;
        entity.Abg = Draft.Abg;
        entity.Eta = Draft.Eta;
        //entity.Ecmount = Draft.Ecmount;
        // ...
    }

    protected override void RollbackPendingState(RubfmtDto entity, bool isNew)
    {
        var rowguid = EntityKeyHelper.GetRowguid(entity);
        if (isNew)
            RemoveFromLocalCollection(entity);

        SetRowState(rowguid, RowState.Default);
        _rowPendingOpTypeByRow.Remove(rowguid);
        _rowOpInfoByRow.Remove(rowguid);
        StateHasChanged();
    }

    protected override void FinalizeConfirmedState(RubfmtDto entity, string message)
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

    protected async Task StartEdit(RubfmtDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.EditPending);
        SetPendingOpType(rowguid, PendingOpType.Update);
        Draft = CloneDraft(item);
        _currentlyEditingRowguid = rowguid;
        StateHasChanged();
    }

    protected async Task StartDelete(RubfmtDto item)
    {
        var rowguid = EntityKeyHelper.GetRowguid(item);
        SetRowState(rowguid, RowState.DeletePending);
        StateHasChanged();
    }

    public int NbChanges { get; set; }
    public string? Imessage { get; set; }
}
