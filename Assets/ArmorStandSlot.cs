using UnityEngine;

public class ArmorStandSlot : MonoBehaviour
{
    public Transform snapPoint;
    public float gizmoRadius = 0.5f; // حجم دائرة الرؤية
    [HideInInspector] public GameObject currentItem;

    public bool IsOccupied => currentItem != null;

    public void SnapItem(GameObject item)
    {
        currentItem = item;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Transform target = (snapPoint != null) ? snapPoint : transform;
        item.transform.position = target.position;
        item.transform.rotation = target.rotation;
        item.transform.SetParent(transform, true);

        PuzzleDoorController doorController = FindFirstObjectByType<PuzzleDoorController>();
        if (doorController != null)
        {
            doorController.CheckPuzzleState();
        }
    }

    public void ReleaseItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            currentItem = null;

            PuzzleDoorController doorController = FindFirstObjectByType<PuzzleDoorController>();
            if (doorController != null)
            {
                doorController.CheckPuzzleState();
            }
        }
    }

    // إظهار مجال النقطة واتجاه الدرع داخل نافذة الـ Scene
    private void OnDrawGizmos()
    {
        Gizmos.color = IsOccupied ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);

        // رسم سهم لمعرفة اتجاه وقفة الدرع
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 0.4f);
    }
}