using System;
using RTS_Cam;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int _currentPage = 0;
    [SerializeField] private GameObject _pcUI;
    [SerializeField] private GameObject[] _tutorialPages;
    [SerializeField] private GameObject _terminalPanel;
    [SerializeField] string pcId = "PC";
    private bool _canTargetPC = false;
    private bool _canAccessPC = false;
    [SerializeField] private GameObject _terminalResponse;
    [SerializeField] private TMP_InputField terminalInput;

    [SerializeField] private Camera _camera;
    [SerializeField] private RTS_Camera _rtsCamera;

    private void Start()
    {
        terminalInput.onEndEdit.AddListener(delegate { OnInputEntered(terminalInput); });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_camera != null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag("PC") && _canAccessPC)
                    {
                        _pcUI.SetActive(true);
                        NextPage();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) // Right mouse button click
        {
            // Setting target
            _rtsCamera.ResetTarget();
    
            if (_camera != null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag("PC") && _canTargetPC)
                    {
                        Transform firstChild = hitInfo.collider.gameObject.transform.GetChild(0);
                        firstChild.GetComponent<Canvas>().enabled = !firstChild.GetComponent<Canvas>().enabled;

                        _rtsCamera.SetTarget(hitInfo.collider.gameObject.transform);
                    }
                }
            }
        }
    }
    
    private void OnInputEntered(TMP_InputField inputField)
    {
        string inputText = inputField.text;
        
        _terminalResponse.SetActive(true);
        _terminalResponse.GetComponent<TMP_Text>().text = "Great! You typed: " + inputText;
        
        NextPage();
    }
    
    public void NextPage()
    {
        _tutorialPages[_currentPage].SetActive(false);
        _currentPage++;
        _tutorialPages[_currentPage].SetActive(true);
    }
    
    public void OpenTerminal()
    {
        _terminalPanel.SetActive(true);
        _tutorialPages[_currentPage].SetActive(false);
    }
    
    public void CanTargetPC()
    {
       _canTargetPC = true;
    }
    
    public void CanAccessPC()
    {
        _canAccessPC = true;
    }
}
