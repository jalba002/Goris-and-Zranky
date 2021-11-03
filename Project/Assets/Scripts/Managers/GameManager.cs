using System.Collections;
using System.Collections.Generic;
using ECM.Examples;
using Player;
using UnityEngine;

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

    public PlayerController player;

    private PauseManager _pauseManager;
    private IEnumerator playerRespawner;

    public void Awake()
    {
        GM = this;
        // transform.parent = null;
        // DontDestroyOnLoad(this.gameObject);
        // gameObject.name = "[LITERAL GOD]";
        m_Camera = FindObjectOfType<Camera>();
        _pauseManager = FindObjectOfType<PauseManager>();
    }

    private void Start()
    {
        GetPlayers();
        
        // PlayGame();
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
        TeleportController(player, pos);
        m_Camera.GetComponent<FollowCameraController>().ForceNewPos();
    }

    public void CoolPlayerTeleport(Vector3 pos)
    {
        if (playerRespawner != null) return;
        playerRespawner = TeleportPhase(pos);
        StartCoroutine(playerRespawner);
    }
    
    public void RespawnPlayer()
    {
        if (playerRespawner != null) return;
        playerRespawner = RespawnPhase(player.GetStartingPos());
        StartCoroutine(playerRespawner);
    }
    
    private IEnumerator TeleportPhase(Vector3 pos)
    {
        // Camera fadeblack
       
        yield return new WaitForSecondsRealtime( HUDManager.Instance.FadeToBlack());
        TeleportPlayer(pos);
        yield return new WaitForSecondsRealtime( HUDManager.Instance.FadeToWhite());
        // Camera fadewhite
        playerRespawner = null;
    }
    
    private IEnumerator RespawnPhase(Vector3 pos)
    {
        // Camera fadeblack
       
        yield return new WaitForSecondsRealtime( HUDManager.Instance.FadeToBlack());
        TeleportPlayer(pos);
        player.ToggleControls(true);
        player.GetComponent<PlayerAnimatorManager>().Restart();
        player.GetComponent<PlayerHealthManager>().Respawn();
        yield return new WaitForSecondsRealtime( HUDManager.Instance.FadeToWhite());
        // Camera fadewhite
        playerRespawner = null;
    }
    
    public void GetPlayers()
    {
        player = FindObjectOfType<PlayerController>();
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

    public void Pause()
    {
        // Get the pause menu and toggle it.
        // If the pause menu is null ignore it.
        if (_pauseManager == null) return;
        
        _pauseManager.TogglePause();
    }

    public GameObject GetPlayerGO()
    {
        return player.gameObject;
    }


    public void UpdateOnSceneLoad()
    {
        player = FindObjectOfType<PlayerController>();
        m_Camera = FindObjectOfType<Camera>();
    }
}