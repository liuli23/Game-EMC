using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class MiniMapGenerator : MonoBehaviour
{
    [Header("�����")]
    public RawImage miniMapDisplay; // С��ͼ UI ���
    public Transform player;        // ��ɫ����

    [Header("��ɫ����")]
    public Color walkableColor = new Color(0.2f, 0.8f, 0.3f, 1.0f); // ��ɫ����ͨ�У�
    public Color obstacleColor = new Color(0.8f, 0.2f, 0.2f, 1.0f); // ��ɫ���ϰ��
    public Color transparentColor = new Color(0, 0, 0, 0); // ͸����������Ұ��

    [Header("С��ͼ����")]
    public int viewSize = 20;   // ��ʾ�����С����λ������Ԫ��
    private GridGraph astarGrid;
    private Texture2D mapTexture;

    void Start()
    {
        // ��ȡ A* ��������
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
        // ��ɫ�������е�λ��
        Vector3 graphLocalPos = astarGrid.transform.InverseTransform(player.position);
        int centerX = Mathf.FloorToInt(graphLocalPos.x / astarGrid.nodeSize);
        int centerY = Mathf.FloorToInt(graphLocalPos.z / astarGrid.nodeSize);

        // ����С��ͼ��Χ
        for (int y = 0; y < viewSize; y++)
        {
            for (int x = 0; x < viewSize; x++)
            {
                // ����ʵ����������
                int gridX = centerX + x - viewSize / 2;
                int gridY = centerY + y - viewSize / 2;

                // ȷ������������Χ
                if (gridX >= 0 && gridX < astarGrid.width && gridY >= 0 && gridY < astarGrid.depth)
                {
                    GridNode node = (GridNode)astarGrid.GetNode(gridX, gridY);
                    mapTexture.SetPixel(x, y, node.Walkable ? walkableColor : obstacleColor);
                }
                else
                {
                    mapTexture.SetPixel(x, y, transparentColor); // ������Χ��Ϊ͸��
                }
            }
        }

        mapTexture.Apply();
    }
}
