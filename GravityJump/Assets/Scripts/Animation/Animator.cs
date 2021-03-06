﻿using System.Collections.Generic;
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
        // Store the different animation names and their sprites in a key value pair data structure
        protected Dictionary<string, Sprite[]> Animations;
       
        protected virtual float SecondPerImage => 1 / 12f;
        protected float TimeSinceLastImage;
        protected int currentFrameIndex;

        protected void Awake()
        {
            this.Animations = new Dictionary<string, Sprite[]>();
        }

        protected void DisplaySprite(Sprite sprite)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            this.TimeSinceLastImage = 0;
        }

        protected void DisplayFirstSprite(Sprite[] animationSprites)
        {
            // Reset the animation and display it
            this.currentFrameIndex = 0;
            this.DisplaySprite(animationSprites[currentFrameIndex]);
        }

        protected void DisplayNextSprite(Sprite[] animationSprites)
        {
            if (this.TimeSinceLastImage >= this.SecondPerImage)
            {
                this.currentFrameIndex = (this.currentFrameIndex + 1) % animationSprites.Length;
                this.DisplaySprite(animationSprites[currentFrameIndex]);
            }
        }
    }
}
