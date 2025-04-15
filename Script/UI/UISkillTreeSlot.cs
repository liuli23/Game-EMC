using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    private UI ui;
    private Image skillImage;


    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color lockedSkillColor;


    public bool unlocked;

    [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UISkillTreeSlot[] shouldBelocked;



    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot-UI-" + skillName;
    }

    /*
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        //Debug.Log("绑定2");
    }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        /*
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Start Button Clicked → 发送事件");
            EventManager.Invoke(EventID.START_BUTTON_CLICK);
        });
        */
        if (unlocked) skillImage.color = Color.white;

        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        //GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        //改动说明：移至unity界面绑定
        //Debug.Log("绑定");
        //EventManager.AddListener(EventID.START_BUTTON_CLICK, UnlockSkillSlot, priority: 10);



        ui = GetComponentInParent<UI>();
    }

    public void UnlockSkillSlot()
    {

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("不能解锁技能,因为"+ shouldBeUnlocked[i]+"未解锁");
                return;
            }

        }

        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked)
            {
                Debug.Log("不能解锁技能");
                return;
            }
        }
        if (unlocked) return;
        if (!PlayerManager.instance.HaveEnoughMoney(skillCost)) return;

        unlocked = true;
        skillImage.color = Color.white;
        //Debug.Log("解锁");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName,skillCost);

        /*
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -150;
        else xOffset = 150;
        if (mousePosition.y > 320)
            yOffset = -150;
        else yOffset = 150;

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        */
        //ui偏移，挺有用的，偏移量需要自己Debug找
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }

    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);

        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}
