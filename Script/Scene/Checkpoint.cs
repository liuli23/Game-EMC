using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStats;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("生成检查点Id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() == null) return;
        ActiveCheckpoint();

    }


    public void ActiveCheckpoint()
    {
        if(activationStats == false)
            AudioManager.instance.PlaySFX(5, transform);

        activationStats = true;
        anim.SetBool("Active", true);

    }

}
