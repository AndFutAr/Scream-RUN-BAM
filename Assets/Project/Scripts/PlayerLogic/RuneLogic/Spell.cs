using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.PlayerLogic.RuneLogic
{
    public enum SpellType
    {
        none, 
        empty,
        fire,
        water,
        electric,
        ice,
    }

    public enum DamageEffect
    {
        fire,
        water,
        electric,
        ice,
    }
    public abstract class Spell : MonoBehaviour
    {
        [SerializeField] protected SpellType spellType;
        [SerializeField] protected DamageEffect damageEffect;
        [SerializeField] protected Vector3 center;
        [SerializeField] protected float duration, radius;
        [SerializeField] protected float damage;
        protected float factor;

        [SerializeField] protected GameObject effectPrefab, enemyEffectPrefab;
        [SerializeField] protected List<AudioSource> spellStartSound;
        [SerializeField] protected AudioSource spellSound;

        public abstract void Cast(Vector3 position, float factor);
    }
}