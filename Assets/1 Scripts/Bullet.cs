using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IShot
{
    public void shot()
    {
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * 100);

        StartCoroutine(fire());
    }
    IEnumerator fire()
    {
        float timeCounter = 0f;
        while(timeCounter < 3f)
        {
            timeCounter += Time.deltaTime;
            this.GetComponent<Rigidbody>().AddForce(transform.forward * 300);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
