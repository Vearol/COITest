namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class ContainerModel
    {
        public int Id { get; set; }
        public int MatrixId { get; set; }

        public ContainerModel(int id, int matrixId)
        {
            Id = id;
            MatrixId = matrixId;
        }
    }
}
