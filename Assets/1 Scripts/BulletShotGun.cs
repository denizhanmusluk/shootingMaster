using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShotGun : MonoBehaviour, IShot
{
    int ballCount = 20;
    float deviationAngle = 15f;

    public void shot()
    {
        transform.GetChild(1).GetComponent<Rigidbody>().AddForce((transform.right + transform.up) * 100);
        for (int i = 0; i < ballCount; i++)
        {
            var ball = Instantiate(this.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
            ball.SetActive(true);
            ball.transform.parent = this.transform;
            ball.transform.localRotation = Quaternion.Euler(Random.Range(-deviationAngle, deviationAngle), Random.Range(-deviationAngle, deviationAngle), 0);
            ball.GetComponent<Rigidbody>().AddForce(ball.transform.forward * 6000);
        }
    }
}
