using UnityEngine;
using System.Collections;

public class DippableBottle : MonoBehaviour
{
    [Header("Settings")]
    public Material redPotionMaterial; // الخامة الحمراء
    public float dipTimeRequired = 2f; // الوقت المطلوب للتغطيس (ثانيتين)

    private Collider bottleCollider;
    private Collider liquidCollider;
    private MeshRenderer meshRenderer;
    private bool isRed = false;
    private bool isDipping = false;
    private Coroutine dipCoroutine;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bottleCollider = GetComponent<Collider>();

        // البحث عن مجسم السائل
        GameObject liquidObj = GameObject.Find("RedLiquid");
        if (liquidObj != null)
        {
            liquidCollider = liquidObj.GetComponent<Collider>();
        }
    }

    void Update()
    {
        if (isRed || liquidCollider == null || bottleCollider == null) return;

        // فحص إذا كانت العلبة ملامسة للسائل
        bool isTouching = bottleCollider.bounds.Intersects(liquidCollider.bounds);

        if (isTouching && !isDipping)
        {
            // بدء العد التنازلي للثانيتين عند الدخول
            dipCoroutine = StartCoroutine(StartDippingTimer());
        }
        else if (!isTouching && isDipping)
        {
            // إلغاء العد إذا طلع اللاعب العلبة قبل ما تخلص الثانيتين
            if (dipCoroutine != null)
            {
                StopCoroutine(dipCoroutine);
            }
            isDipping = false;
        }
    }

    IEnumerator StartDippingTimer()
    {
        isDipping = true;

        // الانتظار للوقت المحدد (2 ثانية)
        yield return new WaitForSeconds(dipTimeRequired);

        // تغيير اللون للأحمر بعد اكتمال الثانيتين
        MakeRed();
    }

    void MakeRed()
    {
        if (meshRenderer != null && redPotionMaterial != null)
        {
            meshRenderer.material = redPotionMaterial;
            isRed = true;
            gameObject.name = "Potion_Red_Dipped";

            // تحديث حالة اللون ليتعرف عليه نظام اللغز والرفوف
            PotionBottle bottle = GetComponent<PotionBottle>();
            if (bottle != null)
            {
                bottle.currentColor = PotionColor.Red;
            }
        }
    }
}