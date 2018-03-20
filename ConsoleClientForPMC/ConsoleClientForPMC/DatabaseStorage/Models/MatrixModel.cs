namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class MatrixModel
    {
        public int Id { get; set; }
        public byte DataType { get; set; }

        public MatrixModel(int id, byte dataType)
        {
            Id = id;
            DataType = dataType;
        }
    }

    public class MatrixPositionModel
    {
        public int MatrixId { get; set; }
        public int PositionId { get; set; }

        public MatrixPositionModel(int id, int positionId)
        {
            MatrixId = id;
            PositionId = positionId;
        }
    }
}
