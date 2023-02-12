using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnrtySlotManager : MonoBehaviour
{
    [SerializeField]
    int x = 4, y = 7;
    // Start is called before the first frame update
    void Start()
    {
        if (x != 0)
        {
            GetComponent<GridLayoutGroup>().constraintCount = x;
            for(int i = 0; i < x * y; i++)
            {
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
