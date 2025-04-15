using UnityEngine;

[CreateAssetMenu(fileName = "Эͬն��", menuName = "����/Ч��/Эͬն��")]
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
