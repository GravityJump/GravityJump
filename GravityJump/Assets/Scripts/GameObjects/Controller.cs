using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        public bool IsMultiplayer { get; private set; }
        public HUD HUD { get; private set; }

        void Awake()
        {
            this.IsMultiplayer = false;
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<HUD>();
        }

        void Update()
        {
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
        }
    }
}
