/*
 * 工具类：算法集合类。
*/
using System;
using UnityEngine;

public class Algorithm
{
    #region --编辑距离算法（Levenshtein Distance）。计算两个字符串的相似度。
    /// <summary>
    /// 比较两个字符串的相似度，并返回相似率。
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static float Levenshtein(string str1, string str2)
    {
        char[] char1 = str1.ToCharArray();
        char[] char2 = str2.ToCharArray();
        //计算两个字符串的长度。  
        int len1 = char1.Length;
        int len2 = char2.Length;
        //建二维数组，比字符长度大一个空间  
        int[,] dif = new int[len1 + 1, len2 + 1];
        //赋初值  
        for (int a = 0; a <= len1; a++)
        {
            dif[a, 0] = a;
        }
        for (int a = 0; a <= len2; a++)
        {
            dif[0, a] = a;
        }
        //计算两个字符是否一样，计算左上的值  
        int temp;
        for (int i = 1; i <= len1; i++)
        {
            for (int j = 1; j <= len2; j++)
            {
                if (char1[i - 1] == char2[j - 1])
                {
                    temp = 0;
                }
                else
                {
                    temp = 1;
                }
                //取三个值中最小的  
                dif[i, j] = Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1, dif[i - 1, j] + 1);
            }
        }
        //计算相似度  
        float similarity = 1 - (float)dif[len1, len2] / Math.Max(len1, len2);
        return similarity;
    }
    /// <summary>
    /// 求最小值
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    private static int Min(params int[] nums)
    {
        int min = int.MaxValue;
        foreach (int item in nums)
        {
            if (min > item)
            {
                min = item;
            }
        }
        return min;
    }
    #endregion

    #region --KMP算法，判断一个字符串是否包含另一个字符串。
    /// <summary>
    /// 判断 _mainStr 字符串是否包含 _modeStr 字符串
    /// </summary>
    /// <param name="_mainStr">主字符串</param>
    /// <param name="_modeStr">模板字符串</param>
    /// <returns></returns>
    public static bool ContainString(string _mainStr, string _modeStr)
    {
        int[] _next = GetNext(_modeStr);
        char[] _mainChars = _mainStr.ToCharArray();
        char[] _modeChars = _modeStr.ToCharArray();
        int _index = 0;
        for (int i = 0; i < _mainChars.Length; i++)
        {
            while (_index > 0 && _modeChars[_index] != _mainStr[i])
            {
                _index = _next[_index - 1];
            }
            if (_modeChars[_index] == _mainStr[i])
            {
                _index++;
            }
            if (_index == _modeChars.Length)
            {
                i = i - _modeChars.Length + 1;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    ///获取部分匹配表
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    private static int[] GetNext(string _str)
    {
        char[] _chars = _str.ToCharArray();
        int[] _next = new int[_chars.Length];
        _next[0] = 0;
        int _index = 0;
        for (int i = 1; i < _chars.Length; i++)
        {
            while (_index > 0 && _chars[i] != _chars[_index])
            {
                _index = _next[_index - 1];
            }
            if (_chars[i] == _chars[_index])
            {
                _index++;
            }
            _next[i] = _index;
        }
        return _next;
    }
    #endregion

    #region --正态分布（高斯分布、常态分布）概率模型
    /// <summary>
    /// 正态分布（高斯分布、常态分布）概率模型
    /// </summary>
    /// <param name="_x">随机变量</param>
    /// <param name="_μ">位置参数</param>
    /// <param name="_σ">尺度参数</param>
    /// <returns></returns>
    private static float NormalDistribution(float _x, float _μ, float _σ)
    {
        float _inverseSqrt2PI = 1 / Mathf.Sqrt(2 * Mathf.PI);
        float _powOfE = -(Mathf.Pow((_x - _μ), 2) / (2 * _σ * _σ));
        float _result = (_inverseSqrt2PI / _σ) * Mathf.Exp(_powOfE);
        return _result;
    }
    #endregion
}
