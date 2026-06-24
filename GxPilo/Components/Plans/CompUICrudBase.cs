using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;

namespace GxPilo.Components.Plans
{
    public abstract class CompUICrudBase : MultiLevelCrudBase
    {
        protected readonly List<PlngenDto> PlanItems = new();
        protected readonly List<RubvarDto> RubItems = new();
        protected readonly List<RubfmtDto> FmtItems = new();
        protected readonly List<TierspDto> TieItems = new();
        protected readonly List<ActsaieDto> ActItems = new();
        protected readonly List<ActdetDto> AdtItems = new();
        protected readonly List<ResdonDto> ResItems = new();
        protected readonly List<ResdetDto> RdtItems = new();
        protected readonly List<ResbroDto> BroItems = new();

        protected readonly Dictionary<EntityLevel, EntityEditState> _editStates = new();
        protected readonly Dictionary<Guid, PendingOpType> _rowPendingOpTypeByRow = new();
        protected readonly Dictionary<Guid, Guid> _pendingOpIdsByRow = new();
        protected object? DraftEntity { get; set; }
        protected CompUICrudBase(IPendingChangesGuard guard) : base(guard)
        {
        }
        protected class EntityEditState
        {
            public bool IsAdd { get; set; }
            public bool IsEdit { get; set; }
            public Guid? AddRowguid { get; set; }
            public Guid? EditRowguid { get; set; }
            public Guid? DeleteRowguid { get; set; }
        }
        
        protected EntityEditState GetEditState(EntityLevel level)
        {
            if (!_editStates.TryGetValue(level, out var state))
            {
                state = new EntityEditState();
                _editStates[level] = state;
            }
            return state;
        }

        protected void BeginEdit(EntityLevel level, Guid rowguid, object draft)
        {
            var s = GetEditState(level);
            s.IsAdd = false;
            s.IsEdit = true;
            s.AddRowguid = null;
            s.EditRowguid = rowguid;
            s.DeleteRowguid = null;

            SetDraft(level, draft);
            BeginEditRow(level, rowguid, draft);
            SetRowState(level, rowguid, RowState.EditPending);
            _rowPendingOpTypeByRow[rowguid] = PendingOpType.Update;
        }

        protected void BeginAdd(EntityLevel level, Guid rowguid, object draft)
        {
            var s = GetEditState(level);
            s.IsAdd = true;
            s.IsEdit = false;
            s.AddRowguid = rowguid;
            s.EditRowguid = null;
            s.DeleteRowguid = null;

            SetDraft(level, draft);
            BeginEditRow(level, rowguid, draft);
            SetRowState(level, rowguid, RowState.AddPending);
            _rowPendingOpTypeByRow[rowguid] = PendingOpType.Insert;
        }

        protected void ClearEditState(EntityLevel level)
        {
            _editStates[level] = new EntityEditState();
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
        protected bool IsLevelBusy(EntityLevel level)
        {
            var es = GetEditState(level);
            return es.IsAdd || es.IsEdit;
        }
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
            _rowPendingOpTypeByRow[rowguid] = PendingOpType.Insert;
            SetLightBackground(rowguid, true);
        }
        protected void BeginEditRow(EntityLevel level, Guid rowguid, object draft)
        {
            SetDraft(level, draft);
            StartRowEdit(rowguid, level);
            SetRowState(level, rowguid, RowState.EditPending);
            _rowPendingOpTypeByRow[rowguid] = PendingOpType.Update;
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
        protected override void RemoveFromLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan) PlanItems.RemoveAll(x => x.Rowguid == plan.Rowguid);
                    break;
                case EntityLevel.Rub:
                    if (entity is RubvarDto rub) RubItems.RemoveAll(x => x.Rowguid == rub.Rowguid);
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt) FmtItems.RemoveAll(x => x.Rowguid == fmt.Rowguid);
                    break;
                case EntityLevel.Tie:
                    if (entity is TierspDto tie) TieItems.RemoveAll(x => x.Rowguid == tie.Rowguid);
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act) ActItems.RemoveAll(x => x.Rowguid == act.Rowguid);
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt) AdtItems.RemoveAll(x => x.Rowguid == adt.Rowguid);
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res) ResItems.RemoveAll(x => x.Rowguid == res.Rowguid);
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt) RdtItems.RemoveAll(x => x.Rowguid == rdt.Rowguid);
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro) BroItems.RemoveAll(x => x.Rowguid == bro.Rowguid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void RollbackPendingState(EntityLevel level, object entity, bool isNew)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    if (entity is PlngenDto plan && isNew)
                        PlanItems.RemoveAll(x => x.Rowguid == plan.Rowguid);
                    break;
                case EntityLevel.Rub:
                    if (entity is RubvarDto rub && isNew)
                        RubItems.RemoveAll(x => x.Rowguid == rub.Rowguid);
                    break;
                case EntityLevel.Fmt:
                    if (entity is RubfmtDto fmt && isNew)
                        FmtItems.RemoveAll(x => x.Rowguid == fmt.Rowguid);
                    break;
                case EntityLevel.Tie:
                    if (entity is TierspDto tie && isNew)
                        TieItems.RemoveAll(x => x.Rowguid == tie.Rowguid);
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act && isNew)
                        ActItems.RemoveAll(x => x.Rowguid == act.Rowguid);
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt && isNew)
                        AdtItems.RemoveAll(x => x.Rowguid == adt.Rowguid);
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res && isNew)
                        ResItems.RemoveAll(x => x.Rowguid == res.Rowguid);
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt && isNew)
                        RdtItems.RemoveAll(x => x.Rowguid == rdt.Rowguid);
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro && isNew)
                        BroItems.RemoveAll(x => x.Rowguid == bro.Rowguid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }

            EndRowEdit();
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

            EndEdit(level);
            StateHasChanged();
        }

        protected static void ReplaceByRowguid<T>(List<T> items, T entity, Func<T, Guid> getRowguid)
        {
            var rowguid = getRowguid(entity);
            var index = items.FindIndex(x => getRowguid(x) == rowguid);
            if (index >= 0)
                items[index] = entity;
        }
        protected override void EndEdit(EntityLevel level)
        {
            EndRowEdit();
        }
    }
}
