using UnityEngine;

namespace Decors
{
    public class ShootingStar : Decor
    {
        private float delay;
        private float duration;
        public Vector3 speed;

        private TrailRenderer render;
        void Start()
        {
            delay = 10 * Random.value;
            duration = 2 * Random.value;
            speed = new Vector3(Random.value, Random.value, 0);
            speed.Normalize();
            speed *= (Random.value + 1) * 30;
            transform.position = new Vector3(Random.value * 30 - 15 + transform.position.x, Random.value * 30 - 15, 20);
            gameObject.GetComponent<Renderer>().enabled = false;
            render = GetComponent<TrailRenderer>();
            render.enabled = false;
        }

        void Update()
        {
            delay -= Time.deltaTime;
            if (delay < -2 * duration)
            {
                Destroy(gameObject);
            }
            else if (delay < -duration)
            {
                render.enabled = false;
            }
            else if (delay < 0)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
                render.enabled = true;
                transform.position += speed * Time.deltaTime;
            }

        }
    }
}
