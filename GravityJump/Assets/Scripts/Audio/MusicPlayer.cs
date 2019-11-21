using UnityEngine;
using System.Collections.Generic;
using System;

namespace Audio
{
    // This class is resonsible for loading music assets from Resources, and exposing methods to play them.
    public class MusicPlayer : MonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }
        private Dictionary<MusicClip, AudioClip> MusicClips;
        private const string MusicResourcesPath = "Audio/";

        // This enum lists all music clips. The name must match the audio file name in Resources.
        public enum MusicClip
        {
            Menu,
            Game,
        }

        // Use this for initialization
        void Awake()
        {
            this.AudioSource = this.gameObject.AddComponent<AudioSource>();
            this.MusicClips = new Dictionary<MusicClip, AudioClip>();
            foreach (MusicClip musicClip in Enum.GetValues(typeof(MusicClip)))
            {
                MusicClips.Add(musicClip, Resources.Load<AudioClip>($"{MusicResourcesPath}{musicClip.ToString("g")}"));
            }
        }

        public void Play(MusicClip musicClip, bool loop)
        {
            this.AudioSource.clip = this.MusicClips[musicClip];
            this.AudioSource.loop = loop;
            this.AudioSource.Play();
        }
    }
}
