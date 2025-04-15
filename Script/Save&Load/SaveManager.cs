using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;


    [ContextMenu("删除存档文件")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if(instance!=null)
            Destroy(instance.gameObject);
        else
            instance = this;


        //DontDestroyOnLoad(gameObject); // 确保 SaveManager 不被销毁
    }

    //来自Grok3,没修好，算了
    #region
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    #endregion

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }


    public void NewGame()
    {
        gameData = new GameData();

    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        //Debug.Log("加载数据");

        if(this.gameData == null)
        {
            //Debug.Log("没有存档");
            NewGame();
        }

        foreach(var manager in saveManagers)
        {
            manager.LoadData(gameData);
        }

        //Debug.Log("读取货币" + gameData.currency);
    }

    public void SaveGame()
    {
        foreach (var manager in saveManagers)
        {
            manager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        //Debug.Log("保存货币" + gameData.currency);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None).OfType<ISaveManager>();
        //改动说明使用包含为激活游戏物体参数的重载

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }



}
