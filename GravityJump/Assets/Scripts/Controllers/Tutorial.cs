using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;

namespace Controllers
{
    public class Tutorial : BaseController
    {
        private new void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            this.MusicPlayer.Play(Audio.MusicPlayer.MusicClip.Menu, true);
        }
    }
}
