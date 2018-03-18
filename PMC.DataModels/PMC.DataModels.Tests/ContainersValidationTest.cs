using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.DataModels.DataModels;
using PMC.DataModels.Points;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.Tests
{
    [TestClass]
    public class ContainersValidationTest
    {
        [TestMethod]
        public void NumberOfMatricesTest()
        {
            var simpleTestPoints1D = new List<int> { 1, 2, 3, 4, 5};
            var testPoints1D = simpleTestPoints1D.Select(x => new OneDimensionPoint<int>(x)).ToList();

            var testPosition1D = new Position<int, OneDimensionPoint<int>>(testPoints1D);

            var positionList = new List<Position<int, OneDimensionPoint<int>>> { testPosition1D };
            var testMatrix1D = new Matrix<int, OneDimensionPoint<int>>(positionList);

            var matrixList1 = new List<IMatrix<int>> { testMatrix1D, testMatrix1D };
            var container1 = new Container<int>(matrixList1);

            var matrixList2 = new List<IMatrix<int>> { testMatrix1D };
            var container2 = new Container<int>(matrixList2);

            var containerList = new List<Container<int>>{ container1, container2 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerList); },
                CustomErrorCode.IncorrectNumberOfMarices);
        }

        [TestMethod]
        public void NumberOfPositionsTest()
        {
            var simpleTestPoints1D = new List<int> { 1, 2, 3, 4, 5 };
            var testPoints1D = simpleTestPoints1D.Select(x => new OneDimensionPoint<int>(x)).ToList();

            var testPosition1D = new Position<int, OneDimensionPoint<int>>(testPoints1D);

            var positionList1 = new List<Position<int, OneDimensionPoint<int>>> { testPosition1D, testPosition1D };
            var positionList2 = new List<Position<int, OneDimensionPoint<int>>> { testPosition1D };

            var testMatrix1D1 = new Matrix<int, OneDimensionPoint<int>>(positionList1);
            var testMatrix1D2 = new Matrix<int, OneDimensionPoint<int>>(positionList2);

            var matrixList1 = new List<IMatrix<int>> { testMatrix1D1, testMatrix1D2 };
            var container1 = new Container<int>(matrixList1);

            var containerList = new List<Container<int>> { container1 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerList); },
                CustomErrorCode.IncorrectNumberOfPositions);
        }

        [TestMethod]
        public void MatricesTypesTest()
        {
            var simpleTestPoints1D = new List<int> { 1, 2, 3, 4, 5 };

            var testPoints1D = simpleTestPoints1D.Select(x => new OneDimensionPoint<int>(x)).ToList();
            var testPoints2D = simpleTestPoints1D.Select(x => new TwoDimensionPoint<int>(x, x)).ToList();

            var testPosition1D = new Position<int, OneDimensionPoint<int>>(testPoints1D);
            var testPosition2D = new Position<int, TwoDimensionPoint<int>>(testPoints2D);

            var positionList1 = new List<Position<int, OneDimensionPoint<int>>> { testPosition1D };
            var positionList2 = new List<Position<int, TwoDimensionPoint<int>>> { testPosition2D };

            var testMatrix1D1 = new Matrix<int, OneDimensionPoint<int>>(positionList1);
            var testMatrix1D2 = new Matrix<int, TwoDimensionPoint<int>>(positionList2);

            var matrixList1 = new List<IMatrix<int>> { testMatrix1D1, testMatrix1D2 };
            var matrixList2 = new List<IMatrix<int>> { testMatrix1D2, testMatrix1D1 };

            var container1 = new Container<int>(matrixList1);
            var container2 = new Container<int>(matrixList2);

            var containerList = new List<Container<int>> { container1, container2 };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerList); },
                CustomErrorCode.IncorrectTypesOfMatrices);
        }

        [TestMethod]
        public void NumberOf3DPointsTest()
        {
            var simpleTestPoints3D1 = new List<int> { 1, 2, 3, 4, 5 };
            var simpleTestPoints3D2 = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            var testPoints3D1 = simpleTestPoints3D1.Select(x => new ThreeDimensionPoint<int>(x, x, x)).ToList();
            var testPoints3D2 = simpleTestPoints3D2.Select(x => new ThreeDimensionPoint<int>(x, x, x)).ToList();

            var testPosition3D1 = new Position<int, ThreeDimensionPoint<int>>(testPoints3D1);
            var testPosition3D2 = new Position<int, ThreeDimensionPoint<int>>(testPoints3D2);

            var positionList = new List<Position<int, ThreeDimensionPoint<int>>> { testPosition3D1, testPosition3D2 };

            var testMatrix3D = new Matrix<int, ThreeDimensionPoint<int>>(positionList);

            var matrixList = new List<IMatrix<int>> { testMatrix3D };
            var container = new Container<int>(matrixList);

            var containerList = new List<Container<int>> { container };

            CustomAsserts.AssertCustomException(delegate { Containers<int>.Validate(containerList); },
                CustomErrorCode.IncorrectNumberOf3DPoints);
        }
    }
}
