using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    [Header("الحالة")]
    public PotionBottle currentBottle;
    public PotionPuzzleManager puzzleManager;
    public Transform snapPoint;

    public PotionColor GetSlotColor()
    {
        return currentBottle != null ? currentBottle.currentColor : PotionColor.None;
    }

    private void OnTriggerStay(Collider other)
    {
        // إذا الخانة أصلاً فيها قارورة، ما تعمل اشي
        if (currentBottle != null) return;

        PotionBottle bottle = other.GetComponent<PotionBottle>();
        if (bottle != null && !bottle.isSnapped)
        {
            SnapBottle(bottle);
        }
    }

    public void SnapBottle(PotionBottle bottle)
    {
        currentBottle = bottle;
        bottle.isSnapped = true;

        // فصل القارورة عن يد اللاعب أو الكاميرا
        bottle.transform.SetParent(null);

        // تثبيت مكان وزاوية القارورة بالسنتر
        Transform target = (snapPoint != null) ? snapPoint : transform;
        bottle.transform.position = target.position;
        bottle.transform.rotation = target.rotation;
        bottle.transform.SetParent(transform);

        // تجميد الفيزياء تماماً
        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // إشعار المدير بفحص اللغز
        if (puzzleManager != null)
        {
            puzzleManager.CheckPuzzle();
        }
    }

    public void ReleaseBottle()
    {
        if (currentBottle != null)
        {
            currentBottle.isSnapped = false;
            currentBottle.transform.SetParent(null);

            Rigidbody rb = currentBottle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            currentBottle = null;

            if (puzzleManager != null)
            {
                puzzleManager.CheckPuzzle();
            }
        }
    }
}