using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectUI : MonoBehaviour
{
    [SerializeField] private UIFadeScreen fadeScreen;
    [SerializeField] private TextMeshProUGUI deadText;
    [SerializeField] private GameObject restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeScreen.gameObject.SetActive(true);
        deadText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndScreen()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    //public void RestartGameButton() => GameManager.instance.RestartScence();
    //这个脚本负责视觉效果的UI部分，功能部分放UI里了

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        deadText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
    }



}
