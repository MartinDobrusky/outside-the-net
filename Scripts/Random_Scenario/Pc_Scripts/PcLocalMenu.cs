using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PcLocalMenu : MonoBehaviour
{
    // Private variables for UI elements
    [SerializeField] private GraphicRaycaster m_Raycaster;
    [SerializeField] private EventSystem m_EventSystem;
    [SerializeField] private GameObject localMenuUI;

    [SerializeField] private PcManager _pcManager;

    private GameObject currentSelected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var pointerEventData = new PointerEventData(m_EventSystem);
            pointerEventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();

            m_Raycaster.Raycast(pointerEventData, results);

            foreach (var result in results.Where(result => result.gameObject.CompareTag("File")))
            {
                CreateLocalMenu(Input.mousePosition);
                currentSelected = result.gameObject; // Store reference to the selected file
                return;
            }

            localMenuUI.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            localMenuUI.SetActive(false);
        }
    }

    private void CreateLocalMenu(Vector3 mousePosition)
    {
        localMenuUI.transform.position = mousePosition;
        localMenuUI.SetActive(true);
        Debug.Log("Selected file: " + currentSelected);
    }

    public void DeleteFile()
    {
        if (currentSelected != null)
        {
            var pcManagerScript = _pcManager.GetComponent<PcManager>();
            var pcData = pcManagerScript.currentPcData;

            var fileIndex = -1;

            // Get the index of the selected file
            for (var i = 0; i < pcData.files.Length; i++)
                if (pcData.files[i] == currentSelected)
                {
                    fileIndex = i;
                    break;
                }

            if (fileIndex != -1)
            {
                // Remove the file from the array
                var filesList = pcData.files.ToList();
                filesList.RemoveAt(fileIndex);
                pcData.files = filesList.ToArray();

                // Update the fileColumnIndices list
                pcData.fileColumnIndices.Remove(fileIndex);
                for (var i = 0; i < pcData.fileColumnIndices.Count; i++)
                    if (pcData.fileColumnIndices[i] > fileIndex)
                        pcData.fileColumnIndices[i]--;

                localMenuUI.SetActive(false);

                // Mark the ScriptableObject as dirty
                #if UNITY_EDITOR
                EditorUtility.SetDirty(pcData);
                #endif
            }

            Destroy(currentSelected);
            localMenuUI.SetActive(false);

            currentSelected = null; // Reset the currentSelected variable
        }
    }


    private bool IsPointerOverUIElement()
    {
        var eventDataCurrentPosition = new PointerEventData(m_EventSystem);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
        var results = new List<RaycastResult>();
        m_Raycaster.Raycast(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }
}