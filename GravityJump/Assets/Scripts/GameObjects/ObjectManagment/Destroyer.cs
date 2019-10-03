using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.parent)
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        Destroy(other.gameObject);
    }
}
