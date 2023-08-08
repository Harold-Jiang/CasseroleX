using System.Drawing;
using System.Text;

namespace CasseroleX.Application.Utils;
public static class ImageExtensions
{
    /// <summary>
    /// Generate letter avatar
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string LetterAvatar(this string avatar)
    {
        uint total = Adler32(avatar);
        int hue = (int)(total % 360);
        Color color = HsvToRgb(hue / 360.0, 0.3, 0.9);

        string bg = $"rgb({color.R},{color.G},{color.B})";
        string textColor = "#ffffff";
        string first = avatar[..1].ToUpper();

        string svg = $@"<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" height=""100"" width=""100"">
                            <rect fill=""{bg}"" x=""0"" y=""0"" width=""100"" height=""100""></rect>
                            <text x=""50"" y=""50"" font-size=""50"" fill=""{textColor}"" text-anchor=""middle"" dominant-baseline=""central"">{first}</text>
                        </svg>";

        byte[] svgBytes = Encoding.UTF8.GetBytes(svg);
        string src = Convert.ToBase64String(svgBytes);
        string value = "data:image/svg+xml;base64," + src;
        return value;
    }



    /// <summary>
    /// Generate file suffix image
    /// </summary>
    /// <returns></returns>
    public static string BuildSuffixImage(this string suffix, string? background = null)
    {

        suffix = suffix.Length > 4 ? suffix.ToUpper()[..4] : suffix.ToUpper();
        var total = HexStream2Dec(Adler32(suffix).ToString());
        var hue = total % 360;
        var color = HsvToRgb(hue / 360, 0.3, 0.9);
        background ??= $"rgb({color.R},{color.G},{color.B})";

        var icon = $"<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\" viewBox=\"0 0 512 512\" style=\"enable-background:new 0 0 512 512;\" xml:space=\"preserve\"><path style=\"fill:#E2E5E7;\" d=\"M128,0c-17.6,0-32,14.4-32,32v448c0,17.6,14.4,32,32,32h320c17.6,0,32-14.4,32-32V128L352,0H128z\"/> <path style=\"fill:#B0B7BD;\" d=\"M384,128h96L352,0v96C352,113.6,366.4,128,384,128z\"/> <polygon style=\"fill:#CAD1D8;\" points=\"480,224 384,128 480,128 \"/> <path style=\"fill:{background};\" d=\"M416,416c0,8.8-7.2,16-16,16H48c-8.8,0-16-7.2-16-16V256c0-8.8,7.2-16,16-16h352c8.8,0,16,7.2,16,16 V416z\"/> <path style=\"fill:#CAD1D8;\" d=\"M400,432H96v16h304c8.8,0,16-7.2,16-16v-16C416,424.8,408.8,432,400,432z\"/> <g><text><tspan x=\"220\" y=\"380\" font-size=\"124\" font-family=\"Verdana, Helvetica, Arial, sans-serif\" fill=\"white\" text-anchor=\"middle\">{suffix}</tspan></text></g></svg>";
        return icon;

    }
    
    

     
    /// <summary>
    /// Adler32 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static uint Adler32(string str)
    {
        const int mod = 65521;
        uint a = 1, b = 0;
        foreach (char c in str)
        {
            a = (a + c) % mod;
            b = (b + a) % mod;
        }
        return (b << 16) | a;
    }

    /// <summary>
    /// for recving msg and parsering the size of data
    /// </summary>
    /// <param name="s"> hex stream </param>
    /// <returns>dec integer</returns>
    private static int HexStream2Dec(string s)
    {
        string tmp = "";
        for (int i = 3; i >= 0; i--)
        {
            UInt32 m = Convert.ToUInt32(s[i]);
            string hexs = Convert.ToString(m, 16);
            tmp += hexs;
        }
        uint res = Convert.ToUInt32(tmp, 16);
        return (int)res;
    }

    private static Color HsvToRgb(double h, double s, double v)
    {
        int hi = Convert.ToInt32(Math.Floor(h * 6)) % 6;
        double f = h * 6 - Math.Floor(h * 6);
        int v1 = Convert.ToInt32(v * 255);
        int p = Convert.ToInt32(v1 * (1 - s));
        int q = Convert.ToInt32(v1 * (1 - f * s));
        int t = Convert.ToInt32(v1 * (1 - (1 - f) * s));

        switch (hi)
        {
            case 0:
                return Color.FromArgb(255, v1, t, p);
            case 1:
                return Color.FromArgb(255, q, v1, p);
            case 2:
                return Color.FromArgb(255, p, v1, t);
            case 3:
                return Color.FromArgb(255, p, q, v1);
            case 4:
                return Color.FromArgb(255, t, p, v1);
            default:
                return Color.FromArgb(255, v1, p, q);
        }
    } 
}
