using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text 组件中的文字以两种颜色渐变显示。
/// </summary>
[RequireComponent(typeof(Text))]
public class TextVerticalGradientTwoColor : BaseMeshEffect
{
    [SerializeField] private Color topColor = Color.red;
    [SerializeField] private Color bottomColor = Color.green;

    private TextVerticalGradientTwoColor() { }

    /// <summary>
    /// 修改某一UI顶点颜色
    /// </summary>
    /// <param name="_UIVertexes"></param>
    /// <param name="_index"></param>
    /// <param name="_color32"></param>
    private static void SetColor(List<UIVertex> _UIVertexes, int _index, Color32 _color32)
    {
        UIVertex vertex = _UIVertexes[_index];
        vertex.color = _color32;
        _UIVertexes[_index] = vertex;
    }
    /// <summary>
    /// 修改所有的顶点颜色
    /// </summary>
    /// <param name="_UIVertexex"></param>
    private void ModifyVertices(List<UIVertex> _UIVertexex)
    {
        /* 由于一个Unity里面的字，是由两个三角形组成的正方形，每个三角形各拥有3个顶点，
         * 那么一个字就拥有左三角形的3个顶点和右三角形的3个顶点，那么一个字的UIVertex个数就是6个，
         * 所以这里 i+=6。
         */
        for (int i = 0; i < _UIVertexex.Count; i += 6)
        {
            SetColor(_UIVertexex, i + 0, topColor);
            SetColor(_UIVertexex, i + 1, topColor);
            SetColor(_UIVertexex, i + 2, bottomColor);
            SetColor(_UIVertexex, i + 3, bottomColor);
            SetColor(_UIVertexex, i + 4, bottomColor);
            SetColor(_UIVertexex, i + 5, topColor);
        }
    }
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!this.IsActive())
        {
            return;
        }

        List<UIVertex> _UIVertexes = new List<UIVertex>(vh.currentVertCount);
        vh.GetUIVertexStream(_UIVertexes);

        ModifyVertices(_UIVertexes);

        vh.Clear();
        vh.AddUIVertexTriangleStream(_UIVertexes);
    }
    public Color TopColor
    {
        get { return topColor; }
        set { topColor = value; }
    }
    public Color BottomColor
    {
        get { return bottomColor; }
        set { bottomColor = value; }
    }
}