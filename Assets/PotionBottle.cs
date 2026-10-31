using UnityEngine;

public enum PotionColor { None, Blue, Red }

public class PotionBottle : MonoBehaviour
{
    public PotionColor currentColor = PotionColor.None;
    [HideInInspector] public bool isSnapped = false;
}