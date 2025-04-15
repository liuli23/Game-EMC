using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
public class UIFadeScreen : MonoBehaviour
{
    public static UIFadeScreen instance;

    private Animator anim;

    void Awake()
    {
        // ����Ѿ�����ʵ���������ٵ�ǰ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ���õ�ǰ����Ϊʵ��������ֹ����
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing on UIFadeScreen!");
        }
    }


    public void FadeOut() => anim.SetTrigger("FadeOut");
    public void FadeIn() => anim.SetTrigger("FadeIn");
}
*/



public class UIFadeScreen : MonoBehaviour
{
    public static UIFadeScreen instance;

    private Image image;
    private Animator anim;

    void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // ��ȡ Image ���
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component is missing on UIFadeScreen!");
        }
        anim = GetComponent<Animator>();
    }




    public void FadeOut()
    {
        if (anim != null)
        {
            anim.SetTrigger("FadeOut");
        }
    }

    public void FadeIn()
    {

        if (anim != null)
        {
            anim.SetTrigger("FadeIn");
        }

    }
}
