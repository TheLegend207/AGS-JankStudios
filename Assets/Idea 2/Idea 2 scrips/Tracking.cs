using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tracking : MonoBehaviour
{

    public PlayerController2 playerController2;
    //tracking
    public float range = 10f;
    public float rotationSpeed = 5.0f;
    public bool smoothRotate = true;

    //damage
    public int damage = 10;
    public float timeBeforeNextAttack;

    //interact
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 3f;      // How close the player must be to select with E

    //cursor
    public bool lockCursorWhenClosed = true;   // Lock + hide the cursor whenever the tower UI is hidden

    //buttons
    public GameObject buttonPanel;        // Parent object holding the buttons (only needs filling on one tower)
    public GameObject[] extraUI;          // Any other UI to show/hide with the selection (labels, cost text)
    public Button upgradeRangeButton;     // Drag your range button here
    public Button upgradeDamageButton;    // Drag your damage button here
    public Button sellbutton;             // Drag your sell button here

    //upgrades
    public float rangeIncrease = 2f;
    public float maxRange = 30f;
    public int damageIncrease = 5;
    public int maxDamage = 100;

    //resetpos
    public bool resetWhenOutOfRange = true;
    public float resetSpeed = 2.0f;

    //only affects selected tower
    public static Tracking Selected { get; private set; }

    // True while a tower is selected and the button panel is showing
    public static bool MenuOpen => Selected != null;

    // Every live tower, used to find the nearest one when E is pressed
    private static readonly List<Tracking> allTowers = new List<Tracking>();

    // Every UI object that shows while a tower is selected and hides otherwise
    private static readonly HashSet<GameObject> selectionUI = new HashSet<GameObject>();

    // Buttons that already have their click handler, so each is wired only once
    private static readonly HashSet<Button> wiredButtons = new HashSet<Button>();

    // Makes sure input is only processed once per frame, however many towers exist
    private static int lastClickFrame = -1;
    private static int lastInteractFrame = -1;

    private Quaternion initialLocalRotation;

    // Snapshot of the enemy list, so enemies dying mid-loop can't break the loop
    private readonly List<EnemyBase> enemyBuffer = new List<EnemyBase>();

    public LevelManager2 levelManager2;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Selected = null;
        allTowers.Clear();
        selectionUI.Clear();
        wiredButtons.Clear();
        lastClickFrame = -1;
        lastInteractFrame = -1;
    }

    // Shows or hides all the selection UI, and frees or locks the cursor to match
    static void SetUIActive(bool active)
    {
        foreach (GameObject ui in selectionUI)
        {
            if (ui != null)
                ui.SetActive(active);
        }

        if (active)
        {
            // UI open: free the cursor so the buttons can be clicked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (AnyTowerLocksCursor())
        {
            // UI closed: lock the cursor back to the screen
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // True if any tower has "Lock Cursor When Closed" ticked
    static bool AnyTowerLocksCursor()
    {
        foreach (Tracking tower in allTowers)
        {
            if (tower != null && tower.lockCursorWhenClosed)
                return true;
        }

        return false;
    }

    // Selects a tower (or pass null to deselect) and shows/hides the UI
    static void SetSelected(Tracking tower)
    {
        if (Selected == tower) return;

        Selected = tower;
        SetUIActive(tower != null);
    }

    // Adds the click handler to a button the first time any tower sees it
    static void Wire(Button button, UnityAction action)
    {
        if (button == null) return;
        if (!wiredButtons.Add(button)) return;

        button.onClick.AddListener(action);
    }

    // Button handlers: they always act on whichever tower is selected
    static void UpgradeSelectedRange()
    {
        if (Selected == null)
        {
            Debug.LogWarning("Range button pressed but no tower is selected. Click a tower first.");
            return;
        }

        Selected.UpgradeRange();

        // Temporary: confirms the button worked. Delete once it works.
        Debug.Log($"Range upgraded on {Selected.name}: now {Selected.range}", Selected);
    }

    static void UpgradeSelectedDamage()
    {
        if (Selected == null)
        {
            Debug.LogWarning("Damage button pressed but no tower is selected. Click a tower first.");
            return;
        }

        Selected.UpgradeDamage();

        // Temporary: confirms the button worked. Delete once it works.
        Debug.Log($"Damage upgraded on {Selected.name}: now {Selected.damage}", Selected);
    }

    static void SellSelected()
    {
        if (Selected == null)
        {
            Debug.LogWarning("Sell button pressed but no tower is selected. Click a tower first.");
            return;
        }

        Selected.Sell();
    }

    // Press E near a tower to select the nearest one within interactRange of the player
    void HandleInteract()
    {
        if (!Input.GetKeyDown(interactKey)) return;
        if (lastInteractFrame == Time.frameCount) return;
        lastInteractFrame = Time.frameCount;

        // Measure from the Player-tagged object, falling back to the camera
        Transform origin = null;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            origin = player.transform;
        else if (Camera.main != null)
            origin = Camera.main.transform;

        if (origin == null) return;

        Tracking nearest = null;
        float nearestSqr = float.MaxValue;

        foreach (Tracking tower in allTowers)
        {
            if (tower == null) continue;

            float sqr = (tower.transform.position - origin.position).sqrMagnitude;
            if (sqr <= tower.interactRange * tower.interactRange && sqr < nearestSqr)
            {
                nearestSqr = sqr;
                nearest = tower;
            }
        }

        // Nearest tower in reach gets selected; if none is in reach, nothing changes
        if (nearest != null)
            SetSelected(nearest);
    }

    // Handles every click in one place: pick the nearest tower under the cursor, or deselect
    static void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (lastClickFrame == Time.frameCount) return;
        lastClickFrame = Time.frameCount;

        // Clicks on UI (including the buttons themselves) never change the selection
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        // Look through everything under the cursor, so colliders in front can't hide a tower
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        Tracking clicked = null;
        float closest = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            Tracking tower = hit.collider.GetComponentInParent<Tracking>();
            if (tower != null && hit.distance < closest)
            {
                closest = hit.distance;
                clicked = tower;
            }
        }

        // A tower selects it; empty ground (null) deselects
        SetSelected(clicked);
    }

    void Start()
    {
        initialLocalRotation = transform.localRotation;

        // UI starts hidden until a tower is selected
        if (Selected == null)
            SetUIActive(false);
    }

    void OnEnable()
    {
        allTowers.Add(this);

        // Register the UI that should follow the selection
        if (buttonPanel != null)
        {
            selectionUI.Add(buttonPanel);
        }
        else
        {
            // No panel assigned: hide the buttons themselves instead
            if (upgradeRangeButton != null) selectionUI.Add(upgradeRangeButton.gameObject);
            if (upgradeDamageButton != null) selectionUI.Add(upgradeDamageButton.gameObject);
            if (sellbutton != null) selectionUI.Add(sellbutton.gameObject);
        }

        if (extraUI != null)
        {
            foreach (GameObject ui in extraUI)
            {
                if (ui != null) selectionUI.Add(ui);
            }
        }

        Wire(upgradeRangeButton, UpgradeSelectedRange);
        Wire(upgradeDamageButton, UpgradeSelectedDamage);
        Wire(sellbutton, SellSelected);
    }

    void OnDisable()
    {
        allTowers.Remove(this);

        if (Selected == this)
            SetSelected(null);
    }

    public void UpgradeRange()
    {
        range = Mathf.Min(range + rangeIncrease, maxRange);
    }

    public void UpgradeDamage()
    {
        damage = Mathf.Min(damage + damageIncrease, maxDamage);
    }

    public void Sell()
    {
        // Hide the UI (and relock the cursor) right away instead of waiting for the end of the frame
        if (Selected == this)
            SetSelected(null);

        // Use the assigned manager, or find the one in the scene if this tower's slot is empty
        if (levelManager2 == null)
            levelManager2 = FindFirstObjectByType<LevelManager2>();

        if (levelManager2 != null)
        {
            playerController2.canLook = true;
            playerController2.canShoot = true;
            print("Sold tower, adding 5 currency");
            levelManager2.AddCurrency(5);
        }
        else
        {
            Debug.LogWarning("Sell: no LevelManager2 found in the scene, so no currency was added.", this);
        }

        // The tower is always sold, even if the currency couldn't be added
        Destroy(gameObject);
    }

    void Update()
    {
        HandleClick();
        HandleInteract();

        // Copy the live list once per frame, then work from the copy
        enemyBuffer.Clear();
        enemyBuffer.AddRange(EnemyBase.All);

        EnemyBase target = FindNearestEnemyInRange();

        if (target == null)
        {
            ResetRotation();
            return;
        }

        // Track and damage only the nearest enemy
        RotateToward(target.transform.position);
        timeBeforeNextAttack += Time.deltaTime;
        if (timeBeforeNextAttack >= 1f)
        {
            target.TakeDamage(damage);
            timeBeforeNextAttack -= 1f;
        }
    }

    EnemyBase FindNearestEnemyInRange()
    {
        EnemyBase nearest = null;
        float nearestSqr = range * range;

        foreach (EnemyBase enemy in enemyBuffer)
        {
            if (enemy == null) continue;

            float sqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqr <= nearestSqr)
            {
                nearestSqr = sqr;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void RotateToward(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        if (direction == Vector3.zero) return;

        if (smoothRotate)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void ResetRotation()
    {
        if (!resetWhenOutOfRange) return;

        if (smoothRotate)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialLocalRotation, resetSpeed * Time.deltaTime);
        }
        else
        {
            transform.localRotation = initialLocalRotation;
        }
    }

    // Yellow = attack range, cyan = how close you must be to press E
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    // Green range on whichever tower is selected in play mode
    void OnDrawGizmos()
    {
        if (Selected != this) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}