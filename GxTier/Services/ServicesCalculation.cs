using GxShared.GxDtos;

using Simple.OData.Client;

namespace GxTie.Services.Calculation
{
    public interface IActsaieDataService
    {
        /// <summary>
        /// Loads all Actsaie rows (with their Actdets expanded) for a tier whose linked
        /// template variables (Irub) are valid for the given session date.
        /// A row is only used when Eta == 1 (active); any other Eta (suspended, excluded, ...)
        /// is discarded regardless of Dgpe — operator-controlled visibility.
        /// Validity then follows Dgpe:
        ///   1 = specific session, matched against Isess (yyyyMMdd)
        ///   2 = echeancier — no extra date rule beyond the Eta check
        ///   3 = date range, via Eddat/Efdat
        ///   4 = always valid, no check
        /// </summary>
        Task<(List<ActsaieDto> Actsaies, List<ActdetDto> Actdets)> LoadForTierAsync(
            int tierId, int idorg, DateTime sessionDate, IEnumerable<int> rubvarIds);
    }

    public sealed class ActsaieDataService : IActsaieDataService
    {
        private readonly IODataClient _odaClient;

        public ActsaieDataService(IODataClient odaClient)
        {
            _odaClient = odaClient;
        }

        public async Task<(List<ActsaieDto>, List<ActdetDto>)> LoadForTierAsync(
            int tierId, int idorg, DateTime sessionDate, IEnumerable<int> rubvarIds)
        {
            var rubIds = rubvarIds.Select(id => (int?)id).ToList();
            if (rubIds.Count == 0)
                return (new List<ActsaieDto>(), new List<ActdetDto>());

            var allActsaies = (await _odaClient
                .For<ActsaieDto>("Actsaies")
                .Filter(a => a.Itie == tierId && a.Idorg == idorg && rubIds.Contains(a.Irub))
                .Expand(a => a.Actdets)
                .FindEntriesAsync())
                .ToList();

            var matched = allActsaies
                .Where(a => IsValidForSession(a, sessionDate))
                .GroupBy(a => a.Irub)
                // ASSUMPTION: if more than one row matches the same Irub for this session,
                // take the one with the latest Eddat. Confirm against how corrections/re-entries are stored.
                .Select(g => g.OrderByDescending(a => a.Eddat).First())
                .ToList();

            var allActdets = matched
                .SelectMany(a => a.Actdets ?? new List<ActdetDto>())
                .ToList();

            return (matched, allActdets);
        }

        private static bool IsValidForSession(ActsaieDto a, DateTime sessionDate)
        {
            // Eta gates every row: 1 = active/usable. Anything else (2=suspended, 3=excluded, ...)
            // is discarded regardless of Dgpe.
            if (a.Eta != 1)
                return false;

            return a.Dgpe switch
            {
                4 => true, // always valid, no date/session check
                3 => (!a.Eddat.HasValue || sessionDate.Date >= a.Eddat.Value.Date)
                  && (!a.Efdat.HasValue || sessionDate.Date <= a.Efdat.Value.Date),
                1 => MatchesSession(a.Isess, sessionDate),
                2 => true, // echeancier: no extra date rule beyond the Eta check above
                _ => false
            };
        }

        private static bool MatchesSession(string? isess, DateTime sessionDate)
        {
            if (string.IsNullOrWhiteSpace(isess))
                return false;
            return isess.Trim() == sessionDate.ToString("yyyyMMdd");
        }
    }
}