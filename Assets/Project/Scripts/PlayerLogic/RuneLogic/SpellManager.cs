using System.Collections.Generic;
using Project.Scripts.PlayerLogic.RuneLogic;
using UnityEngine;


public class SpellManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpellData
    {
        public int spellID;
        public Spell spellPrefab;
    }

    [SerializeField] private List<SpellData> spells;
    [SerializeField] private GameObject Player;

    public void CastSpell(int ID, float strength)
    {
        foreach (SpellData spell in spells)
        {
            if (spell.spellID == ID)
            {
                Spell spellInstance = Instantiate(spell.spellPrefab);
                if (ID >= 1 && ID <= 3) 
                    spellInstance.transform.GetComponent<AoeSpell>().SetPlayer(Player);
                spellInstance.Cast(Player.transform.position, strength);
            }
        }
    }
}