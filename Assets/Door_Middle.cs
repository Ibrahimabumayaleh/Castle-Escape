using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("إعدادات حركة الباب")]
    public float openAngle = 90f; // زاوية الفتح
    public float openSpeed = 2f;  // سرعة الحركة

    [Header("إعدادات الصوت")]
    public AudioSource doorAudioSource;
    public AudioClip doorOpenSound; // اسحب ملف الصوت هنا

    private bool shouldOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // تجهيز مكوّن الصوت تلقائياً إذا لم يتم ربطه يدوياً
        if (doorAudioSource == null)
            doorAudioSource = GetComponent<AudioSource>();

        if (doorAudioSource != null)
        {
            doorAudioSource.playOnAwake = false;
            doorAudioSource.spatialBlend = 0f; // 2D لضمان سماعه بوضوح
            doorAudioSource.volume = 1f;
        }

        closedRotation = transform.localRotation;
        // حساب زاوية الفتح على محور Y
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (shouldOpen)
        {
            // تدوير الباب بسلاسة حتى يفتح بالكامل
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    // دالة يستدعيها مدير اللغز لفتح الباب
    public void OpenDoor()
    {
        if (shouldOpen) return; // منع تكرار الصوت إذا كان الباب يفتح بالفعل

        shouldOpen = true;

        // تشغيل صوت فتح الباب مرة واحدة
        if (doorAudioSource != null && doorOpenSound != null)
        {
            doorAudioSource.PlayOneShot(doorOpenSound);
        }
        else if (doorOpenSound != null)
        {
            AudioSource.PlayClipAtPoint(doorOpenSound, transform.position, 1f);
        }
    }
}