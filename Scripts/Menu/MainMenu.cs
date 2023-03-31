using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    [SerializeField] private TMP_Text highScoreText;
    
    [SerializeField] private GameObject temMenuPanel;
    
    private void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore") + " days";

        if (PlayerPrefs.GetInt("tempmenu", 0) == 0)
        {
            temMenuPanel.SetActive(true);
        }
        else
        {
            temMenuPanel.SetActive(false);
        }
    }
    
    public void TempMenu()
    {
        temMenuPanel.SetActive(false);
        PlayerPrefs.SetInt("tempmenu", 1);
    }

    public void GoToSettings()
    {
        anim.SetTrigger("Settings");
    }
    
    public void GoToMainMenu()
    {
        anim.SetTrigger("MainMenu");
    }
    
    public void GoToLearning()
    {
        anim.SetTrigger("Learning");
    }

    public void HoverSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
