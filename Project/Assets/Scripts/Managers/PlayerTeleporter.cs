using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public Transform destination;
    public void TeleportPlayer()
    {
       GameManager.GM.TeleportPlayer(destination.position);
    }
}
