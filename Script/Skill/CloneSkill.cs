using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool; // 添加命名空间
public class CloneSkill : Skill
{


    [Header("对象池配置")]
    [SerializeField] private int defaultPoolSize = 5;
    [SerializeField] private int maxPoolSize = 20;

    private ObjectPool<GameObject> clonePool;



    [Header("克隆信息")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private float checkDistance;
    [SerializeField] private float delaytime;
    [Range(0f, 1f)]
    [SerializeField] private float cloneAttackMultiplier;//克隆攻击倍率
    public bool canCloneUseEffect;



    [Header("格挡幻影")]
    [SerializeField] private UISkillTreeSlot cloneCounterAttackButton;
    public bool cloneOnCounterAttackUnlocked;
    //public bool cloneOnCounterAttackUnlocked { get; private set; }
    //[SerializeField] private bool cloneOnCounterAttackUnclocked;

    [Header("冲刺幻影")]
    [SerializeField] private UISkillTreeSlot cloneDashButton;
    public bool cloneOnDashUnlocked;
    //public bool cloneOnDashUnlocked { get; private set; }
    //[SerializeField] private bool cloneOnDashUnlocked;

    [Header("冲刺结束幻影")]
    [SerializeField] private UISkillTreeSlot cloneArrivalButton;
    public bool cloneOnArrivalUnlocked;
    //public bool cloneOnArrivalUnlocked{ get; private set; }
    //[SerializeField] private bool cloneOnArrivalUnlocked;


    [Header("普攻幻影")]
    [SerializeField] private UISkillTreeSlot cloneNormalAttackButton;
    public bool cloneOnNormalAttackUnlocked;
    //public bool cloneOnNormalAttackUnlocked { get; private set; }
    //[SerializeField] private bool cloneOnNormalAttackUnclocked;

    public void CreateClone(Transform _clonePosition)
    {
        /*
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition,cloneDuration,canAttack,FindClosestEnemy(newClone.transform,checkDistance),cloneAttackMultiplier);
        */
        // 从对象池获取克隆
        GameObject newClone = clonePool.Get();

        CloneSkillController controller = newClone.GetComponent<CloneSkillController>();
        controller.SetupClone(
            _clonePosition,
            cloneDuration,
            canAttack,
            FindClosestEnemy(newClone.transform, checkDistance),
            cloneAttackMultiplier
        );
    }
    //延迟创建幻影
    /*
    private IEnumerator CreateCloneDelay(Transform _transform)
    {
        yield return new WaitForSeconds(delaytime);
        CreateClone(_transform);
    }
    */
    private IEnumerator CreateCloneDelay(Transform _transform)
    {
        yield return new WaitForSeconds(delaytime);

        // 从对象池获取而不是Instantiate
        GameObject newClone = clonePool.Get();
        CloneSkillController controller = newClone.GetComponent<CloneSkillController>();
        controller.SetupClone(
            _transform,
            cloneDuration,
            canAttack,
            FindClosestEnemy(_transform, checkDistance),
            cloneAttackMultiplier
        );
    }




    protected override void Start()
    {
        base.Start();
        // 延迟0.1秒后添加监听器
        //Invoke("AddListeners", 0.1f);
        //移至unity绑定，方便控制顺序
        //AddListeners();
        //不取消注释CounterAttack的绑定有bug，很奇怪，不太理解
        //变更为手动单独绑定
        // 初始化对象池
        clonePool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject clone = Instantiate(clonePrefab);
                clone.GetComponent<CloneSkillController>().SetPool(clonePool);
                return clone;
            },
            actionOnGet: obj =>
            {
                obj.SetActive(true);
                obj.transform.SetParent(null);
            },
            actionOnRelease: obj =>
            {
                obj.SetActive(false);
                obj.GetComponent<CloneSkillController>().ResetState();
            },
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: defaultPoolSize,
            maxSize: maxPoolSize
        );
    }

    //为按钮事件添加委托，用于实际解锁技能
    public void AddListeners()
    {   /*
        Debug.Log("cloneDashButton: " + cloneDashButton);
        Debug.Log("cloneArrivalButton: " + cloneArrivalButton);
        Debug.Log("cloneCounterAttackButton: " + cloneCounterAttackButton);
        Debug.Log("cloneNormalAttackButton: " + cloneNormalAttackButton);
        */


        cloneCounterAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneCounterAttack);
        cloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
        cloneArrivalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneArrival);
        cloneNormalAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnNormalAttack);
        //Debug.Log("绑定");
        
    }


    protected override void CheckUnlock()
    {
        UnlockCloneDash();
        UnlockCloneCounterAttack();
        UnlockCloneArrival();
        UnlockCloneOnNormalAttack();    
    }


    //解锁冲刺幻影
    public void UnlockCloneDash()
    {
        //Debug.Log("试图解锁冲刺");
        if (cloneDashButton.unlocked && !cloneOnDashUnlocked)
            //Debug.Log("解锁冲刺");
            cloneOnDashUnlocked = true;
    }
    //冲刺时幻影
    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
            CreateClone(player.transform);
    }
    //解锁冲刺结束幻影
    public void UnlockCloneArrival()
    {
        if (cloneArrivalButton.unlocked && !cloneOnArrivalUnlocked)
            cloneOnArrivalUnlocked = true;
    }
    //冲刺结束时幻影
    public void CreateCloneOnDashOver()
    {
        if(cloneOnArrivalUnlocked)
            CreateClone(player.transform);
    }
    //解锁格挡幻影
    public void UnlockCloneCounterAttack()
    {
        //Debug.Log("格挡幻影解锁");
        if (cloneCounterAttackButton.unlocked && !cloneOnCounterAttackUnlocked)
        {
            cloneOnCounterAttackUnlocked = true;
        }


    }

    //格挡成功时幻影，玩家数据可以直接获取，但敌人数据要在调用处额外传入
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(cloneOnCounterAttackUnlocked)
            CreateClone(_enemyTransform);
    }
    //解锁普攻幻影
    public void UnlockCloneOnNormalAttack() 
    {
        //Debug.Log("普攻幻影解锁");
        if (cloneNormalAttackButton.unlocked && !cloneOnNormalAttackUnlocked)
        {
            canCloneUseEffect = true;
            cloneOnNormalAttackUnlocked = true;
        }
    }
    //普攻幻影
    public void CreateCloneOnNormalAttack()
    {
        if (cloneOnNormalAttackUnlocked)
            StartCoroutine(CreateCloneDelay(player.transform));

    }


}
