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
        //Debug.Log("��2");
    }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        /*
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Start Button Clicked �� �����¼�");
            EventManager.Invoke(EventID.START_BUTTON_CLICK);
        });
        */
        if (unlocked) skillImage.color = Color.white;

        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        //GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        //�Ķ�˵��������unity�����
        //Debug.Log("��");
        //EventManager.AddListener(EventID.START_BUTTON_CLICK, UnlockSkillSlot, priority: 10);



        ui = GetComponentInParent<UI>();
    }

    public void UnlockSkillSlot()
    {

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("���ܽ�������,��Ϊ"+ shouldBeUnlocked[i]+"δ����");
                return;
            }

        }

        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked)
            {
                Debug.Log("���ܽ�������");
                return;
            }
        }
        if (unlocked) return;
        if (!PlayerManager.instance.HaveEnoughMoney(skillCost)) return;

        unlocked = true;
        skillImage.color = Color.white;
        //Debug.Log("����");
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
        //uiƫ�ƣ�ͦ���õģ�ƫ������Ҫ�Լ�Debug��
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
