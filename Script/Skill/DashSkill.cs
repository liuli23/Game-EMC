using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("���")]
    public bool dashUnlocked;

    /*
    [Header("���ʱ��Ӱ")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UISkillTreeSlot cloneDashButton;


    [Header("��̽���ʱ��Ӱ")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UISkillTreeSlot cloneArrivalButton;
    */

    public override void UseSkill()
    {
        base.UseSkill();

        //Debug.Log("��̼���");
    }


    protected override void Start()
    {
        base.Start();
        /*
        cloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        cloneArrivalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneArrival);
        //ע�����ܻ���bug����UI����¼�ִ��˳���й�
        */
    }


    /*
    private void UnlockCloneDash()
    {
        //Debug.Log("��ͼ�������");

        if (cloneDashButton.unlocked)
        {
            //Debug.Log("�������");
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneArrival()
    {

        if (cloneArrivalButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }
    */


}
