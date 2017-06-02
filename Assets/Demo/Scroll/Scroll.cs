using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 相对归属（自身本地相对自身不变，然后还是相对父节点）
/// 差量锚点 偏移轴点pivot
/// 世界还是像素（UI）
/// </summary>
public class Scroll : MonoBehaviour {

    public ScrollRect scrollRect;
    int length = 0;
    void Start() {
       Button[] allButtons =  scrollRect.GetComponentsInChildren<Button>();
       foreach (Button b in allButtons)
       {
           b.GetComponentInChildren<Text>().text = b.name;
       }
       length = allButtons.Length;
    }

    /// <summary>
    /// 指定一个 item让其定位到ScrollRect中间
    /// </summary>
    /// <param name="target">需要定位到的目标</param>
    public void CenterOnItem(RectTransform target)
    {
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(scrollRect.viewport));
        Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(target));
        Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);
        // So it has to move this distance
        var difference = itemCenterPositionInScroll - targetPositionInScroll;
        difference.z = 0f;

        var newNormalizedPosition = new Vector2(difference.x / (scrollRect.content.rect.width - scrollRect.viewport.rect.width),
            difference.y / (scrollRect.content.rect.height - scrollRect.viewport.rect.height));

        newNormalizedPosition = scrollRect.normalizedPosition - newNormalizedPosition;

        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

        DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, newNormalizedPosition, 3);
    }

    Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    Vector3 GetWidgetLUWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0f - target.pivot.x) * target.rect.size.x,
            (1f - target.pivot.y) * target.rect.size.y,
            0f);
        return target.position + pivotOffset;
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
    int index = 0;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CenterOnItem(scrollRect.content.Find("Button (" + index + ")").GetComponent<RectTransform>());
            index++;
            index = index % length;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            CenterToSelected(scrollRect.content.Find("Button (" + index + ")").gameObject);
            index++;
            index = index % length;
        }
    }


    void CenterToSelected(GameObject selected)
    {
        var target = selected.GetComponent<RectTransform>();

        //Vector3 maskCenterPos = scrollRect.viewport.position + (Vector3)scrollRect.viewport.rect.center;//等价于
        //Vector3 maskCenterPos = scrollRect.viewport.TransformPoint((Vector3)scrollRect.viewport.rect.center);
        Vector3 maskCenterPos = scrollRect.viewport.position;//左上角 同一或对齐
        Debug.Log("Mask Center Pos: " + maskCenterPos);
        //Vector3 itemCenterPos = target.position;
        Vector3 itemCenterPos = GetWidgetLUWorldPoint(target.transform as RectTransform);
        Debug.Log("Item Center Pos: " + itemCenterPos + "_" + target.position);
        Vector3 difference = maskCenterPos - itemCenterPos;
        difference.z = 0;

        Vector3 newPos = scrollRect.content.position + difference;

        DOTween.To(() => scrollRect.content.position, x => scrollRect.content.position = x, newPos, 5);
    }
}
