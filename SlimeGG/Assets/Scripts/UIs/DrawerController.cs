using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public enum Direction
    {
        Left, Right, Top, Bottom
    }
    [SerializeField]
    private float distanceToMove;
    [SerializeField]
    private Direction directionAttached;

    private Transform button;
    private bool isHidden = false;
    void Start()
    {
        button = transform.Find("Toggle Button");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleDrawer()
    {
        transform.Translate(
            new Vector3(
                (directionAttached == Direction.Left ? -1f : directionAttached == Direction.Right ? 1f : 0f) * (isHidden ? 1f : -1f) * distanceToMove,
                (directionAttached == Direction.Top ? -1f : directionAttached == Direction.Bottom ? 1f : 0f) * (isHidden ? 1f : -1f) * distanceToMove,
                0f)
            );
        button.transform.Rotate(new Vector3(0f, 0f, 180f));
        isHidden = !isHidden;
    }
}
