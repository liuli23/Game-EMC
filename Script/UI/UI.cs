using UnityEngine;

public class UI : MonoBehaviour , ISaveManager
{
    [SerializeField] private GameObject charcaterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject gameUI;

    public UIItemToolTip itemToolTip;
    public UIStatToolTip statToolTip;
    public UICraftWindow craftWindow;
    public UISkillToolTip skillToolTip;


    [SerializeField] private UIVolumeSlider[] volumeSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //itemToolTip = GetComponentInChildren<UIItemToolTip>();
        SwitchTo(gameUI);
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        gameUI.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(charcaterUI);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(optionUI);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            SwitchWithKeyTo(skillTreeUI);
        }

    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            //bool isFadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>()!=null;
            //if(!isFadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        //Debug.Log(GameManager.instance != null);
        if(GameManager.instance != null)
        {
            //Debug.Log(_menu.ToString());
            if (_menu == gameUI)
            {
                GameManager.instance.PauseGame(false);
                //Debug.Log("解除暂停");
            }
            else GameManager.instance.PauseGame(true);
        }


    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        AudioManager.instance.PlaySFX(0, null);
        if(_menu !=null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            SwitchTo(gameUI);
            //gameUI.SetActive(true);
            return;

        }

        SwitchTo(_menu);

    }

    //可用可不用，不用就在SwitchWithKeyTo的return上一行加gameUI.SetActive(true);
    /*
    private void CheckForGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        gameUI.SetActive(true);
    }
    */

    public void RestartGameButton() =>GameManager.instance.RestartScence();

    public void LoadData(GameData _data)
    {
        foreach (var pair in _data.volumeSettings)
        {
            foreach (var item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
        
    }

    /*
    private void DelayLoad(GameData _data)
    {
        foreach (var pair in _data.volumeSettings)
        {
            foreach (var item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }
    */
    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UIVolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
