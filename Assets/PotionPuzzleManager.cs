using UnityEngine;

public class PotionPuzzleManager : MonoBehaviour
{
    [Header("رفوف القوارير (من اليسار لليمين)")]
    public ShelfSlot[] topRowSlots = new ShelfSlot[5];    // الرف العلوي
    public ShelfSlot[] bottomRowSlots = new ShelfSlot[5]; // الرف السفلي

    [Header("سكربت الباب")]
    public DoorController doorController;

    // ترتيب الكتاب:
    // العلوي: أزرق - أحمر - أزرق - أحمر - أزرق
    private readonly PotionColor[] targetTopRow = {
        PotionColor.Blue, PotionColor.Red, PotionColor.Blue, PotionColor.Red, PotionColor.Blue
    };

    // السفلي: أحمر - أزرق - أحمر - أزرق - أحمر
    private readonly PotionColor[] targetBottomRow = {
        PotionColor.Red, PotionColor.Blue, PotionColor.Red, PotionColor.Blue, PotionColor.Red
    };

    private bool isSolved = false;

    public void CheckPuzzle()
    {
        if (isSolved) return;

        // 1. فحص الرف العلوي
        for (int i = 0; i < 5; i++)
        {
            if (topRowSlots[i] == null || topRowSlots[i].GetSlotColor() != targetTopRow[i])
                return;
        }

        // 2. فحص الرف السفلي
        for (int i = 0; i < 5; i++)
        {
            if (bottomRowSlots[i] == null || bottomRowSlots[i].GetSlotColor() != targetBottomRow[i])
                return;
        }

        // 3. إذا كل الألوان صحيحة
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("🎉 تم حل اللغز! الباب يفتح الآن...");

        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}