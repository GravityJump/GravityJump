using UnityEngine;

public class Boost : Collectible
{
    override public void OnCollect()
    {
        target.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(1000, 0, 0));
        Destroy(gameObject);
    }
}