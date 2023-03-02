using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LoaSelfi.Define;
using LoaSelfi.Model;

namespace LoaSelfi.Service;
public class FileLoader
{
    private readonly string _lostarkScreenShotExtension = ".jpg";
    public FileLoader()
    {

    }
    public static string[] GetFiles(string directoryPath)
    {
        if(string.IsNullOrEmpty(directoryPath) ||
           !Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"FileLoader constructor: {directoryPath} does not exist");
        }

        string[] files = Directory.GetFiles(directoryPath);

        return files;
    }
    public ImageInfo? LoadImageAsync(string fileName)
    {
        if(Path.GetExtension(fileName) != _lostarkScreenShotExtension)
        {
            return null;
        }

        Image? image = Finder.GetImage(fileName);
        var imageFileInfo = new FileInfo(fileName);

        if(image == null)
        {
            return null;
        }

        string name = Path.GetFileName(fileName);
        var fileNameSplit = name.Split('_');
        ImageType? imageType = null;
        DateTime createTime = imageFileInfo.CreationTime;

        if(fileNameSplit.Length < 3)
        {
            //ex) Screenshot_211014_232440
            //ex) Selfie_20220918_맹무_000
            return null;
        }

        if(fileNameSplit.Length == 3 &&
           fileNameSplit[0] != null &&
           fileNameSplit[0] == ImageType.Screenshot.ToString())
        {
            imageType = ImageType.Screenshot;
        }
        else if(fileNameSplit.Length == 4 &&
                fileNameSplit[0] != null &&
                fileNameSplit[0] == ImageType.Selfi.ToString())
        {
            imageType = ImageType.Selfi;
        }

        if(imageType == null)
        {
            return null;
        }

        var imageInfo = new ImageInfo()
        {
            Image = image,
            Name = Path.GetFileName(fileName),
            Type = (ImageType)imageType,
            CreateTime = createTime,
        };

        return imageInfo;
    }
    public Task<IList<ImageInfo>> LoadImagesAsync(string directoryPath)
    {
        return new Task<IList<ImageInfo>>(() =>
        {
            IList<ImageInfo> Images = new List<ImageInfo>();

            string[] files = GetFiles(directoryPath);

            foreach(string file in files)
            {
                var image = LoadImageAsync(file);

                if(image == null)
                {
                    continue;
                }

                Images.Add(image);
            }

            return Images;
        });
    }

    private DateTime ConvertStringToDateTime(string[] time)
    {
        int year = 2000 + int.Parse(time[0].Substring(0, 2));
        int month = int.Parse(time[0].Substring(2, 2));
        int day = int.Parse(time[0].Substring(4, 2));
        int hour = int.Parse(time[1].Substring(0, 2));
        int minute = int.Parse(time[1].Substring(2, 2));
        int second = int.Parse(time[1].Substring(4, 2));

        return new DateTime(year, month, day, hour, minute, second);
    }
}
