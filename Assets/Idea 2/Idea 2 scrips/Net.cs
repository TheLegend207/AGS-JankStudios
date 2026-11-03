using UnityEngine;
public class Net : MonoBehaviour
{
    public CaptureSystem captureSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        captureSystem = FindFirstObjectByType<CaptureSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (captureSystem != null)
            {
                captureSystem.StartCapture(other.gameObject);
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
