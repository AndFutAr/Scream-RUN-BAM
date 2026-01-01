using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Project.Scripts.PlayerLogic.RuneLogic;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace Project.Scripts.GameLogic.EnemyLogic
{
    [Serializable]
    public class EnemyEffect
    {
        private EnemyComponent enemy;
        private List<EnemyEffect> damages;
        
        private DamageEffect damageEffect;
        [SerializeField] private float time;
        private GameObject effectObject;

        public bool CheakEffect(DamageEffect effect) => damageEffect == effect;
        public void UpdateTime(float _time) => time = _time;
        public void SetEffect(EnemyComponent _enemy, List<EnemyEffect> _damages, DamageEffect _damageEffect,
            float _time, GameObject _effectObject)
        {
            enemy = _enemy;
            damages = _damages;
            damageEffect = _damageEffect;
            time = _time;
            effectObject = _effectObject;
        }
        
        public float SubstanceTime(float value)
        {
            time -= value;
            if (time <= 0)
            {
                enemy.DeleteEffect(effectObject);
                damages.Remove(this);
            }
            return time;
        }
    }
    public class EnemyComponent : MonoBehaviour
    {
        private EnemyAI _enemyAI;

        private Camera mainCamera;
        [SerializeField] private float hp, maxHp;
        private bool isDeath = false;
        [SerializeField] private GameObject baseModel, diedModel;
        
        [SerializeField] private float factor = 1;
        [SerializeField] private GameObject playerHP, progressLine;

        [SerializeField] private List<EnemyEffect> _enemyEffectsList = new List<EnemyEffect>();
        [SerializeField] private List<AudioSource> spawnSound, deathSound;
        [SerializeField] private ParticleSystem gameOverEffect;

        private void Start()
        {
            baseModel.SetActive(true);
            if (diedModel != null)
                diedModel.SetActive(false);
            
            _enemyAI = transform.GetComponent<EnemyAI>();
            mainCamera = Camera.main;
            
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

        private void Update()
        {
            playerHP.transform.LookAt(mainCamera.transform);
            if (_enemyEffectsList != null)
            {
                foreach (EnemyEffect enemyEffect in _enemyEffectsList)
                {
                    if (enemyEffect != null)
                    {
                        if (enemyEffect.SubstanceTime(Time.deltaTime) <= 0)
                            break;
                    }
                }
            }
        }

        public void TakeDamage(float damage)
        {
            hp -= damage;

            if (hp <= 0 && !isDeath)
            {
                hp = 0;
                factor = hp * 1.0f / maxHp * 1.0f;
                progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
                progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
                Die();
            }

            if (!isDeath)
            {
                factor = hp * 1.0f / maxHp * 1.0f;
                progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
                progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
            }
        }
        
        public void ApplyEffect(DamageEffect effect, float time, GameObject effectPrefab)
        {
            bool flag = false;
            foreach (EnemyEffect enemyEffect in _enemyEffectsList)
            {
                if (enemyEffect.CheakEffect(effect))
                {
                    flag = true;
                    enemyEffect.UpdateTime(time);
                }
            }
            
            if (!flag)
            {
                EnemyEffect _effect = new EnemyEffect();
                if (effectPrefab != null)
                {
                    GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
                    effectObject.transform.SetParent(transform);
                    _effect.SetEffect(this, _enemyEffectsList, effect, time, effectObject);
                }
                else
                    _effect.SetEffect(this, _enemyEffectsList, effect, time, null);
                
                _enemyEffectsList.Add(_effect);
            }
        }
        public void ClearEffects() => _enemyEffectsList.Clear();
        public void DeleteEffect(GameObject effectObject)
        {
            _enemyAI.UpdateSpeed(1);
            Destroy(effectObject);
        }

        public void FinalGame()
        {
            StartCoroutine(Death());
        }
        public void Die()
        {
            StartCoroutine(Death());
            isDeath = true;
            if (diedModel != null)
            {
                baseModel.SetActive(false);
                diedModel.SetActive(true);
            }

            _enemyAI.GetPoints((int)(0.1 * maxHp));
            _enemyAI.enabled = false;
            transform.GetComponentInChildren<BoxCollider>().enabled = false;
            int chance = Random.Range(0, deathSound.Count);
            deathSound[chance].Play();
        }
        IEnumerator Death()
        {
            gameOverEffect.gameObject.SetActive(true);
            gameOverEffect.Play();
            transform.DOMoveY(-5, 4);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}