using System;
using UnityEngine;
using UnityEngine.UI;

//固定起点 （定点） + 顺时针旋转(顺针) + 渐变填充
public class UICircle : RawImage
{
    const int FILL_PERCENT = 100;
    float thickness = 5;

    [SerializeField]
    [Range(3, 360)]
    int _segments = 36;

    public int segments
    {
        get { return _segments; }
        set
        {
            if (_segments != value)
            {
                _segments = value;
                SetVerticesDirty();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(transform);
#endif
            }
        }
    }


    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        this.thickness = (float)Mathf.Clamp(this.thickness, 0, rectTransform.rect.width / 2);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        float outer = -0.5f * rectTransform.rect.width; //顺时针绘制为负值
        //float outer = -rectTransform.pivot.x * rectTransform.rect.width; //错误做法
        float inner = -rectTransform.pivot.x * rectTransform.rect.width + this.thickness;

        vh.Clear();

        Vector2 prevX = Vector2.zero;
        Vector2 prevY = Vector2.zero;
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;

        float tw = rectTransform.rect.width;
        float th = rectTransform.rect.height;

        float angleByStep = (FILL_PERCENT / 100f * (Mathf.PI * 2f)) / segments;
        float currentAngle = 0f;//必然有一个固定起点（1 * outer，0）
        float offset = 0.5f; //固定相对中心点(0,0) -- 对应UV偏移0.5f(左下角0,0)
        for (int i = 0; i < segments + 1; i++)
        {

            float c = Mathf.Cos(currentAngle);
            float s = Mathf.Sin(currentAngle);

            StepThroughPointsAndFill(outer, inner, ref prevX, ref prevY, out pos0, out pos1, out pos2, out pos3, c, s);

            uv0 = new Vector2(pos0.x / tw + offset, pos0.y / th + offset);
            uv1 = new Vector2(pos1.x / tw + offset, pos1.y / th + offset);
            uv2 = new Vector2(pos2.x / tw + offset, pos2.y / th + offset);
            uv3 = new Vector2(pos3.x / tw + offset, pos3.y / th + offset);

            vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));

            currentAngle += angleByStep;
        }
    }

    private void StepThroughPointsAndFill(float outer, float inner, ref Vector2 prevX, ref Vector2 prevY, out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, float c, float s)
    {
        pos0 = prevX;
        pos1 = new Vector2(outer * c, outer * s);

        pos2 = Vector2.zero;
        pos3 = Vector2.zero;

        prevX = pos1;
        prevY = pos2;

        //Debug.Log("prevX:" + prevX + " prevY:" + prevY);
    }

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

}
