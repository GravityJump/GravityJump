using UnityEngine;

namespace Decors
{
    public class Corpse : Decor
    {
        void Start()
        {
            float z;
            if (Random.value > 0.2)
            {
                z = -Random.value * 4 - 2;
            }
            else
            {
                z = Random.value * 4 + 2;
            }
            this.transform.position = new Vector3(this.transform.position.x + Random.value * 2, Random.value * 10 - 5, z);
            this.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            GetComponent<Rigidbody2D>().angularVelocity = Random.value * 20 - 10;
        }
    }
}
