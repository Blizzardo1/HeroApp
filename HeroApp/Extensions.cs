using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp; 

internal static class Extensions {
    public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);

    public static string ToBase64(this Image? img) {
        if (img == null) return string.Empty;
        using var memoryStream = new MemoryStream();
        img.Save(memoryStream, img.RawFormat);
        
        return Convert.ToBase64String(memoryStream.ToArray());
    }
}