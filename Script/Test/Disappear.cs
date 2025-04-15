using System.Collections;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public string objName {  get; private set; }    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(disappear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator disappear()
    {
        yield return new WaitForSeconds(1f);

        PoolManager.instance.ReturnObject(this.name, this.gameObject);
    }


    public string GetName()
    {
        return objName;
    }
}
