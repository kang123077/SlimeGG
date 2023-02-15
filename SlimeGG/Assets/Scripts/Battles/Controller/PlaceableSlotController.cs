using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSlotController : MonoBehaviour
{
    private SpriteRenderer background;
    private bool isPosessed, isForDisplay;
    private int x, y;
    private EntrySlotController entrySlotController;
    private int curStatus = 0;
    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                // 초기화 안됨
                initSetting();
                break;
            case 1:
                // 초기화 완료
                break;
        }
    }

    private void initSetting()
    {
        isPosessed = false;
        background = GetComponent<SpriteRenderer>();
        background.color = pickColor(false);
        curStatus = 1;
    }

    private static Color pickColor(bool isPosessed)
    {
        if (isPosessed) return new Color(0.8f, 0.8f, 0.8f, 1f);
        return new Color(.2f, .2f, .2f, 1f);
    }

    public bool getIsAvailable()
    {
        return !isPosessed && !isForDisplay;
    }

    public void setCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int[] getCoordinate()
    {
        return new int[] { x, y };
    }

    public void installEntrySlot(Transform newEntrySlot)
    {
        newEntrySlot.SetParent(transform);
        newEntrySlot.GetComponent<RectTransform>().anchorMin = Vector2.one * .5f;
        newEntrySlot.GetComponent<RectTransform>().anchorMax = Vector2.one * .5f;
        newEntrySlot.localScale = Vector3.one;
        newEntrySlot.localPosition = new Vector3(0f, 0f, -1f);
        entrySlotController = newEntrySlot.GetComponent<EntrySlotController>();
        entrySlotController.setCoordinate(x, y);
        isPosessed = true;
        background.color = pickColor(true);
    }

    public void uninstallEntrySlot()
    {
        entrySlotController = null;
        isPosessed = false;
        background.color = pickColor(false);
    }

    public void setIsForDisplay(bool isForDisplay)
    {
        this.isForDisplay = isForDisplay;
    }

    public bool getIsForDisplay()
    {
        return isForDisplay;
    }

    public EntrySlotController getInstalledEntrySlotController()
    {
        return entrySlotController;
    }

    public void truncateEntrySlot()
    {
        isPosessed = false;
        background.color = pickColor(false);
        if (entrySlotController != null)
        {
            Destroy(entrySlotController.gameObject);
        }
    }
}
