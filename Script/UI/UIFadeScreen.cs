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
        // 如果已经存在实例，则销毁当前对象
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 设置当前对象为实例，并防止销毁
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
        // 确保只有一个实例存在
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // 获取 Image 组件
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
