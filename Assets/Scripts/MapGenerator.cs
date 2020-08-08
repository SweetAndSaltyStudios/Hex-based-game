using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance
    {
        get;
        private set;
    }

    public GameObject TilePrefab;
    public Mesh Mesh_Water;
    public Mesh Mesh_Flat;
    public Mesh Mesh_Hill;
    public Mesh Mesh_Mountain;

    public Material Oceans_Mat;
    public Material Plains_Mat;
    public Material Grasslands_Mat;
    public Material Mountains_Mat;

    public TileMap CreatedTileMap;

    public bool IsWorldCreated
    {
        get;
        private set;
    }

    [Range(10, 100)]
    public int MapColumns = 20;
    [Range(10, 100)]
    public int MapRows = 20;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreatedTileMap = new TileMap(MapColumns, MapRows);
    }

    private void Start()
    {     
        GenerateMap();

        //CreatedTileMap.UpdateTilePositions();

        //CreatedTileMap.ElevateArea(15, 4, 4);
        //CreatedTileMap.ElevateArea(15, 2, 2);
        //CreatedTileMap.ElevateArea(10, 2, 2);
        //CreatedTileMap.ElevateArea(0, 10, 1);

        //CreatedTileMap.UpdateVisuals();
    }

    private void GenerateMap()
    {
        for (int column = 0; column < MapColumns; column++)
        {
            for (int row = 0; row < MapRows; row++)
            {
                var newTile = Instantiate(TilePrefab, transform).GetComponent<Tile>();
                newTile.Initialize(CreatedTileMap, column, row, 0.5f, -1);
            }
        }

        IsWorldCreated = true;
    }
}
