using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;

    [Header("黑洞信息")]
    [SerializeField] private UISkillTreeSlot blackholeButton;
    public bool canUseBlackhole;
    [SerializeField] private UISkillTreeSlot blackholeJammingButton;
    public bool canJamming;
    [Range(0f, 1f)]
    [SerializeField] private float blackholeMultiplier;

    [Header("吸附属性")]
    [SerializeField] private UISkillTreeSlot blackholeSizeButton;
    [SerializeField]private float maxSize;
    [SerializeField] private float defaultMaxSize;
    [SerializeField] private float maxSizePlus;
    [SerializeField]private float growSpeed;
    [SerializeField]private float shrinkSpeed;
    [SerializeField] private UISkillTreeSlot attrackForceButton;
    [SerializeField]private float attractForce = 5f; // 吸附速度
    [SerializeField] private float defaultAttractForce;
    [SerializeField] private float attractForcePlus;
    [SerializeField]private float blackholeDuration = 5f; // 黑洞的持续时间
    [Header("移动属性")]
    [SerializeField] private float startMoveSpeed;
    [SerializeField] private float speedRecayRate;
    [SerializeField] private float endMoveSpeed;



    public override bool CanUseSkill()
    {
        if(!canUseBlackhole) return false;
        else
            return base.CanUseSkill();
    }


    public override void UseSkill()
    {

        if (!canUseBlackhole) return;
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);

        BlackholeSkillController newBlackholeScript = newBlackhole.GetComponent<BlackholeSkillController>();
        
        newBlackholeScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attractForce,canJamming, blackholeDuration,
                                          startMoveSpeed,speedRecayRate,endMoveSpeed,blackholeMultiplier);
    
    }

    protected override void CheckUnlock()
    {
        UnlockBlackhole();
        UnlockBlackholeAttrackForce();
        UnlockBlackholeSize();
        UnlockBlackholeJamming();
        
    }

    public void UnlockBlackhole()
    {
        if (blackholeButton.unlocked && !canUseBlackhole)
        { 
            canUseBlackhole = true;

            defaultAttractForce = attractForce;
            defaultMaxSize = maxSize;
        }
    }
    public void UnlockBlackholeSize()
    {
        if (blackholeSizeButton.unlocked && maxSize == defaultMaxSize)
            maxSize = maxSizePlus;
    }
    public void UnlockBlackholeAttrackForce()
    {
        if (attrackForceButton.unlocked && attractForce == defaultAttractForce)
            attractForce = attractForcePlus;
    }
    public void UnlockBlackholeJamming()
    {
        if(blackholeJammingButton.unlocked && !canJamming)
            canJamming = true;
    }





    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
