using UnityEngine;
using System.Collections;

public class KnightJumpscare : MonoBehaviour
{
    [Header("Knight Components")]
    [Tooltip("إذا تركته فارغاً سيتم جلبه تلقائياً من نفس المجسم أو الأطفال")]
    public Animator knightAnimator;
    public AudioSource knightAudioSource;

    [Header("Settings")]
    public string triggerParameterName = "ScareTrigger";
    public string stopParameterName = "StopTalking"; // اسم التريجر الذي يعيد الفارس للـ Idle

    [Tooltip("تفعيل هذا الخيار سيقوم بتدمير منطقة الـ Trigger بعد تشغيل القفزة لمنع التكرار نهائياً")]
    public bool destroyTriggerAfterUse = false;

    // متغيّر يمنع التكرار نهائياً بعد المرة الأولى
    private bool hasTriggered = false;

    private void Awake()
    {
        // جلب المكونات تلقائياً إذا لم يتم تعيينها يدوياً في الـ Inspector
        if (knightAnimator == null)
        {
            knightAnimator = GetComponentInChildren<Animator>();
        }

        if (knightAudioSource == null)
        {
            knightAudioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // التأكد من أن الذي دخل هو اللاعب، ولم يتم التفعيل من قبل
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // منع التكرار فوراً

            StartCoroutine(PlayJumpscareSequence());
        }
    }

    private IEnumerator PlayJumpscareSequence()
    {
        float audioDuration = 0f;

        // تشغيل الصوت وتحديد مدته
        if (knightAudioSource != null && knightAudioSource.clip != null)
        {
            knightAudioSource.Play();
            audioDuration = knightAudioSource.clip.length; // جلب مدة الصوت (مثلاً 20 ثانية)
        }
        else
        {
            Debug.LogWarning("KnightJumpscare: لم يتم العثور على AudioSource أو AudioClip!", this);
        }

        // تشغيل حركة الفارس
        if (knightAnimator != null)
        {
            knightAnimator.SetTrigger(triggerParameterName);
        }
        else
        {
            Debug.LogWarning("KnightJumpscare: لم يتم العثور على Animator!", this);
        }

        // الانتظار حتى انتهاء مقطع الصوت بالكامل
        if (audioDuration > 0f)
        {
            yield return new WaitForSeconds(audioDuration);
        }

        // إعادة الفارس لوضعية الـ Idle بعد انتهاء الصوت
        if (knightAnimator != null && !string.IsNullOrEmpty(stopParameterName))
        {
            knightAnimator.SetTrigger(stopParameterName);
        }

        // خيار إضافي: تدمير كائن الـ Trigger بعد الانتهاء
        if (destroyTriggerAfterUse)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}