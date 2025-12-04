using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class TileComponent : MonoBehaviour
    {
        [SerializeField] private int indexI, indexJ;
        
        public int IndexI
        {
            get => indexI;
            set => indexI = value;
        }

        public int IndexJ
        {
            get => indexJ;
            set => indexJ = value;
        }
    }
}