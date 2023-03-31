using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapSegmentGeneration : MonoBehaviour
{
    [SerializeField] private PcData[] pcDataObjects;

    [SerializeField] private PcObjData pcObjData;

    public void GenerateMapSegment(string objectId, int objectsToKeep, GameObject[] prefabsToInstantiate)
    {
        // Find and delete objects
        var objects = FindAndDeleteObjects(objectId, objectsToKeep);

        // Instantiate prefabs on the remaining objects
        InstantiatePrefabs(objects, prefabsToInstantiate);
    }

    private GameObject[] FindAndDeleteObjects(string objectId, int objectsToKeep)
    {
        // Find all objects with the specified ID
        var objects = GameObject.FindGameObjectsWithTag(objectId);

        // Delete all objects except for the desired number of objects to keep
        if (objects.Length > objectsToKeep)
            for (var i = objectsToKeep; i < objects.Length; i++)
                objects[i].SetActive(false);

        return objects;
    }

    private void InstantiatePrefabs(GameObject[] objects, GameObject[] prefabsToInstantiate)
    {
        // Instantiate a random prefab on each remaining object
        foreach (var obj in objects)
        {
            var randomIndex = Random.Range(0, prefabsToInstantiate.Length);
            Instantiate(prefabsToInstantiate[randomIndex], obj.transform.position, obj.transform.rotation);

            // Deactivate previous object
            obj.SetActive(false);
        }
    }

    public void GeneratePCs(string objectId, int objectsToKeep, GameObject[] prefabsToInstantiate)
    {
        // Find and delete objects
        var pcObjects = FindAndDeletePCs(objectId, objectsToKeep);

        // Instantiate prefabs on the remaining objects
        InstantiatePCs(pcObjects, prefabsToInstantiate);
    }

    private GameObject[] FindAndDeletePCs(string objectId, int objectsToKeep)
    {
        // Find all objects with the specified ID
        var objects = GameObject.FindGameObjectsWithTag(objectId);
        
        List<GameObject> tempList = new List<GameObject>(objects);

        // Delete all objects except for the desired number of objects to keep
        if (objects.Length > objectsToKeep)
        {
            for (var i = objectsToKeep; i < objects.Length; i++)
            {
                tempList.Remove(objects[i]);
                objects[i].SetActive(false);
            }   
        }

        objects = tempList.ToArray();
        return objects;
    }

    private void InstantiatePCs(GameObject[] objects, GameObject[] prefabsToInstantiate)
    {
        var index = 0;
        
        // Instantiate a random prefab on each remaining object
        foreach (var obj in objects)
        {
            var randomIndex = Random.Range(0, prefabsToInstantiate.Length);
            var currentPC = Instantiate(prefabsToInstantiate[randomIndex], obj.transform.position,
                obj.transform.rotation);

            currentPC.name = index.ToString();

            foreach (var pcData in pcDataObjects)
                if (pcData.name == currentPC.name)
                {
                    var label = currentPC.GetComponentInChildren<TMP_Text>();
                    label.text = pcData.pcName;
                    pcObjData.pcNames.Add(pcData.id);

                    var canvas = currentPC.GetComponentInChildren<Canvas>();
                    canvas.enabled = false;
                }

            // Deactivate previous object
            obj.SetActive(false);
            
            pcObjData.pcObjects.Add(currentPC);

            index++;
        }
    }
}