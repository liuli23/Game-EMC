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


    [ContextMenu("ɾ���浵�ļ�")]
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


        //DontDestroyOnLoad(gameObject); // ȷ�� SaveManager ��������
    }

    //����Grok3,û�޺ã�����
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
        //Debug.Log("��������");

        if(this.gameData == null)
        {
            //Debug.Log("û�д浵");
            NewGame();
        }

        foreach(var manager in saveManagers)
        {
            manager.LoadData(gameData);
        }

        //Debug.Log("��ȡ����" + gameData.currency);
    }

    public void SaveGame()
    {
        foreach (var manager in saveManagers)
        {
            manager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        //Debug.Log("�������" + gameData.currency);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None).OfType<ISaveManager>();
        //�Ķ�˵��ʹ�ð���Ϊ������Ϸ�������������

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
