using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public enum BulletType
{
    Regular,//常规
    Bounce,//弹射
    Pierce,//穿透
    //Spin//自旋
}

public enum Danyao
{ 
    Regular,//0
    Jamming,//1
}




public class FireSkill : Skill
{
    public BulletType bulletType = BulletType.Regular;
    public Danyao danyao = Danyao.Regular;

    [Header("弹射信息")]
    [SerializeField] private UISkillTreeSlot bounceButton;
    [SerializeField] private UISkillTreeSlot bouncePlusButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private int defaultBounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("穿刺信息")]
    [SerializeField] private UISkillTreeSlot pierceButton;
    [SerializeField] private UISkillTreeSlot piercePlusButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private int defaultPierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("技能信息")]
    [SerializeField] private UISkillTreeSlot aimButton;
    public bool canAim;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2 fireForce;
    [SerializeField] private float bulletGravity;
    [SerializeField] private float bulletSpeed;
    [Range(0f, 1f)]
    [SerializeField] private float bulletMultiplier;

    [Header("弹药信息")]
    [SerializeField] private UISkillTreeSlot jammingButton;
    [SerializeField] private int danyaoNo;
    [SerializeField] private float freezeTimeDuration;
    private Vector2 finalDirection;


    [Header("瞄准点")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    public ObjectPool<GameObject> bulletPool;

    public static FireSkill Instance { get; private set; }

    protected override void Start()
    {


        base.Start();


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化对象池和其他逻辑

        GenerateDots();

        SetupGravity();

        SetupDanyaoNo();

        //AddListenerFire();有点不太好
        defaultBounceAmount = bounceAmount;
        defaultPierceAmount = pierceAmount;


        // 修改后的对象池初始化
        bulletPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                // 创建时：仅实例化预制体，不要设置父对象
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.GetComponent<FireSkillController>().SetPool(bulletPool); // 传递对象池引用
                return bullet;
            },
            actionOnGet: obj =>
            {
                // 取出时：重置位置、激活、设置父级
                obj.transform.position = player.transform.position; // 生成在玩家位置
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(true);
            },
            actionOnRelease: obj =>
            {
                // 释放时：取消父级、停用、重置组件状态
                obj.transform.SetParent(null);
                obj.SetActive(false);
                obj.GetComponent<FireSkillController>().ResetState(); // 关键：重置状态
            },
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true, // 防止重复释放
            defaultCapacity: 20,
            maxSize: 50
        );
    }

    public void AddListenerFire()
    {
        aimButton.GetComponent<Button>().onClick.AddListener(UnlockAim);
        bounceButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        bouncePlusButton.GetComponent<Button>().onClick.AddListener(UnlockBouncePlus);
        piercePlusButton.GetComponent<Button>().onClick.AddListener(UnlockPiercePlus);
        jammingButton.GetComponent<Button>().onClick.AddListener(UnlockJamming);
    }
    //暂时改为手动绑定
    #region


    protected override void CheckUnlock()
    {
        UnlockAim();
        UnlockBounce();
        UnlockPierce();
        UnlockBouncePlus();
        UnlockPiercePlus();
        UnlockJamming();
    }



    public void UnlockAim()
    {
        if (aimButton.unlocked && !canAim)
            canAim = true;//canAim判定写于GroundState中,用于判断进入的是哪一个瞄准状态
    }
    public void UnlockPierce()
    {
        if(pierceButton.unlocked && bulletType == BulletType.Regular)
            bulletType = BulletType.Pierce;
    }
    public void UnlockBounce()
    {
        if (bounceButton.unlocked && bulletType == BulletType.Regular)
            bulletType = BulletType.Bounce;
    }
    public void UnlockBouncePlus()
    {
        if (bouncePlusButton.unlocked && bounceAmount == defaultBounceAmount)
            bounceAmount++;
    }
    public void UnlockPiercePlus()
    {
        if (piercePlusButton.unlocked && pierceAmount == defaultPierceAmount)
            pierceAmount++;
    }
    public void UnlockJamming()
    {
        if (jammingButton.unlocked && danyao == Danyao.Regular)
            danyao = Danyao.Jamming;
    }






    #endregion



    private void SetupDanyaoNo()
    {
        if (danyao == Danyao.Regular) danyaoNo = 0;
        else if(danyao == Danyao.Jamming) danyaoNo = 1;

    }
    private void SetupGravity()
    {
        if (bulletType == BulletType.Bounce)
            bulletGravity = bounceGravity;
        else if (bulletType == BulletType.Pierce)
            bulletGravity = pierceGravity;
        //else if (bulletType == BulletType.Spin)
            //bulletGravity = spinGravity;
    }

    protected override void Update()
    {
        base.Update();//改动说明：忘记加了





        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDirection = new Vector2(AimDirection().normalized.x * fireForce.x, AimDirection().normalized.y * fireForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }

        }

        SetupDanyaoNo();//加到Update里才能播放时随时更改吧。

        //SetupGravity();//?

    }


    public override void UseSkill()
    {
        base.UseSkill();
        CreateBullet();
    }//改动说明，统一技能格式，调用CanUseSkill()进行冷却判断

    public void CreateBullet()
    {
        // 从池中获取子弹
        GameObject newBullet = bulletPool.Get();
        FireSkillController newBulletScript = newBullet.GetComponent<FireSkillController>();

        // 配置子弹参数
        if (bulletType == BulletType.Bounce)
        {
            newBulletScript.SetupBounce(true, bounceAmount, bounceSpeed);
        }
        else if (bulletType == BulletType.Pierce)
        {
            newBulletScript.SetupPierce(pierceAmount);
        }

        // 设置其他参数
        newBulletScript.SetupDanyao(danyaoNo, freezeTimeDuration);

        if (player.fireWay == 2)
        {
            newBulletScript.SetupBullet(finalDirection, bulletGravity, bulletMultiplier);
        }
        else if (player.fireWay == 1)
        {
            newBulletScript.SetupBullet(new Vector2(player.facingDirection * fireForce.x, 0), bulletGravity, bulletMultiplier);
        }

        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    //瞄准以及瞄准点
    #region
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }

    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * fireForce.x * t,
            (AimDirection().normalized.y * fireForce.y * t) + .5f * (Physics2D.gravity.y * bulletGravity) * t * t);
        return position;
    }

    #endregion
}
