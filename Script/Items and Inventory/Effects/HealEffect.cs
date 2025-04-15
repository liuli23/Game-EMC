using UnityEngine;


[CreateAssetMenu(fileName = "����Ч��", menuName = "����/Ч��/����")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //base.ExecuteEffect(_enemyPosition);
        int healAmmount = Mathf.RoundToInt(playerStats.GetMaxHpValue() * healPercent);

        //playerStats.currentHp += healAmmount;
        if(playerStats.currentHp<playerStats.GetMaxHpValue())
        playerStats.IncreaseHp(healAmmount);

    }

}
