using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class Stack
    {
        System.Collections.Generic.Stack<GameObject> stack;

        public Stack()
        {
            this.stack = new System.Collections.Generic.Stack<GameObject>();
        }

        public void Push(GameObject item)
        {
            if (this.stack.Count > 0)
            {
                this.Top().gameObject.SetActive(false);
            }
            this.stack.Push(item);
            this.Top().gameObject.SetActive(true);
        }

        public GameObject Top()
        {
            return this.stack.Peek();
        }

        public GameObject Pop()
        {
            GameObject item = this.stack.Pop();
            item.gameObject.SetActive(false);
            if (this.stack.Count > 0)
            {
                this.Top().gameObject.SetActive(true);
            }

            return item;
        }
    }
}
