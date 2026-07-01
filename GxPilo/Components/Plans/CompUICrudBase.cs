using System.Collections.Generic;
using System.Security.Cryptography;

using BlazorBootstrap;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;

namespace GxPilo.Components.Plans
{
    public abstract class CompUICrudBase : MultiLevelCrudBase
    {
        protected readonly Dictionary<Guid, PendingOpType> _rowPendingOpTypeByRow = new();
        protected readonly Dictionary<Guid, PlngenDto> _edPlnOriginals = new();
        protected readonly Dictionary<Guid, RubvarDto> _edRubOriginals = new();
        protected readonly Dictionary<Guid, RubfmtDto> _edFmtOriginals = new();
        protected readonly Dictionary<Guid, RubhieDto> _edHieOriginals = new();
        protected readonly Dictionary<Guid, RubpstDto> _edPstOriginals = new();
        protected readonly Dictionary<Guid, TierspDto> _edTieOriginals = new();
        protected readonly Dictionary<Guid, ActsaieDto> _edActOriginals = new();
        protected readonly Dictionary<Guid, ActdetDto> _edAdtOriginals = new();
        protected readonly Dictionary<Guid, ResdonDto> _edResOriginals = new();
        protected readonly Dictionary<Guid, ResdetDto> _edRdtOriginals = new();
        protected readonly Dictionary<Guid, ResbroDto> _edBroOriginals = new();
        protected CompUICrudBase(IPendingChangesGuard guard) : base(guard)
        {

        }
        protected abstract void ClearAddRow(EntityLevel level, Guid rowguid);
        protected abstract void ClearEditRow(EntityLevel level, Guid rowguid);
        protected void BeginEdit(EntityLevel level, Guid rowguid, object draft)
        {
            var s = GetEditState(level);
            s.IsAdd = false;
            s.IsEdit = true;
            s.AddRowguid = null;
            s.EditRowguid = rowguid;
            s.DeleteRowguid = null;

            //SetDraft(level, draft);
            BeginEditRow(level, rowguid, draft);
            //SetRowState(level, rowguid, RowState.EditPending);
            ////_rowPendingOpTypeByRow[rowguid] = PendingOpType.Update;
        }
        protected async Task CancelAdd(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedCancelAction(level, entity, PendingOpType.Insert, rowguid);
            ClearAddRow(level, rowguid);
            EndRowEdit();
        }
        protected async Task ConfirmDelete(EntityLevel level, object entity, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedDeleteAction(level, entity, isConfirm);
            ClearAddRow(level, rowguid);
            await InvokeAsync(StateHasChanged);
        }
        protected async Task CancelEdit(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            Console.WriteLine($"CancelEdit rowguid = {rowguid}");
            //await UnifiedCancelAction(level, entity, PendingOpType.Update);
            await UnifiedCancelAction(level, entity, PendingOpType.Update, rowguid);
            ClearEditRow(level, rowguid);
            //await ClearEditRow(level, entity);
            EndRowEdit();
        }
        protected async Task CancelDelete(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedCancelAction(level, entity, PendingOpType.Delete, rowguid);
        }
        private async Task UnifiedCancelAction(EntityLevel level, object entity, PendingOpType opType, Guid rowguid)
        {
            var opInfo = GetOpInfo(level, rowguid);

            if (opInfo is not null)
                await Guard.CancelTrackAsync(opInfo.OpId, opType);

            switch (opType)
            {
                case PendingOpType.Insert:
                    //emoveByRowguid(level, rowguid);
                    RollbackPendingState(level, entity, true);
                    SetRowState(level, rowguid, RowState.Default);
                    break;

                case PendingOpType.Update:
                    //RestoreOriginalGridItem(level, entity);
                    RollbackPendingState(level, entity, false);
                    SetRowState(level, rowguid, RowState.Default);
                    break;

                case PendingOpType.Delete:
                    RollbackPendingState(level, entity, false);
                    SetRowState(level, rowguid, RowState.Default);
                    break;
            }
        }
        private async Task UnifiedDeleteAction(EntityLevel level, object item, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(item);
            var entitySet = GetEntitySet(level);
            var key = MLEntityKeyHelper.GetKeyAsObject(item);
            var opId = await Guard.TrackDelete(entitySet, key, rowguid);
            SetPendingOpType(level, rowguid, PendingOpType.Delete);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Delete));
            RemoveByRowguid(level, rowguid);
            SetDeleteFlags(item);
            Console.WriteLine($"ROW is removed");
            await InvokeAsync(StateHasChanged);
        }
        
        protected void BeginAdd(EntityLevel level, Guid rowguid, object draft)
        {
            var s = GetEditState(level);
            s.IsAdd = true;
            s.IsEdit = false;
            s.AddRowguid = rowguid;
            s.EditRowguid = null;
            s.DeleteRowguid = null;

            //SetDraft(level, draft);
            BeginAddRow(level, rowguid, draft);
            //SetRowState(level, rowguid, RowState.AddPending);
            _rowPendingOpTypeByRow[rowguid] = PendingOpType.Insert;
        }
        protected void CommitPlnDraft(PlngenDto target, PlngenDto source)
        {
            target.Idorg = source.Idorg;
            target.Rowguid = source.Rowguid;
            target.Iui = source.Iui;
            target.Ptyp = source.Ptyp;
            target.Todom = source.Todom;
            target.Ogtyp = source.Ogtyp;
            target.Totyp = source.Totyp;
            target.Toatr = source.Toatr;
            target.Tovue = source.Tovue;
            target.Liba = source.Liba;
            target.Abg = source.Abg;
            target.Fpsrc = source.Fpsrc;
            target.Fpexe = source.Fpexe;
            target.Fpisrc = source.Fpisrc;
            target.Styp = source.Styp;
            target.Eta = source.Eta;
            target.Xadd1 = source.Xadd1;
            target.Xedt1 = source.Xedt1;
        }
        protected void CommitRubDraft(RubvarDto target, RubvarDto source)
        {
            target.Idorg = source.Idorg;
            target.Rowguid = source.Rowguid;
            target.Ipln = source.Ipln;
            target.Iui = source.Iui;
            target.Atyp = source.Atyp;
            target.Scdrub = source.Scdrub;
            target.Frsrc = source.Frsrc;
            //target.Rtyp = source.Rtyp;
            //target.Toatr = source.Toatr;
            target.Tovue = source.Tovue;
            target.Liba = source.Liba;
            target.Abg = source.Abg;

            target.Eta = source.Eta;
            target.Xadd1 = source.Xadd1;
            target.Xedt1 = source.Xedt1;
        }
        protected void CommitFmtDraft(RubfmtDto target, RubfmtDto source)
        {
            target.Idorg = source.Idorg;
            target.Rowguid = source.Rowguid;
            target.Irub = source.Irub;
            target.Iui = source.Iui;
            target.Ztyp = source.Ztyp;
            target.Zcdrub = source.Zcdrub;
            target.Ftsrc = source.Ftsrc;
            //target.Rtyp = source.Rtyp;
            //target.Toatr = source.Toatr;
            //target.Tovue = source.Tovue;
            target.Liba = source.Liba;
            target.Abg = source.Abg;

            target.Eta = source.Eta;
            target.Xadd1 = source.Xadd1;
            target.Xedt1 = source.Xedt1;
        }
        protected void CommitHieDraft(RubhieDto target, RubhieDto source)
        {
            target.Idorg = source.Idorg;
            target.Rowguid = source.Rowguid;
            target.Ipln = source.Ipln;
            target.Iui = source.Iui;
            target.Atyp = source.Atyp;
            target.Scdrub = source.Scdrub;
            target.Frsrc = source.Frsrc;
            //target.Rtyp = source.Rtyp;
            //target.Toatr = source.Toatr;
            target.Tovue = source.Tovue;
            target.Liba = source.Liba;
            target.Abg = source.Abg;

            target.Eta = source.Eta;
            target.Xadd1 = source.Xadd1;
            target.Xedt1 = source.Xedt1;
        }
        protected void CommitPstDraft(RubpstDto target, RubpstDto source)
        {
            target.Idorg = source.Idorg;
            target.Rowguid = source.Rowguid;
            target.Ihie = source.Ihie;
            target.Iui = source.Iui;
            target.Ztyp = source.Ztyp;
            target.Zcdrub = source.Zcdrub;
            target.Ftsrc = source.Ftsrc;
            //target.Rtyp = source.Rtyp;
            //target.Toatr = source.Toatr;
            //target.Tovue = source.Tovue;
            target.Liba = source.Liba;
            target.Abg = source.Abg;

            target.Eta = source.Eta;
            target.Xadd1 = source.Xadd1;
            target.Xedt1 = source.Xedt1;
        }
        protected bool IsPlanEditing(PlngenDto item) => IsEditing(EntityLevel.Plan, item.Rowguid);
        protected bool IsRubEditing(RubvarDto item) => IsEditing(EntityLevel.Rub, item.Rowguid);
        protected bool IsFmtEditing(RubfmtDto item) => IsEditing(EntityLevel.Fmt, item.Rowguid);
        protected bool IsHieEditing(RubhieDto item) => IsEditing(EntityLevel.Hie, item.Rowguid);
        protected bool IsPstEditing(RubpstDto item) => IsEditing(EntityLevel.Pst, item.Rowguid);

        protected bool IsEditing(EntityLevel level, Guid rowguid)
        {
            var es = GetEditState(level);
            return (es.IsAdd || es.IsEdit) &&
                   ((es.IsAdd && es.AddRowguid == rowguid) ||
                    (es.IsEdit && es.EditRowguid == rowguid));
        }

        //protected bool IsPlanEditing(PlngenDto item)
        //{
        //    var es = GetEditState(EntityLevel.Plan);
        //    return (es.IsAdd || es.IsEdit) &&
        //           ((es.IsAdd && es.AddRowguid == item.Rowguid) ||
        //            (es.IsEdit && es.EditRowguid == item.Rowguid));
        //}


        //protected bool IsRubEditing(RubvarDto item)
        //{
        //    var es = GetEditState(EntityLevel.Rub);
        //    return (es.IsAdd || es.IsEdit) &&
        //           ((es.IsAdd && es.AddRowguid == item.Rowguid) ||
        //            (es.IsEdit && es.EditRowguid == item.Rowguid));
        //}

        //protected bool IsFmtEditing(RubfmtDto item)
        //{
        //    var es = GetEditState(EntityLevel.Fmt);
        //    return (es.IsAdd || es.IsEdit) &&
        //           ((es.IsAdd && es.AddRowguid == item.Rowguid) ||
        //            (es.IsEdit && es.EditRowguid == item.Rowguid));
        //}
        protected void ClearEditState(EntityLevel level, Guid rowguid)
        {
            _editStates[level] = new EntityEditState
            {
                IsAdd = false,
                IsEdit = false,
                AddRowguid = null,
                EditRowguid = null,
                DeleteRowguid = null
            };
            Console.WriteLine("editstate = edit false");
            SetLightBackground(rowguid, false);
        }

        protected void SetEditState(EntityLevel level, bool isAdd, bool isEdit, Guid? rowguid)
        {
            var s = GetEditState(level);
            s.IsAdd = isAdd;
            s.IsEdit = isEdit;
            s.AddRowguid = isAdd ? rowguid : null;
            s.EditRowguid = isEdit ? rowguid : null;
            s.DeleteRowguid = null;
        }
        private void SetDeleteFlags(object item) => SetFlags(item, 0, 0, -1);
        // Generic flag setter
        private void SetFlags<T>(T entity, int xadd1, int xedt1, int xdel1)
        {
            typeof(T).GetProperty("Xadd1")?.SetValue(entity, xadd1);
            typeof(T).GetProperty("Xedt1")?.SetValue(entity, xedt1);
            typeof(T).GetProperty("Xdel1")?.SetValue(entity, xdel1);
        }
        protected bool IsLevelBusy(EntityLevel level)
        {
            var es = GetEditState(level);
            return es.IsAdd || es.IsEdit;
        }
        protected async Task SaveAllChanges()
        {
            await Guard.FlushAsync();  // 🔥 AWAIT - blocks until done
            //_messageService.Show("✅ Saved successfully", ToastType.Success);
            StateHasChanged();
        }
        //protected void ClearRowState(EntityLevel level, Guid rowguid)
        //{
        //    _rowStates.Remove((level, rowguid));
        //    Console.WriteLine("22Pln ROW cleared");
        //}
        //protected void ClearEditFlags()
        //{
        //    IsAdd = false;
        //    IsEdit = false;
        //    AddRowguid = null;
        //    EditRowguid = null;
        //    DeleteRowguid = null;
        //}
        //UI Page row manage
        protected void BeginAddRow(EntityLevel level, Guid rowguid, object draft)
        {
            SetDraft(level, draft);
            StartRowEdit(rowguid, level);
            SetRowState(level, rowguid, RowState.AddPending);
            //var opId = await CommitAddOrUpdateAsync(entity, isNew, entitySet);
            ////_rowPendingOpTypeByRow[rowguid] = PendingOpType.Insert;
            //_pendingOpIdsByRow[rowguid] = opId;
            SetLightBackground(rowguid, true);
        }
        protected void BeginEditRow(EntityLevel level, Guid rowguid, object draft)
        {
            SetDraft(level, draft);
            StartRowEdit(rowguid, level);
            SetRowState(level, rowguid, RowState.EditPending);
            //var opId = await CommitAddOrUpdateAsync(entity, isNew, entitySet);
            ////_rowPendingOpTypeByRow[rowguid] = PendingOpType.Update;
            SetLightBackground(rowguid, true);
            //_pendingOpIdsByRow[rowguid] = opId;
        }
        private void SetLightBackground(Guid rowguid, bool isLight)
        {
            _isLightBg[rowguid] = isLight;
        }
        private bool GetLightBackground(Guid rowguid)
        {
            return _isLightBg.GetValueOrDefault(rowguid, false);
        }
        protected override void AddToLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan) PlanItems.Add(plan);
                    break;
                case EntityLevel.Rub:
                    if (entity is RubvarDto rub) RubItems.Add(rub);
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt) FmtItems.Add(fmt);
                    break;
                case EntityLevel.Tie:
                    if (entity is TierspDto tie) TieItems.Add(tie);
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act) ActItems.Add(act);
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt) AdtItems.Add(adt);
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res) ResItems.Add(res);
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt) RdtItems.Add(rdt);
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro) BroItems.Add(bro);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void RollbackPendingState(EntityLevel level, object entity, bool isNew)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);

            if (isNew)
            {
                Console.WriteLine($"NEW ROW TO REMOVE : {rowguid}");
                RemoveByRowguid(level, rowguid);
                ClearAddRow(level, rowguid);
            }
            //RemoveFromLocalCollection(level, entity);
            else
                RestoreOriginalGridItem(level, entity);

            ClearEditState(level, rowguid);

            _rowStates.Remove((level, rowguid));

            // add this
            _rowPendingOpTypeByRow.Remove(rowguid);

            // if you use this too:
            _pendingOpIdsByRow.Remove(rowguid);

            //EndRowEdit();
            SetLightBackground(rowguid, false);
        }

        protected override void ReplaceInLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan)
                    {
                        var index = PlanItems.FindIndex(x => x.Rowguid == plan.Rowguid);
                        if (index >= 0) PlanItems[index] = plan;
                    }
                    break;
                case EntityLevel.Rub:
                    if (entity is RubvarDto rub)
                    {
                        var index = RubItems.FindIndex(x => x.Rowguid == rub.Rowguid);
                        if (index >= 0) RubItems[index] = rub;
                    }
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt)
                    {
                        var index = FmtItems.FindIndex(x => x.Rowguid == fmt.Rowguid);
                        if (index >= 0) FmtItems[index] = fmt;
                    }
                    break;
                case EntityLevel.Tie:
                    if (entity is TierspDto tie)
                    {
                        var index = TieItems.FindIndex(x => x.Rowguid == tie.Rowguid);
                        if (index >= 0) TieItems[index] = tie;
                    }
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act)
                    {
                        var index = ActItems.FindIndex(x => x.Rowguid == act.Rowguid);
                        if (index >= 0) ActItems[index] = act;
                    }
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt)
                    {
                        var index = AdtItems.FindIndex(x => x.Rowguid == adt.Rowguid);
                        if (index >= 0) AdtItems[index] = adt;
                    }
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res)
                    {
                        var index = ResItems.FindIndex(x => x.Rowguid == res.Rowguid);
                        if (index >= 0) ResItems[index] = res;
                    }
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt)
                    {
                        var index = RdtItems.FindIndex(x => x.Rowguid == rdt.Rowguid);
                        if (index >= 0) RdtItems[index] = rdt;
                    }
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro)
                    {
                        var index = BroItems.FindIndex(x => x.Rowguid == bro.Rowguid);
                        if (index >= 0) BroItems[index] = bro;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void CopyDraftToGridItem(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan && _draftPlan is PlngenDto draftPlan)
                    {
                        if (draftPlan == null) return;
                        plan.Idorg = draftPlan.Idorg; //orga
                        plan.Liba = draftPlan.Liba; //designation
                        plan.Abg = draftPlan.Abg; //abrege
                        plan.Fpsrc = draftPlan.Fpsrc; //formule-non valider
                        plan.Eta = draftPlan.Eta; //etat
                        break;
                    }
                    break;
                case EntityLevel.Rub:
                    if (entity is RubvarDto rub && _draftRub is RubvarDto draftRub)
                    {
                        if (draftRub == null) return;
                        rub.Idorg = draftRub.Idorg; //orga
                        rub.Ipln = draftRub.Ipln; //parent
                        rub.Liba = draftRub.Liba; //designation
                        //RUBHIE
                        //if (string.IsNullOrEmpty(draftRub.Raison))
                        //{
                        //    draftRub.Raison = draftRub.Liba;
                        //}
                        rub.Abg = draftRub.Abg; //abrege
                        rub.Scdrub = draftRub.Scdrub; //code
                        rub.Zcod = draftRub.Zcod; //refer
                        rub.Atyp = draftRub.Atyp; //atyp
                        rub.Frsrc = draftRub.Frsrc; //formule
                        rub.Ecmount = draftRub.Ecmount;
                        rub.Ecannu = draftRub.Ecannu;
                        rub.Nbech = draftRub.Nbech;
                        rub.Eta = draftRub.Eta; //etat
                    }
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt && _draftFmt is RubfmtDto draftFmt)
                    {
                        if (draftFmt == null) return;
                        fmt.Idorg = draftFmt.Idorg; //orga
                        fmt.Irub = draftFmt.Irub; //parent
                        fmt.Liba = draftFmt.Liba; //designation
                        fmt.Abg = draftFmt.Abg; //abrege
                        fmt.Zcdrub = draftFmt.Zcdrub; //code
                        fmt.Zcod = draftFmt.Zcod; //refer
                        fmt.Ztyp = draftFmt.Ztyp; //ztyp

                        fmt.Ftsrc = draftFmt.Ftsrc; //formule
                        fmt.Col = draftFmt.Col; //col
                        fmt.Lne = draftFmt.Lne; //lne
                        fmt.Lgtf = draftFmt.Lgtf; //lgtf
                        fmt.Eta = draftFmt.Eta; //etat
                    }
                    break;
                case EntityLevel.Tie:
                    if (entity is TierspDto tie && _draftTie is TierspDto draftTie)
                    {
                        if (draftTie == null) return;
                        tie.Idorg = draftTie.Idorg; //orga
                        tie.Smatri = draftTie.Smatri; //matricule
                        tie.Xmatri = draftTie.Xmatri; //matricule
                        tie.Nom = draftTie.Nom; //abrege
                        tie.Pnom = draftTie.Pnom; //formule-non valider
                        tie.Eta = draftTie.Eta; //etat
                        break;
                    }
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act && _draftAct is ActsaieDto draftAct)
                    {
                        if (draftAct == null) return;
                        act.Idorg = draftAct.Idorg; //orga
                        act.Itie = draftAct.Itie; //parent
                        act.Scdrub = draftAct.Scdrub; //code
                        act.Liba = draftAct.Liba; //designation
                        act.Abg = draftAct.Abg; //abrege
                        ////act.Zcod = draftAct.Zcod; //refer
                        act.Atyp = draftAct.Atyp; //atyp
                        ////act.Frsrc = draftAct.Frsrc; //formule
                        act.Ecmount = draftAct.Ecmount;
                        act.Ecannu = draftAct.Ecannu;
                        act.Nbech = draftAct.Nbech;
                        act.Eta = draftAct.Eta; //etat
                    }
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt && _draftAdt is ActdetDto draftAdt)
                    {
                        if (draftAdt == null) return;
                        adt.Idorg = draftAdt.Idorg; //orga
                        adt.Iact = draftAdt.Iact; //parent
                        adt.Liba = draftAdt.Liba; //designation
                        adt.Abg = draftAdt.Abg; //abrege
                        adt.Zcdrub = draftAdt.Zcdrub; //code
                        ////adt.Zcod = draftAdt.Zcod; //refer
                        adt.Atyp = draftAdt.Atyp; //ztyp

                        ////adt.Ftsrc = draftAdt.Ftsrc; //formule
                        adt.Eta = draftAdt.Eta; //etat
                    }
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res && _draftRes is ResdonDto draftRes)
                    {
                        if (draftRes == null) return;
                        res.Idorg = draftRes.Idorg; //orga
                        res.Itie = draftRes.Itie; //parent
                        res.Iact = draftRes.Iact; //parent2
                        res.Scdrub = draftRes.Scdrub; //code
                        //res.Liba = draftRes.Liba; //designation
                        //res.Abg = draftRes.Abg; //abrege
                        ////res.Zcod = draftRes.Zcod; //refer
                        res.Atyp = draftRes.Atyp; //atyp
                        //res.Ecmount = draftRes.Ecmount;
                        //res.Ecannu = draftRes.Ecannu;
                        //res.Nbech = draftRes.Nbech;
                        res.Eta = draftRes.Eta; //etat
                        break;
                    }
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt && _draftRdt is ResdetDto draftRdt)
                    {
                        if (draftRdt == null) return;
                        rdt.Idorg = draftRdt.Idorg; //orga
                        rdt.Ires = draftRdt.Ires; //parent
                        //rdt.Liba = draftRdt.Liba; //designation
                        //rdt.Abg = draftRdt.Abg; //abrege
                        rdt.Zcdrub = draftRdt.Zcdrub; //code
                        ////rdt.Zcod = draftRdt.Zcod; //refer
                        rdt.Atyp = draftRdt.Atyp; //ztyp

                        rdt.Eta = draftRdt.Eta; //etat
                    }
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro && _draftBro is ResbroDto draftBro)
                    {
                        if (draftBro == null) return;
                        bro.Idorg = draftBro.Idorg; //orga
                        bro.Itie = draftBro.Itie; //parent
                        bro.Iact = draftBro.Iact; //parent2
                        bro.Scdrub = draftBro.Scdrub; //code
                        //bro.Liba = draftBro.Liba; //designation
                        //bro.Abg = draftBro.Abg; //abrege
                        ////bro.Zcod = draftBro.Zcod; //refer
                        bro.Atyp = draftBro.Atyp; //atyp
                        //bro.Ecmount = draftBro.Ecmount;
                        //bro.Ecannu = draftBro.Ecannu;
                        //bro.Nbech = draftBro.Nbech;
                        bro.Eta = draftBro.Eta; //etat
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void RestoreOriginalGridItem(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan &&
                        _edPlnOriginals.TryGetValue(plan.Rowguid, out var originalPlan))
                    {
                        plan.Rowguid = originalPlan.Rowguid;
                        plan.Idorg = originalPlan.Idorg;
                        plan.Liba = originalPlan.Liba;
                        plan.Abg = originalPlan.Abg;
                        plan.Fpsrc = originalPlan.Fpsrc;
                        plan.Eta = originalPlan.Eta;
                    }
                    break;

                case EntityLevel.Rub:
                    if (entity is RubvarDto rub &&
                        _edRubOriginals.TryGetValue(rub.Rowguid, out var originalRub))
                    {
                        rub.Rowguid = originalRub.Rowguid;
                        rub.Idorg = originalRub.Idorg;
                        rub.Ipln = originalRub.Ipln;
                        rub.Liba = originalRub.Liba;
                        rub.Abg = originalRub.Abg;
                        rub.Scdrub = originalRub.Scdrub;
                        rub.Zcod = originalRub.Zcod;
                        rub.Atyp = originalRub.Atyp;
                        rub.Frsrc = originalRub.Frsrc;
                        rub.Ecmount = originalRub.Ecmount;
                        rub.Ecannu = originalRub.Ecannu;
                        rub.Nbech = originalRub.Nbech;
                        rub.Eta = originalRub.Eta;
                    }
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt &&
                        _edFmtOriginals.TryGetValue(fmt.Rowguid, out var originalFmt))
                    {
                        fmt.Rowguid = originalFmt.Rowguid;
                        fmt.Idorg = originalFmt.Idorg; //orga
                        fmt.Irub = originalFmt.Irub; //parent
                        fmt.Liba = originalFmt.Liba; //designation
                        fmt.Abg = originalFmt.Abg; //abrege
                        fmt.Zcdrub = originalFmt.Zcdrub; //code
                        fmt.Zcod = originalFmt.Zcod; //refer
                        fmt.Ztyp = originalFmt.Ztyp; //ztyp

                        fmt.Ftsrc = originalFmt.Ftsrc; //formule
                        fmt.Col = originalFmt.Col; //col
                        fmt.Lne = originalFmt.Lne; //lne
                        fmt.Lgtf = originalFmt.Lgtf; //lgtf
                        fmt.Eta = originalFmt.Eta; //etat
                    }
                    break;
            }
        }
        protected object? GetDraft(EntityLevel level)
        {
            return level switch
            {
                EntityLevel.Plan => _draftPlan,
                EntityLevel.Rub => _draftRub,
                EntityLevel.Fmt => _draftFmt,
                EntityLevel.Tie => _draftTie,
                EntityLevel.Act => _draftAct,
                EntityLevel.Adt => _draftAdt,
                EntityLevel.Res => _draftRes,
                EntityLevel.Rdt => _draftRdt,
                EntityLevel.Bro => _draftBro,
                _ => throw new ArgumentOutOfRangeException(nameof(level))
            };
        }
        protected void SetDraft(EntityLevel level, object? draft)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    _draftPlan = draft;
                    break;
                case EntityLevel.Rub:
                    _draftRub = draft;
                    break;
                case EntityLevel.Fmt:
                    _draftFmt = draft;
                    break;
                case EntityLevel.Hie:
                    _draftRub = draft;
                    break;
                case EntityLevel.Pst:
                    _draftFmt = draft;
                    break;
                case EntityLevel.Tie:
                    _draftTie = draft;
                    break;
                case EntityLevel.Act:
                    _draftAct = draft;
                    break;
                case EntityLevel.Adt:
                    _draftAdt = draft;
                    break;
                case EntityLevel.Res:
                    _draftRes = draft;
                    break;
                case EntityLevel.Rdt:
                    _draftRdt = draft;
                    break;
                case EntityLevel.Bro:
                    _draftBro = draft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void FinalizeConfirmedState(EntityLevel level, object entity, string message)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);

            ResetRowState(level, rowguid);

            var s = GetEditState(level);
            s.IsAdd = false;
            s.IsEdit = false;
            s.AddRowguid = null;
            s.EditRowguid = null;
            s.DeleteRowguid = null;

            EndRowEdit(); //
            StateHasChanged();
        }

        protected static void ReplaceByRowguid<T>(List<T> items, T entity, Func<T, Guid> getRowguid)
        {
            var rowguid = getRowguid(entity);
            var index = items.FindIndex(x => getRowguid(x) == rowguid);
            if (index >= 0)
                items[index] = entity;
        }
        //protected override void EndEdit(EntityLevel level)
        //{
        //    EndRowEdit();
        //}
    }
}
