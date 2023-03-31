using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] VirusManager virusManager;
    
    [SerializeField] private int time = 10;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image timerImage;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text dayText;
    public int dayNumber = 1;
    
    [SerializeField] private PcObjData pcObjData;
    [SerializeField] private PcData[] pcData;
    [SerializeField] private Button[] pcButtons;
    
    [SerializeField] private RectTransform infoPanelReactTransform;
    [SerializeField] private RectTransform timerRectTransform;
    [SerializeField] private float decreaseAmount = 60f;
    [SerializeField] private TMP_Text[] virusTexts;
    
    [SerializeField] private GameObject endOfGamePanel;
    [SerializeField] private TMP_Text endOfGameText;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
        
        DetermineThreats();
    }

    IEnumerator CountdownCoroutine()
    {
        float interval = 1.0f / time; // The interval at which the image should be updated
        float fillAmount = 1f; // The current fill amount of the image

        while (time > 0)
        {
            fillAmount -= interval;
            timerImage.fillAmount = fillAmount;
            yield return new WaitForSeconds(1.0f);
            time--;
        }

        // Countdown is finished, show the panel
        _animator.SetTrigger("DayEnd");
        dayText.text = "Day " + dayNumber + " is over!";
        
        LoadPcDataObjects();
    }
    
    public void NextDay()
    {
        if (pcObjData.pcObjects.Count <= 0)
        {
            EndGame();
        }
        else
        {
            // Reset the time
            time = 10;
            // Reset the image
            timerImage.fillAmount = 1f;
            // Hide the panel
            _animator.SetTrigger("DayStart");
            // Start the countdown again
            StartCoroutine(CountdownCoroutine());
            // Increase the day number
            dayNumber++;
            // Resume the game
            Time.timeScale = 1;

            DetermineThreats();
            
            virusManager.EndOfDayWithVirus();
        }
    }
    
    private void DetermineThreats()
    {
        var index = 0;
        
        for (int i = 0; i < pcData.Length; i++)
        {
            if (pcData[i].virusLevel == 2)
            {
                virusTexts[index].text = pcData[i].virusDescription;
                infoPanelReactTransform.anchoredPosition = new Vector2(infoPanelReactTransform.anchoredPosition.x, infoPanelReactTransform.anchoredPosition.y + decreaseAmount);
                timerRectTransform.anchoredPosition = new Vector2(timerRectTransform.anchoredPosition.x, timerRectTransform.anchoredPosition.y - decreaseAmount);
                index++;
            }
            else
            {
                virusTexts[i].text = " ";
            }
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void LoadPcDataObjects()
    {
        SortPcDataObjects();
        Debug.Log(pcObjData.pcObjects.Count);
        
        for (int i = 0; i < pcObjData.pcObjects.Count; i++)
        {
            var pcObj = pcObjData.pcObjects[i];

            if (pcObj.name == pcData[i].name)
            {
                pcButtons[i].gameObject.SetActive(true);
                pcButtons[i].GetComponentInChildren<TMP_Text>().text = pcData[i].pcName;
                var i1 = i;
                pcButtons[i].onClick.AddListener(() => DeletePcDataObject(pcObj, pcButtons[i1]));   
            }
        }
    }

    private void DeletePcDataObject(GameObject pcObj, Button button)
    {
        int index = pcObjData.pcObjects.IndexOf(pcObj);
        pcObjData.pcNames.RemoveAt(index);
        pcObjData.pcObjects.Remove(pcObj);
        Destroy(pcObj);
        LoadPcDataObjects();
        
        Destroy(button.gameObject);
    }
    
    private void SortPcDataObjects()
    {
        pcObjData.pcObjects.Sort(new GameObjectNameComparer());
    }
    
    public class GameObjectNameComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            int xName = int.Parse(x.name);
            int yName = int.Parse(y.name);
            return xName.CompareTo(yName);
        }
    }
    
    private void EndGame()
    {
        endOfGamePanel.SetActive(true);
        endOfGameText.text = "You ran out of resources (and managed to fire all employees) in " + dayNumber + " days";
        
        if (dayNumber > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", dayNumber);
        }
    }
}

