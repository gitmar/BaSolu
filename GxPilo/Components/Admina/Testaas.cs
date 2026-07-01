namespace GxPilo.Components.Admina
{
    if (item is null)
                return;

            var rowguid = GetRowguid(item);

            if (!isConfirm)
            {
                ResetRowState(rowguid);
    await InvokeAsync(StateHasChanged);
                return;
            }

removeFromCollection(item);

var key = GetEntityKey(item);
var opId = await _guard.TrackDelete(entitySet, key, rowguid);
_pendingOpIdsByRow[rowguid] = opId;

SetDeleteFlags(item);
ResetRowState(rowguid);
_rowPendingOpTypeByRow.Remove(rowguid);

await InvokeAsync(StateHasChanged);
}
