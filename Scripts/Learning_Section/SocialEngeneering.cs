using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SocialEngeneering : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private int numOfObjects;
    [SerializeField] private Button nextSectionButton;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject panel;
    
    [SerializeField] private GameObject firstSection;
    [SerializeField] private GameObject secondSection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_camera != null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag("Bad"))
                    {
                        numOfObjects--;
                        Destroy(hitInfo.collider.gameObject);
                    }
                }
            }
        }
        
        if (numOfObjects == 0)
        {
            nextSectionButton.interactable = true;
            text.text = "You have successfully removed all the bad objects. Good job! Click the button to continue.";
            panel.GetComponent<Image>().color = new Color(0, 1, 0, 0.2f);
        }
    }
    
    public void NextSection()
    {
        firstSection.SetActive(false);
        secondSection.SetActive(true);
    }
}
