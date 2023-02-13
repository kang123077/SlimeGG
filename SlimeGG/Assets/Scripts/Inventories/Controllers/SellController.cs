using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellController : MonoBehaviour
{
    private TextMeshProUGUI priceText;
    // Start is called before the first frame update
    void Start()
    {
        priceText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        checkAbove();
    }

    public void viewSellPrice(ContentController content)
    {
        if (content == null)
        {
            priceText.text = string.Empty;
        }
        else
        {
            priceText.text = $"+ {CommerceFunction.weighPriceOfContent(content)}G";
        }
    }

    public void checkAbove()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 2f))
        {
            viewSellPrice(hit.transform.GetComponent<ContentController>());
        }
        else
        {
            viewSellPrice(null);
        }
    }
}
