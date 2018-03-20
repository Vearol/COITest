namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class PositionModel
    {
        public int Id { get; set; }
        public byte DataType { get; set; }

        public PositionModel(int id, byte dataType)
        {
            Id = id;
            DataType = dataType;
        }
    }

    public class PositionPointsModel
    {
        public int PositionId { get; set; }
        public int PointId { get; set; }

        public PositionPointsModel(int id, int pointId)
        {
            PositionId = id;
            PointId = pointId;
        }
    }
}
