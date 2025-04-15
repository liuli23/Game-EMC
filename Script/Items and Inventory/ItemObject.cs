using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Vector2 velocity;

    /*
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    */

    /*
    private void OnValidate()
    {
        SetUpVisual();
    }
    */

    private void SetUpVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object -" + itemData.name;
    }

    /*private void Start()
    {
        sr.sprite = itemData.icon;
    }
    */

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) rb.linearVelocity = velocity;


    }*///²âÊÔ×°ÖÃ


    public void SetupItem(ItemData _itemDate,Vector2 _velocity)
    {
        itemData = _itemDate;
        rb.linearVelocity = _velocity;

        SetUpVisual();
    }



    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 4);
            PlayerManager.instance.player.fx.CreateText("±³°üÒÑÂú");
            return;
        }
        AudioManager.instance.PlaySFX(22,transform,true,0.8f,1.2f);

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
