using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class ToolTipHandler : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvasObj;
    [SerializeField] private RectTransform popupObj;
    [SerializeField] private TextMeshProUGUI infoTxt;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float padding;

    private Canvas popupCanvas;

    void Awake()
    {
        popupCanvas = popupCanvasObj.GetComponent<Canvas>();
    }

    void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if(!popupCanvasObj.activeSelf)
            return;

        Vector3 newPos = Input.mousePosition + offset;
        newPos.z = 0;
        float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObj.rect.width * popupCanvas.scaleFactor) - padding;
        if(rightEdgeToScreenEdgeDistance < 0)
        {
            newPos.x += rightEdgeToScreenEdgeDistance;
        }

        float lefttEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObj.rect.width * popupCanvas.scaleFactor) + padding;
        if(lefttEdgeToScreenEdgeDistance > 0)
        {
            newPos.x += lefttEdgeToScreenEdgeDistance;
        }

        float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObj.rect.height * popupCanvas.scaleFactor) - padding;
        if(topEdgeToScreenEdgeDistance < 0)
        {
            newPos.y += topEdgeToScreenEdgeDistance;
        }

        popupObj.transform.position = newPos;
    }

    public void DisplayInfo(Item item)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<size=7>").Append(item.ColoredName).Append("</size>").AppendLine();
        builder.Append(item.GetToolTipInfoText());

        infoTxt.text = builder.ToString();

        popupCanvasObj.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObj);
    }

    public void HideInfo()
    {
        popupCanvasObj.SetActive(false);
    }
}
