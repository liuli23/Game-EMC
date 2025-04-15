using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closetCheckPointId;

    [Header("爆金币")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);

    }


    

    public void RestartScence()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
    

    //来自Grok3,没修好，算了
    /*
    public void RestartScence()
    {
        SaveManager.instance.SaveGame();
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name));
    }

    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForEndOfFrame(); // 确保保存操作完成
        SceneManager.LoadScene(sceneName);
    }

    */


    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
        /*
        LoadLostCurrency(_data);

        LoadCheckpoint(_data);

        //PlacePlayerAtClosetCheckpoint(_data);
        //closetCheckPointId = _data.closestCheckpointId;
        //Invoke("PlacePlayerAtClosetCheckpoint(_data)",1f);
        Invoke("PlacePlayerAtClosetCheckpoint", .1f);//改动说明：延迟最近坐标id使用，好像是因为SaveManager和GameManager执行顺序问题，导致死亡后无法加载正确的位置坐标
        */
    }




    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyX = PlayerManager.instance.player.transform.position.x;
        _data.lostCurrencyY = PlayerManager.instance.player.transform.position.y;
        _data.lostCurrencyAmount = lostCurrencyAmount;

        if(FindClosestCheckpoint()!=null)
            _data.closestCheckpointId =FindClosestCheckpoint().id;

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStats);
        }
    }

    
    /*
    private void PlacePlayerAtClosetCheckpoint(GameData _data)
    {
        foreach (var checkpoint in checkpoints)
        {
            if (_data.closestCheckpointId == checkpoint.id)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }
    */
    
    
    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> Pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == Pair.Key && Pair.Value == true)
                {
                    checkpoint.ActiveCheckpoint();
                }
            }
        }
        //Debug.Log("加载锚点");
    }

    private void PlacePlayerAtClosetCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null) return;

        closetCheckPointId = _data.closestCheckpointId;

        foreach (var checkpoint in checkpoints)
        {
            if (closetCheckPointId == checkpoint.id)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab,new Vector3(lostCurrencyX,lostCurrencyY),Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);

        PlacePlayerAtClosetCheckpoint(_data);
        LoadLostCurrency(_data);

    }
    

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkpoint.activationStats == true)
            {
                closestCheckpoint = checkpoint;
                closestDistance = distanceToCheckpoint;
            }
        }
        return closestCheckpoint;
    }


    public void PauseGame(bool _pause)
    {
        //Debug.Log("PauseGame called with parameter: " + _pause);
        if (_pause)
        {
            Time.timeScale = 0;
            //Debug.Log("Game paused. Time.timeScale set to 0.");
        }
        else
        {
            Time.timeScale = 1;
            //Debug.Log("Game resumed. Time.timeScale set to 1.");
        }
    }

}
