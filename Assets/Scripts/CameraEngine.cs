using UnityEngine;

public class CameraEngine : MonoBehaviour
{
    public static CameraEngine Instance
    {
        get;
        private set;
    }

    private Camera mainCamera;

    private Vector3 previousPosition;   

    public Vector3 CurrentPosition
    {
        get
        {
            return transform.position;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = GetComponent<Camera>();

        previousPosition = CurrentPosition;
    }

    private void Update()
    {
        CheckIfCameraMoved();
    }

    private void CheckIfCameraMoved()
    {
        if(previousPosition != CurrentPosition)
        {
            previousPosition = CurrentPosition;

            MapGenerator.Instance.CreatedTileMap.UpdateTilePositions();          
        }
    }

    public void PanToTile(Tile tile)
    {

    }
}
