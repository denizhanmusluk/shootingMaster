using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.tag == "zombie")
        {
            Debug.Log("collision zombi");
            GameObject zombi = collision.transform.root.gameObject;
            zombi.GetComponent<Enemy>().enemyHit(collision.transform, this.transform);
        }
    }
}
