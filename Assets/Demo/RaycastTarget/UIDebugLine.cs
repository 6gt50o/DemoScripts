using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDebugLine : MonoBehaviour {

    private Vector3[] fourCorners = new Vector3[4];
    void OnDrawGizmos()
    {
        MaskableGraphic[] mGraphics = GetComponentsInChildren<MaskableGraphic>(true);
        for (int index = 0, length = mGraphics.Length; index < length; index++)
        {
            if (mGraphics[index].raycastTarget)
            {
                RectTransform recTrans = mGraphics[index].rectTransform;
                recTrans.GetWorldCorners(fourCorners);
                Gizmos.color = Color.yellow;
                for (int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i+1)%4]);
                }
            }
        }
    }
}
