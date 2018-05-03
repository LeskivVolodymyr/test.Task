using System;
using System.Data;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class MatrixService : IMatrixService
    {
        public short[][] GenerateMatrix(int size = 0)
        {
            var rnd = new Random();

            if (size == default(int))
            {
                size = rnd.Next(3, 16); // from 3 to 15
            }

            short[][] arr = new short[size][];

            for (var i = 0; i < size; i++)
            {
                arr[i] = new short[size];
                for (var j = 0; j < size; j++)
                    arr[i][j] = (short)rnd.Next(0, Int16.MaxValue);
            }

            return arr;
        }

        public short[][] TurnMatrix(short[][] arr)
        {
            var size = arr.GetLength(0);
            short temp;

            for (int i = 0; i < size / 2; i++)
            {
                for (int j = i; j < size - 1 - i; j++)
                {
                    temp = arr[i][j];
                    arr[i][j] = arr[size - j - 1][i];
                    arr[size - j - 1][i] = arr[size - i - 1][size - j - 1];
                    arr[size - i - 1][size - j - 1] = arr[j][size - i - 1];
                    arr[j][size - i - 1] = temp;
                }
            }

            return arr;
        }

        public short[][] GetArrayFromTable(DataTable csvTable)
        {
            var result = new short[csvTable.Rows.Count][];
            char sp = ',';

            for (var i = 0; i < csvTable.Rows.Count; i++)
            {
                var rowStr = (string)csvTable.Rows[i].ItemArray[0];
                if (rowStr.IndexOf(";") >= 0)
                    sp = ';';
                var splited = rowStr.Split(sp);
                result[i] = new short[splited.Length];

                for (var s = 0; s < splited.Length - 1; s++)
                {
                    short currItem = 0;
                    var converted = Int16.TryParse(splited[s], out currItem);
                    if (converted)
                        result[i][s] = currItem;
                }
            }

            return result;
        }

    }
}
