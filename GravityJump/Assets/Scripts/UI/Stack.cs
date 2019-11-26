namespace UI
{
    public class Stack
    {
        private System.Collections.Generic.Stack<IGameState> InternalStack { get; set; }

        public Stack()
        {
            this.InternalStack = new System.Collections.Generic.Stack<IGameState>();
        }

        public IGameState Top()
        {
            return this.InternalStack.Peek();
        }

        public void Push(IGameState gameState)
        {
            if (this.InternalStack.Count > 0)
            {
                this.Top().OnPause();
            }

            this.InternalStack.Push(gameState);
            this.Top().OnStart();
        }

        public IGameState Pop()
        {
            IGameState gameState = this.InternalStack.Pop();
            gameState.OnStop();

            if (this.InternalStack.Count > 0)
            {
                this.Top().OnResume();
            }

            return gameState;
        }

        public int Count()
        {
            return this.InternalStack.Count;
        }
    }
}
