using System.Collections;
using System.Collections.Generic;
using Project.Scripts.GameLogic.EnemyLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.GameLogic.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [FormerlySerializedAs("mushroomPrefabs")] [SerializeField] private List<GameObject> enemyPrefabs;

        [SerializeField] private float spawnRate;
        private int count;
        [SerializeField] private float minRadius = 4f, maxRadius = 10f;
        private List<GameObject> spawnedEnemies = new List<GameObject>();

        [SerializeField] private Transform player;

        public void StopGame()
        {
            StopAllCoroutines();
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                    enemy.transform.GetComponent<EnemyComponent>().FinalGame();
            }
        }
        
        public void SpawnEnemy()
        {
            StartCoroutine(SpawnerEnemy());
        }
        public IEnumerator SpawnerEnemy()
        {
            count = 1;
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(spawnRate);
                Vector2 randomCircle = Random.insideUnitCircle.normalized * minRadius;
                Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        
                float randomDistance = Random.Range(minRadius, maxRadius);
        
                spawnPosition = player.position + (spawnPosition - player.position).normalized * randomDistance;
                spawnPosition.y = 0;

                int index = Random.Range(0, enemyPrefabs.Count);
                
                GameObject enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                enemy.transform.GetComponent<EnemyAI>().SetPlayer(player);
            }
        }
    }
}