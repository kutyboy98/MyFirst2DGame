using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointManager : MonoBehaviour
{
    public PointComponent Point { get; set; }
    public Vector2 CenterPoint { get { return new Vector2(transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2); } }
    private SpriteRenderer spriteRenderer { get; set; }
    public bool isEmpty { get; set; }
    private Color32 greenColor = new Color32(0, 237, 255, 255);
    private Color32 redColor = new Color32(255, 101, 0, 255);
    private Color32 noneColor = new Color32(96, 255, 90, 255);
    private TowerManager towerManager;

    public bool IsWalkAble
    {
        get { return isEmpty; }
        private set { }
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(PointComponent point, Vector3 vector3, Transform parent)
    {
        this.Point = point;
        transform.position = vector3;
        transform.SetParent(parent);
        GrassManager.Instance.GrassCoordinatesDict.Add(new PointComponent(point.X, point.Y), this);
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.TowerBtnSelection != null)
        {
            if (!isEmpty)
                SetBackground(redColor);
            else
                SetBackground(greenColor);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.childCount == 0 && !EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.TowerBtnSelection != null)
            {
                PlaceTower();
            }
            else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.TowerBtnSelection == null)
            {
                if (towerManager != null)
                    GameManager.Instance.SelectTower(towerManager);
                else
                    GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        SetBackground(noneColor);
    }

    private void PlaceTower()
    {
        isEmpty = false;
        if (AStar.GetPath(GrassManager.Instance.StartPoint, GrassManager.Instance.DestinationPoint) == null)
        {
            isEmpty = true;
            return;
        }
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.TowerBtnSelection.Tower, new Vector3(CenterPoint.x, CenterPoint.y), Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = Point.Y;
        tower.transform.SetParent(transform);
        towerManager = tower.transform.GetChild(0).GetComponent<TowerManager>();
        towerManager.Price = GameManager.Instance.TowerBtnSelection.Price;
        GameManager.Instance.BuyTower();
        isEmpty = false;
    }

    private void SetBackground(Color32 color)
    {
        spriteRenderer.color = color;
    }
}
