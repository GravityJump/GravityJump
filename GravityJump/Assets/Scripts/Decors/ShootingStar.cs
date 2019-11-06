using UnityEngine;

namespace Decors
{
    public class ShootingStar : Decor
    {
        private float delay;
        private float duration;
        private Vector3 speed;

        private TrailRenderer render;
        void Start()
        {
            this.delay = 10 * Random.value;
            this.duration = 2 * Random.value;
            this.speed = new Vector3(Random.value, Random.value, 0);
            this.speed.Normalize();
            this.speed *= (Random.value + 1) * 30;
            this.transform.position = new Vector3(Random.value * 30 - 15 + this.transform.position.x, Random.value * 30 - 15, 20);
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.render = GetComponent<TrailRenderer>();
            this.render.enabled = false;
        }

        void Update()
        {
            this.delay -= Time.deltaTime;
            if (this.delay < -2 * this.duration)
            {
                Destroy(this.gameObject);
            }
            else if (this.delay < -this.duration)
            {
                this.render.enabled = false;
            }
            else if (this.delay < 0)
            {
                this.gameObject.GetComponent<Renderer>().enabled = true;
                this.render.enabled = true;
                this.transform.position += this.speed * Time.deltaTime;
            }

        }
    }
}
