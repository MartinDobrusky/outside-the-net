using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS_Cam;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PcManager : MonoBehaviour
{
    // PC variables (UI)
    public bool isActive = false;
    [FormerlySerializedAs("pc_UI")] [SerializeField] private GameObject pcUI;
    [FormerlySerializedAs("pc_Time_Text")] public GameObject pcTimeText;
    [FormerlySerializedAs("pc_Date_Text")] public GameObject pcDateText;
    
    // Time variables
    private int _hour;
    private int _minutes;
    private int _seconds;
    private int _day;
    private int _month;
    private int _year;
    private Camera _camera;

    [SerializeField] private PcData[] pcDataObjects;
    public PcData currentPcData;
    public int currentPcIndex;
    
    [SerializeField] private GameObject[] _columns;

    // [SerializeField] private GameObject[] pcWindows;
    
    private TextMeshProUGUI _textMeshProUGUI;
    private TextMeshProUGUI _textMeshProUGUI1;
    
    [SerializeField] private GameObject dock;
    private List<int> _usedIndexes = new List<int>();
    
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private TerminalManager _terminalManager;
    public TerminalInterpreter _terminalInterpreter;
    
    [SerializeField] private GameObject[] _pcWindows;
    
    [SerializeField] private GameObject pcText;
    private bool _isTextActive = false;
    
    public VirusManager _virusManager;
    // Virus signs variables
    [SerializeField] private int targetFrameRate = 10;
    [SerializeField] private int timeToWait = 1;
    private bool _canLoad = true;
    [SerializeField] private GameObject _ransomwareWindow;
    [SerializeField] private TMP_Text _ransomwareText;
    [SerializeField] private GameObject[] _popupWindows;
    [SerializeField] private Sprite[] _badBackgrounds;


    [SerializeField] private RTS_Camera _rtsCamera;

    // Start is called before the first frame update and updates time and date and gets components
    private void Start()
    {
        _textMeshProUGUI1 = pcDateText.GetComponent<TextMeshProUGUI>();
        _textMeshProUGUI = pcTimeText.GetComponent<TextMeshProUGUI>();
        _camera = Camera.main;
        
        _day = DateTime.Now.Day;
        _month = DateTime.Now.Month;
        _year = DateTime.Now.Year;
        _textMeshProUGUI1.text = "" + _day + ". " + _month + ". " + _year;
        
        _rtsCamera = _rtsCamera.GetComponent<RTS_Camera>();
    }

    private void Update()
    {
        // Left mouse button click (Set active PC)
        if (Input.GetMouseButtonDown(0))
        {
            if (_camera != null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag("PC"))
                    {
                        pcUI.SetActive(true);
                        isActive = true;
                        GetPC(hitInfo.collider.gameObject.name);
                    }
                    
                    if (hitInfo.collider.gameObject.CompareTag("Disk"))
                    {
                        Destroy(hitInfo.collider.gameObject);
                    }
                }
            }
        }

        // Right mouse button click (Set target for RTS camera and display name)
        if (Input.GetMouseButtonDown(1)) // Right mouse button click
        {
            // Setting target
            _rtsCamera.ResetTarget();
    
            if (_camera != null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag("PC"))
                    {
                        Transform firstChild = hitInfo.collider.gameObject.transform.GetChild(0);
                        firstChild.GetComponent<Canvas>().enabled = !firstChild.GetComponent<Canvas>().enabled;

                        _rtsCamera.SetTarget(hitInfo.collider.gameObject.transform);
                    }
                }
            }
        }
        
        // Update time and date
        if (isActive)
        {
            _hour = DateTime.Now.Hour;
            _minutes = DateTime.Now.Minute;

            _textMeshProUGUI.text = "" + _hour + ":" + _minutes;
        }
    }

    // Get PC data from ScriptableObject
    private void GetPC(string pcId)
    {
        foreach (var dataObject in pcDataObjects)
        {
            if (dataObject.name == pcId)
            {
                currentPcData = dataObject;
                currentPcIndex = Array.IndexOf(pcDataObjects, dataObject);
                InitPC(dataObject);
            }
        }
    }

    // Init PC with data from ScriptableObject
    private void InitPC(PcData pcData)
    {
        pcUI.GetComponent<Image>().sprite = pcData.pcBackground;

        PlaceIcons(pcData);
        PlaceFiles(pcData);
        Debug.Log("Current version: " + currentPcData.virusLevel);
        
        if (currentPcData.virusLevel == 2)
        {
            var virusSign = currentPcData.viursSign;
            Debug.Log("Current pc virus sign: " + virusSign);

            // Switch for virus signs
            switch (virusSign)
            {
                case "popup":
                    var randomIndex = Random.Range(0, _popupWindows.Length);
                    _popupWindows[randomIndex].SetActive(true);
                    Debug.Log("Popup");
                    break;
                case "lag":
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = targetFrameRate;
                    Debug.Log("Lag");
                    break;
                case "background":
                    var rnd = Random.Range(0, _badBackgrounds.Length);
                    pcUI.GetComponent<Image>().sprite = _badBackgrounds[rnd];
                    Debug.Log("Background");
                    break;
            }
        }
        
        if (currentPcData.virusLevel == 3)
        {
            _ransomwareWindow.SetActive(true);
            _ransomwareText.text = $"Dear, {currentPcData.pcName}. Your files have been encrypted. Pay 100$ to get them back.";
        }
    }
    
    public void PopUpWindow()
    {
       currentPcData.virusLevel = 3;
       _ransomwareWindow.SetActive(true);
         _ransomwareText.text = $"Dear, {currentPcData.pcName}. Your files have been encrypted. Pay 100$ to get them back.";
    }
    
    // Place icons on the dock
    private void PlaceIcons(PcData pcData)
    {
        foreach (var icon in pcData.dockIcons)
        {
            if (icon == null) continue;
            Instantiate(icon, dock.transform);
        }
    }
    
    // Place files in the columns
    private void PlaceFiles(PcData pcData)
    {
        
    // Initialize the lists for each column
    List<Transform>[] columnLists = new List<Transform>[_columns.Length];
    for (int i = 0; i < _columns.Length; i++)
    {
        columnLists[i] = new List<Transform>();
    }

    // Check if there is saved placement data in the ScriptableObject
    if (pcData.fileColumnIndices != null && pcData.fileColumnIndices.Count > 0)
    {
        // Use the saved placement data to distribute files into the columns
        for (int i = 0; i < pcData.files.Length; i++)
        {
            var file = pcData.files[i];
            if (file == null) continue;
            int columnIndex = pcData.fileColumnIndices[i];
            columnLists[columnIndex].Add(file.transform);
        }
    }
    else
    {
        // Generate new random placement data and save it to the ScriptableObject
        pcData.fileColumnIndices = new List<int>();
        foreach (var file in pcData.files)
        {
            if (file == null) continue;
            int randomColumnIndex = Random.Range(0, _columns.Length);
            pcData.fileColumnIndices.Add(randomColumnIndex);
            columnLists[randomColumnIndex].Add(file.transform);
        }
    }

    // Instantiate files in their respective columns
    for (int i = 0; i < _columns.Length; i++)
    {
        var col = _columns[i];
        foreach (var fileTransform in columnLists[i])
        {
            // Instantiate the file and set its parent to the column
            var fileInstance = Instantiate(fileTransform.gameObject, col.transform);
            fileInstance.transform.SetParent(col.transform, false);
        }
    }
    }

    // Close PC button with reset of variables
    public void Button_Close_PC ()
    {
        isActive = false;
        pcUI.SetActive(isActive);
        
        foreach (Transform child in dock.transform)
        {
            if (child.gameObject.CompareTag("Icon"))
            {
                Destroy(child.gameObject);
            }
        }
        
        foreach (var col in _columns)
        {
            foreach (Transform child in col.transform)
            {
                if (child.gameObject.CompareTag("File"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
        
        _terminalManager.ClearTerminal();
    }

    public void Open_Tab(int index)
    {
        _pcWindows[index].SetActive(true);
    }
    
    public void Close_Tab(int index)
    {
        _pcWindows[index].SetActive(false);
    }
}
