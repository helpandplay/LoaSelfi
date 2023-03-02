namespace LoaSelfi.Test;

public class FinderUnitTest
{
    [Fact]
    public void FindLostArkFolderPathTest()
    {
        string myLOSTARKFolderPath = @"C:\Program Files (x86)\Smilegate\Games\LOSTARK\EFGame\Screenshots";
        var getfolderPathTask = Service.Finder.Instance.GetFolderPathTask();

        getfolderPathTask.Start();
        getfolderPathTask.Wait();

        Assert.Equal(myLOSTARKFolderPath, getfolderPathTask.Result);
    }
}