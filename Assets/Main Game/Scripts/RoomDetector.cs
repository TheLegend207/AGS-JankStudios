using UnityEngine;

public class RoomDetector : MonoBehaviour
{
    public MinimapController minimap;
    public int roomNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minimap.SetActiveRoom(roomNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minimap.SetInactiveRoom(roomNumber);
        }
    }
}