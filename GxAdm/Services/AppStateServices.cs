namespace GxAdm.Services
{
    public class AppState
    {
        public event Action? OnChange;

        private bool _isAdmLoggedIn;
        public bool IsAdmLoggedIn
        {
            get => _isAdmLoggedIn;
            set
            {
                if (_isAdmLoggedIn != value)
                {
                    Console.WriteLine($"AppState has changed {_isAdmLoggedIn}");
                    _isAdmLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
