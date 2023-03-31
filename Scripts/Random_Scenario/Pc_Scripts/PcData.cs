using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PcData", menuName = "ScriptableObjects/PcData", order = 1)]
public class PcData : ScriptableObject
{
    public int id;
    public string pcName;
    public string hostame;
    
    public int virusLevel;
    public string viursSign;
    public int daysWithVirus;
    public string virusDescription;
    
    public string IP;
    public string Mac;
    public int cpuUsage;
    public int ramUsage;
    public int gpuUsage;

    public Sprite pcBackground;
    public GameObject[] dockIcons;
    public GameObject[] files;
    public List<int> fileColumnIndices;
    public Vector2 cmdPosition;
    
    public FileSystemNode fileSystemRoot;
}


