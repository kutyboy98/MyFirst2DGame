using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : SingleTon<GrassManager>
{
    [SerializeField]
    private GameObject[] tileObjects;
    [SerializeField]
    private CameraManager cameraManager;
    [SerializeField]
    private GameObject portalPrefab;
    [SerializeField]
    private GameObject flagPrefab;
    [SerializeField]
    private Transform grassContainer;
    [SerializeField]
    private PointComponent mapSize;
    private PointComponent startPoint;
    private PointComponent destinationPoint;
    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            if (path == null) GeneratePath();
            return new Stack<Node>(new Stack<Node>(path));
        }
        set
        {
            path = null;
        }
    }
    public Dictionary<PointComponent, PointManager> GrassCoordinatesDict;
    public BluePortalManager BluePortalManager;
    public RedPortalManager RedPortalManager;
    public float TileXSize { get { return tileObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; } }
    public float TileYSize { get { return tileObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y; } }

    public PointComponent StartPoint { get => startPoint; }
    public PointComponent DestinationPoint { get => destinationPoint; }

    void Start()
    {
        startPoint = new PointComponent(0, 0);
        destinationPoint = new PointComponent(10, 5);
        CreateGrasses();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateGrasses()
    {
        mapSize = new PointComponent(40, 10);
        GrassCoordinatesDict = new Dictionary<PointComponent, PointManager>();
        Vector3 startPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        Vector3 maxTile = Vector3.zero;
        for (int y = 0; y <= mapSize.Y; y++)
        {
            for (int x = 0; x <= mapSize.X; x++)
            {
                PlaceTile(x, y, startPoint);
            }
        }
        maxTile = GrassCoordinatesDict[mapSize].transform.position;
        cameraManager.SetLimits(new Vector3(maxTile.x + TileXSize, maxTile.y - TileYSize));
        PlaceFox();
    }

    private void PlaceTile(int x, int y, Vector3 startPoint)
    {
        PointManager pointTile = Instantiate(tileObjects[0]).GetComponent<PointManager>();
        pointTile.SetUp(new PointComponent(x, y), new Vector3(startPoint.x + TileXSize * x, startPoint.y - TileYSize * y), grassContainer);
    }

    private void PlaceFox()
    {
        BluePortalManager = Instantiate(portalPrefab, GrassCoordinatesDict[startPoint].CenterPoint, Quaternion.identity).GetComponent<BluePortalManager>();
        RedPortalManager = Instantiate(flagPrefab, GrassCoordinatesDict[destinationPoint].CenterPoint, Quaternion.identity).GetComponent<RedPortalManager>();
    }

    public bool InBound(PointComponent iPoint)
    {
        return iPoint.X >= 0 && iPoint.Y >= 0 && iPoint.X <= mapSize.X && iPoint.Y <= mapSize.Y;
    }

    private void GeneratePath()
    {
        path = AStar.GetPath(startPoint, destinationPoint);
    }
}
