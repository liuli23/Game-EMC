using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    //[SerializeField] private UIFadeScreenMain fadeScreen;
    [SerializeField] private GameObject dark;


    private void Start()
    {
        if(!SaveManager.instance.HasSaveData()) continueButton.SetActive(false);
        //UIFadeScreen.instance.FadeIn();

        // 注册场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public void ContinueGame()
    {
        //UIFadeScreen.instance.gameObject.SetActive(true);
        StartCoroutine(LoadSceneWithFadeEffect(3f));
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        //UIFadeScreen.instance.gameObject.SetActive(true);
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(3f));
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("退出");
        Application.Quit();

    }


    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        //UIFadeScreen.instance.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
    /*
    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        UIFadeScreen.instance.gameObject.SetActive(true);
        // 触发渐出动画
        UIFadeScreen.instance.FadeOut();

        // 启动异步加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // 暂时暂停场景激活

        // 记录开始时间
        float startTime = Time.time;

        // 等待延迟时间，或者场景加载完成（取两者中的较大值）
        while (Time.time - startTime < _delay || asyncOperation.progress <.9f)
        {
            yield return null;
        }

        // 激活场景
        asyncOperation.allowSceneActivation = true;


        Debug.Log("场景加载完成");
    }



    */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 如果加载的场景是主菜单场景，调用 FadeIn()
        if (scene.name == "MainMenu")
        {
           // Debug.Log("主菜单场景加载完成，调用 FadeIn()");
            //UIFadeScreen.instance.FadeIn();
           // UIFadeScreen.instance.gameObject.SetActive(false);
        }
        //dark.SetActive(false);
    }
    

    private void QuXiao() =>UIFadeScreen.instance.gameObject.SetActive(false);
}
