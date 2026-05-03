using GxShared.GxDtos;

namespace GxTie.Services
{
    interface IModelPlusService
    {
        Task<GsgfixDto> GetGvar(int Jseq, List<GsgfixDto> myGlist);
        Task<RubvarDto> GetVrub(int Jseq, List<RubvarDto> myVlist);
        Task<RubhieDto> GetVhie(int Jseq, List<RubhieDto> myHlist);
    }
    public class ModelPlusService : IModelPlusService
    {
        public async Task<GsgfixDto> GetGvar(int Jseq, List<GsgfixDto> myGlist)
        {
            var selGvar = myGlist.FirstOrDefault(ug => ug.Xseq == Jseq);
            return selGvar;
        }
        public async Task<RubvarDto> GetVrub(int Jseq, List<RubvarDto> myVlist)
        {
            var selVrub = myVlist.FirstOrDefault(ug => ug.Id == Jseq);
            return selVrub;
        }
        public async Task<RubhieDto> GetVhie(int Jseq, List<RubhieDto> myHlist)
        {
            var selVhie = myHlist.FirstOrDefault(ug => ug.Id == Jseq);
            return selVhie;
        }
    }    
}
