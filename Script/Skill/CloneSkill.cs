using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool; // ��������ռ�
public class CloneSkill : Skill
{


    [Header("���������")]
    [SerializeField] private int defaultPoolSize = 5;
    [SerializeField] private int maxPoolSize = 20;

    private ObjectPool<GameObject> clonePool;



    [Header("��¡��Ϣ")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private float checkDistance;
    [SerializeField] private float delaytime;
    [Range(0f, 1f)]
    [SerializeField] private float cloneAttackMultiplier;//��¡��������
    public bool canCloneUseEffect;



    [Header("�񵲻�Ӱ")]
    [SerializeField] private UISkillTreeSlot cloneCounterAttackButton;
    public bool cloneOnCounterAttackUnlocked;
    //public bool cloneOnCounterAttackUnlocked { get; private set; }
    //[SerializeField] private bool cloneOnCounterAttackUnclocked;

    [Header("��̻�Ӱ")]
    [SerializeField] private UISkillTreeSlot cloneDashButton;
    public bool cloneOnDashUnlocked;
    //public bool cloneOnDashUnlocked { get; private set; }
    //[SerializeField] private bool cloneOnDashUnlocked;

    [Header("��̽�����Ӱ")]
    [SerializeField] private UISkillTreeSlot cloneArrivalButton;
    public bool cloneOnArrivalUnlocked;
    //public bool cloneOnArrivalUnlocked{ get; private set; }
    //[SerializeField] private bool cloneOnArrivalUnlocked;


    [Header("�չ���Ӱ")]
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
        // �Ӷ���ػ�ȡ��¡
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
    //�ӳٴ�����Ӱ
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

        // �Ӷ���ػ�ȡ������Instantiate
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
        // �ӳ�0.1�����Ӽ�����
        //Invoke("AddListeners", 0.1f);
        //����unity�󶨣��������˳��
        //AddListeners();
        //��ȡ��ע��CounterAttack�İ���bug������֣���̫���
        //���Ϊ�ֶ�������
        // ��ʼ�������
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

    //Ϊ��ť�¼����ί�У�����ʵ�ʽ�������
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
        //Debug.Log("��");
        
    }


    protected override void CheckUnlock()
    {
        UnlockCloneDash();
        UnlockCloneCounterAttack();
        UnlockCloneArrival();
        UnlockCloneOnNormalAttack();    
    }


    //������̻�Ӱ
    public void UnlockCloneDash()
    {
        //Debug.Log("��ͼ�������");
        if (cloneDashButton.unlocked && !cloneOnDashUnlocked)
            //Debug.Log("�������");
            cloneOnDashUnlocked = true;
    }
    //���ʱ��Ӱ
    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
            CreateClone(player.transform);
    }
    //������̽�����Ӱ
    public void UnlockCloneArrival()
    {
        if (cloneArrivalButton.unlocked && !cloneOnArrivalUnlocked)
            cloneOnArrivalUnlocked = true;
    }
    //��̽���ʱ��Ӱ
    public void CreateCloneOnDashOver()
    {
        if(cloneOnArrivalUnlocked)
            CreateClone(player.transform);
    }
    //�����񵲻�Ӱ
    public void UnlockCloneCounterAttack()
    {
        //Debug.Log("�񵲻�Ӱ����");
        if (cloneCounterAttackButton.unlocked && !cloneOnCounterAttackUnlocked)
        {
            cloneOnCounterAttackUnlocked = true;
        }


    }

    //�񵲳ɹ�ʱ��Ӱ��������ݿ���ֱ�ӻ�ȡ������������Ҫ�ڵ��ô����⴫��
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if(cloneOnCounterAttackUnlocked)
            CreateClone(_enemyTransform);
    }
    //�����չ���Ӱ
    public void UnlockCloneOnNormalAttack() 
    {
        //Debug.Log("�չ���Ӱ����");
        if (cloneNormalAttackButton.unlocked && !cloneOnNormalAttackUnlocked)
        {
            canCloneUseEffect = true;
            cloneOnNormalAttackUnlocked = true;
        }
    }
    //�չ���Ӱ
    public void CreateCloneOnNormalAttack()
    {
        if (cloneOnNormalAttackUnlocked)
            StartCoroutine(CreateCloneDelay(player.transform));

    }


}
