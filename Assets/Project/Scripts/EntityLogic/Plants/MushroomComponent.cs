using System;
using System.Collections.Generic;
using Project.Scripts.PlayerLogic;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class MushroomComponent : MonoBehaviour
    {
        [SerializeField] private int increasedHealth = 5;
        private float healthTime = 10;
        [SerializeField] private List<AudioSource> putSound;

        private void Update()
        {
            healthTime -= Time.deltaTime;
            if (healthTime <= 0)
                Destroy(gameObject);
        }

        public void UseMushroom(PlayerComponent _player)
        {
            _player.IncreaseHp(increasedHealth);
            int chance = UnityEngine.Random.Range(0, putSound.Count);
            putSound[chance].Play();
            Destroy(this.gameObject);
        }
    }
}