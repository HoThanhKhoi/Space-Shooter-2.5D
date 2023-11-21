using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemyContainer;
    private bool _stopSpawning = false;

    [SerializeField] private GameObject[] powerups; 

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleShotPowerUpRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemy);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnTripleShotPowerUpRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (_stopSpawning == false)
        {
            int randomPowerups = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerups]);

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
