using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class ItemSpawner : MonoBehaviour
{
    [Title("SpawnPoints")] public List<Transform> spawnPoints;
    public Transform m_ParentTransform;
    
    [TabGroup("Resources")] public List<string> spawnablePrefabs = new List<string>()
    {
        
    };

    [Title("Game Elements")] public List<GameObject> spawnedPrefabs;

    
    private void Start()
    {
        SpawnItems();
    }

    [Button("Spawn Items", ButtonSizes.Large)]
    public void SpawnItems()
    {
        if (spawnedPrefabs.Count > 0)
        {
            Debug.Log("Already spawned bunch.");
            return;
        }
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            SpawnItem(spawnPoints[i].position, spawnablePrefabs[i]);
        }
    }

    private void SpawnItem(Vector3 position, string path = "Models/Potion_00")
    {
        GameObject go = Instantiate(Resources.Load(path) as GameObject, m_ParentTransform);
        go.transform.position = position;
        // go.transform.rotation = spawnPoints[i].rotation;
        
        spawnedPrefabs.Add(go);
    }
    
 
    [Button("Clear List")]
    public void ClearList()
    {
        spawnedPrefabs = new List<GameObject>();
    }
}