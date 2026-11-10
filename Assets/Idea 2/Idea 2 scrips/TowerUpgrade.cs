using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text interactionText;
    public GameObject upgradeButtonsGroup;
    public PlayerController2 playerController2;

    private bool isPlayerInZone = false;

    void Start()
    {
        // Hide everything at the start of the game
        if (interactionText != null) interactionText.gameObject.SetActive(false);
        if (upgradeButtonsGroup != null) upgradeButtonsGroup.SetActive(false);
    }

    void Update()
    {
        // If the player is in the zone and presses the 'E' key
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (upgradeButtonsGroup != null)
            {
                playerController2.canShoot = !playerController2.canShoot;
                playerController2.canLook = !playerController2.canLook;
                // Toggle the buttons (turns them on if off, or off if on)
                bool currentStatus = upgradeButtonsGroup.activeSelf;
                upgradeButtonsGroup.SetActive(!currentStatus);

                
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(currentStatus); 
                }

                
                ManageCursor(!currentStatus);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (interactionText != null && (upgradeButtonsGroup == null || !upgradeButtonsGroup.activeSelf))
            {
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            playerController2.canLook = true;
            playerController2.canShoot = true;
            if (interactionText != null) interactionText.gameObject.SetActive(false);
            if (upgradeButtonsGroup != null) upgradeButtonsGroup.SetActive(false);
            
            ManageCursor(false); // Lock the cursor back to the game
        }
    }

    private void ManageCursor(bool showCursor)
    {
        // Unlocks the mouse cursor when the menu is open, locks it when closed
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
