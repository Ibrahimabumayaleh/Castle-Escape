using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider brightnessSlider;
    public Slider volumeSlider;

    [Header("Lighting")]
    public Light[] sceneLights; // مصفوفة لجميع المشاعل/الأضواء

    void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f);
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        brightnessSlider.value = savedBrightness;
        volumeSlider.value = savedVolume;

        SetBrightness(savedBrightness);
        SetVolume(savedVolume);

        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetBrightness(float value)
    {
        if (sceneLights != null)
        {
            foreach (Light l in sceneLights)
            {
                if (l != null)
                {
                    l.intensity = value;
                }
            }
        }
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
    public void QuitGame()
    {
        // لإغلاق اللعبة بعد عمل Build (ملف exe)
        Application.Quit();

        // لإيقاف تشغيل اللعبة داخل محرر Unity أثناء التجربة
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}