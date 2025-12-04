using System;
using System.Collections.Generic;
using Project.Scripts.PlayerLogic;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class BirchComponent : MonoBehaviour
    {
        [SerializeField] private int hpClicks, maxHPClicks;
        [SerializeField] private int birchBarkCount = 1;
        
        [SerializeField] private GameObject originalTree, felledTree;
        [SerializeField] private List<AudioSource> hitSound, fellSound;
        
        [SerializeField] private float factor = 1;
        [SerializeField] private GameObject playerHP, progressLine, clickBox;
        
        private PlayerComponent player;

        private void Start()
        {
            originalTree.gameObject.SetActive(true);
            felledTree.gameObject.SetActive(false);
            
            playerHP.SetActive(false);
        }

        private void Update()
        {
            playerHP.transform.LookAt(Camera.main.transform);
        }

        public void SetHP()
        {
            playerHP.SetActive(true);
            clickBox.SetActive(true);
        }
        public void ClickBirch(PlayerComponent _player)
        {
            playerHP.SetActive(true);
            clickBox.SetActive(false);
            player = _player;
            hpClicks--;
            if (hpClicks <= 0)
            {
                int chance = UnityEngine.Random.Range(0, fellSound.Count);
                fellSound[chance].Play();
                player.IncreaseBirchBackCount(birchBarkCount);
                player.IncreasePoints(5);
                originalTree.SetActive(false);
                felledTree.SetActive(true);
            }
            else
            {
                int chance = UnityEngine.Random.Range(0, hitSound.Count);
                hitSound[chance].Play();
            }
            factor = hpClicks * 1.0f / maxHPClicks * 1.0f;
            progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
            progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
        }
    }
}