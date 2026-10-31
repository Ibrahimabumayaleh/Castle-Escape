using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [Header("UI & References")]
    public GameObject keyIconUI;
    public Transform doorTransform;
    public float openAngle = 90f;
    public float openSpeed = 1.2f;

    [Header("Audio")]
    public AudioSource doorAudioSource;
    public AudioClip openSound; // ضع هنا ملف الصوت الواحد المدمج

    [Header("Interaction Settings")]
    public float interactionDistance = 3.5f;

    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetOpenRotation;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        if (doorAudioSource == null)
            doorAudioSource = GetComponent<AudioSource>();

        if (doorAudioSource != null)
        {
            doorAudioSource.playOnAwake = false;
            doorAudioSource.spatialBlend = 0f; // 2D Sound لضمان سماعه بوضوح
            doorAudioSource.volume = 1f;
        }

        if (doorTransform == null) doorTransform = transform;

        closedRotation = doorTransform.rotation;
        targetOpenRotation = closedRotation * Quaternion.Euler(0, -openAngle, 0);
    }

    void Update()
    {
        if (isDoorOpen && doorTransform != null)
        {
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, targetOpenRotation, Time.deltaTime * openSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckDoorInteraction();
        }
    }

    void CheckDoorInteraction()
    {
        if (isDoorOpen) return;
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                TryOpenDoorWithKey();
            }
        }
    }

    public void TryOpenDoorWithKey()
    {
        if (isDoorOpen) return;

        if (KeyPickup.hasKey)
        {
            isDoorOpen = true;
            KeyPickup.hasKey = false;

            if (keyIconUI != null)
            {
                keyIconUI.SetActive(false);
            }

            if (doorAudioSource != null && openSound != null)
            {
                doorAudioSource.PlayOneShot(openSound);
            }
        }
    }
}  