using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalHandler : Button
{
    private PcManager _pcManager;

    void Start()
    {
        _pcManager = GameObject.Find("PC_Manager").GetComponent<PcManager>();
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        _pcManager.Open_Tab(0);
        
        Debug.Log("Button clicked!");
    }
}
