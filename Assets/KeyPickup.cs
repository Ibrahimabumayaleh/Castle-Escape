using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject keyIconUI;
    public GameObject notificationUI;
    public float notificationDuration = 2.5f;

    [Header("Interaction Settings")]
    public float pickupDistance = 3.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip keyPickupSound;

    public static bool hasKey = false;
    private Camera mainCam;
    private bool isPicked = false;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPicked)
        {
            CheckPickupRay();
        }
    }

    void CheckPickupRay()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            // فحص ما إذا كان المؤشر يشير إلى المفتاح نفسه أو مجسم فرعي داخله
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                PickKey();
            }
        }
    }

    public void PickKey()
    {
        isPicked = true;
        hasKey = true;

        if (keyIconUI != null)
        {
            keyIconUI.SetActive(true);
        }

        if (keyPickupSound != null)
        {
            AudioSource.PlayClipAtPoint(keyPickupSound, transform.position);
        }

        if (notificationUI != null)
        {
            notificationUI.SetActive(true);
            KeyPickupHelper helper = FindFirstObjectByType<KeyPickupHelper>();
            if (helper == null)
            {
                GameObject helperObj = new GameObject("KeyPickupHelper");
                helper = helperObj.AddComponent<KeyPickupHelper>();
            }
            helper.HideAfterDelay(notificationUI, notificationDuration);
        }

        gameObject.SetActive(false);
    }
}