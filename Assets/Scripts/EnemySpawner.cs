using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] Transform spawnPos;
    [SerializeField] float spawnTimer;
    [SerializeField] bool doNotDespawn;

    void SpawnEnemy(float timer) {
        // Not sure if I should allow overwrite
        doNotDespawn = false;
        enemy.transform.position = spawnPos.position;
        enemy.gameObject.SetActive(true);
        enemy.SetEnemyAttributes(spawnTimer, false, spawnPos);
        gameObject.SetActive(false);
    }

    void SpawnEnemyNoDespawn() {
        doNotDespawn = true;
        enemy.transform.position = spawnPos.position;
        enemy.gameObject.SetActive(true);
        enemy.SetEnemyAttributes(spawnTimer, true, spawnPos);
        gameObject.SetActive(false);
    }
}
