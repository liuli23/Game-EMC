using UnityEngine;
using UnityEngine.UI;

public class PlayerMarkerController : MonoBehaviour
{
    public Transform realPlayer;  // 角色对象
    public RectTransform miniMapRect; // 小地图 UI 组件
    public Image playerMarker; // 角色标记的 Image 组件
    public int viewSize = 20; // 小地图显示区域大小

    void Start()
    {
        // 确保角色标记可见
        if (playerMarker != null)
        {
            playerMarker.enabled = true;
        }
    }

    void Update()
    {
        // 让角色标记始终保持在小地图中心
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // 确保角色标记在小地图上正确渲染
        if (playerMarker != null)
        {
            playerMarker.transform.SetAsLastSibling(); // 让标记在最上层
        }
    }
}
