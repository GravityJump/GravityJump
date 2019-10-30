using UnityEngine;

namespace Decors
{
    public class Corpse : Decor
    {
        void Start()
        {
            this.transform.position = new Vector3(this.transform.position.x, Random.value * 10 - 5, Random.value * -5 + 1);
            this.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            GetComponent<Rigidbody2D>().angularVelocity = Random.value * 20 - 10;
        }
    }
}
