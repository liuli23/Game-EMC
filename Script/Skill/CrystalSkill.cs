using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystal;
    private GameObject currentCrystal;

    [Header("��������")]
    [SerializeField] private UISkillTreeSlot crystalButton;
    public bool canUseCrystal;



    [Header("׷����Ϣ")]
    [SerializeField] private UISkillTreeSlot crystalCheckDistanceButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float launchSpeed;
    [SerializeField] private float checkDistance;
    [SerializeField] private float biggerCheckDistance;
    [SerializeField] private float homingSpeed;

    [Header("��ը��Ϣ")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed;
    [Range(0f, 2f)]
    [SerializeField] private float crystalMultiplier;

    [Header("�෢��Ϣ")]
    [SerializeField] private UISkillTreeSlot crystalCooldownButton; 
    [SerializeField] private UISkillTreeSlot crystalMutiButton;
    
    public bool canMuti;
    public int amountOfStacks=0;
    [SerializeField] private float mutiStackCooldown;
    //[SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
    [SerializeField] private int maxCapacity;
    [SerializeField] private float mutiTimer = 0;

    protected override void Start()
    {
        base.Start();

        //AddListenerCryStal();//�Ķ�˵��������ᵼ��ȫ���󶨣��޷�����������
        /*
        crystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        crystalCooldownButton.GetComponent<Button>().onClick.AddListener(UnlockCoolDown);
        crystalMutiButton.GetComponent<Button>().onClick.AddListener(UnlockMuti);
        crystalCheckDistanceButton.GetComponent<Button>().onClick.AddListener(UnlockCheckDistance);
        */
    }

    public void AddListenerCryStal()
    {
        crystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        crystalCooldownButton.GetComponent<Button>().onClick.AddListener(UnlockCoolDown);
        crystalMutiButton.GetComponent<Button>().onClick.AddListener(UnlockMuti);
        crystalCheckDistanceButton.GetComponent<Button>().onClick.AddListener(UnlockCheckDistance);
    }
    //�˺�����ʱû��ʹ�ã���ʱ��Ϊ�ֶ���


    //��������
    #region


    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCoolDown();
        UnlockMuti();
        UnlockCheckDistance();
    }

    public void UnlockCrystal()
    {
        if (crystalButton.unlocked && !canUseCrystal)
            canUseCrystal = true;
    }
    public void UnlockCoolDown()
    {
        if (crystalCooldownButton.unlocked && !(SkillManager.instance.crystal.cooldown == mutiStackCooldown))
            SkillManager.instance.crystal.cooldown = mutiStackCooldown;
    }
    public void UnlockMuti()
    {
        if(crystalMutiButton.unlocked&& !canMuti)
            canMuti = true;        
    }
    public void UnlockCheckDistance()
    {
        if(crystalCheckDistanceButton.unlocked && !(checkDistance == biggerCheckDistance))
            checkDistance = biggerCheckDistance;
    }

    #endregion


    protected override void Update()
    {
        base.Update();
        if(mutiTimer<=mutiStackCooldown&&amountOfStacks<maxCapacity) mutiTimer += Time.deltaTime;
        if(canMuti)MutiCapacity();
        
    }

    

    public override void UseSkill()
    {
        base.UseSkill();

            currentCrystal = Instantiate(crystal, player.transform.position, Quaternion.identity);
            CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
            currentCrystalScript.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,launchSpeed,homingSpeed,growSpeed,FindClosestEnemy(currentCrystal.transform,checkDistance), crystalMultiplier);

        if (canMuti) --amountOfStacks;
        /*
        if (currentCrystal == null)
        {
        //�Ķ�˵���������쵼���Ƴ�ȥ��
        }
        
        else
        {

            if (canMoveToEnemy)
                return;

            player.transform.position = currentCrystal.transform.position;
            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
        }
        */
    }


    public override bool CanUseSkill()
    {
        if (!canUseCrystal) return false;
        else
        {
            if (amountOfStacks > 0)
            {
                cooldownTimer = cooldown;
                return true;
            }
            else return base.CanUseSkill();
        }

    }

    private void MutiCapacity()
    {
        if(amountOfStacks<maxCapacity&&mutiTimer>=mutiStackCooldown)
        {
            ++amountOfStacks;
            mutiTimer = 0;
        }

    }


}
