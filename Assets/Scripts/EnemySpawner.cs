using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 30f;
    public float checkRadius = 2f;
    int enemiesToSpawn = 20;

    NavMeshTriangulation triangulation;

    void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
    }

    public void BeginSpawn()
    {
        int spawnedEnemies = 0;
        while (spawnedEnemies < enemiesToSpawn)
        {
            StartCoroutine(SpawnEnemy());
            spawnedEnemies++;
        }
    }

    public IEnumerator SpawnEnemy()
    {
        GenEnemy();
        yield return new WaitForSeconds(1f);
    }

    void GenEnemy()
    {
        int index = Random.Range(0, triangulation.vertices.Length);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(triangulation.vertices[index], out hit, 2f, -1))
        {
            int i = Random.Range(0, 6);
            GameObject enemy = Instantiate(enemyPrefab, hit.position, transform.rotation);
            enemy.GetComponent<WeaponController>().weaponData = GameManager.gameManager.weaponType[i];             
            enemy.GetComponent<WeaponController>().ammo = GameManager.gameManager.weaponType[i].maxAmmo;
        }
        else Debug.LogError("action failed");
    }
}
