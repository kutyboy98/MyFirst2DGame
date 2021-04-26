using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : SingleTon<Hover>
{
    private SpriteRenderer spriteRenderer { get; set; }
    private SpriteRenderer rangeSpriteRender { get; set; }
    public bool IsVisible { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangeSpriteRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }
    }

    public void ActiveSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
        rangeSpriteRender.enabled = true;
        IsVisible = true;
    }

    public void DeactiveSprite()
    {
        spriteRenderer.enabled = false;
        rangeSpriteRender.enabled = false;
        GameManager.Instance.TowerBtnSelection = null;
        IsVisible = false;
    }

    private void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactiveSprite();
        }
    }
}
