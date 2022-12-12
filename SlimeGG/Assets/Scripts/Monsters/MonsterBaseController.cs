using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseController : MonoBehaviour
{
    private float zCoor = 18f;
    private Vector2 direction;
    private Transform curTileSet;
    private int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(1, 1);
        layerMask = 1 << LayerMask.NameToLayer("TileSet");
    }

    // Update is called once per frame
    void Update()
    {
        //moveTo(direction, 0.5f);
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
            curTileSet.GetComponent<TileSetController>().removeMonster();
        }
    }

    private void OnMouseUp()
    {
        Vector3 rayStartPosition = transform.position;
        rayStartPosition.z += 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, transform.forward, 1.0f, layerMask);
        Debug.DrawRay(rayStartPosition, transform.forward * 1.0f, Color.green, 100.0f);
        if (hit)
        {
            if (hit.transform.tag == "Tile Set")
            {
                curTileSet = hit.transform;
                curTileSet.GetComponent<TileSetController>().addMonster(transform);
            }
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collided!");
        direction = new Vector2(UnityEngine.Random.Range(0, direction.x >= 0 ? 1 : -1), UnityEngine.Random.Range(0, direction.y >= 0 ? 1 : -1));
    }

    private void moveTo(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
