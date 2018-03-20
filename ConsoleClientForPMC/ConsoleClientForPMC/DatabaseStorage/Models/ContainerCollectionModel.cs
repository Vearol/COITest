namespace ConsoleClientForPMC.DatabaseStorage.Models
{
    public class ContainerCollectionModel
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }

        public ContainerCollectionModel(int id, int containerId)
        {
            Id = id;
            ContainerId = containerId;
        }
    }
}
