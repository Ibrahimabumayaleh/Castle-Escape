using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinSequenceManager : MonoBehaviour
{
    [Header("Player & HUD References")]
    public FirstPersonController playerController;
    public GameObject pauseHUDButton;

    [Header("UI References")]
    public Image whiteFadeScreen;
    public GameObject winParchmentPanel; // لوحة المخطوطة والأزرار

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float delayBeforePanel = 0.5f;
    public string mainMenuSceneName = "start";

    private bool isWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isWon)
        {
            isWon = true;
            StartCoroutine(PlayWinSequence());
        }
    }

    private IEnumerator PlayWinSequence()
    {
        // 1. إيقاف حركة اللاعب والكاميرا فوراً
        if (playerController != null)
        {
            playerController.playerCanMove = false;
            playerController.cameraCanMove = false;

            // تصفير سرعة الجسم الفيزيائي إن وجد
            Rigidbody rb = playerController.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // 2. إخفاء زر التوقف على الشاشة
        if (pauseHUDButton != null)
        {
            pauseHUDButton.SetActive(false);
        }

        // 3. التلاشي التدريجي للشاشة البيضاء
        if (whiteFadeScreen != null)
        {
            whiteFadeScreen.gameObject.SetActive(true);
            float elapsed = 0f;
            Color col = whiteFadeScreen.color;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                col.a = Mathf.Clamp01(elapsed / fadeDuration);
                whiteFadeScreen.color = col;
                yield return null;
            }

            col.a = 1f;
            whiteFadeScreen.color = col;
        }

        // انتظار بسيط قبل إظهار المخطوطة
        yield return new WaitForSeconds(delayBeforePanel);

        // 4. إظهار لوحة المخطوطة وفك قفل الماوس
        if (winParchmentPanel != null)
        {
            winParchmentPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // دوال الأزرار
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}