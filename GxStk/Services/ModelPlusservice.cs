using GxWapi.DaModels;

namespace GxStk.Services
{
    interface IModelPlusService
    {
        Task<Gsgfix> GetGvar(int Jseq, List<Gsgfix> myGlist);
        Task<Rubvar> GetVrub(int Jseq, List<Rubvar> myVlist);
        Task<Rubhie> GetVhie(int Jseq, List<Rubhie> myHlist);
    }
    public class ModelPlusService : IModelPlusService
    {
        public async Task<Gsgfix> GetGvar(int Jseq, List<Gsgfix> myGlist)
        {
            var selGvar = myGlist.FirstOrDefault(ug => ug.Xseq == Jseq);
            return selGvar;
        }
        public async Task<Rubvar> GetVrub(int Jseq, List<Rubvar> myVlist)
        {
            var selVrub = myVlist.FirstOrDefault(ug => ug.Id == Jseq);
            return selVrub;
        }
        public async Task<Rubhie> GetVhie(int Jseq, List<Rubhie> myHlist)
        {
            var selVhie = myHlist.FirstOrDefault(ug => ug.Id == Jseq);
            return selVhie;
        }
    }    
}
