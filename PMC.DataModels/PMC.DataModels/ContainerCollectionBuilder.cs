using PMC.DataModels.DataModels;

namespace PMC.DataModels
{
    public abstract class ContainerCollectionBuilder<T>
    {
        public abstract Containers<T> Create();
    }

    public class IntContainers : ContainerCollectionBuilder<int>
    {
        public IntContainers() { }

        public override Containers<int> Create()
        {
            return new Containers<int>();
        }
    }

    public class DoubleContainers : ContainerCollectionBuilder<double>
    {
        public DoubleContainers() { }

        public override Containers<double> Create()
        {
            return new Containers<double>();
        }
    }

    public class DecimalContainers : ContainerCollectionBuilder<decimal>
    {
        public DecimalContainers() { }

        public override Containers<decimal> Create()
        {
            return new Containers<decimal>();
        }
    }
}
