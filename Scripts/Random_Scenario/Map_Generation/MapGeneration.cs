using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private GameObject mapParent;
    
    [FormerlySerializedAs("_room")] [SerializeField] private GameObject room;

    [FormerlySerializedAs("_bossRoom")] [SerializeField] private GameObject bossRoom;
    [FormerlySerializedAs("_storageRoom")] [SerializeField] private GameObject storageRoom;

    [SerializeField] private GameObject tilePrefab0;
    [SerializeField] private GameObject tilePrrefab1;
    [SerializeField] private Vector3[] tilePositions0;
    [SerializeField] private Vector3[] tilePositions1;

    [SerializeField] private MapSegmentGeneration _mapSegmentGeneration;

    [SerializeField] private GameObject meatingRoom;
    [SerializeField] private int _pcToKeep = 1;
    [SerializeField] private GameObject[] _pcPrefabs;

    [SerializeField] private string objectId0;
    [SerializeField] private int objectsToKeep0;
    [SerializeField] private GameObject[] prefabsToInstantiate0;

    [SerializeField] private string objectId1;
    [SerializeField] private int objectsToKeep1;
    [SerializeField] private GameObject[] prefabsToInstantiate1;

    [SerializeField] private string objectId2;
    [SerializeField] private int objectsToKeep2;
    [SerializeField] private GameObject[] prefabsToInstantiate2;

    [SerializeField] private string objectId3;
    [SerializeField] private int objectsToKeep3;
    [SerializeField] private GameObject[] prefabsToInstantiate3;

    [SerializeField] private string objectId4;
    [SerializeField] private int objectsToKeep4;
    [SerializeField] private GameObject[] prefabsToInstantiate4;

    [SerializeField] private string objectId5;
    [SerializeField] private int objectsToKeep5;
    [SerializeField] private GameObject[] prefabsToInstantiate5;

    [SerializeField] private GameObject cart;
    [SerializeField] private Vector3[] cartPositions;

    private readonly string _pcId = "PC";

    private bool _switchRooms = false;
    
    [SerializeField] private PcObjData pcObjData;

    private void Start()
    {
        pcObjData.pcObjects.Clear();
        pcObjData.pcNames.Clear();
        
        var difficulty = PlayerPrefs.GetInt("difficulty");

        _pcToKeep = difficulty switch
        {
            0 => 4,
            1 => 6,
            2 => 8,
            _ => _pcToKeep
        };
        
        Debug.Log(difficulty);
        Debug.Log("PCs to keep: " + _pcToKeep);

        SwitchRooms();

        SpawnTiles();

        SpawnCart();

        RotateMeatingRoom();

        _mapSegmentGeneration.GenerateMapSegment(objectId0, objectsToKeep0, prefabsToInstantiate0);
        _mapSegmentGeneration.GenerateMapSegment(objectId1, objectsToKeep1, prefabsToInstantiate1);
        _mapSegmentGeneration.GenerateMapSegment(objectId2, objectsToKeep2, prefabsToInstantiate2);
        _mapSegmentGeneration.GenerateMapSegment(objectId3, objectsToKeep3, prefabsToInstantiate3);
        _mapSegmentGeneration.GenerateMapSegment(objectId4, objectsToKeep4, prefabsToInstantiate4);
        _mapSegmentGeneration.GenerateMapSegment(objectId5, objectsToKeep5, prefabsToInstantiate5);

        _mapSegmentGeneration.GeneratePCs(_pcId, _pcToKeep, _pcPrefabs);
    }

    private void SwitchRooms()
    {
        var rnd = Random.Range(0, 2) == 0;

        if (rnd)
        {
            bossRoom.transform.position = new Vector3(18.9400005f, 8.79935265f, -19.25f);
            bossRoom.transform.rotation = Quaternion.Euler(0, 90, 0);

            storageRoom.transform.position = new Vector3(44.6100006f, 2.81782627f, -24.3099995f);
            storageRoom.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            bossRoom.transform.position = new Vector3(42.3739395f, 8.79935265f, -29.0072517f);
            bossRoom.transform.rotation = Quaternion.Euler(0, 0, 0);

            storageRoom.transform.position = new Vector3(21.3596573f, 2.81782627f, -20.5608463f);
            storageRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void SpawnTiles()
    {
        var rnd = Random.Range(0, 2) == 0;

        if (rnd)
        {
            foreach (var position in tilePositions0)
            {
                var newObject = Instantiate(tilePrefab0, room.transform);
                newObject.transform.localPosition = position;
            }

            foreach (var position in tilePositions1)
            {
                var newObject = Instantiate(tilePrrefab1, room.transform);
                newObject.transform.localPosition = position;
            }
        }
        else
        {
            foreach (var position in tilePositions0)
            {
                var newObject = Instantiate(tilePrrefab1, room.transform);
                newObject.transform.localPosition = position;
            }

            foreach (var position in tilePositions1)
            {
                var newObject = Instantiate(tilePrefab0, room.transform);
                newObject.transform.localPosition = position;
            }
        }
    }

    private void SpawnCart()
    {
        var randomIndex = Random.Range(0, cartPositions.Length);
        cart.transform.localPosition = cartPositions[randomIndex];

        var angle = Random.Range(-45f, 45f);
        var randomRotation = Quaternion.Euler(0f, angle, 0f);
        cart.transform.rotation = randomRotation;
    }

    private void RotateMeatingRoom()
    {
        var rnd = Random.Range(0, 2) == 0;

        if (rnd)
        {
            meatingRoom.transform.rotation = Quaternion.Euler(0, 180, 0);
            meatingRoom.transform.position = new Vector3(39.9799995f, 15.1684256f, -40.3800011f);
        }
        else
        {
            meatingRoom.transform.rotation = Quaternion.Euler(0, 0, 0);
            meatingRoom.transform.position = new Vector3(45.6426811f, 15.1684256f, -33.9812622f);
        }
    }

    private void RunRanderOptimalization()
    {
        // Get all child objects of the parent object
        Transform[] children = mapParent.GetComponentsInChildren<Transform>(true);
    
        // Loop through each child object and set it as static
        foreach(Transform child in children)
        {
            GameObject childObject = child.gameObject;
            childObject.isStatic = true;
        }
        
        // Lightmapping.BakeAsync();
    }
}