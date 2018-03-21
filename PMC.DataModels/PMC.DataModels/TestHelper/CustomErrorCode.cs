namespace PMC.DataModels.TestHelper
{
    /// <summary>
    /// Custom error codes for custom exceptions. Usefull in unit tests.
    /// </summary>
    public enum CustomErrorCode : byte
    {
        IncorrectNumberOfMarices = 0,
        IncorrectNumberOfPositions = 1,
        IncorrectTypesOfMatrices = 2,
        IncorrectNumberOf3DPoints = 3,
        WrongDataType = 4,
        MissingDataInDatabase = 5,
        IncorrectTypeOfSomePoints = 6,
        IncorrectTypeOfSomePositions = 7,
        IncorrectDataTypeInContainers = 8,
    }
}
    