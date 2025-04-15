using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Player player;

    private Material originalMat;
    private Material instanceMat;  // 每个对象独有的材质实例
    private Coroutine flashRoutine;

    [Header("文字弹出")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("屏幕抖动")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shakePower;


    [Header("打击特效")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private GameObject hitFX_刀光;
    [SerializeField] private GameObject hitFX_飞溅;
    [SerializeField] private GameObject hitFX_光圈;

    [Header("残影特效")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float AfterImageCooldown;
    private float afterImageCooldownTimer;//创建间隔

    private bool Crital;


    [Header("对象池配置")]
    [SerializeField] private int defaultFXPoolSize = 5;
    [SerializeField] private int maxFXPoolSize = 20;

    // 为每种特效类型创建独立对象池
    private ObjectPool<GameObject> bladeFXPool;
    private ObjectPool<GameObject> splashFXPool;
    private ObjectPool<GameObject> auraFXPool;



    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMat = sr.material;
        screenShake = player.GetComponent<CinemachineImpulseSource>();

        // 创建材质实例（重要！确保每个对象使用独立材质）
        instanceMat = new Material(originalMat);
        sr.material = instanceMat;


        // 初始化刀光特效池
        bladeFXPool = CreateFXPool(hitFX_刀光);
        // 初始化飞溅特效池
        splashFXPool = CreateFXPool(hitFX_飞溅);
        // 初始化光圈特效池
        auraFXPool = CreateFXPool(hitFX_光圈);
    }

    // 通用对象池创建方法
    private ObjectPool<GameObject> CreateFXPool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = null; // 先声明池变量
        pool = new ObjectPool<GameObject>(
        //return new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.AddComponent<FXAutoRelease>(); // 添加自动释放组件
                return obj;
            },
            actionOnGet: obj =>
            {
                obj.SetActive(true);
                obj.transform.SetParent(null);

                // 新增：设置当前池引用到组件
                FXAutoRelease autoRelease = obj.GetComponent<FXAutoRelease>();
                if (autoRelease != null)
                {
                    autoRelease.SetPool(pool); // 将当前池传递给组件
                }
            },
            actionOnRelease: obj =>
            {
                obj.SetActive(false);
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                // 重置粒子系统（如果有）
                ParticleSystem ps = obj.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            },
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: defaultFXPoolSize,
            maxSize: maxFXPoolSize
        );
        return pool;
    }


    private void SetupFX(GameObject fx, Vector3 position, float rotationZ = 0)
    {
        fx.transform.position = position;
        fx.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        // 自动设置释放回池
        FXAutoRelease autoRelease = fx.GetComponent<FXAutoRelease>();
        if (autoRelease != null)
        {
            autoRelease.StartReleaseTimer(0.3f); // 0.3秒后自动释放
        }
    }

    // 新建自动释放组件
    public class FXAutoRelease : MonoBehaviour
    {
        private ObjectPool<GameObject> pool;
        private Coroutine releaseRoutine;

        public void SetPool(ObjectPool<GameObject> targetPool) => pool = targetPool;

        public void StartReleaseTimer(float delay)
        {
            if (releaseRoutine != null)
                StopCoroutine(releaseRoutine);

            releaseRoutine = StartCoroutine(ReleaseAfterDelay(delay));
        }

        private IEnumerator ReleaseAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (pool != null)
            {
                pool.Release(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            if (releaseRoutine != null)
                StopCoroutine(releaseRoutine);
        }
    }





    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(float _shakeMultiplier = 0, float _x = 0,float _y=0,float _z=0,bool facing =true)
    {
        if (_shakeMultiplier == 0) _shakeMultiplier = shakeMultiplier;
        Vector3 shakeDirection = new Vector3(_x, _y, _z);
        if (shakeDirection == Vector3.zero) shakeDirection = shakePower;

        if(facing)
            screenShake.DefaultVelocity = new Vector3(shakeDirection.x * player.facingDirection, shakeDirection.y) * _shakeMultiplier;
        else if(!facing)
            screenShake.DefaultVelocity = new Vector3(shakeDirection.x , shakeDirection.y) * _shakeMultiplier;
        screenShake.GenerateImpulse();


    }

    public void CreateText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX,randomY,0);


        GameObject newText = Instantiate(popUpTextPrefab,transform.position,Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;

    }



    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = AfterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position,transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);

        }
    }


    public void Flash()
    {
        // 如果已经在闪烁，先停止之前的协程
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashFX());
    }

    private IEnumerator FlashFX()
    {
        // 启用白色
        instanceMat.SetFloat("_UseWhite", 1f);

        yield return new WaitForSeconds(flashDuration);

        // 恢复原始颜色
        instanceMat.SetFloat("_UseWhite", 0f);

        flashRoutine = null;
    }

    // 可选：对象销毁时清理材质实例
    private void OnDestroy()
    {
        if (instanceMat != null)
            Destroy(instanceMat);
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }


    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void CreateHitFX(Transform _target,bool _Critical = false)
    {

        /*

        float randomZ = Random.Range(-90, 90);
        float randomZ_火花 = Random.Range(20, -40);

        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        GameObject newHitFX_刀光 = Instantiate(hitFX_刀光, _target.position, Quaternion.identity);//最后一个参数,_target为特效跟随，
        GameObject newHitFX_光圈 = Instantiate(hitFX_光圈, _target.position, Quaternion.identity);//把特效设置成目标的子物体
        newHitFX_刀光.transform.Rotate(new Vector3(0, 0, randomZ));
        */
        // 从池中获取刀光特效
        GameObject newHitFX_刀光 = bladeFXPool.Get();
        SetupFX(newHitFX_刀光, _target.position, Random.Range(-90, 90));
        // 从池中获取光圈特效
        GameObject newHitFX_光圈 = auraFXPool.Get();
        SetupFX(newHitFX_光圈, _target.position);

        /*
        if (Crital)
        {
            GameObject newHitFX_火花 = Instantiate(hitFX_飞溅, _target.position, Quaternion.identity);
            newHitFX_火花.transform.Rotate(new Vector3(0, 0, randomZ_火花));
            newHitFX_火花.transform.localScale = new Vector3(GetComponent<Entity>().facingDirection, 1, 1);
            Destroy(newHitFX_火花, .3f);
        }
        */


        if (_Critical)
        {
            // 从池中获取飞溅特效
            GameObject newHitFX_火花 = splashFXPool.Get();
            SetupFX(newHitFX_火花, _target.position, Random.Range(20, -40));
            newHitFX_火花.transform.localScale = new Vector3(
                GetComponent<Entity>().facingDirection, 1, 1);
        }


        //Destroy(newHitFX_光圈, .3f);
        //Destroy(newHitFX_刀光, .3f);

    }


}



