using UnityEngine;
using UnityEngine.UI;

public class PlayerMarkerController : MonoBehaviour
{
    public Transform realPlayer;  // ��ɫ����
    public RectTransform miniMapRect; // С��ͼ UI ���
    public Image playerMarker; // ��ɫ��ǵ� Image ���
    public int viewSize = 20; // С��ͼ��ʾ�����С

    void Start()
    {
        // ȷ����ɫ��ǿɼ�
        if (playerMarker != null)
        {
            playerMarker.enabled = true;
        }
    }

    void Update()
    {
        // �ý�ɫ���ʼ�ձ�����С��ͼ����
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // ȷ����ɫ�����С��ͼ����ȷ��Ⱦ
        if (playerMarker != null)
        {
            playerMarker.transform.SetAsLastSibling(); // �ñ�������ϲ�
        }
    }
}
