using UnityEngine;
using System.Collections;

public class KeyPickupHelper : MonoBehaviour
{
    public void HideAfterDelay(GameObject target, float delay)
    {
        StartCoroutine(HideRoutine(target, delay));
    }

    private IEnumerator HideRoutine(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            target.SetActive(false);
        }
    }
}