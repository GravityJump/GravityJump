using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreen : BasicScreen
    {

        public Button Back;
        public Button Resume;

        public override void Awake()
        {
            this.Panel = GameObject.Find("GameController/HUD/PauseScreen/");
            this.Back = GameObject.Find("GameController/HUD/PauseScreen/BackButton").GetComponent<Button>();
            this.Resume = GameObject.Find("GameController/HUD/PauseScreen/ResumeButton").GetComponent<Button>();
        }

        public override void OnStart()
        {
            Time.timeScale = 0f;
            this.Panel.SetActive(true);
        }

        public override void OnStop()
        {
            this.Panel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
