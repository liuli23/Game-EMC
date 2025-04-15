using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager instance;

    [SerializeField] private float bulletTimeScale = 0.2f;   // 子弹时间的时间缩放值
    [SerializeField] private float normalTimeScale = 1.0f;    // 正常时间缩放值
    [SerializeField] private float karouDuration = 0.12f;
    [SerializeField] private float slowDownDuration = 0.5f;  // 渐变减速的持续时间

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color targetColor;


    private Coroutine bulletTimeCoroutine; // 用于跟踪子弹时间协程
    private bool isBulletTimeActive = false; // 标记子弹时间是否正在激活
    private float timer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject); // 确保只有一个实例
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 确保在场景切换时不会被销毁
        }
    }

    // 启动卡肉效果的子弹时间
    public void BulletTime(float _timer = 0)
    {
        if (_timer == 0)
            timer = karouDuration;
        else timer = _timer;

        if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // 停止之前的协程
        bulletTimeCoroutine = StartCoroutine(StartBulletTime());


    }

    // 启动带有渐变减速的子弹时间
    public void BulletTimeWithSlowDown()
    {
        if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // 停止之前的协程
        bulletTimeCoroutine = StartCoroutine(StartBulletTimeWithSlowDown());
    }

    // 恢复正常时间
    public void RestoreNormalTime()
    {
        //if (bulletTimeCoroutine != null) StopCoroutine(bulletTimeCoroutine); // 停止当前的子弹时间协程
        //Time.timeScale = normalTimeScale; // 恢复正常时间缩放
        isBulletTimeActive = false; // 重置子弹时间状态
    }

    // 卡肉效果的子弹时间协程
    IEnumerator StartBulletTime()
    {
        Time.timeScale = bulletTimeScale; // 设置为子弹时间缩放值
        yield return new WaitForSecondsRealtime(timer); // 等待 2 秒
        Time.timeScale = normalTimeScale; // 恢复正常时间缩放
        bulletTimeCoroutine = null; // 重置协程引用
    }

    // 带有渐变减速的子弹时间协程
    IEnumerator StartBulletTimeWithSlowDown()
    {
        isBulletTimeActive = true; // 标记子弹时间正在激活

        // 渐变减速到目标时间缩放值
        yield return StartCoroutine(SlowDownTime());

        // 屏幕变暗
        yield return StartCoroutine(DarkenScreen());



        /*
        // 渐变减速到目标时间缩放值
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(normalTimeScale, bulletTimeScale, t); // 插值减缓时间缩放
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = bulletTimeScale; // 确保时间缩放达到目标值
        isBulletTimeActive = true; // 标记子弹时间正在激活
        */
        // 保持子弹时间状态，直到手动调用恢复方法
        while (isBulletTimeActive)
        {
            //Debug.Log("循环");
            //Debug.Log(isBulletTimeActive);
            yield return null;
            
        }

        //Debug.Log("停止");
        // 渐变恢复到正常时间缩放
        yield return StartCoroutine(SpeedUpTime());

        // 屏幕恢复
        yield return StartCoroutine(RestoreScreen());

        Time.timeScale = normalTimeScale; // 确保时间缩放恢复到正常值
        bulletTimeCoroutine = null; // 重置协程引用

        /*
        // 渐变恢复到正常时间缩放
        elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, normalTimeScale, t); // 插值恢复时间缩放
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        */
    }


    // 渐变减速的时间缩放
    IEnumerator SlowDownTime()
    {
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(normalTimeScale, bulletTimeScale, t); // 插值减缓时间缩放
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = bulletTimeScale; // 确保时间缩放达到目标值
    }

    // 渐变加速的时间缩放
    IEnumerator SpeedUpTime()
    {
        float elapsedTime = 0;
        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, normalTimeScale, t); // 插值恢复时间缩放
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = normalTimeScale; // 确保时间缩放恢复到正常值
    }

    // 屏幕变暗
    IEnumerator DarkenScreen()
    {
        float elapsedTime = 0;
        Color originalColor = globalLight.color; // 保存原始颜色

        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            globalLight.color = Color.Lerp(originalColor, targetColor, t); // 颜色逐渐变暗
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    // 屏幕恢复
    IEnumerator RestoreScreen()
    {
        float elapsedTime = 0;
        Color targetColor = new Color(1f, 1f, 1f); // 恢复的目标颜色
        Color originalColor = globalLight.color; // 保存当前颜色

        while (elapsedTime < slowDownDuration)
        {
            float t = elapsedTime / slowDownDuration;
            globalLight.color = Color.Lerp(originalColor, targetColor, t); // 颜色逐渐恢复
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}