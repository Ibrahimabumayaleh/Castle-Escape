using UnityEngine;
using System.Collections;

public class AutoTutorialHint : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("عدد الثواني التي ستبقى فيها الرسالة ظاهرة")]
    public float displayDuration = 5f;

    [Tooltip("لوحة الملاحظة التي ستختفي")]
    public GameObject hintPanel;

    void Start()
    {
        if (hintPanel == null)
            hintPanel = gameObject;

        hintPanel.SetActive(true);
        StartCoroutine(HideHintRoutine());
    }

    IEnumerator HideHintRoutine()
    {
        yield return new WaitForSeconds(displayDuration);
        hintPanel.SetActive(false);
    }
}