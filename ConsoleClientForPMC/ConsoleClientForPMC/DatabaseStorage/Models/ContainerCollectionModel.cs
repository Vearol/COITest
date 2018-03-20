namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class ContainerCollectionModel
    {
        public int Id { get; set; }
        public byte DataType { get; set; }

        public ContainerCollectionModel(int id, byte dataType)
        {
            Id = id;
            DataType = dataType;
        }
    }

    public class ContainerCollectionContainerModel
    {
        public int ContainerCollectionId { get; set; }
        public int ContainerId { get; set; }

        public ContainerCollectionContainerModel(int id, int containerId)
        {
            ContainerCollectionId = id;
            ContainerId = containerId;
        }
    }
}
