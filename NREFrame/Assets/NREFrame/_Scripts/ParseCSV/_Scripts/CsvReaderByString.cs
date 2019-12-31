/*
 * 根据字符串进行读取。（只是读取方式不一样，核心内容一致）
 */
using System.Collections;
using System.IO;
using UnityEngine;

public class CsvReaderByString
{
    private ArrayList rowAL;//行链表,CSV文件的每一行就是一个链

    public CsvReaderByString(string _parseString)
    {
        this.rowAL = new ArrayList();
        StringReader _sr = new StringReader(_parseString);
        string _csvDataLine = "";

        while (true)
        {
            string _fileDataLine = _sr.ReadLine();
            if (_fileDataLine == null)
            {
                break;
            }
            if (_csvDataLine == "")
            {
                _csvDataLine = _fileDataLine;
            }
            else
            {
                _csvDataLine += "/r/n" + _fileDataLine;
            }
            //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
            if (!IfOddQuota(_csvDataLine))
            {
                AddNewDataLine(_csvDataLine);
                _csvDataLine = "";
            }
        }
        _sr.Close();
        //数据行出现奇数个引号
        if (_csvDataLine.Length > 0)
        {
            throw new UnityException("CSV文件的格式有错误");
        }
    }

    /// <summary>
    /// 获取行数
    /// </summary>
    public int RowCount
    {
        get { return this.rowAL.Count; }
    }
    /// <summary>
    /// 获取列数
    /// </summary>
    public int ColCount
    {
        get
        {
            int _maxCol = 0;
            for (int i = 0; i < this.rowAL.Count; i++)
            {
                ArrayList _colAL = (ArrayList)this.rowAL[i];
                _maxCol = (_maxCol > _colAL.Count) ? _maxCol : _colAL.Count;
            }
            return _maxCol;
        }
    }

    /// <summary>
    /// 获取某行某列的数据
    /// row:行,row = 1代表第一行
    /// col:列,col = 1代表第一列  
    /// </summary>
    public string this[int row, int col]
    {
        get
        {
            //数据有效性验证
            CheckRowValid(row);
            CheckColValid(col);
            ArrayList colAL = (ArrayList)this.rowAL[row - 1];

            //如果请求列数据大于当前行的列时,返回空值
            if (colAL.Count < col)
            {
                return "";
            }
            return colAL[col - 1].ToString();
        }
    }

    /// <summary>
    /// 检查行数是否是有效的
    /// </summary>
    /// <param name="col"></param>  
    private void CheckRowValid(int row)
    {
        if (row <= 0)
        {
            throw new UnityException("行数不能小于0");
        }
        if (row > RowCount)
        {
            throw new UnityException("没有当前行的数据");
        }
    }
    /// <summary>
    /// 检查最大行数是否是有效的
    /// </summary>
    /// <param name="col"></param>  
    private void CheckMaxRowValid(int maxRow)
    {
        if (maxRow <= 0 && maxRow != -1)
        {
            throw new UnityException("行数不能等于0或小于-1");
        }
        if (maxRow > RowCount)
        {
            throw new UnityException("没有当前行的数据");
        }
    }
    /// <summary>
    /// 检查列数是否是有效的
    /// </summary>
    /// <param name="col"></param>  
    private void CheckColValid(int col)
    {
        if (col <= 0)
        {
            throw new UnityException("列数不能小于0");
        }
        if (col > ColCount)
        {
            throw new UnityException("没有当前列的数据");
        }
    }
    /// <summary>
    /// 检查检查最大列数是否是有效的
    /// </summary>
    /// <param name="col"></param>  
    private void CheckMaxColValid(int maxCol)
    {
        if (maxCol <= 0 && maxCol != -1)
        {
            throw new UnityException("列数不能等于0或小于-1");
        }
        if (maxCol > ColCount)
        {
            throw new UnityException("没有当前列的数据");
        }
    }

    /// <summary>
    /// 判断字符串是否包含奇数个引号
    /// </summary>
    /// <param name="dataLine">数据行</param>
    /// <returns>为奇数时，返回为真；否则返回为假</returns>
    private bool IfOddQuota(string dataLine)
    {
        int _quotaCount = 0;//引号个数
        bool _oddQuota = false;

        for (int i = 0; i < dataLine.Length; i++)
        {
            if (dataLine[i] == '\"')
            {
                _quotaCount++;
            }
        }

        if (_quotaCount % 2 == 1)
        {
            _oddQuota = true;
        }
        return _oddQuota;
    }
    /// <summary>
    /// 判断是否以奇数个引号开始
    /// </summary>
    /// <param name="dataCell"></param>
    /// <returns></returns>
    private bool IfOddStartQuota(string dataCell)
    {
        int _quotaCount = 0;
        bool _oddQuota = false;

        for (int i = 0; i < dataCell.Length; i++)
        {
            if (dataCell[i] == '\"')
            {
                _quotaCount++;
            }
            else
            {
                break;
            }
        }

        if (_quotaCount % 2 == 1)
        {
            _oddQuota = true;
        }

        return _oddQuota;
    }
    /// <summary>
    /// 判断是否以奇数个引号结尾
    /// </summary>
    /// <param name="dataCell"></param>
    /// <returns></returns>
    private bool IfOddEndQuota(string dataCell)
    {
        int _quotaCount = 0;
        bool _oddQuota = false;

        for (int i = dataCell.Length - 1; i >= 0; i--)
        {
            if (dataCell[i] == '\"')
            {
                _quotaCount++;
            }
            else
            {
                break;
            }
        }

        if (_quotaCount % 2 == 1)
        {
            _oddQuota = true;
        }
        return _oddQuota;
    }
    /// <summary>
    /// 去掉格子的首尾引号，把双引号变成单引号
    /// </summary>
    /// <param name="fileCellData"></param>
    /// <returns></returns>
    private string GetHandleData(string fileCellData)
    {
        if (fileCellData == "")
        {
            return "";
        }
        if (IfOddStartQuota(fileCellData))
        {
            if (IfOddEndQuota(fileCellData))
            {
                return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
            }
            else
            {
                throw new UnityException("数据引号无法匹配" + fileCellData);
            }
        }
        else
        {
            //考虑形如""    """"      """"""   
            if (fileCellData.Length > 2 && fileCellData[0] == '\"')
            {
                fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
            }
        }

        return fileCellData;
    }

    /// <summary>
    /// 加入新的数据行
    /// </summary>
    /// <param name="newDataLine">新的数据行</param>
    private void AddNewDataLine(string newDataLine)
    {
        ArrayList _colAL = new ArrayList();
        string[] _dataArray = newDataLine.Split(',');
        bool _oddStartQuota = false;//是否以奇数个引号开始
        string _cellData = "";

        for (int i = 0; i < _dataArray.Length; i++)
        {
            if (_oddStartQuota)
            {
                //因为前面用逗号分割,所以要加上逗号
                _cellData += "," + _dataArray[i];
                //是否以奇数个引号结尾
                if (IfOddEndQuota(_dataArray[i]))
                {
                    _colAL.Add(GetHandleData(_cellData));
                    _oddStartQuota = false;
                    continue;
                }
            }
            else
            {
                //是否以奇数个引号开始
                if (IfOddStartQuota(_dataArray[i]))
                {
                    //是否以奇数个引号结尾,不能是一个双引号,并且不是奇数个引号
                    if (IfOddEndQuota(_dataArray[i]) && _dataArray[i].Length > 2 && !IfOddQuota(_dataArray[i]))
                    {
                        _colAL.Add(GetHandleData(_dataArray[i]));
                        _oddStartQuota = false;
                        continue;
                    }
                    else
                    {
                        _oddStartQuota = true;
                        _cellData = _dataArray[i];
                        continue;
                    }
                }
                else
                {
                    _colAL.Add(GetHandleData(_dataArray[i]));
                }
            }
        }
        if (_oddStartQuota)
        {
            throw new UnityException("数据格式有问题");
        }
        this.rowAL.Add(_colAL);
    }
}