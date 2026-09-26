using GxShared.GxDtos;

using Simple.OData.Client;

namespace GxTie.Services.Calculation
{
    public interface IActsaieDataService
    {
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
                .Select(g => g.OrderByDescending(a => a.Eddat).First())
                .ToList();

            var allActdets = matched
                .SelectMany(a => a.Actdets ?? new List<ActdetDto>())
                .ToList();

            return (matched, allActdets);
        }

        // Dgpe: 2 = echeancier (always), 3 = moment (Eddat/Efdat range), 4 = always.
        // Dgpe == 1 (session/Isess) has been retired — every date-bounded case now
        // goes through Dgpe == 3's Eddat/Efdat range instead.
        private static bool IsValidForSession(ActsaieDto a, DateTime sessionDate)
        {
            if (a.Eta != 1)
                return false;

            return a.Dgpe switch
            {
                4 => true,
                2 => true,
                3 => (!a.Eddat.HasValue || sessionDate.Date >= a.Eddat.Value.Date)
                  && (!a.Efdat.HasValue || sessionDate.Date <= a.Efdat.Value.Date),
                _ => false
            };
        }
    }
}