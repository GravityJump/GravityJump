using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Animation
{
    // Base class for animating the sprite component of a gameObject.
    // It provides logic that load sprites from the Resources folder and store them grouped by type in a Dictionary.
    // Each animated gameObject must have its own folder in Resources/Animations. This folder contains one subfolder per animation type. Each subfolder contains the animation sprites.
    // Example:
    // 
    // Player
    // ├── Walk
    // │    ├── 1.png
    // │    ├── 2.png
    // │    ├── 3.png
    // ├── Jump
    // │    ├── 1.png
    // │    ├── 2.png
    // │    ├── 3.png
    //
    public abstract class Animator : MonoBehaviour
    {
        // The name of the directory in Resources/Animations that stores the object animations folder.
        // Example: "Player", in the example above.
        protected abstract string GameObjectAnimationsDirectoryName { get; }
        // Store the different animation names and their sprites in a key value pair data structure
        // Note: the animation name will be the name of the folder containing the animation sprites, in Resources
        protected Dictionary<string, Sprite[]> Animations;

        protected void Awake()
        {
            this.Animations = new Dictionary<string, Sprite[]>();

            // Load sprites
            foreach (string animationDirectory in Directory.GetDirectories($"Assets/Resources/Animation/{GameObjectAnimationsDirectoryName}/"))
            {
                // Get the name of the directory, without the path
                string animationType = new DirectoryInfo(animationDirectory).Name;
                string path = $"Animation/{GameObjectAnimationsDirectoryName}/{animationType}";
                this.Animations.Add(animationType, Resources.LoadAll<Sprite>(path));
            }
        }
    }
}
