using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int currency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() == null) return;
        //Debug.Log("ºÒ«Æ");
        PlayerManager.instance.currency += currency;
        Destroy(this.gameObject);

    }
}
