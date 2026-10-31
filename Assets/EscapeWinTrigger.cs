using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeWinTrigger : MonoBehaviour
{
    [Header("UI Fade Settings")]
    public CanvasGroup whiteFadeGroup; // اسحب الـ Image التي عليها Canvas Group هنا
    public float fadeDuration = 2.5f;   // سرعة تحول الشاشة للضوء الأبيض

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip victorySound;      // صوت سحري أو موسيقى الفوز

    private bool hasTriggered = false;

    void Start()
    {
        if (whiteFadeGroup != null)
        {
            whiteFadeGroup.alpha = 0f;
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // صوت محيطي واضح
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        // التحقق من دخول اللاعب عبر الباب
        if (other.CompareTag("Player") || other.name.ToLower().Contains("firstperson") || other.transform.root.name.ToLower().Contains("player"))
        {
            hasTriggered = true;
            StartCoroutine(FadeToWhiteRoutine());
        }
    }

    private IEnumerator FadeToWhiteRoutine()
    {
        // 1. تشغيل صوت الفوز
        if (victorySound != null)
        {
            if (audioSource != null) audioSource.PlayOneShot(victorySound);
            else AudioSource.PlayClipAtPoint(victorySound, transform.position, 1f);
        }

        // 2. زيادة سطوع الضوء الأبيض تدريجياً
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (whiteFadeGroup != null)
            {
                whiteFadeGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            }
            yield return null;
        }

        if (whiteFadeGroup != null) whiteFadeGroup.alpha = 1f;

        // 3. تحرير الماوس بعد امتلاء الشاشة بالضوء
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // دالة لإعادة اللعب (يمكن ربطها بزر Restart)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}