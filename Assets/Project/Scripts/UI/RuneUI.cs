using UnityEngine;

namespace Project.Scripts.UI
{
    public class RuneUI : MonoBehaviour
    {
        [SerializeField] private GameObject selectedRune;

        public void SelectRune(bool isRune)
        {
            selectedRune.SetActive(isRune);
        }
    }
}