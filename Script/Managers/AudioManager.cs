using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private float sfxMaxDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;//bgm曲目 

    private bool canPlaySFX;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        if(!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }

        Invoke("AllowSFX", 1f);
    }


    //音效与背景音的区别，音效同时可以播放多个，背景音一般只播放一个
    //播放某音效

    public void PlaySFX(int _sfxIndex , Transform _source , bool someRandom=false,float minRange =1,float maxRange =1)
    {
        if (!canPlaySFX) return;
        //if (sfx[_sfxIndex].isPlaying)
        //    return;
        //处理音量与距离,一般角色不用填这个参数
        if (_source != null)
        {
            float originalVolume = sfx[_sfxIndex].volume;
            float sfxDistance = Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position);
            if (sfxDistance > sfxMaxDistance)
                return;
            else if (sfxDistance < sfxMinDistance)
                sfx[_sfxIndex].volume = originalVolume;
            else sfx[_sfxIndex].volume = (sfxDistance - sfxMinDistance) / (sfxMaxDistance-sfxMinDistance) *originalVolume;

            sfx[_sfxIndex].volume = originalVolume;
        }

        if (_sfxIndex < sfx.Length)
        {
            if (someRandom)
            {
                //float minRange = sfx[_sfxIndex].pitch * .7f;
                //float maxRange = sfx[_sfxIndex].pitch * 1.3f;

                sfx[_sfxIndex].pitch = Random.Range(minRange,maxRange);
            }
            sfx[_sfxIndex].Play();
        }
    }
    //停止某音效
    public void StopSFX(int _index) => sfx[_index].Stop();
    //背景音随机播放功能

    public void StopSFXWithTime(int  _index) => StartCoroutine(DecreaseValue(sfx[_index]));

    // 协程
    private IEnumerator DecreaseValue(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume>.1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.5f);

            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }





    public void PlayRandowBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    //播放某背景音
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }
    //停止所有背景音
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }


    private void AllowSFX() => canPlaySFX = true;

}
