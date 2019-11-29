namespace UI
{
    // IGameState defines the transition method of the menu state machine.
    public interface IGameState
    {
        void OnStart();
        void OnStop();
        void OnPause();
        void OnResume();
    }
}
