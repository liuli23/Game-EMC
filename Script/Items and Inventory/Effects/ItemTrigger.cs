using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemObject itemObject =>GetComponentInParent<ItemObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if(collision.GetComponent<CharacterStats>().isDead)
            {
                return;
            }

            Debug.Log("ºÒ∆ŒÔ∆∑");
            itemObject.PickUpItem();
        }
    }

}
