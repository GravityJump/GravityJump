using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // HUD displays the score during a game.
    public class HUD : MonoBehaviour
    {
        public float Distance { get; set; }
        private Text DistanceText { get; set; }
        private Text GameOverWin { get; set; }
        private Text GameOverLose { get; set; }

        private void Awake()
        {
            this.Distance = 0f;
            this.DistanceText = GameObject.Find("GameController/HUD/Distance").GetComponent<Text>();
            this.GameOverLose = GameObject.Find("GameController/HUD/YouLose").GetComponent<Text>();
            this.GameOverWin = GameObject.Find("GameController/HUD/YouWin").GetComponent<Text>();
        }

        private void Start()
        {
            this.GameOverWin.gameObject.SetActive(false);
            this.GameOverLose.gameObject.SetActive(false);
        }

        public void GameOver(bool didWin)
        {
            if (didWin)
            {
                this.GameOverWin.gameObject.SetActive(true);
            }
            else
            {
                this.GameOverLose.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            this.DistanceText.text = $"Distance {this.Distance.ToString("0.00")} a.l.";
        }

        public void UpdateDistance(float instantaneousSpeed, float timeDelta)
        {
            this.Distance += instantaneousSpeed * timeDelta;
        }
    }
}
