using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class MonsterBaseController : MonoBehaviour
{
    private float zCoor = 18f;
    private Vector2 direction = new Vector2(0f, 0f);
    private Transform curTileSet;
    private MonsterInfo monsterInfo;
    private float moveTime = 0f;
    private bool isStopped = false;
    private Vector2 correctionCoor;
    private Transform bg;
    private Animator anim;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + LocalDictionary.monsters[monsterInfo.accuSpecies.Last()].resourcePath
            );
        Destroy(bg.GetComponent<PolygonCollider2D>());
        bg.AddComponent<PolygonCollider2D>();
        anim = bg.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.ANIMATION + LocalDictionary.monsters[monsterInfo.accuSpecies.Last()].resourcePath + "/Controller"
            );
    }

    void Start()
    {
        direction.x = Random.Range(-10, 10) / 10.0f;
        direction.y = Random.Range(-10, 10) / 10.0f;
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
                anim.SetInteger("DirectionState", direction.x < 0f ? 1 : 2);
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
        Vector3 mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            10f
            );
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objPosition.z = zCoor;
        transform.position = objPosition - new Vector3(correctionCoor.x, correctionCoor.y, 0f);
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = new Vector3(
       Input.mousePosition.x - correctionCoor.x,
       Input.mousePosition.y - correctionCoor.y,
       10f
       );
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        correctionCoor = objPosition - transform.position;

        GameObject.Find("UI").GetComponent<UIController>().UIOnChecker();
        GameObject.Find("Popup UI").GetComponent<PopupUIController>().generateUI(monsterInfo);

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
                if (hit3D.collider.transform.GetComponent<SocketController>().tileSetInstalled != null)
                {
                    assignMonsterToTileSet(hit3D.collider.transform.GetComponent<SocketController>().tileSetInstalled, false);
                    return;
                }
            }
        }
        assignMonsterToTileSet(curTileSet, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isStopped = true;
        moveTime = 0f;
        Vector2 colPoint = collision.contacts[0].point;
        direction.x = Random.Range(3f, 10f) / 5.0f * (
            colPoint.x - transform.position.x < 1f && colPoint.x - transform.position.x > 0f
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
        anim.SetInteger("DirectionState", direction.x < 0f ? 1 : 2);
    }

    private void moveTo(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void assignMonsterToTileSet(Transform tileSet, bool isReturned)
    {
        monsterInfo.installedPosition = LocalStorage.tileSetTransforms.IndexOf(tileSet);
        curTileSet = tileSet;
        curTileSet.GetComponent<TileSetController>().addMonster(transform, isReturned);
    }
}