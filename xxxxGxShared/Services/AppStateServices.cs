namespace GxShared.Services
{
    public class ClieAppState
    {
        public event Action? OnChange;
        public string ogRaison { get; set; } = string.Empty;
        public string usFulnom { get; set; } = string.Empty;
        
        private bool _isGxLoggedIn;
        public bool IsGxLoggedIn
        {
            get => _isGxLoggedIn;
            set
            {
                if (_isGxLoggedIn != value)
                {
                    Console.WriteLine($"AppState has changed {_isGxLoggedIn}");
                    _isGxLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }
        public void UpdateDisplvalue(string raison, string usnom)
        {
            ogRaison = raison;
            usFulnom = usnom;
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
