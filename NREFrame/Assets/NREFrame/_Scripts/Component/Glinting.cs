/*
 * 基于自发光Shader实现的一个高亮闪烁组件。（有一定局限性）
 */
using System.Collections;
using UnityEngine;

public class Glinting : MonoBehaviour
{
    #region --变量定义
    private readonly string _keyword = "_EMISSION";//与Shader中的变量名称对应
    private readonly string _colorName = "_EmissionColor";//与Shader中的变量名称对应

    [Tooltip("勾选此项则启动时自动开始闪烁")]
    [SerializeField] private bool autoStart = false;//是否自动闪烁

    [SerializeField] private Color color = Color.yellow;//闪烁颜色

    [Tooltip("最低发光亮度，需小于最高发光亮度")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float minBrightness = 0.0f;//最低发光亮度，取值范围[0,1]，需小于最高发光亮度。
    [Tooltip("最高发光亮度，需大于最低发光亮度")]
    [Range(0.0f, 1)]
    [SerializeField] private float maxBrightness = 0.5f; //最高发光亮度，取值范围[0, 1]，需大于最低发光亮度。
    [Range(0.2f, 30.0f)]
    [SerializeField] private float rate = 1;//闪烁频率，取值范围[0.2,30.0]。

    private float h, s, v;//色调，饱和度，亮度
    private float deltaBrightness;//最低最高亮度差
    private Material glintingMaterial;//闪烁材质球

    private Coroutine glinting;//闪烁协程
    #endregion

    #region --系统函数
    private void Start()
    {
        glintingMaterial = this.GetComponent<Renderer>().material;
        if (autoStart)
        {
            StartGlinting();
        }
    }
    /// <summary>
    /// 校验数据，并保证运行时的修改能够得到应用。
    /// 该方法只在编辑器模式中生效！！！
    /// </summary>
    private void OnValidate()
    {
        // 限制亮度范围
        if (minBrightness < 0 || minBrightness > 1)
        {
            minBrightness = 0.0f;
            Debug.LogError("最低亮度超出取值范围[0, 1]，已重置为0。");
        }
        if (maxBrightness < 0 || maxBrightness > 1)
        {
            maxBrightness = 1.0f;
            Debug.LogError("最高亮度超出取值范围[0, 1]，已重置为1。");
        }
        if (minBrightness >= maxBrightness)
        {
            minBrightness = 0.0f;
            maxBrightness = 1.0f;
            Debug.LogError("最低亮度[MinBrightness]必须低于最高亮度[MaxBrightness]，已分别重置为0/1！");
        }

        // 限制闪烁频率
        if (rate < 0.2f || rate > 30.0f)
        {
            rate = 1;
            Debug.LogError("闪烁频率超出取值范围[0.2, 30.0]，已重置为1.0。");
        }

        // 更新亮度差
        deltaBrightness = maxBrightness - minBrightness;

        // 更新颜色
        // 注意不能使用 _v ，否则在运行时修改参数会导致亮度突变
        float tempV = 0;
        Color.RGBToHSV(color, out h, out s, out tempV);
    }
    #endregion

    #region --自定义函数
    /// <summary>
    /// 开始闪烁。
    /// </summary>
    public void StartGlinting()
    {
        glintingMaterial.EnableKeyword(_keyword);

        if (glinting != null)
        {
            StopCoroutine(glinting);
        }
        glinting = StartCoroutine(IEGlinting());
    }
    /// <summary>
    /// 停止闪烁。
    /// </summary>
    public void StopGlinting()
    {
        glintingMaterial.DisableKeyword(_keyword);

        if (glinting != null)
        {
            StopCoroutine(glinting);
        }
    }
    /// <summary>
    /// 控制自发光强度。
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEGlinting()
    {
        Color.RGBToHSV(color, out h, out s, out v);
        v = minBrightness;
        deltaBrightness = maxBrightness - minBrightness;

        bool increase = true;
        while (true)
        {
            if (increase)
            {
                v += deltaBrightness * Time.deltaTime * rate;
                increase = v <= maxBrightness;
            }
            else
            {
                v -= deltaBrightness * Time.deltaTime * rate;
                increase = v <= minBrightness;
            }
            glintingMaterial.SetColor(_colorName, Color.HSVToRGB(h, s, v));
            yield return null;
        }
    }
    #endregion
}