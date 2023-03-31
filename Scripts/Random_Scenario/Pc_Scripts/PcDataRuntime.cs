using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimePcData
{
    public string pcName;
    public int virusLevel;
    public string IP;
    public string Mac;
    public Sprite pcBackground;
    public GameObject[] dockIcons;
    public GameObject[] files;
    public List<int> fileColumnIndices;
    public Vector2 cmdPosition;
}

