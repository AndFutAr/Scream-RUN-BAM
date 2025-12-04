using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    [SerializeField] private int maxPoints = 0;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private List<AudioSource> buttonSound;

    private void Start()
    {
        maxPoints = PlayerPrefs.GetInt("MaxPoints");
        pointsText.text = maxPoints.ToString();
    }
    
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        /*int chance = UnityEngine.Random.Range(0, buttonSound.Count);
        buttonSound[chance].Play();*/
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("GameScene");
    }
    
    public void QuitGame()
    {
        StartCoroutine(StopGameCoroutine());
    }
    IEnumerator StopGameCoroutine()
    {
        /*int chance = UnityEngine.Random.Range(0, buttonSound.Count);
        buttonSound[chance].Play();*/
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
    }
}
