using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HUD : MonoBehaviour
    {
        private float Distance { get; set; }
        private Text DistanceText;

        void Awake()
        {
            this.Distance = 0f;
            this.DistanceText = GameObject.Find("GameController/HUD/Distance").GetComponent<Text>();
        }

        void Update()
        {
            this.DistanceText.text = $"Distance {this.Distance.ToString("0.00")} a.l.";
        }

        public void UpdateDistance(float instantaniousSpeed, float timeDelta)
        {
            this.Distance += instantaniousSpeed * timeDelta;
        }
    }
}
