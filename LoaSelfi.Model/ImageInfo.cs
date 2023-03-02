using System;
using System.Drawing;
using LoaSelfi.Define;

namespace LoaSelfi.Model;
public class ImageInfo
{
    public Image? Image { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public ImageType Type { get; set; }
}
