using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 3.5f;
    public float snapDetectionRadius = 2.0f;

    [Header("Hold Scale Settings")]
    public float holdScaleMultiplier = 0.45f;

    [Header("Default Audio Settings")]
    public AudioSource audioSource;
    public AudioClip defaultPickupSound;
    public AudioClip defaultDropSound;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private ItemAudio heldItemAudio;
    private Vector3 originalScale;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (heldObject == null) TryPickUpObject();
            else DropObject();
        }
    }

    void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                heldObject = hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();
                heldItemAudio = heldObject.GetComponentInChildren<ItemAudio>();

                ShelfSlot currentSlot = heldObject.GetComponentInParent<ShelfSlot>();
                if (currentSlot != null) currentSlot.ReleaseBottle();

                ArmorStandSlot currentArmorSlot = heldObject.GetComponentInParent<ArmorStandSlot>();
                if (currentArmorSlot != null) currentArmorSlot.ReleaseItem();

                if (heldRb != null)
                {
                    heldRb.isKinematic = true;
                    heldRb.useGravity = false;
                }

                heldObject.layer = LayerMask.NameToLayer("HeldItem");
                originalScale = heldObject.transform.localScale;

                heldObject.transform.SetParent(holdPoint);

                // فحص إذا كان الغرض هو القارورة
                bool isBottle = heldObject.GetComponentInChildren<PotionBottle>() != null ||
                                heldObject.name.ToLower().Contains("potion") ||
                                heldObject.name.ToLower().Contains("blue") ||
                                heldObject.name.ToLower().Contains("bottle");

                if (isBottle)
                {
                    // إعادة الزجاجة إلى موضعها البعيد السابق تماماً (أمام الكاميرا مباشرة بمسافة Z = 2)
                    heldObject.transform.localScale = originalScale;
                    heldObject.transform.localPosition = new Vector3(-holdPoint.localPosition.x, 0, 1.8f);
                    heldObject.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    // الدرع وباقي الأغراض تبقى في مكان اليد القريب على اليمين وتصغر
                    heldObject.transform.localScale = originalScale * holdScaleMultiplier;
                    heldObject.transform.localPosition = Vector3.zero;
                    heldObject.transform.localRotation = Quaternion.identity;
                }

                AudioClip clipToPlay = (heldItemAudio != null && heldItemAudio.pickupSound != null)
                                       ? heldItemAudio.pickupSound
                                       : defaultPickupSound;
                PlaySound(clipToPlay);
            }
        }
    }

    void DropObject()
    {
        if (heldObject == null) return;

        AudioClip dropClipToPlay = (heldItemAudio != null && heldItemAudio.dropSound != null)
                                   ? heldItemAudio.dropSound
                                   : defaultDropSound;
        PlaySound(dropClipToPlay);

        heldObject.transform.localScale = originalScale;

        // 1. فحص رف القوارير
        PotionBottle bottle = heldObject.GetComponentInChildren<PotionBottle>();
        if (bottle != null)
        {
            ShelfSlot[] allSlots = FindObjectsByType<ShelfSlot>(FindObjectsSortMode.None);
            float closestDist = snapDetectionRadius;
            ShelfSlot targetSlot = null;

            foreach (ShelfSlot slot in allSlots)
            {
                if (slot.currentBottle == null)
                {
                    Transform snapTarget = (slot.snapPoint != null) ? slot.snapPoint : slot.transform;
                    float dist = Vector3.Distance(holdPoint.position, snapTarget.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        targetSlot = slot;
                    }
                }
            }

            if (targetSlot != null)
            {
                heldObject.layer = LayerMask.NameToLayer("Default");
                heldObject.transform.SetParent(null);
                targetSlot.SnapBottle(bottle);
                heldObject = null;
                heldRb = null;
                heldItemAudio = null;
                return;
            }
        }

        // 2. فحص حامل الدروع
        ArmorItem armor = heldObject.GetComponentInChildren<ArmorItem>();
        if (armor != null)
        {
            ArmorStandSlot[] allArmorSlots = FindObjectsByType<ArmorStandSlot>(FindObjectsSortMode.None);
            float closestDist = snapDetectionRadius;
            ArmorStandSlot targetArmorSlot = null;

            foreach (ArmorStandSlot slot in allArmorSlots)
            {
                if (!slot.IsOccupied)
                {
                    Transform snapTarget = (slot.snapPoint != null) ? slot.snapPoint : slot.transform;
                    float dist = Vector3.Distance(holdPoint.position, snapTarget.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        targetArmorSlot = slot;
                    }
                }
            }

            if (targetArmorSlot != null)
            {
                heldObject.layer = LayerMask.NameToLayer("Default");
                heldObject.transform.SetParent(null);
                targetArmorSlot.SnapItem(heldObject);
                heldObject = null;
                heldRb = null;
                heldItemAudio = null;
                return;
            }
        }

        // 3. التنزيل العادي بالفيزياء
        heldObject.transform.SetParent(null);
        heldObject.layer = LayerMask.NameToLayer("Default");

        Physics.SyncTransforms();

        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.angularDamping = 10f;
            heldRb.linearDamping = 5f;
            heldRb.linearVelocity = Vector3.zero;
            heldRb.angularVelocity = Vector3.zero;
        }

        heldObject = null;
        heldRb = null;
        heldItemAudio = null;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}