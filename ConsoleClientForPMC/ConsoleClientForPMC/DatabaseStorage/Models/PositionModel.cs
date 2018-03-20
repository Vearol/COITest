namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class PositionModel
    {
        public int Id { get; set; }
        public int PointId { get; set; }

        public PositionModel(int id, int pointId)
        {
            Id = id;
            PointId = pointId;
        }
    }
}
