using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void UIOnChecker()
    {
        transform.Find("Popup UI").gameObject.SetActive(true);
    }
}
