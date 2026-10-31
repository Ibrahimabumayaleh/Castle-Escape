using UnityEngine;

public class PuzzleDoorController : MonoBehaviour
{
    public Transform doorTransform;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public AudioSource doorSound;

    public int requiredItemsCount = 3;

    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetOpenRotation;

    void Start()
    {
        if (doorTransform != null)
        {
            closedRotation = doorTransform.rotation;
            targetOpenRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        }
    }

    void Update()
    {
        if (isDoorOpen && doorTransform != null)
        {
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, targetOpenRotation, Time.deltaTime * openSpeed);
        }
    }

    public void CheckPuzzleState()
    {
        ArmorStandSlot[] slots = FindObjectsByType<ArmorStandSlot>(FindObjectsSortMode.None);
        int filledSlotsCount = 0;

        foreach (ArmorStandSlot slot in slots)
        {
            if (slot.IsOccupied) filledSlotsCount++;
        }

        if (filledSlotsCount >= requiredItemsCount && !isDoorOpen)
        {
            isDoorOpen = true;

            if (doorSound != null)
            {
                doorSound.Play();
            }
        }
    }
}