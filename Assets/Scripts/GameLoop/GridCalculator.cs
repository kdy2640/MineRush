using UnityEngine;
using UnityEngine.Rendering;

public class GridCalculator : MonoBehaviour
{
    [SerializeField] Grid TileGrid;
    [SerializeField] bool isGizmo = false;
    private static float tileWidth;
    private static float tileHeight;
    private static Vector3 deltaPos = new Vector2(0, -3.75f);
    private void Awake()
    {
        tileWidth = TileGrid.cellSize.x;
        tileHeight = TileGrid.cellSize.y;
    }

    public static Vector3 GridToWorld(Vector2 gridPos)
    {
        float worldX = (gridPos.x - gridPos.y) * tileWidth * 0.5f;
        float worldY = (gridPos.x + gridPos.y) * tileHeight * 0.5f;

        return new Vector3(worldX, worldY, 0f) + deltaPos;
    }

    private void OnDrawGizmos()
    {
        if (!isGizmo) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                Gizmos.DrawSphere(GridToWorld(new Vector2(i, j)), 0.1f);
            }
        }
    }
}
