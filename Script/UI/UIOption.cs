using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIOption : MonoBehaviour
{
    [SerializeField]private UIFadeScreen fadeScreen;

    // ���� SaveManager
    public SaveManager saveManager;

    // ��ת�������˵���������
    public string sceneName = "MainMenu";

    // ���� SaveManager �ı��湦��
    public void OnSaveAndExitButtonClicked()
    {
        // ���� SaveManager �ı��湦��
        saveManager.SaveGame();

        StartCoroutine(LoadSceneWithFadeEffect(3f));
    }


    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        // ������������
        fadeScreen.FadeOut();

        // �����첽���س���
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // ��ʱ��ͣ��������

        // ��¼��ʼʱ��
        float startTime = Time.unscaledTime;

        // �ȴ��ӳ�ʱ�䣬���߳���������ɣ�ȡ�����еĽϴ�ֵ��
        while (Time.unscaledTime - startTime < _delay || asyncOperation.progress < .9f)
        {
            yield return null;
        }

        // �����
        asyncOperation.allowSceneActivation = true;


        Debug.Log("�����������");
    }



}

