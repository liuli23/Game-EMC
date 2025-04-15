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
    private Material instanceMat;  // ÿ��������еĲ���ʵ��
    private Coroutine flashRoutine;

    [Header("���ֵ���")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("��Ļ����")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shakePower;


    [Header("�����Ч")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private GameObject hitFX_����;
    [SerializeField] private GameObject hitFX_�ɽ�;
    [SerializeField] private GameObject hitFX_��Ȧ;

    [Header("��Ӱ��Ч")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float AfterImageCooldown;
    private float afterImageCooldownTimer;//�������

    private bool Crital;


    [Header("���������")]
    [SerializeField] private int defaultFXPoolSize = 5;
    [SerializeField] private int maxFXPoolSize = 20;

    // Ϊÿ����Ч���ʹ������������
    private ObjectPool<GameObject> bladeFXPool;
    private ObjectPool<GameObject> splashFXPool;
    private ObjectPool<GameObject> auraFXPool;



    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMat = sr.material;
        screenShake = player.GetComponent<CinemachineImpulseSource>();

        // ��������ʵ������Ҫ��ȷ��ÿ������ʹ�ö������ʣ�
        instanceMat = new Material(originalMat);
        sr.material = instanceMat;


        // ��ʼ��������Ч��
        bladeFXPool = CreateFXPool(hitFX_����);
        // ��ʼ���ɽ���Ч��
        splashFXPool = CreateFXPool(hitFX_�ɽ�);
        // ��ʼ����Ȧ��Ч��
        auraFXPool = CreateFXPool(hitFX_��Ȧ);
    }

    // ͨ�ö���ش�������
    private ObjectPool<GameObject> CreateFXPool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = null; // �������ر���
        pool = new ObjectPool<GameObject>(
        //return new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.AddComponent<FXAutoRelease>(); // ����Զ��ͷ����
                return obj;
            },
            actionOnGet: obj =>
            {
                obj.SetActive(true);
                obj.transform.SetParent(null);

                // ���������õ�ǰ�����õ����
                FXAutoRelease autoRelease = obj.GetComponent<FXAutoRelease>();
                if (autoRelease != null)
                {
                    autoRelease.SetPool(pool); // ����ǰ�ش��ݸ����
                }
            },
            actionOnRelease: obj =>
            {
                obj.SetActive(false);
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                // ��������ϵͳ������У�
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

        // �Զ������ͷŻس�
        FXAutoRelease autoRelease = fx.GetComponent<FXAutoRelease>();
        if (autoRelease != null)
        {
            autoRelease.StartReleaseTimer(0.3f); // 0.3����Զ��ͷ�
        }
    }

    // �½��Զ��ͷ����
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
        // ����Ѿ�����˸����ֹ֮ͣǰ��Э��
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashFX());
    }

    private IEnumerator FlashFX()
    {
        // ���ð�ɫ
        instanceMat.SetFloat("_UseWhite", 1f);

        yield return new WaitForSeconds(flashDuration);

        // �ָ�ԭʼ��ɫ
        instanceMat.SetFloat("_UseWhite", 0f);

        flashRoutine = null;
    }

    // ��ѡ����������ʱ�������ʵ��
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
        float randomZ_�� = Random.Range(20, -40);

        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        GameObject newHitFX_���� = Instantiate(hitFX_����, _target.position, Quaternion.identity);//���һ������,_targetΪ��Ч���棬
        GameObject newHitFX_��Ȧ = Instantiate(hitFX_��Ȧ, _target.position, Quaternion.identity);//����Ч���ó�Ŀ���������
        newHitFX_����.transform.Rotate(new Vector3(0, 0, randomZ));
        */
        // �ӳ��л�ȡ������Ч
        GameObject newHitFX_���� = bladeFXPool.Get();
        SetupFX(newHitFX_����, _target.position, Random.Range(-90, 90));
        // �ӳ��л�ȡ��Ȧ��Ч
        GameObject newHitFX_��Ȧ = auraFXPool.Get();
        SetupFX(newHitFX_��Ȧ, _target.position);

        /*
        if (Crital)
        {
            GameObject newHitFX_�� = Instantiate(hitFX_�ɽ�, _target.position, Quaternion.identity);
            newHitFX_��.transform.Rotate(new Vector3(0, 0, randomZ_��));
            newHitFX_��.transform.localScale = new Vector3(GetComponent<Entity>().facingDirection, 1, 1);
            Destroy(newHitFX_��, .3f);
        }
        */


        if (_Critical)
        {
            // �ӳ��л�ȡ�ɽ���Ч
            GameObject newHitFX_�� = splashFXPool.Get();
            SetupFX(newHitFX_��, _target.position, Random.Range(20, -40));
            newHitFX_��.transform.localScale = new Vector3(
                GetComponent<Entity>().facingDirection, 1, 1);
        }


        //Destroy(newHitFX_��Ȧ, .3f);
        //Destroy(newHitFX_����, .3f);

    }


}



