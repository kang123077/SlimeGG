using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBaseController : MonoBehaviour
{
    private GameObject socketMounted = null;
    private GameObject tempSocketMountable = null;

    public void detach()
    {
        if (socketMounted != null)
        {
            socketMounted.GetComponent<SocketController>().tileSetInstalled = null;
            socketMounted = null;
        }
    }

    public Vector2 attach(Transform parentTileSet)
    {
        socketMounted = tempSocketMountable;
        tempSocketMountable = null;
        socketMounted.GetComponent<SocketController>().tileSetInstalled = parentTileSet;
        return socketMounted.GetComponent<SocketController>().coor;
    }

    public GameObject returnSocketMountable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 11.0f))
        {
            GameObject curSocketTr = hit.collider.gameObject;
            if (curSocketTr.tag == "Socket" && curSocketTr.GetComponent<SocketController>().tileSetInstalled == null)
            {
                tempSocketMountable = curSocketTr;
                return curSocketTr;
            }
        }
        return null;
    }
}
