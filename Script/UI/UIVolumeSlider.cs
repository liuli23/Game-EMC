using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mutiplier;

    public void SliderValue(float _value)
    {
        
        //audioMixer.SetFloat(parameter, Mathf.Log10(_value)*mutiplier);
        audioMixer.SetFloat(parameter, _value);
        //Debug.Log("设置音量");
    }

    public void LoadSlider(float _value)
    {
        //Debug.Log("加载音量");
        slider.value = _value; 
        SliderValue(slider.value);
    }
}
