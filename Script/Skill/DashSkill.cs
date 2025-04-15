using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("冲刺")]
    public bool dashUnlocked;

    /*
    [Header("冲刺时幻影")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UISkillTreeSlot cloneDashButton;


    [Header("冲刺结束时幻影")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UISkillTreeSlot cloneArrivalButton;
    */

    public override void UseSkill()
    {
        base.UseSkill();

        //Debug.Log("冲刺技能");
    }


    protected override void Start()
    {
        base.Start();
        /*
        cloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        cloneArrivalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneArrival);
        //注：可能会有bug，和UI里的事件执行顺序有关
        */
    }


    /*
    private void UnlockCloneDash()
    {
        //Debug.Log("试图解锁冲刺");

        if (cloneDashButton.unlocked)
        {
            //Debug.Log("解锁冲刺");
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
