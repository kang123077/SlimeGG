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
            socketMounted.GetComponent<SocketController>().isMounted = false;
            socketMounted = null;
        }
    }

    public Vector2 attach()
    {
        socketMounted = tempSocketMountable;
        tempSocketMountable = null;
        Vector3 socketPos = socketMounted.transform.position;
        socketMounted.GetComponent<SocketController>().isMounted = true;
        return socketMounted.GetComponent<SocketController>().coor;
    }

    public GameObject returnSocketMountable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 11.0f))
        {
            GameObject curSocketTr = hit.collider.gameObject;
            if (curSocketTr.tag == "Socket" && curSocketTr.GetComponent<SocketController>().isMounted == false)
            {
                tempSocketMountable = curSocketTr;
                return curSocketTr;
            }
        }
        return null;
    }

    // 임시 타일 타일 색 명시용
    public void setTileType(TileType tileType)
    {
    }
}
