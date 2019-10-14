using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class Stack
    {
        System.Collections.Generic.Stack<IGameState> stack { get; set; }

        public Stack()
        {
            this.stack = new System.Collections.Generic.Stack<IGameState>();
        }

        public IGameState Top()
        {
            return this.stack.Peek();
        }

        public void Push(IGameState gameState)
        {
            if (this.stack.Count > 0)
            {
                this.Top().OnPause();
            }
            this.stack.Push(gameState);
            this.Top().OnStart();
        }

        public IGameState Pop()
        {
            IGameState gameState = this.stack.Pop();
            gameState.OnStop();

            if (this.stack.Count > 0)
            {
                this.Top().OnResume();
            }

            return gameState;
        }
    }
}
