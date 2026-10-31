using UnityEngine;

public class ApplySettings : MonoBehaviour
{
    [Header("Lighting")]
    public Light[] sceneLights;

    // مصفوفة لحفظ الشدة الأصلية لكل ضوء في المشهد
    private float[] baseIntensities;

    void Awake()
    {
        // حفظ شدة كل ضوء كما صممتها بالضبط قبل بدء اللعب
        if (sceneLights != null && sceneLights.Length > 0)
        {
            baseIntensities = new float[sceneLights.Length];
            for (int i = 0; i < sceneLights.Length; i++)
            {
                if (sceneLights[i] != null)
                {
                    baseIntensities[i] = sceneLights[i].intensity;
                }
            }
        }
    }

    void Start()
    {
        // 1. تطبيق نسبة الإضاءة (القيمة الافتراضية 1 تعني 100% من الإضاءة الأصلية)
        float brightnessMultiplier = PlayerPrefs.GetFloat("Brightness", 1f);
        ApplyBrightness(brightnessMultiplier);

        // 2. تطبيق الصوت
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volume;
    }

    public void ApplyBrightness(float multiplier)
    {
        if (sceneLights != null && baseIntensities != null)
        {
            for (int i = 0; i < sceneLights.Length; i++)
            {
                if (sceneLights[i] != null)
                {
                    // ضرب الشدة الأصلية بنسبة السلايدر
                    sceneLights[i].intensity = baseIntensities[i] * multiplier;
                }
            }
        }
    }
}