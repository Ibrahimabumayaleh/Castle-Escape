using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator wallAnimator;                 // الـ Animator الخاص بـ wall5
    public string animationName = "New Animation"; // اسم ملف الأنيمايشن
    public float interactDistance = 3.0f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip torchInteractSound;          // صوت سحب/ضغط الشعلة (اختياري)
    public AudioClip wallOpenSound;               // صوت انزلاق وتحرك الحائط الحجري

    private Camera mainCam;
    private bool isOpened = false;

    void Start()
    {
        mainCam = Camera.main;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // جعل الصوت 3D واقعي يخرج من مكان الحائط والشعلة
        audioSource.spatialBlend = 1.0f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            CheckInteraction();
        }
    }

    void CheckInteraction()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                if (wallAnimator != null)
                {
                    // تشغيل الأنيميشن
                    wallAnimator.Play(animationName);
                    isOpened = true;

                    // تشغيل صوت تفاعل الشعلة
                    if (torchInteractSound != null)
                    {
                        audioSource.PlayOneShot(torchInteractSound);
                    }

                    // تشغيل صوت تحرك الجدار السري
                    if (wallOpenSound != null)
                    {
                        audioSource.PlayOneShot(wallOpenSound);
                    }
                }
                else
                {
                    Debug.LogWarning("تأكد من سحب wall5 إلى خانة Wall Animator في الـ Inspector!");
                }
            }
        }
    }
}