using System.Collections;
using System.Collections.Generic;
using Project.Scripts.GameLogic.Spawners;
using Project.Scripts.PlayerLogic;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class TimeReader : MonoBehaviour
    {
        private bool isGame = true;
        public void GameOver() => isGame = false;
        
        [SerializeField] private int enemyCooldown;
        [SerializeField] private List<AudioSource> startSound, endSound;
        [SerializeField] private List<AudioSource> gameSound;

        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private MushroomSpawner mushroomSpawner;
        [SerializeField] private PlayerComponent playerComponent;
        
        private void Start()
        {
            NewGame();
            int chance = Random.Range(0, startSound.Count);
            startSound[chance].Play();
            
            int chance2 = Random.Range(0, gameSound.Count);
            gameSound[chance2].Play();
        }
        
        private void NewGame()
        {
            isGame = true;
            StartCoroutine(MushroomSpawner());
            StartCoroutine(EnemySpawner());
            StartCoroutine(GetPoints());
        }
        public void StopGame()
        {
            isGame = false;
            StopAllCoroutines();
            enemySpawner.StopGame();
            StartCoroutine(GameStop());
        }

        IEnumerator GameStop()
        {
            yield return new WaitForSeconds(2.4f);
            int chance = Random.Range(0, endSound.Count);
            endSound[chance].Play();
        }

        IEnumerator MushroomSpawner()
        {
            while (isGame)
            {
                yield return new WaitForSeconds(15);
                mushroomSpawner.SpawnMushroom();
            }
        }
        IEnumerator EnemySpawner()
        {
            while(isGame)
            {
                yield return new WaitForSeconds(enemyCooldown);
                enemySpawner.SpawnEnemy();
            }
        }

        IEnumerator GetPoints()
        {
            while (isGame)
            {
                yield return new WaitForSeconds(1);
                playerComponent.IncreasePoints(1);
            }
        }
    }
}