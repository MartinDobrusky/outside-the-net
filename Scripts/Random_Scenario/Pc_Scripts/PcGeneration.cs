using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PcGeneration : MonoBehaviour
{
    [SerializeField] private PcData[] _pcData;

    private int id = 0;
    public string[] names;
    public string[] surnames;
    public Sprite[] pcBackgrounds;
    public GameObject[] dockIcons;
    public GameObject cmdIcon;
    public GameObject[] files;
    public int minFiles = 5;
    public int maxFiles = 20;
    public string[] goodFilesNames;
    public string[] goodFileExtensions;
    public string[] badFilesNames;
    public string[] badFileExtensions;
    
    [SerializeField] private string[] dirNames;

    void Awake()
    {
        foreach (var dataObject in _pcData)
        {
            ResetData(dataObject);
            GenerateId(dataObject);
            GenerateName(dataObject);
            GenerateBackroud(dataObject);
            GenerateDock(dataObject);
            GenerateFiles(dataObject);
            GenerateIp(dataObject);
            GenerateMac(dataObject);
            GenerateUsage(dataObject);
            
            InitializeFileSystem(dataObject, dirNames);
        }
    }
    
    private void ResetData(PcData pcData)
    {
        pcData.id = 0;
        pcData.pcName = "";
        pcData.dockIcons = new GameObject[dockIcons.Length];
        pcData.IP = "";
        pcData.virusLevel = 0;
        pcData.viursSign = "";
        pcData.daysWithVirus = 0;
        pcData.virusDescription = "";
        pcData.cpuUsage = 0;
        pcData.ramUsage = 0;
        pcData.gpuUsage = 0;
        pcData.fileSystemRoot = null;
        pcData.fileColumnIndices = new List<int>();
        pcData.cmdPosition = new Vector2();
    }
    
    private void GenerateId(PcData pcData)
    {
        pcData.id = id;
        id++;
    }

    private void GenerateName(PcData pcData)
    {
        var rnd = Random.Range(0, names.Length - 1);
        var rnd2 = Random.Range(0, surnames.Length - 1);
        
        pcData.pcName = names[rnd] + " " + surnames[rnd2];
        pcData.hostame = names[rnd].ToLower() + surnames[rnd2].ToLower();
    }

    private void GenerateBackroud(PcData pcData)
    {
        var pcBackground = pcBackgrounds[Random.Range(0, pcBackgrounds.Length)];
        pcData.pcBackground = pcBackground;
    }
    
    private void GenerateDock(PcData pcData)
    {
        var numberOfIcons = Random.Range(4, dockIcons.Length - 1);
        var usedIndexes = new HashSet<int>();

        // add cmdIcon to dockIcons
        if (!usedIndexes.Contains(dockIcons.Length - 1))
        {
            pcData.dockIcons[dockIcons.Length - 1] = cmdIcon;
            usedIndexes.Add(dockIcons.Length - 1);
        }

        for (var i = 0; i < numberOfIcons; i++)
        {
            int rndIcon;
            do
            {
                rndIcon = Random.Range(0, dockIcons.Length - 1);
            } while (usedIndexes.Contains(rndIcon));

            pcData.dockIcons[rndIcon] = dockIcons[rndIcon];
            usedIndexes.Add(rndIcon);
        }
    }


    private void GenerateFiles(PcData pcData)
    { 
            var rndFileCount = Random.Range(minFiles, maxFiles);

            for (var i = 0; i < rndFileCount; i++)
            {
                var rnd = Random.Range(0, files.Length);
                var curFile = files[rnd];
                
                var rnd4 = Random.Range(0, goodFilesNames.Length - 1);

                curFile.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = goodFilesNames[rnd4] + goodFileExtensions[rnd];
                
                pcData.files[i] = curFile;
                        
                // var fileName = goodFilesNames[rnd4] + badFileExtensions[rnd];
                // _terminalInterpreter._dektopFiles.Add(fileName);
            }
    }
    
    private void GenerateIp(PcData pcData)
    {
        var rnd = Random.Range(0, 255);
        var rnd2 = Random.Range(0, 255);
        var rnd3 = Random.Range(0, 255);
        var rnd4 = Random.Range(0, 255);
        pcData.IP = rnd + "." + rnd2 + "." + rnd3 + "." + rnd4;
    }
    
    private void GenerateMac(PcData pcData)
    {
        var rnd = Random.Range(0, 255);
        var rnd2 = Random.Range(0, 255);
        var rnd3 = Random.Range(0, 255);
        var rnd4 = Random.Range(0, 255);
        var rnd5 = Random.Range(0, 255);
        var rnd6 = Random.Range(0, 255);
        pcData.Mac = rnd + ":" + rnd2 + ":" + rnd3 + ":" + rnd4 + ":" + rnd5 + ":" + rnd6;
    }
    
    private void GenerateUsage(PcData pcData)
    {
        var rnd = Random.Range(0, 20);
        pcData.cpuUsage = rnd;
        
        var rnd2 = Random.Range(0, 25);
        pcData.ramUsage = rnd;
        
        var rnd3 = Random.Range(0, 15);
        pcData.gpuUsage = rnd;
    }
    
    public void InitializeFileSystem(PcData pcData, string[] possibleNames) {
        var fileSystemRoot = new FileSystemNode("root", true);

        FileSystemNode documentsDir = new FileSystemNode("Documents", true);
        FileSystemNode picturesDir = new FileSystemNode("Pictures", true);
        FileSystemNode musicDir = new FileSystemNode("Music", true);
        FileSystemNode file1 = new FileSystemNode("file1.txt", false);
        FileSystemNode file2 = new FileSystemNode("file2.txt", false);

        // Generate random subfolders
        int subfolderCount = Random.Range(1, possibleNames.Length + 1);
        List<FileSystemNode> subfolders = new List<FileSystemNode>();
        for (int i = 0; i < subfolderCount; i++) {
            int randomIndex = Random.Range(0, possibleNames.Length);
            string randomName = possibleNames[randomIndex];
            FileSystemNode subfolder = new FileSystemNode(randomName, true);
            subfolders.Add(subfolder);
        }

        // Add parents to directories
        for (int i = 0; i < subfolders.Count - 1; i++) {
            subfolders[i + 1].parent = subfolders[i];
            subfolders[i].children.Add(subfolders[i + 1]);
        }
        subfolders[0].parent = documentsDir;
        documentsDir.children.Add(subfolders[0]);

        fileSystemRoot.children.Add(documentsDir);
        fileSystemRoot.children.Add(picturesDir);
        fileSystemRoot.children.Add(musicDir);
        fileSystemRoot.children.Add(file1);
        fileSystemRoot.children.Add(file2);

        pcData.fileSystemRoot = fileSystemRoot;
    }
}
