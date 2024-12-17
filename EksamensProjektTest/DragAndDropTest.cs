using EksamensProjekt.Services;

[TestClass]
public class DragAndDropServiceTests
{
    private DragAndDropService dragAndDropService;

    [TestInitialize]
    public void Setup()
    {
        dragAndDropService = new DragAndDropService();
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    [DeploymentItem("Resources/Files", "Resources")]
    public void ValidateFileFormat_InvalidFileFormat_ThrowsFormatException()
    {
        // Arrange: Path to the invalid file
        var filePath = Path.Combine("Resources", "FileWithInvalidFormat.ods");
        var files = new string[] { filePath }; // Wrap filePath into an array

        // Act: Call the validation logic
        dragAndDropService.ValidateFileFormat(files);

        // Assert is handled by ExpectedException
    }

    [TestMethod]
    public void ValidateFileFormat_ValidFileFormat_DoesNotThrow()
    {
        // Arrange: Path to a valid file with .xlsx
        var validFilePath = Path.Combine("Resources", "ValidFile.xlsx");
        var files = new string[] { validFilePath }; // Wrap filePath into an array

        // Act & Assert: No exception thrown
        dragAndDropService.ValidateFileFormat(files);
    }
}
