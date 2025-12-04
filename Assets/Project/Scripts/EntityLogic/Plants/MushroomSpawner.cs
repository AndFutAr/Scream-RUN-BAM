using System.Collections;
using UnityEngine;

namespace Project.Scripts.GameLogic.Spawners
{
    public class MushroomSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject mushroomPrefab;

        [SerializeField] private float spawnRate;
        [SerializeField] private int count;
        [SerializeField] private float minRadius = 4f, maxRadius = 10f;

        [SerializeField] private Transform player;

        public void SpawnMushroom()
        {
            StartCoroutine(SpawnerMushroom());
        }
        public IEnumerator SpawnerMushroom()
        {
            count = 1;
            for (int i = 0; i < count; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * minRadius;
                Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        
                float randomDistance = Random.Range(minRadius, maxRadius);
        
                spawnPosition = player.position + (spawnPosition - player.position).normalized * randomDistance;
                spawnPosition.y = 0;

                GameObject mushroom = Instantiate(mushroomPrefab, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}