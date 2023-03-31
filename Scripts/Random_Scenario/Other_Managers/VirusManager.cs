using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class VirusManager : MonoBehaviour
{
    TimeManager _timeManager;
    
    [SerializeField] private PcObjData pcObjData;
    
    private int numberOfThreats = 0;
    [SerializeField] private PcData[] pcData;
    List<PcData> pcsWithoutVirus = new List<PcData>(); // Create a new instance of List<PcData>
    List<PcData> pcsWithVirus = new List<PcData>();
    
    public string[] virusSigns;
    
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private TMP_Text endGameText;
    
    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        
        Init();

        foreach (var data in pcObjData.pcNames)
        {
            Debug.Log(data);
        }
    }

    private void Init()
    {
        foreach (var data in pcData)
        {
            data.virusLevel = 0;
        }
    }

    public void EndOfDayWithVirus()
    {
        LevelUpExistingThreats();
        SetNewThreat();
    }
    
    private void LevelUpExistingThreats()
    {
        foreach (var data in pcData)
        {
            if (data.virusLevel >= 3)
            {
                EndGame();
            }
            
            if (data.virusLevel >= 1)
            {
                data.virusLevel += 1;

                if (data.virusLevel == 2)
                {
                    var rnd = Random.Range(0, virusSigns.Length);
                    data.viursSign = virusSigns[rnd];
                }
            }
        }
    }

    private void SetNewThreat()
    {
        var nubmerOfHealthyPcs = 0;
        
        // Two most important lines of code in this script
        // They map the data from PcObjData to PcData and determine which PCs are still healthy
        PcData[] matchingPcData = pcData.Where(data => pcObjData.pcNames.Contains(data.id)).ToArray();
        
        PcData[] filteredPcData = matchingPcData.Where(data => data.virusLevel == 0).ToArray();

        if (filteredPcData.Length > 0)
        {
            int rnd = Random.Range(0, filteredPcData.Length);
            filteredPcData[rnd].virusLevel = 1;
            Debug.Log("Ano, nastaveno");
        }
    }
    
    private void EndGame()
    {
        endGameCanvas.SetActive(true);
        endGameText.text = "Your network was taken over by ransomware. Company went bancrupt in " + _timeManager.dayNumber + " days";
        
        if (_timeManager.dayNumber > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _timeManager.dayNumber);
        }
    }
}
