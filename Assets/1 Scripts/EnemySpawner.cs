using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IStartGameObserver,ILoseObserver
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnPeriod;
    public bool spawnActive = true;
    int spawnPointSelect;
    int spawnPointCount;
    void Start()
    {
        spawnPointCount = transform.childCount;
        GameManager.Instance.Add_StartObserver(this);
        GameManager.Instance.Add_LoseObserver(this);
    }
    public void StartGame()
    {
        StartCoroutine(spawning());
    }
    public void LoseScenario()
    {
        spawnActive = false;
    }


    IEnumerator spawning()
    {
        while (spawnActive)
        {

            spawnPointSelect = Random.Range(0, spawnPointCount);


            var enemy = Instantiate(enemyPrefab, transform.GetChild(spawnPointSelect).transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnPeriod);
        }
    }
}
