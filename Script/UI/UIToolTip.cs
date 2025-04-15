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

        // 获取屏幕宽度和高度
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 计算偏移量，确保提示框不会超出屏幕
        if (mousePosition.x > screenWidth / 2)
        {
            newXOffset = -xOffset; // 鼠标在屏幕右侧，提示框向左偏移
        }
        else
        {
            newXOffset = xOffset; // 鼠标在屏幕左侧，提示框向右偏移
        }

        if (mousePosition.y > screenHeight / 2)
        {
            newYOffset = -yOffset; // 鼠标在屏幕上侧，提示框向下偏移
        }
        else
        {
            newYOffset = yOffset; // 鼠标在屏幕下侧，提示框向上偏移
        }

        // 设置提示框的位置
        transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }



}
