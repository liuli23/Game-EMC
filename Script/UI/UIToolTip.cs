using UnityEngine;

public class UIToolTip : MonoBehaviour
{
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        // ��ȡ��Ļ��Ⱥ͸߶�
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ����ƫ������ȷ����ʾ�򲻻ᳬ����Ļ
        if (mousePosition.x > screenWidth / 2)
        {
            newXOffset = -xOffset; // �������Ļ�Ҳ࣬��ʾ������ƫ��
        }
        else
        {
            newXOffset = xOffset; // �������Ļ��࣬��ʾ������ƫ��
        }

        if (mousePosition.y > screenHeight / 2)
        {
            newYOffset = -yOffset; // �������Ļ�ϲ࣬��ʾ������ƫ��
        }
        else
        {
            newYOffset = yOffset; // �������Ļ�²࣬��ʾ������ƫ��
        }

        // ������ʾ���λ��
        transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }



}
