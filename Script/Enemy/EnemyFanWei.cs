using UnityEngine;

public class EnemyFanWei : MonoBehaviour
{
    [SerializeField] Enemy Enemy;
    [SerializeField] private bool flag = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("��ײ");
        // �����ײ�����Ƿ��ǵ���
        //Debug.Log("��ײ��������: " + collision.gameObject.name);
        //Debug.Log("��ײ�����Ƿ���� Enemy: " + (collision.GetComponent<Enemy>() != null));
        //Debug.Log("��ײ�����Ƿ���� PlayerState: " + (collision.GetComponent<PlayerStats>() != null));
        if (collision.GetComponent<Player>() != null)
        {
            // ��ȡ���˵�״̬���
            PlayerStats _target = collision.GetComponent<PlayerStats>();

            if (_target != null)
            {
                //Debug.Log("����");
                // �Ե�������˺�
                Enemy.stats.DoDamage(_target,1.5f);

                // ������Ļ��Ч��
                if (flag)
                {
                    PlayerManager.instance.player.fx.ScreenShake(
                        0.2f,
                        Random.Range(0.6f, 1.2f),
                        Random.Range(0.6f, 1.2f),
                        0
                    );
                    //PlayerManager.instance.player.SetVelocity(PlayerManager.instance.player.rb.linearVelocityX, PlayerManager.instance.player.xiaPiSpeed);

                    TimeScaleManager.instance.BulletTime();
                }
                flag = false;//��ֹ����

                // ��������������Ч�������� Effect �����Ѷ��壩
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }

        flag = true;
    }
}
