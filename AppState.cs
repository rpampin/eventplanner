using System;
using System.Threading.Tasks;

namespace EventPlanner
{
    public class AppState
    {
        public bool IsLoading { get; private set; }

        public event Action OnChange;

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task RunAsync(Func<Task> func)
        {
            SetIsLoading(true);

            await Task.Run(async () =>
              {
                  try
                  {
                      await func();
                  }
                  finally
                  {
                      SetIsLoading(false);
                      NotifyStateChanged();
                  }
              });
        }
    }
}
