namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class MatrixModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }

        public MatrixModel(int id, int positionId)
        {
            Id = id;
            PositionId = positionId;
        }
    }
}
