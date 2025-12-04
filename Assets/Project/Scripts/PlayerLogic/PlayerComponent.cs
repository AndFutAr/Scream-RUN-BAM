using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.GameLogic;
using Project.Scripts.PlayerLogic.Drawing;
using Project.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Project.Scripts.PlayerLogic
{
    public class PlayerComponent : MonoBehaviour
    {
        [Header("Components")]
        private PlayerMovement playerMovement;
        [SerializeField] private TimeReader timeReader;
        [SerializeField] private MapBuilder mapBuilder;
        [SerializeField] private BarkUI barkUI;
        [SerializeField] private LineGeneration lineGeneration;
        
        [Header("Map")]
        private TileComponent currentTile;
        private int cyclePoints = 0, maxPoints = 0;
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private GameObject MainMenu, GameMenu;

        [Header("HP")]
        [SerializeField] private LayerMask mushroomLayer;
        [SerializeField] private int hp;
        [SerializeField] private int maxHp;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private List<AudioSource> damageSound, deathSound;
        [SerializeField] private List<AudioSource> deathOutSound;

        [SerializeField] private float factor = 1;
        [SerializeField] private GameObject playerHP, progressLine;

        [Header("BirchBark")]
        [SerializeField] private LayerMask birchLayer;
        private bool isFell = false;
        [SerializeField] private int birchBackCount;
        [SerializeField] private int maxBirchBackCount;
        [SerializeField] private TMP_Text birchText;
        [SerializeField] private List<AudioSource> birchSound;

        public void IncreaseHp(int value)
        {
            if (hp + value >= maxHp)
                hp = maxHp;
            else hp += value;
            
            factor = hp * 1.0f / maxHp * 1.0f;
            progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
            progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
        }
        public void DecreaseHp(int value)
        {
            int chance = Random.Range(0, damageSound.Count);
            damageSound[chance].Play();
            if (hp - value <= 0)
            {
                hp = 0;
                StartCoroutine(Death());
            }
            else hp -= value;
            
            factor = hp * 1.0f / maxHp * 1.0f;
            progressLine.transform.localScale = new Vector3(2.2f * factor, 0.5f, 0.25f);
            progressLine.transform.localPosition = new Vector3((1 - factor) * 1.1f, 0, 0);
        }

        public void IncreasePoints(int value)
        {
            cyclePoints += value;
            pointsText.text = cyclePoints.ToString();
        }

        public bool GetBirchBark()
        {
            if (birchBackCount > 0)
            {
                birchBackCount--;
                return true;
            }
            return false;
        }
        public void IncreaseBirchBackCount(int value)
        {
            if (birchBackCount + value >= maxBirchBackCount)
                birchBackCount = maxBirchBackCount;
            else birchBackCount += value;
        }
        public void IncreaseMaxBirchBackCount(int value)
        {
            maxBirchBackCount += value;
        }


        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            hp = maxHp;
            cyclePoints = 0;
            pointsText.text = cyclePoints.ToString();
            birchBackCount = 0;
        }

        private void Update()
        {
            birchText.text = birchBackCount.ToString() + "/" + maxBirchBackCount.ToString();

            playerHP.transform.LookAt(Camera.main.transform);
        }

        IEnumerator Death()
        {
            timeReader.StopGame();
            yield return new WaitForSeconds(0.2f);
            playerMovement.Die();
            playerMovement.enabled = false;
            barkUI.enabled = false;
            yield return new WaitForSeconds(1f);
            deathParticles.gameObject.SetActive(true);
            
            int chance = Random.Range(0, deathSound.Count);
            deathSound[chance].Play();
            deathParticles.Play();
            yield return new WaitForSeconds(1f);
            int chance2 = Random.Range(0, deathOutSound.Count);
            deathOutSound[chance2].Play();
            
            maxPoints = PlayerPrefs.GetInt("MaxPoints");
            if (cyclePoints >= maxPoints)
                PlayerPrefs.SetInt("MaxPoints", cyclePoints);
            
            GameMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Mushroom"))
            {
                other.transform.GetComponentInParent<MushroomComponent>().UseMushroom(this);
            }
            
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Tile"))
            {
                if (currentTile == null)
                {
                    currentTile = other.gameObject.GetComponentInParent<TileComponent>();
                }
                else if (other.gameObject != currentTile.gameObject)
                {
                    TileComponent newTile = other.gameObject.GetComponentInParent<TileComponent>();
                    mapBuilder.GridShift(newTile.IndexI - currentTile.IndexI, newTile.IndexJ - currentTile.IndexJ);
                    currentTile = newTile;
                }
            }
        }

        private void OnCollisionStay(Collision birch)
        {
            if (birch.gameObject.layer == LayerMask.NameToLayer("Birch") /*&&  lineGeneration.enabled == false*/)
            {
                birch.transform.GetComponentInParent<BirchComponent>().SetHP();

                if (Input.GetMouseButton(1) && !isFell)
                {
                    StartCoroutine(Fell(birch.gameObject));
                }
            }
        }
        private IEnumerator Fell(GameObject birch)
        {
            isFell = true;
            playerMovement.SetHit(true);
            int chance = Random.Range(0, birchSound.Count);
            birchSound[chance].Play();
            yield return new WaitForSeconds(1f);
            birch.transform.GetComponentInParent<BirchComponent>().ClickBirch(this);
            yield return new WaitForSeconds(1f);
            playerMovement.SetHit(false);
            isFell = false;
        }
    }
}