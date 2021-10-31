using System;
using System.Collections;
using System.Collections.Generic;
using ECM.Examples;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

// This class is a literal god.
public class GameManager : MonoBehaviour, IUpdateOnSceneLoad
{
    private static GameManager _gameManager;

    public static GameManager GM
    {
        get => _gameManager;

        set
        {
            if (_gameManager != null)
                Destroy(value.gameObject);
            else
            {
                _gameManager = value;
            }
        }
    }

    public Camera m_Camera;
    
    public List<PlayerController> m_Players;

    [FoldoutGroup("Game Start")] public bool playGame = false;
    [FoldoutGroup("Game Start")] public Transform startSpawn;

    public void Awake()
    {
        GM = this;
        // transform.parent = null;
        // DontDestroyOnLoad(this.gameObject);
        // gameObject.name = "[LITERAL GOD]";
        m_Camera = Camera.main;
        GetPlayers();
    }

    private void Start()
    {
        
        // PlayGame();
    }

    [ButtonGroup("Game Start/Force Start Game")]
    public void PlayGame()
    {
        if (playGame)
        {
            
        }
    }

    // public void SetMovementReferenceToPlayers(CameraSpot cameraSpot)
    // {
    //     foreach (var player in m_Players)
    //     {
    //         player.m_Alignment = cameraSpot.GetReference().gameObject;
    //     }
    // }

    // public void TeleportAllPlayers(Transform position)
    // {
    //     for (int i = 0; i < m_Players.Count; i++)
    //     {
    //         TeleportController(m_Players[i], 
    //             position.position + Vector3.forward * (i * m_Players[i].Controller.radius));
    //     }
    // }

    void TeleportController(PlayerController pc, Vector3 position)
    {
        pc.Controller.enabled = false;
        pc.gameObject.transform.position = position;
        pc.Controller.enabled = true;
    }

    public void TeleportPlayer(Vector3 pos)
    {
        TeleportController(m_Players[0], pos);
        m_Camera.GetComponent<FollowCameraController>().ForceNewPos();
    }
    
    [Button("Get Players")]
    public void GetPlayers()
    {
        m_Players = new List<PlayerController>();
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        int i = 0;
        foreach (var player in players)
        {
            m_Players.Add(players[i]);
            i++;
        }
    }
    
    
    // public void TeleportPlayerToWaitingRoom(PlayerController pc)
    // {
    //     TeleportController(pc, waitingRoom.position);
    // }

    // public void TogglePlayerControl(bool enable)
    // {
    //     Debug.Log("Player has " + (enable ? "YES" : "NO") + " control.");
    //     foreach (var item in m_Players)
    //     {
    //         item.ToggleInput(enable);
    //     }
    // }

    // [Button("Set camera spot")]
    // public void SetCameraSpot(CameraSpot cameraSpot)
    // {
    //     m_Camera.transform.position = cameraSpot.transform.position;
    //     m_Camera.transform.rotation = cameraSpot.transform.rotation;
    //     m_Camera.fieldOfView = cameraSpot.FOV;
    // }
    public void UpdateOnSceneLoad()
    {
        // 
    }

    public GameObject GetPlayerGO()
    {
        return m_Players[0].gameObject;
    }
}