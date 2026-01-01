using System;
using Project.Scripts.PlayerLogic.RuneLogic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Project.Scripts.GameLogic.EnemyLogic;

public class AirSpell : Spell
{
    public override void Cast(Vector3 position, float strength)
    {
        center = position;
        factor = strength;
        if (effectPrefab != null)
        {
            effectPrefab = Instantiate(effectPrefab, position, Quaternion.identity);
            effectPrefab.transform.SetParent(transform);
        }
        int chance = UnityEngine.Random.Range(0, spellStartSound.Count);
        spellStartSound[chance].Play();
        
        StartCoroutine(DoDamageOverTime());
    }

    private IEnumerator DoDamageOverTime()
    {
        yield return new WaitForSeconds(0.3f);
        spellSound.Play();

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Vector3 direction = hitCollider.transform.position - center;
                direction.y = center.y;
                direction.Normalize();
                direction = new Vector3(direction.x * radius * 2, direction.y, direction.z * radius * 2);
                hitCollider.transform.DOMove(hitCollider.transform.position + direction, 1);
                
                hitCollider.GetComponent<EnemyComponent>().TakeDamage(damage * factor);
                if (spellType != SpellType.none)
                {
                    if (spellType == SpellType.empty)
                        hitCollider.GetComponent<EnemyComponent>().ClearEffects();
                    else
                        hitCollider.GetComponent<EnemyComponent>().ApplyEffect(damageEffect, duration, enemyEffectPrefab);
                }
            }
        }

        yield return new WaitForSeconds(duration - 0.3f);
        Destroy(gameObject);
    }
}