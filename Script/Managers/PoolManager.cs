using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance {  get; private set; }

    private Dictionary<string,Stack<GameObject>> pools = new Dictionary<string,Stack<GameObject>>();

    private Dictionary<string,Transform> parents = new Dictionary<string,Transform>();

    void Awake()
    {
        if(instance == null)
                instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject GetObject(string name,GameObject prefab)
    {
        GameObject obj;
        if(!pools.ContainsKey(name))
        {
            CreatPool(name);
        }

        if (pools[name].Count>0)
        {
            obj = pools[name].Pop();
        }
        else
        {
            obj = Instantiate(prefab,parents[name]);//实例化之后会+clone,重置一下就好了
            obj.name = prefab.name;
        }
        obj.SetActive(true);
        return obj;
    }



    public void ReturnObject(string name,GameObject obj)
    {
        if (!pools.ContainsKey(name)) CreatPool(name);
        obj.SetActive(false);
        pools[name].Push(obj);
    }


    private void CreatPool(string name)
    {
        Transform parent = new GameObject(name + "_Pool").transform;
        parent.SetParent(transform);

        Stack<GameObject> stack = new Stack<GameObject>();

        pools.Add(name, stack);
        parents.Add(name , parent);
    }

    public void ClearPool()
    {
        pools.Clear();
        parents.Clear();
    }
}
