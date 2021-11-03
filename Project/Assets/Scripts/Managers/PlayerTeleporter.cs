using Player;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public Transform destination;
    public void TeleportPlayer()
    {
       GameManager.GM.CoolPlayerTeleport(destination.position);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        // 
        PlayerController playerCheck = other.gameObject.GetComponent<PlayerController>();
        Debug.Log(other.gameObject.name);

        if (playerCheck != null)
        {
            TeleportPlayer();
        }
    }
}
