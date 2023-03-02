namespace LoaSelfi.Test;
public class FileLoaderUnitTest
{
    [Fact]
    public void GetImagesTest()
    {
        string myLOSTARKFolderPath = @"C:\Program Files (x86)\Smilegate\Games\LOSTARK\EFGame\Screenshots";
        var fileLoader = new Service.FileLoader();

        var getImagesTask = fileLoader.LoadImagesAsync(myLOSTARKFolderPath);

        getImagesTask.Start();
        getImagesTask.Wait();


        Assert.Equal(117, getImagesTask.Result.Count);
    }
}
