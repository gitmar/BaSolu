using GxShared.Formulas.ForaContext;
using GxShared.Formulas.ForaModels;

using GxWapi.DaModels;

using Microsoft.AspNetCore.Components.Forms;

namespace GxAdm.Components.Opera
{
    public class PersonEvaluationSession
    {
        public long Idtie { get; }
        public DateTime SessionDate { get; }
        public List<FormulaLine> FormulaLines { get; private set; } = new();
        public List<InDataLineStream> Inputs { get; private set; } = new();
        public List<OutDataLineStream> Outputs { get; private set; } = new();
        public PersonEvaluationSession(long idtie, DateTime sessionDate)
        {
            Idtie = idtie;
            SessionDate = sessionDate;
        }
        public void HydrateInputs(IEnumerable<Actsaie> actsaies, IEnumerable<Resdon> resdons)
        {
            Inputs = InputHydrator.Hydrate(Idtie, SessionDate, actsaies, resdons);
        }

        public void LoadFormulaLines(IEnumerable<FormulaLine> registry)
        {
            FormulaLines = registry.Where(l => l.Idtie == Idtie).ToList();
        }

        public void Evaluate()
        {
            var context = new FormulaEvaluationContext
            {
                Idtie = Convert.ToInt32(Idtie),
                InputData = Inputs,
                PreviousLines = FormulaLines,
                SessionDate = SessionDate
            };

            var evaluator = new FormulaOutcomeEvaluator(context);
            Outputs = evaluator.EvaluateAll();
        }

        public void PersistResults(IResultWriter writer)
        {
            writer.Write(Outputs);
        }

        public void Clear()
        {
            Inputs.Clear();
            FormulaLines.Clear();
            Outputs.Clear();
        }
    }
    public static class InputHydrator
    {
        public static List<InDataLineStream> Hydrate(
            long idtie,
            DateTime sessionDate,
            IEnumerable<Actsaie> actsaies,
            IEnumerable<Resdon> resdons)
        {
            var inputs = new List<InDataLineStream>();

            // Top-level Actsaie
            var validActsaies = actsaies
                        .Where(a => a.Idtie == idtie && IsValidPeriod(a.Dtperi, a.Datvalid, sessionDate))
                        .GroupBy(a => a.Scdrub)
                        .Select(g => g.OrderByDescending(a => a.Datvalid).First());

            foreach (var a in validActsaies)
            {
                inputs.Add(MapToInput(a, TableType.Actsaie));

                // Include Actdet children
                if (a.Actdets.Any())
                {
                    foreach (var d in a.Actdets)
                        inputs.Add(MapToInput(d, TableType.Actdet));
                }
            }

            // Top-level Resdon
            var validResdons = resdons
                        .Where(r => r.Idtie == idtie && IsValidPeriod(r.Dtperi, r.Datvalid, sessionDate))
                        .GroupBy(r => r.Scdrub)
                        .Select(g => g.OrderByDescending(r => r.Datvalid).First());
            foreach (var r in validResdons)
            {
                inputs.Add(MapToInput(r, TableType.Resdon));

                // Include Resdet children
                if (r.Resdets.Any())
                {
                    foreach (var d in r.Resdets)
                        inputs.Add(MapToInput(r, TableType.Resdet));
                }
            }

            return inputs;
        }

        public static bool IsValidPeriod(int? dperi, DateTimeOffset? datvalid, DateTime sessionDate)
        {
            return dperi == 1 || (datvalid.HasValue && datvalid.Value.Date == sessionDate.Date);
        }
        private static InDataLineStream MapToInput(dynamic entity, TableType type)
        {
            return new InDataLineStream
            {
                Idtie = entity.Idtie,
                Scdrub = entity.Scdrub,
                Etyp = type,
                Datval = entity.Datval,
                Result = new FormulaResult
                {
                    Value = entity.Value,
                    Type = entity.Type,
                    Trust = TrustLevel.Valid
                },
                Liba = entity.Liba,
                Abg = entity.Abg,
                Nom = entity.Nom,
                Pnom = entity.Pnom,
                Smatri = entity.Smatri
            };
        }
    }
    public interface IResultWriter
    {
        void Write(List<OutDataLineStream> results);
    }
}

