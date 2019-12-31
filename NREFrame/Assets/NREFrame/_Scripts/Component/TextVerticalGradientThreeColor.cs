using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Text 组件中的文字以三种颜色渐变显示。
/// </summary>
[RequireComponent(typeof(Text))]
public class TextVerticalGradientThreeColor : BaseMeshEffect
{
    [SerializeField] private Color topColor = Color.red;
    [SerializeField] private Color centerColor = Color.blue;
    [SerializeField] private Color bottomColor = Color.green;

    private TextVerticalGradientThreeColor() { }
    /// <summary>
    /// 修改顶点颜色
    /// </summary>
    /// <param name="_vertex"></param>
    /// <param name="_color"></param>
    /// <returns></returns>
    private UIVertex ModifyColor(UIVertex _vertex, Color _color)
    {
        _vertex.color = _color;
        return _vertex;
    }
    /// <summary>
    /// 根据两个顶点信息计算出中间顶点的信息，并返回
    /// </summary>
    /// <param name="_topVertex"></param>
    /// <param name="_bottomVertex"></param>
    /// <returns></returns>
    private UIVertex CalcCenterVertex(UIVertex _topVertex, UIVertex _bottomVertex)
    {
        UIVertex _centerVertex = new UIVertex();
        _centerVertex.normal = (_topVertex.normal + _bottomVertex.normal) / 2;
        _centerVertex.position = (_topVertex.position + _bottomVertex.position) / 2;
        _centerVertex.tangent = (_topVertex.tangent + _bottomVertex.tangent) / 2;
        _centerVertex.uv0 = (_topVertex.uv0 + _bottomVertex.uv0) / 2;
        _centerVertex.uv1 = (_topVertex.uv1 + _bottomVertex.uv1) / 2;
        _centerVertex.color = centerColor;
        return _centerVertex;
    }
    private void ModifyVertexes(VertexHelper vh)
    {
        List<UIVertex> _UIVertexes = new List<UIVertex>(vh.currentVertCount);
        vh.GetUIVertexStream(_UIVertexes);
        vh.Clear();

        /* 由于一个Unity里面的字，是由两个三角形组成的正方形，每个三角形各拥有3个顶点，
         * 那么一个字就拥有左三角形的3个顶点和右三角形的3个顶点，那么一个字的UIVertex个数就是6个，
         * 所以这里 i+=6。
         */
        for (int i = 0; i < _UIVertexes.Count; i += 6)
        {
            UIVertex _topLeftVertex = ModifyColor(_UIVertexes[i + 0], topColor);
            UIVertex _topRightVertex = ModifyColor(_UIVertexes[i + 1], topColor);
            UIVertex _bottomLeftVertex = ModifyColor(_UIVertexes[i + 4], bottomColor);
            UIVertex _bottomRightVertex = ModifyColor(_UIVertexes[i + 3], bottomColor);
            UIVertex _cneterLeftVertex = CalcCenterVertex(_UIVertexes[i + 0], _UIVertexes[i + 4]);
            UIVertex _centerRightVertex = CalcCenterVertex(_UIVertexes[i + 1], _UIVertexes[i + 2]);

            vh.AddVert(_topLeftVertex);
            vh.AddVert(_topRightVertex);
            vh.AddVert(_centerRightVertex);
            vh.AddVert(_centerRightVertex);
            vh.AddVert(_cneterLeftVertex);
            vh.AddVert(_topLeftVertex);

            vh.AddVert(_cneterLeftVertex);
            vh.AddVert(_centerRightVertex);
            vh.AddVert(_bottomRightVertex);
            vh.AddVert(_bottomRightVertex);
            vh.AddVert(_bottomLeftVertex);
            vh.AddVert(_cneterLeftVertex);
        }

        //添加三角形
        /* 由于一个Unity里面的字，是由两个三角形组成的正方形，每个三角形各拥有3个顶点，
         * 那么一个字就拥有左三角形的3个顶点和右三角形的3个顶点，那么一个字的UIVertex个数就是6个，
         * 为了显示三种颜色，上面对Mesh进行了重绘，每个字用四个三角形显示，也就是12个顶点组成，所以这里 i+=12。
         */
        for (int i = 0; i < vh.currentVertCount; i += 12)
        {
            vh.AddTriangle(i + 0, i + 1, i + 2);
            vh.AddTriangle(i + 3, i + 4, i + 5);
            vh.AddTriangle(i + 6, i + 7, i + 8);
            vh.AddTriangle(i + 9, i + 10, i + 11);
        }
    }
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!this.IsActive())
        {
            return;
        }
        ModifyVertexes(vh);
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
    public Color CenterColor
    {
        get { return centerColor; }
        set { centerColor = value; }
    }
}