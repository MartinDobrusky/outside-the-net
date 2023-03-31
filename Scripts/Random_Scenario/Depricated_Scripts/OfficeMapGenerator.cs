using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfficeMapGenerator : MonoBehaviour
{
    // These arrays contain the prefabs for the different types of objects that can be placed in the office map
    public GameObject[] deskPrefabs;
    public GameObject[] pcPrefabs;
    public GameObject[] chairPrefabs;
    public GameObject[] plantPrefabs;
    public GameObject[] decorationPrefabs;

    public GameObject meetingRoomTablePrefab;

    // The size of the office map (in units)
    private Vector2 _mapSize;

    // The number of desks, PCs, chairs, plants, and decorations to place in the map
    public int _numDesks;
    public int numPCs;
    public int numChairs;
    public int numPlants;
    public int numDecorations;

    [SerializeField] private int meetingRoomMaxCount = 2;
    [SerializeField] private int bossRoomMaxCount = 1;

    private int _meetingRoomCount;
    private int _bossRoomCount;
    private int _officeRoomCount;

    public float minDistance = 1.0f;
    
    public Quaternion[] maxRotationAngles;
    
    private enum RoomType { Office, MeetingRoom, BossRoom, BreakRoom, Hallway, StorageRoom}
    
    void Start()
    {
        GenerateOfficeMap();
    }

    private void GenerateOfficeMap()
    {
        _meetingRoomCount = 0;
        _bossRoomCount = 0;
        
        var difficulty = PlayerPrefs.GetInt("difficulty", 0);
        
        var minSize = 10;
        var maxSize = 40;

        switch (difficulty)
        {
            case 0:
                minSize = 10;
                maxSize = 20;
                break;
            case 1:
                minSize = 20;
                maxSize = 30;
                break;
            case 2:
                minSize = 30;
                maxSize = 40;
                break;
        }
        
        _mapSize = new Vector2(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
        var mapSize2 = new Vector2(Random.Range(minSize, _mapSize.x), Random.Range(minSize, _mapSize.x));
        var mapSize3 = new Vector2(Random.Range(minSize, _mapSize.y), Random.Range(minSize, _mapSize.y));
        var mapSize4 = new Vector2(Random.Range(minSize, _mapSize.x), Random.Range(minSize, _mapSize.x));
        
        CreateFloor(new Vector3(0, 0f, 0), new Vector3(_mapSize.x, 0.5f, _mapSize.y));
        CreateFloor( new Vector3(mapSize2.x / 2 + _mapSize.x / 2, 0f, 0), new Vector3(mapSize2.x, 0.5f, mapSize2.y));
        CreateFloor(new Vector3(0, 0f, mapSize3.y / 2 + _mapSize.y / 2), new Vector3(mapSize3.x, 0.5f, mapSize3.y));
        CreateFloor(new Vector3(-(mapSize4.x / 2 + _mapSize.x / 2), 0f, 0), new Vector3(mapSize4.x, 0.5f, mapSize4.y));
        
        CreateWall(_mapSize, new Vector3(0, 0f, 0));
        CreateWall(mapSize2, new Vector3(mapSize2.x / 2 + _mapSize.x / 2, 0f, 0));
        CreateWall(mapSize3, new Vector3(0, 0f, mapSize3.y / 2 + _mapSize.y / 2));
        CreateWall(mapSize4, new Vector3(-(mapSize4.x / 2 + _mapSize.x / 2), 0f, 0));
        
        PlaceDesks();
    }
    
    private void CreateFloor(Vector3 position, Vector3 scale)
    {
        var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.tag = "Floor";
        floor.transform.position = position;
        floor.transform.localScale = scale;
        floor.AddComponent<BoxCollider>();
        
        RoomType roomType = RoomType.Office;

        switch (scale)
        {
            case {x: < 12, z: > 20} or {z: < 12, x: > 20}:
                roomType = RoomType.Hallway;
                floor.GetComponent<Renderer>().material.color = Color.white;
                break;
            case {x: < 11, z: < 11}:
                if (_bossRoomCount < bossRoomMaxCount)
                {
                    roomType = RoomType.BossRoom;
                    floor.GetComponent<Renderer>().material.color = Color.yellow;   
                }
                else
                {
                    roomType = RoomType.StorageRoom;
                    floor.GetComponent<Renderer>().material.color = Color.black;
                }
                _bossRoomCount++;
                break;
            case {x: > 15, z: > 15}:
                roomType = RoomType.Office;
                floor.GetComponent<Renderer>().material.color = Color.red;
                _officeRoomCount++;
                break;
            case {x: > 11, x: < 15} or {z: > 11, z: < 15}:
                if (_officeRoomCount < 1)
                {
                    roomType = RoomType.Office;
                    floor.GetComponent<Renderer>().material.color = Color.red;
                    _officeRoomCount++;
                    break;
                }
                if (_meetingRoomCount < meetingRoomMaxCount && _officeRoomCount > 1)
                {
                    roomType = RoomType.MeetingRoom;
                    floor.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if(_officeRoomCount > 1)
                {
                    roomType = RoomType.BreakRoom;
                    floor.GetComponent<Renderer>().material.color = Color.gray;
                }
                _meetingRoomCount++;
                break;
            default:
                if (_officeRoomCount < 1)
                {
                    roomType = RoomType.Office;
                    floor.GetComponent<Renderer>().material.color = Color.red;
                    _officeRoomCount++;
                    break;
                }

                roomType = RoomType.BreakRoom;
                floor.GetComponent<Renderer>().material.color = Color.gray;
                break;
        }

        switch (roomType)
        {
            case RoomType.Hallway:
                break;
            case RoomType.BossRoom:
                break;
            case RoomType.Office:
                break;
            case RoomType.MeetingRoom:
                GenerateMeetingRoom(position);
                break;
            case RoomType.BreakRoom:
                break;
            case RoomType.StorageRoom:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void CreateWall(Vector2 mapSize, Vector3 position)
    {
        GameObject wallParent = new GameObject();
        
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.tag = "Wall";
        wall.transform.position = new Vector3(0, 2.5f,mapSize.y / 2);
        wall.transform.localScale = new Vector3(mapSize.x + 0.5f, 5.5f, 0.5f);
        wall.transform.parent = wallParent.transform;
        
        GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall2.tag = "Wall";
        wall2.transform.position = new Vector3(0, 2.5f,-mapSize.y / 2);
        wall2.transform.localScale = new Vector3(mapSize.x + 0.5f, 5.5f, 0.5f);
        wall2.transform.parent = wallParent.transform;
        
        GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall3.tag = "Wall";
        wall3.transform.position = new Vector3(mapSize.x / 2, 2.5f,0);
        wall3.transform.localScale = new Vector3(0.5f, 5.5f, mapSize.y - 0.5f);
        wall3.transform.parent = wallParent.transform;
        
        GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall4.tag = "Wall";
        wall4.transform.position = new Vector3(-mapSize.x / 2, 2.5f,0);
        wall4.transform.localScale = new Vector3(0.5f, 5.5f, mapSize.y - 0.5f);
        wall4.transform.parent = wallParent.transform;
        
        wallParent.transform.position = position;
    }
    
    private void GenerateMeetingRoom(Vector3 position)
    {
        GameObject meetingTable = meetingRoomTablePrefab;
        meetingTable.tag = "Desk";
        Instantiate(meetingRoomTablePrefab, position, Quaternion.identity);
    }
    
    private void PlaceDesks()
    {
        // Keep track of the number of desks placed
        int numDesks = 0;
        
        // Continue placing desks until we reach the maximum number
        while (numDesks < _numDesks)
        {
            // Instantiate a desk at a random position and rotation
            int prefabIndex = Random.Range(0, deskPrefabs.Length - 1);
            GameObject deskPrefab = deskPrefabs[prefabIndex];
            
            int rotationIndex = Random.Range(0, maxRotationAngles.Length - 1);
            Quaternion randomRotation = maxRotationAngles[rotationIndex];
            
            GameObject desk = Instantiate(deskPrefab, RandomPosition(), randomRotation);

            // Check if the desk is too close to any other desks
            bool tooClose = false;
            foreach (Collider collider in Physics.OverlapSphere(desk.transform.position, minDistance))
            {
                if (collider.gameObject.CompareTag("Desk"))
                {
                    tooClose = true;
                    break;
                }
            }

            // If the desk is too close to another desk, destroy it and try again
            if (tooClose)
            {
                Destroy(desk);
            }
            // Otherwise, increment the number of desks placed and continue
            else
            {
                numDesks++;
            }
        }
    }

    // Returns a random position within the boundaries of the room
    Vector3 RandomPosition()
    {
        var x = Random.Range((-_mapSize.x + 3f) / 2, (_mapSize.x - 3f) / 2);
        var y = 0.25f;
        var z = Random.Range((-_mapSize.y + 3f) / 2, (_mapSize.y - 3f) / 2);
        return new Vector3(x, y, z);
    }

    private static void DeleteCubes ()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Floor");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] desks = GameObject.FindGameObjectsWithTag("Desk");
        
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
        
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
        
        foreach (GameObject desk in desks)
        {
            Destroy(desk);
        }
    }
    
    public void GenerateMap ()
    {
        DeleteCubes();
        GenerateOfficeMap();
    }
}
