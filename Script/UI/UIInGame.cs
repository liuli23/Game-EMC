using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    private HP_Sub_Tween hpTween;

    //[SerializeField] private float dashCooldown;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image fireImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image cloneImage;
    [SerializeField] private Image flaskImage;
    [SerializeField] private TextMeshProUGUI crystalAmount;

    [SerializeField] private TextMeshProUGUI mechanicalComponents;

    private SkillManager skill;//非必要，可用单例


    [Header("货币信息")]
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private float moneyAmount;
    [SerializeField] private float increaseRate = 100;

    void Start()
    {
        hpTween = GetComponentInChildren<HP_Sub_Tween>();
        if (playerStats != null) playerStats.onHpChanged += UpdateHpUI;

        crystalAmount.color = Color.clear;
        skill = SkillManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        UpdataMoneyUI();

        //mechanicalComponents.text = PlayerManager.instance.GetCurrency().ToString("#,# :");


        if (Input.GetKeyDown(KeyCode.L)) SetCooldown(dashImage);
        if (Input.GetKeyDown(KeyCode.O) && skill.blackhole.canUseBlackhole) SetCooldown(blackholeImage);
        if (Input.GetKeyUp(KeyCode.H)) SetCooldown(fireImage);
        if (Input.GetKeyDown(KeyCode.P) && skill.crystal.canUseCrystal) SetCooldown(crystalImage);
        if (Input.GetKeyDown(KeyCode.F) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null) SetCooldown(flaskImage);

        GetCrystalAmount();

        CheckCooldown(dashImage, skill.dash.cooldown);
        CheckCooldown(blackholeImage, skill.blackhole.cooldown);
        CheckCooldown(fireImage, skill.fire.cooldown);
        CheckCooldown(cloneImage, skill.clone.cooldown);
        CheckCooldown(flaskImage, Inventory.instance.flaskCooldown);
        CheckCooldown(crystalImage, skill.crystal.cooldown);

    }

    private void UpdataMoneyUI()
    {
        if (moneyAmount < PlayerManager.instance.GetCurrency())
        {
            moneyAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            moneyAmount = PlayerManager.instance.GetCurrency();
        }

        currentMoney.text = ((int)moneyAmount).ToString();
    }

    private void UpdateHpUI()
    {
        slider.maxValue = playerStats.GetMaxHpValue();
        slider.value = playerStats.currentHp;

        if (hpTween == null)
        {
            return;
        }
        hpTween.Start_Tween();
    }

    private void SetCooldown(Image _image)
    {
        if (_image.fillAmount == 0 ) _image.fillAmount = 1;
    }

    private void CheckCooldown(Image _image, float _cooldown)
    {
        if (_cooldown == 0)
        {
            _image.fillAmount = 0;
            return;
        }
        if (_image.fillAmount > 0) _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void GetCrystalAmount()
    {
        if (!skill.crystal.canMuti) return;
        crystalAmount.color = Color.white;
        crystalAmount.text = skill.crystal.amountOfStacks.ToString();
    }


}
