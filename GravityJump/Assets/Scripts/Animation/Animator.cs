using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Animation
{
    public abstract class Animator : MonoBehaviour
    {
        protected abstract string AnimationDirectoryPath { get; }
        // Store the different animation types and their sprites in a key value pair data structure
        protected Dictionary<string, Sprite[]> Animations;

        protected void Awake()
        {
            this.Animations = new Dictionary<string, Sprite[]>();

            // Load sprites
            foreach (string animationDirectory in Directory.GetDirectories($"Assets/Resources/{AnimationDirectoryPath}"))
            {
                // Get the name of the directory, without the path
                string animationType = new DirectoryInfo(animationDirectory).Name;
                string path = $"{AnimationDirectoryPath}{animationType}";
                this.Animations.Add(animationType, Resources.LoadAll<Sprite>(path));
            }
        }
    }
}
