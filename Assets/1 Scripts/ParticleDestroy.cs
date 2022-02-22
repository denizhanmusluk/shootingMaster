using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destObject());
    }

    IEnumerator destObject()
    {
        yield return new WaitForSeconds(3f);
        gameObject.transform.parent = null;
        Destroy(gameObject);
    }
}
