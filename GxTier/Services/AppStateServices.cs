namespace GxTie.Services
{
    public class AppState
    {
        public event Action? OnChange;
        private bool _isTieLoggedIn;
        public bool IsTieLoggedIn
        {
            get => _isTieLoggedIn;
            set
            {
                if (_isTieLoggedIn != value)
                {
                    Console.WriteLine($"AppState has changed {_isTieLoggedIn}");
                    _isTieLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
