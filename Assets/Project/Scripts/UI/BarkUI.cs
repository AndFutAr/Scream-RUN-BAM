using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.PlayerLogic;
using Project.Scripts.PlayerLogic.Drawing;
using UnityEngine;

namespace Project.Scripts.UI
{
    public enum ChosedType
    {
        numbers,
        arrows,
        mouse,
    }
    public class BarkUI : MonoBehaviour
    {
        [SerializeField] private bool isRunes = false;
        [SerializeField] private GameObject runes;
        [SerializeField] private List<GameObject> barks;
        [SerializeField] private GameObject barksCursor, pointer;
        private Vector3 startRunesPos = Vector2.zero;
        private int curBirchBark = 0; 
        private bool chosedRune = false;
        private ChosedType chosedType;
        
        [SerializeField]private GameObject getBirchBarkText;
        
        [SerializeField] private LineGeneration lineGeneration;
        [SerializeField] private SpellManager spellManager;
        [SerializeField] private PlayerComponent _playerComponent;


        private void Update()
        {
            if (Input.GetKey(KeyCode.Space) && isRunes)
            {
                pointer.SetActive(true);
                if (!chosedRune)
                {
                    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) ||
                        Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        chosedRune = true;
                        chosedType = ChosedType.arrows;
                    }

                    if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) ||
                        Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Alpha4))
                    {
                        chosedRune = true;
                        chosedType = ChosedType.numbers;
                    }
                }

                if (chosedRune)
                {
                    if (chosedType == ChosedType.numbers)
                    {
                        if (Input.GetKeyUp(KeyCode.Alpha1)) curBirchBark = 1;
                        if (Input.GetKeyUp(KeyCode.Alpha2)) curBirchBark = 2;
                        if (Input.GetKeyUp(KeyCode.Alpha3)) curBirchBark = 3;
                        if (Input.GetKeyUp(KeyCode.Alpha4)) curBirchBark = 4;
                    }
                    else if (chosedType == ChosedType.arrows)
                    {
                        switch (curBirchBark)
                        {
                            case 0:
                                if (Input.GetKeyUp(KeyCode.LeftArrow)) curBirchBark = 1;
                                if (Input.GetKeyUp(KeyCode.UpArrow)) curBirchBark = 2;
                                if (Input.GetKeyUp(KeyCode.RightArrow)) curBirchBark = 3;
                                if (Input.GetKeyUp(KeyCode.DownArrow)) curBirchBark = 4;
                                break;
                            case 1:
                                if (Input.GetKeyUp(KeyCode.LeftArrow)) curBirchBark = 1;
                                if (Input.GetKeyUp(KeyCode.UpArrow)) curBirchBark = 2;
                                if (Input.GetKeyUp(KeyCode.RightArrow)) curBirchBark = 0;
                                if (Input.GetKeyUp(KeyCode.DownArrow)) curBirchBark = 4;
                                break;    
                            case 2:
                                if (Input.GetKeyUp(KeyCode.LeftArrow)) curBirchBark = 1;
                                if (Input.GetKeyUp(KeyCode.UpArrow)) curBirchBark = 2;
                                if (Input.GetKeyUp(KeyCode.RightArrow)) curBirchBark = 3;
                                if (Input.GetKeyUp(KeyCode.DownArrow)) curBirchBark = 0;
                                break;
                            case 3:
                                if (Input.GetKeyUp(KeyCode.LeftArrow)) curBirchBark = 0;
                                if (Input.GetKeyUp(KeyCode.UpArrow)) curBirchBark = 2;
                                if (Input.GetKeyUp(KeyCode.RightArrow)) curBirchBark = 3;
                                if (Input.GetKeyUp(KeyCode.DownArrow)) curBirchBark = 4;
                                break;
                            case 4:
                                if (Input.GetKeyUp(KeyCode.LeftArrow)) curBirchBark = 1;
                                if (Input.GetKeyUp(KeyCode.UpArrow)) curBirchBark = 0;
                                if (Input.GetKeyUp(KeyCode.RightArrow)) curBirchBark = 3;
                                if (Input.GetKeyUp(KeyCode.DownArrow)) curBirchBark = 4;
                                break;
                        }
                    }
                }
                else
                {
                    chosedType = ChosedType.mouse;
                    
                    Vector3 minRuneDistance, mouseDistance;
                    mouseDistance = (Input.mousePosition - startRunesPos) / 2f;
                    Debug.Log(mouseDistance);
                    float xDist, yDist;
                    xDist = Mathf.Clamp(mouseDistance.x, -300, 300);
                    yDist = Mathf.Clamp(mouseDistance.y, -200, 200);
                    barksCursor.transform.localPosition = new Vector3(xDist, yDist, 0);

                    minRuneDistance = barks[0].transform.position - runes.transform.position;
                    for (int i = 0; i < barks.Count; i++)
                    {
                        Vector3 barkPos = barks[i].transform.position - runes.transform.position;
                        if (Vector3.Distance(barkPos, mouseDistance) <=
                            Vector3.Distance(mouseDistance, minRuneDistance))
                        {
                            minRuneDistance = barkPos;
                            curBirchBark = i;
                            pointer.transform.position = barks[i].transform.position;
                        }
                    }
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && !isRunes)
            {
                StartRune();
            }
            else if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && isRunes)
            {
                if (_playerComponent.GetBirchBark())
                {
                    spellManager.CastSpell(curBirchBark, 1f);
                    EndRune();
                }
                else
                {
                    EndRune();
                    StartCoroutine(BirchBarkText());
                }
            }
            else if (Input.GetKeyUp(KeyCode.Escape) && isRunes)
            {
                EndRune();
            }
        }

        IEnumerator BirchBarkText()
        {
            getBirchBarkText.SetActive(true);
            yield return new WaitForSeconds(2f);
            getBirchBarkText.SetActive(false);
        }
        
        public void StartRune()
        {
            isRunes = true;
            runes.SetActive(true);
            barksCursor.transform.localPosition = new Vector3(0, 0, 0);
            startRunesPos = Input.mousePosition;
        }
        public void EndRune()
        {
            isRunes = false;
            chosedType = ChosedType.mouse;
            pointer.SetActive(false);
            runes.SetActive(false);
            curBirchBark = 0;
            chosedRune = false;
        }
    }
}