using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager instance;

    [SerializeField] private float bulletTimeScale = 0.2f;   // �ӵ�ʱ���ʱ������ֵ
    [SerializeField] private float normalTimeScale = 1.0f;    // ����ʱ������ֵ
    [SerializeField] private float karouDuration = 0.12f;
    [SerializeField] private float slowDownDuration = 0.5f;  // ������ٵĳ���ʱ��

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color targetColor;


    private Coroutine bulletTimeCoroutine; // ���ڸ����ӵ�ʱ��Э��
    private bool isBulletTimeActive = false; // ����ӵ�ʱ���Ƿ����ڼ���
    private float timer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject); // ȷ��ֻ��һ��ʵ��
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ȷ���ڳ����л�ʱ���ᱻ����
        }
    }

    // ��������Ч�����ӵ�ʱ��
    public void BulletTime(float _timer = 0)
    {
        if (_timer == 0)
            timer = karouDuration;
        else timer = _timer;

        if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // ֹ֮ͣǰ��Э��
        bulletTimeCoroutine = StartCoroutine(StartBulletTime());


    }

    // �������н�����ٵ��ӵ�ʱ��
    public void BulletTimeWithSlowDown()
    {
        if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // ֹ֮ͣǰ��Э��
        bulletTimeCoroutine = StartCoroutine(StartBulletTimeWithSlowDown());
    }

    // �ָ�����ʱ��
    public void RestoreNormalTime()
    {
        //if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // ֹͣ��ǰ���ӵ�ʱ��Э��
        //Time.timeScale = normalTimeScale; // �ָ�����ʱ������
        isBulletTimeActive = false; // �����ӵ�ʱ��״̬
    }

    // ����Ч�����ӵ�ʱ��Э��
    IEnumerator StartBulletTime()
    {
        Time.timeScale = bulletTimeScale; // ����Ϊ�ӵ�ʱ������ֵ
        yield return new WaitForSecondsRealtime(timer); // �ȴ� 2 ��
        Time.timeScale = normalTimeScale; // �ָ�����ʱ������
        bulletTimeCoroutine = null; // ����Э������
    }

    // ���н�����ٵ��ӵ�ʱ��Э��
    IEnumerator StartBulletTimeWithSlowDown()
    {
        isBulletTimeActive = true; // ����ӵ�ʱ�����ڼ���

        // ������ٵ�Ŀ��ʱ������ֵ
        yield return StartCoroutine(SlowDownTime());

        // ��Ļ�䰵
        yield return StartCoroutine(DarkenScreen());



        /*
        // ������ٵ�Ŀ��ʱ������ֵ
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(normalTimeScale, bulletTimeScale, t); // ��ֵ����ʱ������
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = bulletTimeScale; // ȷ��ʱ�����ŴﵽĿ��ֵ
        isBulletTimeActive = true; // ����ӵ�ʱ�����ڼ���
        */
        // �����ӵ�ʱ��״̬��ֱ���ֶ����ûָ�����
        while (isBulletTimeActive)
        {
            //Debug.Log("ѭ��");
            //Debug.Log(isBulletTimeActive);
            yield return null;
            
        }

        //Debug.Log("ֹͣ");
        // ����ָ�������ʱ������
        yield return StartCoroutine(SpeedUpTime());

        // ��Ļ�ָ�
        yield return StartCoroutine(RestoreScreen());

        Time.timeScale = normalTimeScale; // ȷ��ʱ�����Żָ�������ֵ
        bulletTimeCoroutine = null; // ����Э������

        /*
        // ����ָ�������ʱ������
        elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, normalTimeScale, t); // ��ֵ�ָ�ʱ������
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        */
    }


    // ������ٵ�ʱ������
    IEnumerator SlowDownTime()
    {
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(normalTimeScale, bulletTimeScale, t); // ��ֵ����ʱ������
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = bulletTimeScale; // ȷ��ʱ�����ŴﵽĿ��ֵ
    }

    // ������ٵ�ʱ������
    IEnumerator SpeedUpTime()
    {
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, normalTimeScale, t); // ��ֵ�ָ�ʱ������
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = normalTimeScale; // ȷ��ʱ�����Żָ�������ֵ
    }

    // ��Ļ�䰵
    IEnumerator DarkenScreen()
    {
        float elapsedTime = 0;
        Color originalColor = globalLight.color; // ����ԭʼ��ɫ

        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            globalLight.color = Color.Lerp(originalColor, targetColor, t); // ��ɫ�𽥱䰵
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    // ��Ļ�ָ�
    IEnumerator RestoreScreen()
    {
        float elapsedTime = 0;
        Color targetColor = new Color(1f, 1f, 1f); // �ָ���Ŀ����ɫ
        Color originalColor = globalLight.color; // ���浱ǰ��ɫ

        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            globalLight.color = Color.Lerp(originalColor, targetColor, t); // ��ɫ�𽥻ָ�
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}