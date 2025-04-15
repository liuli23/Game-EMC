using UnityEngine;

public class XieTongController : MonoBehaviour
{
    private PlayerStats playerStats;
    [Range(0f, 1f)]
    [SerializeField] private float xieTongMultiplier = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoDamage(enemyTarget, xieTongMultiplier);

        }

    }


}
