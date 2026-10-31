using System.Collections.Generic;
using UnityEngine;

public class CauldronDipPuzzle : MonoBehaviour
{
    [Header("Red Liquid Material")]
    public Material redPotionMaterial;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip dipSound;

    [Header("Detection Distance")]
    public float dipThreshold = 1.6f; // مسافة التغميس داخل السائل

    // تتبع القوارير الموجودة حالياً داخل السائل
    private HashSet<int> bottlesCurrentlyInside = new HashSet<int>();

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = 1f;
        }
    }

    void Update()
    {
        CheckBottlesState();
    }

    void CheckBottlesState()
    {
        // العثور على جميع القوارير في المشهد
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        foreach (MeshRenderer r in renderers)
        {
            if (r.transform == transform || r.transform.IsChildOf(transform)) continue;

            string objName = r.gameObject.name.ToLower();
            string rootName = r.transform.root.name.ToLower();

            // فحص إذا كان المجسم قارورة
            if (objName.Contains("potion") || objName.Contains("blue") || objName.Contains("bottle") ||
                rootName.Contains("potion") || rootName.Contains("blue") || rootName.Contains("bottle") ||
                r.CompareTag("BlueBottle") || r.CompareTag("Pickable"))
            {
                int bottleID = r.transform.root.gameObject.GetInstanceID();
                float dist = Vector3.Distance(r.transform.position, transform.position);

                // 1. لحظة دخول القارورة إلى السائل
                if (dist <= dipThreshold)
                {
                    if (!bottlesCurrentlyInside.Contains(bottleID))
                    {
                        bottlesCurrentlyInside.Add(bottleID);

                        // تغيير اللون للأحمر
                        ChangeLiquidMaterial(r.transform.root.gameObject);

                        // تشغيل الصوت ضربة واحدة فقط
                        PlayDipAudio();
                    }
                }
                // 2. لحظة إخراج القارورة بعيداً عن السائل
                else if (dist > dipThreshold + 0.5f)
                {
                    if (bottlesCurrentlyInside.Contains(bottleID))
                    {
                        bottlesCurrentlyInside.Remove(bottleID); // تصبح جاهزة لتشغيل الصوت عند إدخالها ثانية
                    }
                }
            }
        }
    }

    void ChangeLiquidMaterial(GameObject rootBottle)
    {
        if (redPotionMaterial == null) return;

        MeshRenderer[] allRends = rootBottle.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in allRends)
        {
            if (rend.name.ToLower().Contains("liquid") || rend.name.ToLower().Contains("fluid") || rend.name.ToLower().Contains("inside"))
            {
                rend.material = redPotionMaterial;
                return;
            }
        }

        // إذا كانت مادة واحدة
        foreach (MeshRenderer rend in allRends)
        {
            Material[] mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.ToLower().Contains("blue") || mats[i].name.ToLower().Contains("liquid"))
                {
                    mats[i] = redPotionMaterial;
                }
            }
            rend.materials = mats;
        }
    }

    void PlayDipAudio()
    {
        if (dipSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(dipSound);
        }
        else
        {
            AudioSource.PlayClipAtPoint(dipSound, transform.position, 1f);
        }
    }
}