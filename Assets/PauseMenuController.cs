using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenuPanel;
    public GameObject pauseHUDButton;

    [Header("Blur Volume")]
    public Volume blurVolume;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "start";

    public static bool isGamePaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        isGamePaused = false;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (pauseHUDButton != null) pauseHUDButton.SetActive(true);
        if (blurVolume != null) blurVolume.weight = 0f;
    }

    void Update()
    {
        bool escPressed = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            escPressed = true;
#else
        if (Input.GetKeyDown(KeyCode.Escape))
            escPressed = true;
#endif

        if (escPressed)
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (pauseHUDButton != null) pauseHUDButton.SetActive(false);
        if (blurVolume != null) blurVolume.weight = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (pauseHUDButton != null) pauseHUDButton.SetActive(true);
        if (blurVolume != null) blurVolume.weight = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Debug.Log("Restart clicked");
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Main Menu clicked");
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit clicked");
        Time.timeScale = 1f;
        Application.Quit();
    }
}