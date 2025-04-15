using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolText : MonoBehaviour
{
    //public GameObject prefab;
    public List<GameObject> list = new List<GameObject>();

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) PoolManager.instance.GetObject(list[0].name, list[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2)) PoolManager.instance.GetObject(list[1].name, list[1]);
    }


}
