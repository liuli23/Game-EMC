using UnityEngine;

public class PlayerFanWei : MonoBehaviour
{
    [SerializeField]private bool flag = true; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("��ײ");
                         // �����ײ�����Ƿ��ǵ���
        //Debug.Log("��ײ��������: " + collision.gameObject.name);
        //Debug.Log("��ײ�����Ƿ���� Enemy: " + (collision.GetComponent<Enemy>() != null));
        //Debug.Log("��ײ�����Ƿ���� EnemyStats: " + (collision.GetComponent<EnemyStats>() != null));
        if (collision.GetComponent<Enemy>() != null)
        {
            // ��ȡ���˵�״̬���
            EnemyStats _target = collision.GetComponent<EnemyStats>();

            if (_target != null)
            {
                //Debug.Log("����");
                // �Ե�������˺�
                PlayerManager.instance.player.stats.DoDamage(_target);

                // ������Ļ��Ч��
                if (flag)
                {
                    PlayerManager.instance.player.fx.ScreenShake(
                        0.2f,
                        Random.Range(0.6f, 1.2f),
                        Random.Range(0.6f, 1.2f),
                        0
                    );
                    PlayerManager.instance.player.SetVelocity(PlayerManager.instance.player.rb.linearVelocityX, PlayerManager.instance.player.xiaPiSpeed);

                    TimeScaleManager.instance.BulletTime();
                }
                flag=false;//��ֹ����

                // ��������������Ч�������� Effect �����Ѷ��壩
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }

        flag = true;
    }


   // private void OnTriggerExit2D(Collider2D collision)
   // {
   //     // �������뿪������ʱ�����ñ��
   //    flag = false;
   // }
}