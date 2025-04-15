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

        // ע�᳡����������¼�
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
        Debug.Log("�˳�");
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
        // ������������
        UIFadeScreen.instance.FadeOut();

        // �����첽���س���
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // ��ʱ��ͣ��������

        // ��¼��ʼʱ��
        float startTime = Time.time;

        // �ȴ��ӳ�ʱ�䣬���߳���������ɣ�ȡ�����еĽϴ�ֵ��
        while (Time.time - startTime < _delay || asyncOperation.progress <.9f)
        {
            yield return null;
        }

        // �����
        asyncOperation.allowSceneActivation = true;


        Debug.Log("�����������");
    }



    */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ������صĳ��������˵����������� FadeIn()
        if (scene.name == "MainMenu")
        {
           // Debug.Log("���˵�����������ɣ����� FadeIn()");
            //UIFadeScreen.instance.FadeIn();
           // UIFadeScreen.instance.gameObject.SetActive(false);
        }
        //dark.SetActive(false);
    }
    

    private void QuXiao() =>UIFadeScreen.instance.gameObject.SetActive(false);
}
