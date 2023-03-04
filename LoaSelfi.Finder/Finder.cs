using System.Drawing;
using System.IO;
using System.Linq;

namespace LoaSelfi.Service;
public class Finder
{
    private static Finder? finder = null;
    private readonly string _smilegateFolderName = "Smilegate";
    private readonly string _programX86FolderName = "Program Files (x86)";
    private readonly string _lostArkScreenShotsFolderPath = "\\Games\\LOSTARK\\EFGame\\Screenshots";
    private Finder()
    {

    }

    public static Finder Instance
    {
        get
        {
            if(finder == null)
            {
                finder = new Finder();
            }
            return finder;
        }
    }

    public string GetFolderPathAllSerachDrives()
    {
        string lostArkScreenShotFolderPath = string.Empty;

        DriveInfo[] driveInfo = DriveInfo.GetDrives();

        if(driveInfo == null) return lostArkScreenShotFolderPath;

        string currentFolderPath = string.Empty;
        foreach(DriveInfo drive in driveInfo)
        {
            currentFolderPath = drive.Name;

            if(!ExistFolderPath(currentFolderPath))
            {
                return lostArkScreenShotFolderPath;
            }

            string[] rootDirectorys = Directory.GetDirectories(currentFolderPath);
            string? programX86Folder = rootDirectorys.FirstOrDefault(folderName => folderName.Equals($"{currentFolderPath}{_programX86FolderName}"));

            if(!ExistFolderPath(programX86Folder))
            {
                return lostArkScreenShotFolderPath;
            }

            string[] programX86Directorys = Directory.GetDirectories(programX86Folder);
            string? smilegateFolder = programX86Directorys.FirstOrDefault(folderName => folderName.Equals($"{programX86Folder}\\{_smilegateFolderName}"));

            if(!ExistFolderPath(smilegateFolder))
            {
                return lostArkScreenShotFolderPath;
            }

            if(ExistFolderPath(smilegateFolder + _lostArkScreenShotsFolderPath))
            {
                lostArkScreenShotFolderPath = smilegateFolder + _lostArkScreenShotsFolderPath;
                return lostArkScreenShotFolderPath;
            }
        }

        return lostArkScreenShotFolderPath;
    }

    private static bool ExistFolderPath(string? folderPath)
    {
        if(string.IsNullOrEmpty(folderPath))
        {
            return false;
        }

        return Directory.Exists(folderPath);
    }

    public static bool ExistFile(string filePath)
    {
        if(string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        return File.Exists(filePath);
    }

    public static Image? GetImage(string filePath)
    {
        if(!ExistFile(filePath))
        {
            return null;
        }

        return Image.FromFile(filePath);
    }
}
