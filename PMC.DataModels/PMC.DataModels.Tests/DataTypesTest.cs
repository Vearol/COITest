using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.DataModels.DataModels;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.Tests
{
    [TestClass]
    public class DataTypesTest
    {
        private const byte testDataSize = 10;

        [TestMethod]
        public void CheckPointsInPositionTest()
        {
            var testPoints = new Point<int>[testDataSize];

            for (var i = 0; i < testDataSize; i++)
            {
                switch (i / 2)
                {
                    case 0:
                        testPoints[i] = new Point<int>(i);
                        break;
                    case 1:
                        testPoints[i] = new Point<int>(i, i);
                        break;
                }
            }

            CustomAsserts.AssertCustomException(delegate
                {
                    var position = new Position<int>(testPoints, Dimension.D1);
                },
                CustomErrorCode.IncorrectTypeOfSomePoints);
        }

        [TestMethod]
        public void CheckPositionsInMatrixTest()
        {
            var testPoints1D = new Point<int>[testDataSize];
            var testPoints2D = new Point<int>[testDataSize];

            for (var i = 0; i < testDataSize; i++)
            {
                testPoints1D[i] = new Point<int>(i);
                testPoints2D[i] = new Point<int>(i, i);
            }

            var position1D = new Position<int>(testPoints1D, Dimension.D1);
            var position2D = new Position<int>(testPoints2D, Dimension.D2);

            CustomAsserts.AssertCustomException(delegate
                {
                    var matrix = new Matrix<int>(new Position<int>[] { position1D, position2D });
                },
                CustomErrorCode.IncorrectTypeOfSomePositions);
        }
        
    }
}
