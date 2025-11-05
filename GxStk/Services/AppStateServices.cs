namespace GxStk.Services
{
    public class AppState
    {
        public event Action? OnChange;

        private bool _isStkLoggedIn;
        public bool IsStkLoggedIn
        {
            get => _isStkLoggedIn;
            set
            {
                if (_isStkLoggedIn != value)
                {
                    Console.WriteLine($"AppState has changed {_isStkLoggedIn}");
                    _isStkLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
