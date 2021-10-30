using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class DelayedTrigger : MonoBehaviour
{
    // When one player enters collision with the teleporter, the ready check activates. 
    // And until the second one hasn't entered the teleporter it does not trigger the advancement.
    // 
    [Title("Settings")]
    [Range(0, 100)] public float timeBetweenEvents = 1f;

    [Title("Events")]
    public UnityEvent BeforeTeleportation = new UnityEvent();
    public PlayerTeleporter playerTeleporter;
    public UnityEvent AfterTeleportation = new UnityEvent();
    
    private IEnumerator coroutineHolder;
    
    public void OnTriggerEnter(Collider other)
    {
        // 
        PlayerController playerCheck = other.gameObject.GetComponent<PlayerController>();
        Debug.Log(other.gameObject.name);

        if (playerCheck != null)
        {
            //GameManager.GM.TeleportPlayerToWaitingRoom(playerCheck);
            CompleteTeleport();
        }
    }

    
    [Button("Start teleportation sequence", ButtonSizes.Large)]
    public void CompleteTeleport()
    {
        if (coroutineHolder != null) return;

        coroutineHolder = TeleportPhase();
        StartCoroutine(coroutineHolder);
    }

    private IEnumerator TeleportPhase()
    {
        // Trigger the HUD black screen
        // HUDManager.HUD.PlayClip(fadeClip);
        // HUDManager.HUD.HideHUD();

        // Disable player control!
        //GameManager.GM.TogglePlayerControl(false);

        BeforeTeleportation.Invoke();

        // Wait until its over
        // if (fadeClip != null)
        // {
        //     yield return new WaitForSecondsRealtime(fadeClip.length * 1.1f); // Hardcoded?
        // }
        // else
        // {
        //     yield return new WaitForSecondsRealtime(1f); // Hardcoded?
        // }

        // Trigger the camera change (move it!)
        //GameManager.GM.SetCameraSpot(newCameraSpot);
        // And trigger the new alignment!
        //GameManager.GM.SetMovementReferenceToPlayers(newCameraSpot);
        
        yield return new WaitForSecondsRealtime(timeBetweenEvents);
        playerTeleporter.TeleportPlayer();
        yield return new WaitForSecondsRealtime(0.1f);

        // Trigger the player teleport!
        //GameManager.GM.TeleportAllPlayers(destinationTeleporter);

        // Trigger HUD for visibility

        // Wait the time fro the HUD to change.
        // if (whiteClip != null)
        // {
        //     yield return new WaitForSecondsRealtime(whiteClip.length * 1f);
        // }
        // else
        // {
        //     yield return new WaitForSecondsRealtime(1.05f);
        // }
        // HUDManager.HUD.PlayClip(whiteClip);

        // Enable player control again!
        //GameManager.GM.TogglePlayerControl(true);

        AfterTeleportation.Invoke();

        coroutineHolder = null;
    }
}