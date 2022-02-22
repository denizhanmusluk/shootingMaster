using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Point : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(pointUp());
    }

    IEnumerator pointUp()
    {

        float counter = 0;
        float speed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 2 * Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed *= 20;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * speed);
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - (counter / (Mathf.PI / 2)));

            yield return null;
        }
        Destroy(gameObject);
    }

}
