using System;
using System.Collections.Generic;
using System.Text;
/* =====================================
 * Author: 朱乙
 * Blog: http://blog.csdn.net/sq_zhuyi
 * Email: sqzhuyi@gmail.com
======================================== */
namespace SudokuResponder
{
    public class SudokuService
    {
        Number[][][][] pArr = null;//存储数独数字

        public SudokuService(string numStr)
        {
            string s = numStr.Replace(",", "");
            if (s.Length != 81)
            {
                throw new Exception("初始数独错误，必须是81个数字");
            }
            int[] numArr = new int[81];
            for (int i = 0; i < s.Length; i++)
            {
                numArr[i] = Convert.ToInt32(s[i].ToString());
            }
            pArr = GetArray(numArr);
        }
        public SudokuService(int[] numArr)
        {
            if (numArr.Length != 81)
            {
                throw new Exception("初始数独错误，必须是81个数字");
            }
            pArr = GetArray(numArr);
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public string GetResult()
        {
            FillArray();

            StringBuilder sb = new StringBuilder();
            for (int m = 0; m < 3; m++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            sb.Append(pArr[m][n][i][j].Value);
                        }
                    }
                    sb.Append(",");
                }
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 打印结果
        /// </summary>
        public void PrintResult()
        {
            for (int m = 0; m < 3; m++)
            {
                if (m == 0) Console.WriteLine("".PadLeft(25, '-'));
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        if (n == 0) Console.Write("| ");
                        for (int j = 0; j < 3; j++)
                        {
                            var pv = pArr[m][n][i][j];
                            if (pv.Default) Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("{0} ", pv.Value);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write("| ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("".PadLeft(25, '-'));
            }
        }

        private Number[][][][] GetArray(int[] defVal)
        {
            Number[][][][] pArr = new Number[3][][][];
            for (int m = 0; m < 3; m++)
            {
                pArr[m] = new Number[3][][];
                for (int n = 0; n < 3; n++)
                {
                    pArr[m][n] = new Number[3][];
                    for (int i = 0; i < 3; i++)
                    {
                        pArr[m][n][i] = new Number[3];
                        for (int j = 0; j < 3; j++)
                        {
                            pArr[m][n][i][j] = new Number() { Value = 0, Default = false };
                        }
                    }
                }
            }

            int c = 0;
            for (int m = 0; m < 3; m++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int val = defVal[c++];
                            pArr[m][n][i][j].Value = val;
                            if (val > 0)
                                pArr[m][n][i][j].Default = true;
                        }
                    }
                }
            }

            return pArr;
        }
        private void FillArray()
        {
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (pArr[m][n][i][j].Default == false)
                            {//空白格
                                var pv = GetNumber(new int[4] { m, n, i, j });

                                if (!pv.Ok)
                                {
                                    m = pv.Position[0];
                                    n = pv.Position[1];
                                    i = pv.Position[2];
                                    j = pv.Position[3];
                                    ClearAfter(pv.Position);

                                }
                                pArr[m][n][i][j].Value = pv.Value;
                            }
                        }
                    }//end small
                }
            }//end big
        }
        //清空该位置后边的空格
        private void ClearAfter(int[] pos)
        {
            int min = pos[0] * 1000 + pos[1] * 100 + pos[2] * 10 + pos[3];
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (m * 1000 + n * 100 + i * 10 + j > min)
                            {
                                if (pArr[m][n][i][j].Default == false)
                                {
                                    pArr[m][n][i][j].Value = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        //记录每个位置的可能数字
        private List<PosValues> posVals = new List<PosValues>();

        //获取当前位置的可能数字
        private PosValues GetNumber(int[] pos)
        {
            List<int> nums = new List<int>();
            for (int n = 1; n <= 9; n++)
            {
                nums.Add(n);
            }
            //3宫格内
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int a = pArr[pos[0]][pos[1]][i][j].Value;
                    if (a > 0 && nums.Contains(a))
                    {
                        nums.Remove(a);
                    }
                }
            }
            //横向
            for (int n = 0; n < 3; n++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int a = pArr[pos[0]][n][pos[2]][j].Value;
                    if (a > 0 && nums.Contains(a))
                    {
                        nums.Remove(a);
                    }
                }
            }
            //纵向
            for (int m = 0; m < 3; m++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int a = pArr[m][pos[1]][i][pos[3]].Value;
                    if (a > 0 && nums.Contains(a))
                    {
                        nums.Remove(a);
                    }
                }
            }

            if (nums.Count == 0)
            {
                if (posVals.Count == 0)
                {
                    return new PosValues()
                    {
                        Value = 0
                    };
                }
                var pv = posVals[posVals.Count - 1];

                pv.Ok = false;
                pv.Value = pv.Values[0];
                pv.Values.Remove(pv.Value);
                if (pv.Values.Count == 0)
                {
                    posVals.Remove(pv);
                }
                return pv;
            }
            else
            {
                var pv = new PosValues();
                pv.Position = pos;
                pv.Value = nums[0];
                nums.Remove(pv.Value);
                pv.Values = nums;
                if (nums.Count > 0)
                {
                    posVals.Add(pv);
                }
                return pv;
            }
        }
    }
    public struct Number
    {
        public int Value;
        public bool Default;
    }
    public class PosValues
    {
        public int[] Position;

        public List<int> Values;

        public bool Ok = true;

        public int Value;
    }
}
