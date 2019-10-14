namespace UI
{
    public interface IGameState
    {
        void OnStart();
        void OnStop();
        void OnPause();
        void OnResume();
    }
}
