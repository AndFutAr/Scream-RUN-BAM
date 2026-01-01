using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject startUI, videoPreview;
    [SerializeField] private float videoDuration;
    private bool isVideoPlaying = false;
    
    [SerializeField] private int maxPoints = 0;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private List<AudioSource> buttonSound;

    private void Start() 
    {
        isVideoPlaying = PlayerPrefs.GetInt("isVideoPlaying", 0) == 1 ? true : false;
        if (!isVideoPlaying) 
            StartCoroutine(StartVideo());
        
        maxPoints = PlayerPrefs.GetInt("MaxPoints");
        pointsText.text = maxPoints.ToString();
    }

    IEnumerator StartVideo()
    {
        startUI.SetActive(false);
        videoPreview.SetActive(true);
        yield return new WaitForSeconds(videoDuration);
        videoPreview.SetActive(false);
        startUI.SetActive(true);
        PlayerPrefs.SetInt("isVideoPlaying", 1);
    }

    private void SkipVideo()
    {
        StopAllCoroutines();
        videoPreview.SetActive(false);
        startUI.SetActive(true);
        PlayerPrefs.SetInt("isVideoPlaying", 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SkipVideo();
    }
    
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        PlayerPrefs.SetInt("isVideoPlaying", 0);
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
