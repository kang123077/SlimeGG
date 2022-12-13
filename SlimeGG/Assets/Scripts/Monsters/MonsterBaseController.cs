using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBaseController : MonoBehaviour
{
    private static string sprite_path = "Sprites/Monsters/Infants/";
    private float zCoor = 18f;
    private Vector2 direction;
    private Transform curTileSet;
    private int layerMask;
    private MonsterInfo monsterInfo;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        Transform bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite_path + monsterInfo.infantType.ToString());
        Destroy(bg.GetComponent<PolygonCollider2D>());
        bg.AddComponent<PolygonCollider2D>();
    }
    void Start()
    {
        direction = new Vector2(1, 1);
        layerMask = 1 << LayerMask.NameToLayer("TileSet");
    }

    // Update is called once per frame
    void Update()
    {
        moveTo(direction, 0.5f);
    }
    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoor);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objPosition.z = zCoor;
        transform.position = objPosition;
    }

    private void OnMouseDown()
    {
        transform.SetParent(null);
        if (curTileSet != null)
        {
            curTileSet.GetComponent<TileSetController>().removeMonster(transform);
        }
        transform.gameObject.layer = 7;
    }

    private void OnMouseUp()
    {
        transform.gameObject.layer = 3;
        Vector3 rayStartPosition = transform.position;
        rayStartPosition.z += 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, transform.forward, 1.0f, layerMask);
        if (hit)
        {
            if (hit.transform.tag == "Tile Set")
            {
                assignMonsterToTileSet(hit.collider.transform);
            }
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction.x = UnityEngine.Random.Range(-10, 10) / 10.0f * (direction.x >= 0f ? -1.0f : 1.0f);
        direction.y = UnityEngine.Random.Range(-10, 10) / 10.0f * (direction.y >= 0f ? -1.0f : 1.0f);
    }

    private void moveTo(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void assignMonsterToTileSet(Transform tileSet)
    {
        curTileSet = tileSet;
        curTileSet.GetComponent<TileSetController>().addMonster(transform);
    }
}
