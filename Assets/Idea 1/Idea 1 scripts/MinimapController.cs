using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public Image room1Image;
    public Image room2Image;

    public Color activeColour = Color.yellow;
    public Color inactiveColour = Color.gray;

    public void SetActiveRoom(int roomNumber)
    {
        room1Image.color = inactiveColour;
        room2Image.color = inactiveColour;

        if (roomNumber == 1)
            room1Image.color = activeColour;

        else if (roomNumber == 2)
            room2Image.color = activeColour;
    }

    public void SetInactiveRoom(int roomNumber)
    {
        if (roomNumber == 1)
            room1Image.color = inactiveColour;

        else if (roomNumber == 2)
            room2Image.color = inactiveColour;
    }
}