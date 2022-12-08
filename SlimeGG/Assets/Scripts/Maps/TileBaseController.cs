using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBaseController : MonoBehaviour
{
    private GameObject socketMounted = null;
    private GameObject tempSocketMountable = null;
    private float zCoor = 19f;

    public void detach()
    {
        if (socketMounted != null)
        {
            socketMounted.GetComponent<SocketController>().isMounted = false;
            socketMounted = null;
        }
    }

    public void attach()
    {
        socketMounted = tempSocketMountable;
        tempSocketMountable = null;
        Vector3 socketPos = socketMounted.transform.position;
        transform.position = new Vector3(socketPos.x, socketPos.y, zCoor);
        socketMounted.GetComponent<SocketController>().isMounted = true;
    }

    public GameObject returnSocketMountable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
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
}
