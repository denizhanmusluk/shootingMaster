using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDirection : MonoBehaviour
{
    [SerializeField] Transform player;
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.eulerAngles.y, transform.eulerAngles.z);
    }
}
