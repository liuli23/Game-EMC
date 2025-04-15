using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIOption : MonoBehaviour
{
    [SerializeField]private UIFadeScreen fadeScreen;

    // 引用 SaveManager
    public SaveManager saveManager;

    // 跳转到的主菜单场景名称
    public string sceneName = "MainMenu";

    // 调用 SaveManager 的保存功能
    public void OnSaveAndExitButtonClicked()
    {
        // 调用 SaveManager 的保存功能
        saveManager.SaveGame();

        StartCoroutine(LoadSceneWithFadeEffect(3f));
    }


    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        // 触发渐出动画
        fadeScreen.FadeOut();

        // 启动异步加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // 暂时暂停场景激活

        // 记录开始时间
        float startTime = Time.unscaledTime;

        // 等待延迟时间，或者场景加载完成（取两者中的较大值）
        while (Time.unscaledTime - startTime < _delay || asyncOperation.progress < .9f)
        {
            yield return null;
        }

        // 激活场景
        asyncOperation.allowSceneActivation = true;


        Debug.Log("场景加载完成");
    }



}

