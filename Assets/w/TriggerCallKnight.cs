using UnityEngine;

public class TriggerCallKnight : MonoBehaviour
{
    public KnightEndScene knight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && knight != null)
        {
            knight.TriggerKnightSequence();
            GetComponent<Collider>().enabled = false; // لمنع التكرار
        }
    }
}