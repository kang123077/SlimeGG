using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    float zCoor = 9f;
    private GameObject socketMounted = null;

    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoor);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objPosition.z = zCoor;
        transform.position = objPosition;
    }

    private void OnMouseDown()
    {
        if (socketMounted != null)
        {
            socketMounted.GetComponent<SocketController>().isMounted = false;
            socketMounted = null;
            print("Socket Detached!!");
        }
    }

    private void OnMouseUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            GameObject curSocketTr = hit.collider.gameObject;
            if (curSocketTr.tag == "Socket")
            {
                socketMounted = curSocketTr;
                Vector3 socketPos = socketMounted.transform.position;
                transform.position = new Vector3(socketPos.x, socketPos.y, 9f);
                socketMounted.GetComponent<SocketController>().isMounted = true;
            }
        }
    }
}
