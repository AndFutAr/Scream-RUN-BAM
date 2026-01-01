using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
   [SerializeField] private List<AudioSource> buttonSound;

   private void Start()
   {
      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = false;
   }

   public void RestartGame()
   {
      StartCoroutine(Restart());
   }
   IEnumerator Restart()
   {
      /*int chance = UnityEngine.Random.Range(0, buttonSound.Count);
      buttonSound[chance].Play();*/
      yield return new WaitForSeconds(0.2f);
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
   
   public void ExitGame()
   {
      StartCoroutine(Exit());
   }
   IEnumerator Exit()
   {
      /*int chance = UnityEngine.Random.Range(0, buttonSound.Count);
      buttonSound[chance].Play();*/
      yield return new WaitForSeconds(0.2f);
      SceneManager.LoadScene(0);
   }
}
