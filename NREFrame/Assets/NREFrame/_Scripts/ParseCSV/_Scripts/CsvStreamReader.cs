/*
 * 使用文件流进行读取。（只是读取方式不一样，核心内容一致）
 */
using System;
using System.Collections;
using System.IO;
using System.Text;

public class CsvStreamReader
{
    private ArrayList rowAL;       //行链表,CSV文件的每一行就是一个链
    private string fileName;       //文件名
    private Encoding encoding;     //编码

    /// <summary>
    ///
    /// </summary>
    /// <param name="_fileName">文件名,包括文件路径</param>
    public CsvStreamReader(string _fileName)
    {
        this.rowAL = new ArrayList();
        this.fileName = _fileName;
        this.encoding = Encoding.Default;
        LoadCsvFile();
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="_fileName">文件名,包括文件路径</param>
    /// <param name="_encoding">文件编码</param>
    public CsvStreamReader(string _fileName, Encoding _encoding)
    {
        this.rowAL = new ArrayList();
        this.fileName = _fileName;
        this.encoding = _encoding;
        LoadCsvFile();
    }
    /// <summary>
    /// 文件名,包括文件路径
    /// </summary>
    public string FileName
    {
        set
        {
            this.fileName = value;
            LoadCsvFile();
        }
    }
    /// <summary>
    /// 文件编码
    /// </summary>
    public Encoding FileEncoding
    {
        set
        {
            this.encoding = value;
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
                ArrayList _colAL = (ArrayList)this.rowAL[i];    //rowAL中的某个元素依然是ArrayList类型
                _maxCol = (_maxCol > _colAL.Count) ? _maxCol : _colAL.Count;     //取最大值
            }
            return _maxCol;
        }
    }
    /// <summary>
    /// 获取某行某列的数据
    /// row:行,row = 1代表第一行
    /// col:列,col = 1代表第一列  
    /// </summary>
    public string this[int _row, int _col]
    {
        get
        {
            //数据有效性验证
            CheckRowValid(_row);
            CheckColValid(_col);
            ArrayList _colAL = (ArrayList)this.rowAL[_row - 1];

            //如果请求列数据大于当前行的列时,返回空值
            if (_colAL.Count < _col)
            {
                return "";
            }

            return _colAL[_col - 1].ToString();
        }
    }

    /// <summary>
    /// 检查行数是否是有效的
    /// </summary> 
    private void CheckRowValid(int _row)
    {
        if (_row <= 0)
        {
            throw new Exception("行数不能小于0");
        }
        if (_row > RowCount)
        {
            throw new Exception("没有当前行的数据");
        }
    }
    /// <summary>
    /// 检查最大行数是否是有效的
    /// </summary> 
    private void CheckMaxRowValid(int _maxRow)
    {
        if (_maxRow <= 0 && _maxRow != -1)
        {
            throw new Exception("行数不能等于0或小于-1");
        }
        if (_maxRow > RowCount)
        {
            throw new Exception("没有当前行的数据");
        }
    }
    /// <summary>
    /// 检查列数是否是有效的
    /// </summary>
    /// <param name="_col"></param>  
    private void CheckColValid(int _col)
    {
        if (_col <= 0)
        {
            throw new Exception("列数不能小于0");
        }
        if (_col > ColCount)
        {
            throw new Exception("没有当前列的数据");
        }
    }
    /// <summary>
    /// 检查检查最大列数是否是有效的
    /// </summary>  
    private void CheckMaxColValid(int _maxCol)
    {
        if (_maxCol <= 0 && _maxCol != -1)
        {
            throw new Exception("列数不能等于0或小于-1");
        }
        if (_maxCol > ColCount)
        {
            throw new Exception("没有当前列的数据");
        }
    }

    /// <summary>
    /// 载入CSV文件
    /// </summary>
    private void LoadCsvFile()
    {
        //对数据的有效性进行验证
        if (this.fileName == null)
        {
            throw new Exception("请指定要载入的CSV文件名");
        }
        else if (!File.Exists(this.fileName))
        {
            throw new Exception("指定的CSV文件不存在");
        }
        if (this.encoding == null)
        {
            this.encoding = Encoding.Default;
        }

        StreamReader _sr = new StreamReader(this.fileName, this.encoding);
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
            throw new Exception("CSV文件的格式有错误");
        }
    }

    /// <summary>
    /// 获取两个连续引号变成单个引号的数据行
    /// </summary>
    /// <param name="_fileDataLine">文件数据行</param>
    /// <returns></returns>
    private string GetDeleteQuotaDataLine(string _fileDataLine)
    {
        return _fileDataLine.Replace("\"\"", "\"");
    }
    /// <summary>
    /// 判断字符串是否包含奇数个引号
    /// </summary>
    /// <param name="_dataLine">数据行</param>
    /// <returns>为奇数时，返回为真；否则返回为假</returns>
    private bool IfOddQuota(string _dataLine)
    {
        int _quotaCount = 0;
        bool _oddQuota = false;

        for (int i = 0; i < _dataLine.Length; i++)
        {
            if (_dataLine[i] == '\"')
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
    /// <param name="_dataCell"></param>
    /// <returns></returns>
    private bool IfOddStartQuota(string _dataCell)
    {
        int _quotaCount = 0;
        bool _oddQuota = false;

        for (int i = 0; i < _dataCell.Length; i++)
        {
            if (_dataCell[i] == '\"')
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
    /// <param name="_dataCell"></param>
    /// <returns></returns>
    private bool IfOddEndQuota(string _dataCell)
    {
        int _quotaCount = 0;
        bool _oddQuota = false;

        for (int i = _dataCell.Length - 1; i >= 0; i--)
        {
            if (_dataCell[i] == '\"')
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
    /// 加入新的数据行
    /// </summary>
    /// <param name="_newDataLine">新的数据行</param>
    private void AddNewDataLine(string _newDataLine)
    {
        ArrayList _colAL = new ArrayList();
        string[] _dataArray = _newDataLine.Split(',');
        bool _oddStartQuota = false;       //是否以奇数个引号开始
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
            throw new Exception("数据格式有问题");
        }
        this.rowAL.Add(_colAL);
    }
    /// <summary>
    /// 去掉格子的首尾引号，把双引号变成单引号
    /// </summary>
    /// <param name="_fileCellData"></param>
    /// <returns></returns>
    private string GetHandleData(string _fileCellData)
    {
        if (_fileCellData == "")
        {
            return "";
        }
        if (IfOddStartQuota(_fileCellData))
        {
            if (IfOddEndQuota(_fileCellData))
            {
                return _fileCellData.Substring(1, _fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
            }
            else
            {
                throw new Exception("数据引号无法匹配" + _fileCellData);
            }
        }
        else
        {
            //考虑形如""    """"      """"""   
            if (_fileCellData.Length > 2 && _fileCellData[0] == '\"')
            {
                _fileCellData = _fileCellData.Substring(1, _fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
            }
        }
        return _fileCellData;
    }
}