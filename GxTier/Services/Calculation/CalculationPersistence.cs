using GxFormula.Forasource;

using GxShared.GxGuards;
using GxShared.Interfaces;

using GxTie.StaticHelpers;

namespace GxTie.Services.Calculation
{
    public interface ICalculationPersistence
    {
        Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);
        Task SaveCalcAsync(CalcContext ctx, CalcSession session);
    }
    public sealed class CalculationPersistence : ICalculationPersistence
    {
        private readonly IPendingChangesGuard _guard;

        public CalculationPersistence(IPendingChangesGuard guard)
        {
            _guard = guard;
        }

        public async Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
        {
            // Ensure grid values are synced into Actsaies/Actdets
            SyncGridToSaieSession(session);
            foreach (var act in session.Actsaies)
            {
                act.Itie = ctx.Itie;
                act.Ipln = ctx.Ipln;
                act.Iraw = MyConverters.Trunc1000(act.Iraw).ToString();
            }

            foreach (var det in session.Actdets)
            {
                det.Iraw = MyConverters.Trunc1000(det.Iraw).ToString();
            }
            foreach (var act in session.Actsaies)
                await _guard.TrackInsert("Actsaie", act);

            foreach (var det in session.Actdets)
                await _guard.TrackInsert("Actdet", det);

            if (inSaveMode == PendingSaveMode.Immediate)
                await _guard.FlushAsync();
        }
        public async Task SaveCalcAsync(CalcContext ctx, CalcSession session)
        {
            // Ensure grid values are synced into Actsaies/Actdets
            SyncGridToCalcSession(session);
            foreach (var don in session.Resdons)
            {
                don.Itie = ctx.Itie;
                don.Ipln = ctx.Ipln;
                don.Iraw = MyConverters.Trunc1000(don.Iraw).ToString();
            }

            foreach (var bro in session.Resbros)
            {
                bro.Itie = ctx.Itie;
                bro.Ipln = ctx.Ipln;
                bro.Iraw = MyConverters.Trunc1000(bro.Iraw).ToString();
            }

            foreach (var det in session.Resdets)
                det.Iraw = MyConverters.Trunc1000(det.Iraw).ToString();

            await TrackAndFlushAsync("Resdon", session.Resdons);
            await TrackAndFlushAsync("Resbro", session.Resbros);
            await TrackAndFlushAsync("Resdet", session.Resdets);
        }
        private void SyncGridToSaieSession(SaieSession session)
        {
            // Sync RubVarRows -> Actsaies
            foreach (var row in session.RubVarRows)
            {
                var act = session.Actsaies.FirstOrDefault(a => a.Irub == row.Irub);
                if (act != null)
                {
                    act.Inptvalue = row.InputValue;
                    act.Aval = row.Aval;
                    act.Iraw = row.Iraw;
                    act.Vgpe = row.Vgpe; // <-- ensure Vgpe flows
                }

                // Sync RubFmtRows -> Actdets
                foreach (var detail in row.Details)
                {
                    var det = session.Actdets.FirstOrDefault(d =>
                        d.Irub == detail.Irub && d.Ifmt == detail.Ifmt);

                    if (det != null)
                    {
                        det.Inptvalue = detail.InputValue;
                        det.Aval = detail.Aval;
                        det.Iraw = detail.Iraw;
                        det.Vgpe = detail.Vgpe; // <-- ensure Vgpe flows
                    }
                }
            }
        }
        private void SyncSessionSaieToGrid(SaieSession session)
        {
            foreach (var act in session.Actsaies)
            {
                var row = session.RubVarRows.FirstOrDefault(r => r.Irub == act.Irub);
                if (row != null)
                {
                    row.InputValue = act.Inptvalue ?? row.InputValue;
                    row.Aval = act.Aval ?? row.Aval;
                    row.Iraw = act.Iraw ?? row.Iraw;
                    row.Vgpe = act.Vgpe; // <-- copy back Vgpe
                }
            }

            foreach (var det in session.Actdets)
            {
                var master = session.RubVarRows.FirstOrDefault(r => r.Irub == det.Irub);
                if (master == null) continue;

                var detail = master.Details.FirstOrDefault(d => d.Irub == det.Irub && d.Ifmt == det.Ifmt);
                if (detail != null)
                {
                    detail.InputValue = det.Inptvalue ?? detail.InputValue;
                    detail.Aval = det.Aval ?? detail.Aval;
                    detail.Iraw = det.Iraw ?? detail.Iraw;
                    detail.Vgpe = det.Vgpe; // <-- copy back Vgpe
                }
            }
        }

        private void SyncGridToCalcSession(CalcSession session)
        {
            // Sync RubVarRows -> Actsaies
            //foreach (var row in session.RubVarRows)
            //{
            //    var act = session.Actsaies.FirstOrDefault(a => a.Irub == row.Irub);
            //    if (act != null)
            //    {
            //        act.Inptvalue = row.InputValue;
            //        act.Aval = row.Aval;
            //        act.Iraw = row.Iraw;
            //        act.Vgpe = row.Vgpe; // <-- ensure Vgpe flows
            //    }

            //    // Sync RubFmtRows -> Actdets
            //    foreach (var detail in row.Details)
            //    {
            //        var det = session.Actdets.FirstOrDefault(d =>
            //            d.Irub == detail.Irub && d.Ifmt == detail.Ifmt);

            //        if (det != null)
            //        {
            //            det.Inptvalue = detail.InputValue;
            //            det.Aval = detail.Aval;
            //            det.Iraw = detail.Iraw;
            //            det.Vgpe = detail.Vgpe; // <-- ensure Vgpe flows
            //        }
            //    }
            //}
        }
        private async Task TrackAndFlushAsync<TDto>(string entitySet, IEnumerable<TDto> items) where TDto : class
        {
            foreach (var item in items)
                await _guard.TrackInsert(entitySet, item);


            if (_guard.GetSaveMode() == PendingSaveMode.Immediate)
                return;

            await _guard.FlushAsync();
        }
    }
}
