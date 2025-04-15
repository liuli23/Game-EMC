using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats stats;
    private RectTransform rectTransform;
    private Slider slider;
    private HP_Sub_Tween hpTween;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();
        hpTween = GetComponentInChildren<HP_Sub_Tween>();



        entity.onFlipped += FlipUI;
        stats.onHpChanged += UpdateHpUI;

        UpdateHpUI();


    }

    /*
    
    private void Update()
    {
        UpdateHpUI();

    }
    */

    private void UpdateHpUI()
    {
        slider.maxValue = stats.GetMaxHpValue();
        slider.value = stats.currentHp;


        if (hpTween == null)
        {
            return;
        }
        hpTween.Start_Tween();

    }



    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        stats.onHpChanged -= UpdateHpUI;
    }

}
