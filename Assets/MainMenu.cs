using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("اكتب هنا اسم مشهد اللعبة الأساسي تماماً كما هو مكتوب في ملفاته")]
    public string gameSceneName = "castleDemo";

    void Start()
    {
        // إظهار الماوس وتحريره داخل القائمة
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        Time.timeScale = 1f; // لضمان أن الوقت يعمل
        SceneManager.LoadScene(gameSceneName);
    }
}