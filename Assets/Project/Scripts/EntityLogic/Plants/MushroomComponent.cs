using System;
using System.Collections.Generic;
using Project.Scripts.GameLogic.Spawners;
using Project.Scripts.PlayerLogic;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class MushroomComponent : MonoBehaviour
    {
        [SerializeField] private int increasedHealth = 5;
        private float healthTime = 10;
        [SerializeField] private List<AudioSource> putSound;
        
        private MushroomSpawner spawner;

        private void Update()
        {
            healthTime -= Time.deltaTime;
            if (healthTime <= 0)
                Destroy(gameObject);
        }
        public void SetSpawner(MushroomSpawner spawner) => this.spawner = spawner;

        public void UseMushroom(PlayerComponent _player)
        {
            _player.IncreaseHp(increasedHealth);
            int chance = UnityEngine.Random.Range(0, putSound.Count);
            putSound[chance].Play();
            Destroy(this.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Tile"))
            {
                if (spawner != null)
                    spawner.Spawn();
                Destroy(gameObject);
            }
        }
    }
}