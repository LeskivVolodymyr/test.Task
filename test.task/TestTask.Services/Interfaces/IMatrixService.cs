using System.Data;

namespace TestTask.Services.Interfaces
{
    public interface IMatrixService
    {
        short[][] GenerateMatrix(int size = 0);
        short[][] TurnMatrix(short[][] arr);
        short[][] GetArrayFromTable(DataTable csvTable);
    }
}
