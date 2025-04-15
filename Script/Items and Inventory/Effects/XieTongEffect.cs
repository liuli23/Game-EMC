using UnityEngine;

[CreateAssetMenu(fileName = "协同斩击", menuName = "数据/效果/协同斩击")]
public class XieTongEffect : ItemEffect
{
    [SerializeField] private GameObject zhanJiPrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //base.ExecuteEffect();
        GameObject newZhanJi = Instantiate(zhanJiPrefab,_enemyPosition.position,Quaternion.identity);

        Destroy(newZhanJi,1);

    }



}
