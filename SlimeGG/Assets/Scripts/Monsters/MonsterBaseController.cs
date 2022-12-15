using Unity.VisualScripting;
using UnityEngine;

public class MonsterBaseController : MonoBehaviour
{
    private static string sprite_path = "Sprites/Monsters/Infants/";
    private float zCoor = 18f;
    private Vector2 direction = new Vector2(0f, 0f);
    private Transform curTileSet;
    private int layerMask;
    private MonsterInfo monsterInfo;
    private float moveTime = 0f;
    private bool isStopped = false;

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
        direction.x = Random.Range(-10, 10) / 10.0f;
        direction.y = Random.Range(-10, 10) / 10.0f;
        layerMask = 1 << LayerMask.NameToLayer("TileSet");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTime >= Random.Range(1f, 2f))
        {
            moveTime = 0f;
            if (!isStopped)
            {
                direction.x = Random.Range(6f, 10f) / 5.0f * (direction.x > 1.5f ? -1f : 1f);
                direction.y = Random.Range(6f, 10f) / 5.0f * (direction.y > 1.5f ? -1f : 1f);
            }
            isStopped = !isStopped;
        }
        else
        {
            moveTime += Time.deltaTime;
            if (!isStopped)
            {
                moveTo(direction, 1f);
            }
        }
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
        RaycastHit hit3D;
        if (Physics.Raycast(rayStartPosition, transform.forward, out hit3D, 2.0f))
        {
            if (hit3D.collider.transform.tag == "Socket")
            {
                assignMonsterToTileSet(hit3D.collider.transform.GetComponent<SocketController>().tileSetInstalled);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isStopped = true;
        moveTime = 0f;
        Vector2 colPoint = collision.contacts[0].point;
        direction.x = Random.Range(3f, 10f) / 5.0f * (
            colPoint.x - transform.position.x < 1f  && colPoint.x - transform.position.x > 0f
            ? -1f
            : transform.position.x - colPoint.x < 1f && colPoint.x - transform.position.x > 0f
            ? 1f 
            : Random.Range(-1f, 1f)
            );
        direction.y = Random.Range(3f, 10f) / 5.0f * (
            colPoint.y - transform.position.y < 1f && colPoint.y - transform.position.y > 0f
            ? -1f
            : transform.position.y - colPoint.y < 1f && transform.position.y - colPoint.y > 0f
            ? 1f
            : Random.Range(-1f, 1f)
            );
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
