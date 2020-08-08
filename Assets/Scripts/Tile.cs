using UnityEngine;

public class Tile : MonoBehaviour
{
    #region VARIABLES

    private TileMap tileMap;

    public float Elevation;
    public float Moisture;

    private int offset;
    private float radius;
    private readonly float widthMultiplier = Mathf.Sqrt(3) * 0.5f;

    private TextMesh tilePositionText;

    public int Column
    {
        get;
        private set;
    }
    public int Row
    {
        get;
        private set;
    }

    public MeshRenderer MeshRenderer
    {
        get;
        private set;
    }

    #endregion VARIABLES

    #region PROPERTIES

    private float Width
    {
        get
        {
            return widthMultiplier * Height;
        }
    }
    private float Height
    {
        get
        {
            return radius * 2;
        }
    }
    private float HorizontalSpacing
    {
        get
        {
            return Width;
        }
    }
    private float VerticalSpacing
    {
        get
        {
            return Height * 0.75f;
        }
    }

    public Vector3 Position
    {
        get
        {
            return new Vector3
                (
                    HorizontalSpacing * (Column + Row * 0.5f),
                    0,
                    VerticalSpacing * Row
                );
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
        tilePositionText = GetComponentInChildren<TextMesh>();
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    public void Initialize(TileMap tileMap, int column, int row, float radius, int elevation)
    {
        this.tileMap = tileMap;
        Column = column;
        Row = row;     

        offset = -(column + row);

        this.radius = radius;

        tileMap.AddTile(this, column, row);

        tilePositionText.text = column + " , " + row;

        Elevation = elevation;

        gameObject.name = string.Format("TILE: {0},{1}", column, row);

        transform.position = Position;
    }

    #endregion CUSTOM_FUNCTIONS   

    public void UpdatePosition(Vector3 cameraPosition, float numColumns, float numRows)
    {
        var currentPositionFromCamera = PositionFromCamera(cameraPosition, numColumns, numRows);
        transform.position = new Vector3(currentPositionFromCamera.x, 0, currentPositionFromCamera.z);
    }

    private Vector3 PositionFromCamera(Vector3 cameraPosition, float numColumns, float numRows)
    {
        var mapWidth = numColumns * HorizontalSpacing;
        var mapHeight = numRows * VerticalSpacing;

        var position = Position;

        if (MapGenerator.Instance.CreatedTileMap.AllowWrapHorizontally)
        {
            var howManyWidthsFromCamera = Mathf.Round((position.x - cameraPosition.x) / mapWidth);
            var howManyWidthToFix = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthToFix * mapWidth;
        }

        if (MapGenerator.Instance.CreatedTileMap.AllowWrapVertically)
        {
            var howManyHeightsFromCamera = Mathf.Round((position.z - cameraPosition.z) / mapHeight);
            var howManyHeightsToFix = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsToFix * mapHeight;
        }

        return position;
    }

    public void UpdateVisuals(Material material)
    {
        MeshRenderer.material = material;
    }

    public static float Distance(Tile a, Tile b)
    {
        return Mathf.Max(
             Mathf.Abs(a.Column - b.Column),
             Mathf.Abs(a.Row - b.Row),
             Mathf.Abs(a.offset - b.offset)
             );
    }
}
