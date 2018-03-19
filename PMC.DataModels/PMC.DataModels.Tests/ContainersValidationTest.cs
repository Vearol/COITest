using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.DataModels.DataModels;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.Tests
{
    [TestClass]
    public class ContainersValidationTest
    {
        [TestMethod]
        public void NumberOfMatricesTest()
        {
            var simpleTestPoints1D = new int[] { 1, 2, 3, 4, 5};
            var testPoints1D = simpleTestPoints1D.Select(x => new Point<int>(x)).ToArray();

            var testPosition1D = new Position<int>(testPoints1D, Dimension.D1);

            var positionArr = new Position<int>[] { testPosition1D };
            var testMatrix1D = new Matrix<int>(positionArr);

            var matrixArr1 = new Matrix<int>[] { testMatrix1D, testMatrix1D };
            var container1 = new Container<int>(matrixArr1);

            var matrixArr2 = new Matrix<int>[] { testMatrix1D };
            var container2 = new Container<int>(matrixArr2);

            var containerArr = new Container<int>[] { container1, container2 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerArr); },
                CustomErrorCode.IncorrectNumberOfMarices);
        }

        [TestMethod]
        public void NumberOfPositionsTest()
        {
            var simpleTestPoints1D = new int[] { 1, 2, 3, 4, 5 };
            var testPoints1D = simpleTestPoints1D.Select(x => new Point<int>(x)).ToArray();

            var testPosition1D = new Position<int> (testPoints1D, Dimension.D1);

            var positionArr1 = new Position<int>[] { testPosition1D, testPosition1D };
            var positionArr2 = new Position<int>[] { testPosition1D };

            var testMatrix1D1 = new Matrix<int> (positionArr1);
            var testMatrix1D2 = new Matrix<int> (positionArr2);

            var matrixArr1 = new Matrix<int>[] { testMatrix1D1, testMatrix1D2 };
            var container1 = new Container<int>(matrixArr1);

            var containerArr = new Container<int>[] { container1 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerArr); },
                CustomErrorCode.IncorrectNumberOfPositions);
        }

        [TestMethod]
        public void MatricesTypesTest()
        {
            var simpleTestPoints1D = new int[] { 1, 2, 3, 4, 5 };

            var testPoints1D = simpleTestPoints1D.Select(x => new Point<int>(x)).ToArray();
            var testPoints2D = simpleTestPoints1D.Select(x => new Point<int>(x, x)).ToArray();

            var testPosition1D = new Position<int>(testPoints1D, Dimension.D1);
            var testPosition2D = new Position<int>(testPoints2D, Dimension.D2);

            var positionArr1 = new Position<int>[] { testPosition1D };
            var positionArr2 = new Position<int>[] { testPosition2D };

            var testMatrix1D1 = new Matrix<int>(positionArr1);
            var testMatrix1D2 = new Matrix<int>(positionArr2);

            var matrixArr1 = new Matrix<int>[] { testMatrix1D1, testMatrix1D2 };
            var matrixArr2 = new Matrix<int>[] { testMatrix1D2, testMatrix1D1 };

            var container1 = new Container<int>(matrixArr1);
            var container2 = new Container<int>(matrixArr2);

            var containerArr = new Container<int>[] { container1, container2 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerArr); },
                CustomErrorCode.IncorrectTypesOfMatrices);
        }

        [TestMethod]
        public void NumberOf3DPointsTest()
        {
            var simpleTestPoints3D1 = new int[] { 1, 2, 3, 4, 5 };
            var simpleTestPoints3D2 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            var testPoints3D1 = simpleTestPoints3D1.Select(x => new Point<int>(x, x, x)).ToArray();
            var testPoints3D2 = simpleTestPoints3D2.Select(x => new Point<int>(x, x, x)).ToArray();

            var testPosition3D1 = new Position<int>(testPoints3D1, Dimension.D3);
            var testPosition3D2 = new Position<int>(testPoints3D2, Dimension.D3);

            var positionArr = new Position<int>[] { testPosition3D1, testPosition3D2 };

            var testMatrix3D = new Matrix<int>(positionArr);

            var matrixArr = new Matrix<int>[] { testMatrix3D };
            var container = new Container<int>(matrixArr);

            var containerArr = new Container<int>[] { container };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerArr); },
                CustomErrorCode.IncorrectNumberOf3DPoints);
        }
    }
}
