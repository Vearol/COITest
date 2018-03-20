namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class ContainerModel
    {
        public int Id { get; set; }
        public byte DataType { get; set; }

        public ContainerModel(int id, byte dataType)
        {
            Id = id;
            DataType = dataType;
        }
    }

    public class ContainerMatrixModel
    {
        public int ContainerId { get; set; }
        public int MatrixId { get; set; }

        public ContainerMatrixModel(int id, int matrixId)
        {
            ContainerId = id;
            MatrixId = matrixId;
        }
    }
}
