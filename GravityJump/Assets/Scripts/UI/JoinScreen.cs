using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinScreen : MonoBehaviour
{
    public Text IP;
    void Start()
    {
        this.IP.text = $"IP {Network.Utils.GetIPAddress()}";
    }

    void Update()
    {

    }
}
