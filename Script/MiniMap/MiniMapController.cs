using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class MiniMapGenerator : MonoBehaviour
{
    [Header("绑定组件")]
    public RawImage miniMapDisplay; // 小地图 UI 组件
    public Transform player;        // 角色对象

    [Header("颜色设置")]
    public Color walkableColor = new Color(0.2f, 0.8f, 0.3f, 1.0f); // 绿色（可通行）
    public Color obstacleColor = new Color(0.8f, 0.2f, 0.2f, 1.0f); // 红色（障碍物）
    public Color transparentColor = new Color(0, 0, 0, 0); // 透明（超出视野）

    [Header("小地图设置")]
    public int viewSize = 20;   // 显示区域大小（单位：网格单元）
    private GridGraph astarGrid;
    private Texture2D mapTexture;

    void Start()
    {
        // 获取 A* 网格数据
        astarGrid = AstarPath.active.data.gridGraph;
        mapTexture = new Texture2D(viewSize, viewSize);
        mapTexture.wrapMode = TextureWrapMode.Clamp;

        miniMapDisplay.texture = mapTexture;
    }

    void Update()
    {
        UpdateMiniMap();
    }

    void UpdateMiniMap()
    {
        // 角色在网格中的位置
        Vector3 graphLocalPos = astarGrid.transform.InverseTransform(player.position);
        int centerX = Mathf.FloorToInt(graphLocalPos.x / astarGrid.nodeSize);
        int centerY = Mathf.FloorToInt(graphLocalPos.z / astarGrid.nodeSize);

        // 遍历小地图范围
        for (int y = 0; y < viewSize; y++)
        {
            for (int x = 0; x < viewSize; x++)
            {
                // 计算实际网格坐标
                int gridX = centerX + x - viewSize / 2;
                int gridY = centerY + y - viewSize / 2;

                // 确保不超出网格范围
                if (gridX >= 0 && gridX < astarGrid.width && gridY >= 0 && gridY < astarGrid.depth)
                {
                    GridNode node = (GridNode)astarGrid.GetNode(gridX, gridY);
                    mapTexture.SetPixel(x, y, node.Walkable ? walkableColor : obstacleColor);
                }
                else
                {
                    mapTexture.SetPixel(x, y, transparentColor); // 超出范围设为透明
                }
            }
        }

        mapTexture.Apply();
    }
}
