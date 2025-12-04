using System;
using Project.Scripts.PlayerLogic.RuneLogic;
using UnityEngine;
using System.Collections;
using Project.Scripts.GameLogic.EnemyLogic;

public class AoeSpell : Spell
{
    [SerializeField] private bool isPlayer = false;

    public void SetPlayer(GameObject player)
    {
        if (isPlayer)
            transform.SetParent(player.transform);
    }
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
        float elapsed = 0f;
        
        yield return new WaitForSeconds(0.3f);
        spellSound.Play();
        while (elapsed < duration)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    hitCollider.GetComponent<EnemyComponent>().TakeDamage(damage * factor * Time.deltaTime);
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}