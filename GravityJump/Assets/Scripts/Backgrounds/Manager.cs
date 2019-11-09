using System.Collections.Generic;
using UnityEngine;

namespace Backgrounds
{
    public class Manager : MonoBehaviour
    {
        // On every update, if the camera is at 62,5% or more, we translate the Tile by 25%
        protected List<GameObject> Backgrounds { get; set; }
        protected List<float> Scales { get; set; }
        protected List<float> NextXTrigger { get; set; }
        private void Awake()
        {
            this.Backgrounds = new List<GameObject>();
            this.Scales = new List<float>();
            this.NextXTrigger = new List<float>();
        }
        private void Start()
        {
            for (int index = 1; index < 4; index++)
            {
                // Instantiate the background tile
                GameObject background = Instantiate(Resources.Load("Prefabs/Backgrounds/Background" + index.ToString()) as GameObject);
                this.Backgrounds.Add(background);
                this.Scales.Add(background.transform.localScale.x);
                // 148.51f would be the tiles' width at scale 1.
                this.NextXTrigger.Add(0.5f * background.transform.localScale.x * 148.51f);
            }

        }
        private void Update()
        {
            // On every update, for each tiles, if the camera is at 62,5% or more, we translate the tile by 25%
            for (int index = 0; index < this.Backgrounds.Count; index++)
            {
                if (this.transform.position.x > this.NextXTrigger[index])
                {
                    Vector3 position = this.Backgrounds[index].transform.position;
                    this.Backgrounds[index].transform.position = position + new Vector3(this.Scales[index] * 148.51f, 0, 0);
                    this.NextXTrigger[index] += this.Scales[index] * 148.51f;
                }
            }
        }
    }
}
