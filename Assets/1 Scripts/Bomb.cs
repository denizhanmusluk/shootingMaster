using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IShot
{
    public void shot()
    {
        StartCoroutine(fire());
        this.GetComponent<Rigidbody>().AddForce(transform.forward * 8000);

    }
    IEnumerator fire()
    {
        float timeCounter = 0f;
        while (timeCounter < 1f)
        {
            timeCounter += Time.deltaTime;
            this.GetComponent<Rigidbody>().AddForce(-Vector3.up * 100 * timeCounter);
            yield return null;
        }


        for (int i = 0; i< this.GetComponent<FieldOfView>().visibleTargets.Count; i++)
        {
            Transform hitPart = this.GetComponent<FieldOfView>().visibleTargets[i].transform;
            hitPart.root.GetComponent<RagdollToggle>().RagdollActivate2(true, 0);

            hitPart.GetComponent<Rigidbody>().AddForce((hitPart.transform.position - transform.position).normalized * 5000);
            Enemy _enemy = hitPart.root.GetComponent<Enemy>();
            _enemy.currentBehaviour = Enemy.States.dead;
            _enemy.health = 0;
            _enemy.healthBarSet();
        }
        var particle = Instantiate(transform.GetChild(1).gameObject, transform.position, Quaternion.identity);
        particle.SetActive(true);
        Destroy(this.gameObject);
    }
}
