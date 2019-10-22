using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreen : BasicScreen
    {

        Button Back;
        public Button Resume;
        float resumedTimeScale;

        public override void Awake()
        {
            this.Panel = GameObject.Find("GameController/HUD/PauseScreen/");
            this.Back = GameObject.Find("GameController/HUD/PauseScreen/BackButton").GetComponent<Button>();
            this.Resume = GameObject.Find("GameController/HUD/PauseScreen/ResumeButton").GetComponent<Button>();
            this.resumedTimeScale = Time.timeScale;
        }

        public void Start()
        {
            this.Back.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Menu");
            });
        }

        public override void OnStart()
        {
            this.resumedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            this.Panel.SetActive(true);
        }

        public override void OnStop()
        {
            this.Panel.SetActive(false);
            Time.timeScale = this.resumedTimeScale;
        }
    }
}
