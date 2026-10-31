using UnityEngine;
using TMPro;
using System.Collections;

public class KnightEndScene : MonoBehaviour
{
    [Header("Target & Movement")]
    public Transform playerTransform;
    public float walkSpeed = 1.8f;
    [Tooltip("المسافة التي سيتوقف عندها أمام اللاعب بالأمتار")]
    public float stopDistance = 3.0f;

    [Header("Components")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip speechClip;

    [Header("Animation State Names")]
    public string walkStateName = "walk";       // اسم مربع المشي في الـ Animator
    public string talkStateName = "Talking";    // اسم مربع الكلام في الـ Animator

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [TextArea(3, 5)]
    public string dialogueString = "مرحباً بك مجدداً أيها البطل.. لقد حللت جميع ألغاز القلعة ببراعة! طريق الحرية مفتوح أمامك الآن.";

    [Header("Win Sequence Canvas")]
    public GameObject winCanvas;

    private bool hasStarted = false;

    public void TriggerKnightSequence()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(RunSequence());
        }
    }

    private IEnumerator RunSequence()
    {
        // 1. بدء تشغيل الصوت وظهور النص وأنيميشن المشي فوراً
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            if (dialogueText != null) dialogueText.text = dialogueString;
        }

        if (audioSource != null && speechClip != null)
        {
            audioSource.PlayOneShot(speechClip);
        }

        if (animator != null)
        {
            // الانتقال الفوري لحركة المشي
            animator.CrossFade(walkStateName, 0.1f);
        }

        // 2. المشي نحو اللاعب حتى يصل للمسافة المحددة (stopDistance)
        while (playerTransform != null)
        {
            Vector3 playerPosFlat = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            float currentDist = Vector3.Distance(transform.position, playerPosFlat);

            // إذا وصل للمسافة المطلوبة، يتوقف عن الحركة
            if (currentDist <= stopDistance)
            {
                break;
            }

            // الالتفات نحو اللاعب والمشي للأمام
            transform.LookAt(playerPosFlat);
            transform.position += transform.forward * (walkSpeed * Time.deltaTime);

            yield return null;
        }

        // 3. إيقاف المشي فوراً وتشغيل أنيميشن التحدث (Talking)
        if (animator != null)
        {
            animator.CrossFade(talkStateName, 0.15f);
        }

        // 4. الانتظار حتى ينتهي الصوت والكلام
        float waitTime = (speechClip != null) ? speechClip.length : 5f;
        yield return new WaitForSeconds(waitTime);

        // 5. إخفاء مربع الحوار وإظهار شاشة الفوز
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}