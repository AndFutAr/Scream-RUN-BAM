using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.PlayerLogic;
using Project.Scripts.PlayerLogic.Drawing;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class BarkUI : MonoBehaviour
    {
        private bool isRunes = false, isBarked = false;
        [SerializeField] private GameObject runes;
        [SerializeField] private List<GameObject> barks;
        [SerializeField] private GameObject birchBark;
        private Vector3 birchBarkPos;
        private int curBirchBark = 0; 
        
        [SerializeField]private GameObject getBirchBarkText;
        /*[SerializeField] private List<AudioSource> getRuneSound, chooseRuneSound;*/
        
        [SerializeField] private LineGeneration lineGeneration;
        [SerializeField] private SpellManager spellManager;
        [SerializeField] private PlayerComponent _playerComponent;

        private void Start()
        {
            lineGeneration.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && curBirchBark == 0)
            {
                if (!isRunes) ChooseRune();
                else UnchooseRune();
            }

            if (isRunes)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (!isBarked/* && curBirchBark != 1*/)
                    {
                        if (_playerComponent.GetBirchBark()/* || curBirchBark != 0*/)
                            spellManager.CastSpell(1, 1);
                        else
                            StartCoroutine(ShowGetText());
                    }
                    /*else if (curBirchBark == 1)
                    {
                        StartCoroutine(EndDrawRunes());
                    }*/
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (!isBarked/* && curBirchBark != 2*/)
                    {
                        if (_playerComponent.GetBirchBark()/* || curBirchBark != 0*/)
                            spellManager.CastSpell(2, 1);
                        else
                            StartCoroutine(ShowGetText());
                    }
                    /*else if (curBirchBark == 2)
                    {
                        StartCoroutine(EndDrawRunes());
                    }*/
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (!isBarked/* && curBirchBark != 3*/)
                    {
                        if (_playerComponent.GetBirchBark()/* || curBirchBark != 0*/)
                            spellManager.CastSpell(3, 1);
                        else
                            StartCoroutine(ShowGetText());
                    }
                    /*else if (curBirchBark == 3)
                    {
                        StartCoroutine(EndDrawRunes());
                    }*/
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (lineGeneration.enabled == true)
                    {
                        if (!isBarked)
                        {
                            StartCoroutine(EndDrawRunes());
                            _playerComponent.IncreaseBirchBackCount(1);
                        }
                    }
                    else
                        UnchooseRune();
                }

                if (Input.GetKeyDown(KeyCode.F) && lineGeneration.enabled == true)
                {
                    spellManager.CastSpell(curBirchBark, 1);
                    StartCoroutine(EndDrawRunes());
                }
            }
        }

        IEnumerator ShowGetText()
        {
            getBirchBarkText.SetActive(true);
            yield return new WaitForSeconds(1f);
            getBirchBarkText.SetActive(false);
        }

        public void ChooseRune()
        {
            isRunes = true;
            /*int chance = UnityEngine.Random.Range(0, getRuneSound.Count);
            getRuneSound[chance].Play();*/
            runes.transform.DOLocalMoveY(-720, 0.5f);
        }
        public void UnchooseRune()
        {
            isRunes = false;
            /*int chance = UnityEngine.Random.Range(0, getRuneSound.Count);
            getRuneSound[chance].Play();*/
            runes.transform.DOLocalMoveY(-1220, 0.5f);
        }

        IEnumerator EndDrawRunes()
        {
            isBarked = true;
            lineGeneration.enabled = false;
            curBirchBark = 0;
            
            /*int chance = UnityEngine.Random.Range(0, chooseRuneSound.Count);
            chooseRuneSound[chance].Play();*/
            birchBark.transform.DOMove(birchBarkPos, 0.5f);
            birchBark.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            birchBark.transform.GetChild(0).gameObject.SetActive(true);
            birchBark.transform.GetChild(1).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            birchBark.transform.SetParent(runes.transform);
            isBarked = false;
        }
        IEnumerator StartDrawRunes(int index)
        {
            isBarked = true;
            if (birchBark != null)
            {
                yield return StartCoroutine(EndDrawRunes());
                isBarked = true;
            }
            
            curBirchBark = index + 1;
            birchBark = barks[index];
            birchBarkPos = birchBark.transform.position;
            birchBark.transform.SetParent(birchBark.transform.parent.transform.parent);
            birchBark.transform.SetSiblingIndex(index);
            /*int chance = UnityEngine.Random.Range(0, chooseRuneSound.Count);
            chooseRuneSound[chance].Play();*/
            
            birchBark.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f);
            birchBark.transform.DOScale(new Vector3(2, 2, 2), 0.5f);
            birchBark.transform.GetChild(0).gameObject.SetActive(false);
            birchBark.transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            lineGeneration.enabled = true;
            
            isBarked = false;
        }
    }
}