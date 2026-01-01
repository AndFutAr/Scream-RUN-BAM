using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.PlayerLogic;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class BirchComponent : MonoBehaviour
    {
        [SerializeField] private float cycleTime;
        [SerializeField] private int hpClicks, maxHPClicks;
        [SerializeField] private int birchBarkCount = 1;
        
        [SerializeField] private GameObject originalTree, felledTree;
        [SerializeField] private List<AudioSource> hitSound, fellSound;
        
        [SerializeField] private float factor = 1;
        [SerializeField] private GameObject playerHP, progressLine;
        
        private PlayerComponent player;
        private GameObject CameraMain;

        private void Start()
        {
            NewCycle();
        }
        private void NewCycle()
        {
            CameraMain = Camera.main.gameObject;
            player = null;
            
            hpClicks = maxHPClicks;
            originalTree.gameObject.SetActive(true);
            felledTree.gameObject.SetActive(false);
            
            playerHP.SetActive(false);
            factor = hpClicks * 1.0f / maxHPClicks * 1.0f;
            progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
            progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
        }

        private void Update()
        {
            playerHP.transform.LookAt(CameraMain.transform);
        }

        public void SetHP()
        {
            playerHP.SetActive(true);
        }
        public void ClickBirch(PlayerComponent _player)
        {
            playerHP.SetActive(true);
            player = _player;
            hpClicks--;
            if (hpClicks <= 0)
            {
                int chance = UnityEngine.Random.Range(0, fellSound.Count);
                fellSound[chance].Play();
                StartCoroutine(IncludingBirchBark());
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

        IEnumerator IncludingBirchBark()
        {
            originalTree.SetActive(false);
            felledTree.SetActive(true);
            player.IncludeIcon();
            yield return new WaitForSeconds(0.5f);
            
            player.IncreaseBirchBackCount(birchBarkCount);
            player.IncreasePoints(5);

            StartCoroutine(RestartTree());
        }

        IEnumerator RestartTree()
        {
            yield return new WaitForSeconds(cycleTime);
            NewCycle();
        }
    }
}