namespace GxAdm.Services
{
    // LoadingService.cs
    public class LoadingService
    {
        public event Action<bool, string> OnLoadingChanged;
        public async Task Show(string label = "Loading...")
        {
            OnLoadingChanged?.Invoke(true, label);
            await Task.Delay(2);
        }
        public async Task Hide()
        {
            OnLoadingChanged?.Invoke(false, "");
            await Task.Delay(2);
        }
    }
}
