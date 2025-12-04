using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.EnemyLogic
{
    public class EnemyComponent : MonoBehaviour
    {
        private EnemyAI _enemyAI;

        [SerializeField] private float hp, maxHp;
        
        [SerializeField] private List<AudioSource> spawnSound, deathSound;
        [SerializeField] private ParticleSystem gameOverEffect;

        private void Start()
        {
            _enemyAI = transform.GetComponent<EnemyAI>();
            int chance = Random.Range(1, 10);
            if (chance <= 2)
            {
                int chance2 = Random.Range(0, spawnSound.Count);
                spawnSound[chance2].Play();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Tile"))
            {
                transform.SetParent(other.transform);
            }
        }

        public void TakeDamage(float damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }

        public void FinalGame()
        {
            StartCoroutine(Death());
        }
        public void Die()
        {
            StartCoroutine(Death());
            _enemyAI.GetPoints((int)(0.1 * maxHp));
            int chance = Random.Range(0, deathSound.Count);
            deathSound[chance].Play();
        }
        IEnumerator Death()
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
            transform.DOMoveY(-3, 3);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}