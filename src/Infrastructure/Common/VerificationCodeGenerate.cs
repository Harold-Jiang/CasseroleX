using SkiaSharp;

namespace CasseroleX.Infrastructure.Common;

/// <summary>
/// 验证码帮助类
/// </summary>
public class VerificationCodeGenerate
{
    private static readonly string Letters = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
    ///<summary>
    ///Color collection of interference lines
    ///</summary>
    private static List<SKColor> colors { get => new()
    {    
            SKColors.AliceBlue,
            SKColors.PaleGreen,
            SKColors.PaleGoldenrod,
            SKColors.Orchid,
            SKColors.OrangeRed,
            SKColors.Orange,
            SKColors.OliveDrab,
            SKColors.Olive,
            SKColors.OldLace,
            SKColors.Navy,
            SKColors.NavajoWhite,
            SKColors.Moccasin,
            SKColors.MistyRose,
            SKColors.MintCream,
            SKColors.MidnightBlue,
            SKColors.MediumVioletRed,
            SKColors.MediumTurquoise,
            SKColors.MediumSpringGreen,
            SKColors.LightSlateGray,
            SKColors.LightSteelBlue,
            SKColors.LightYellow,
            SKColors.Lime,
            SKColors.LimeGreen,
            SKColors.Linen,
            SKColors.PaleTurquoise,
            SKColors.Magenta,
            SKColors.MediumAquamarine,
            SKColors.MediumBlue,
            SKColors.MediumOrchid,
            SKColors.MediumPurple,
            SKColors.MediumSeaGreen,
            SKColors.MediumSlateBlue,
            SKColors.Maroon,
            SKColors.PaleVioletRed,
            SKColors.PapayaWhip,
            SKColors.PeachPuff,
            SKColors.Snow,
            SKColors.SpringGreen,
            SKColors.SteelBlue,
            SKColors.Tan,
            SKColors.Teal,
            SKColors.Thistle,
            SKColors.SlateGray,
            SKColors.Tomato,
            SKColors.Violet,
            SKColors.Wheat,
            SKColors.White,
            SKColors.WhiteSmoke,
            SKColors.Yellow,
            SKColors.YellowGreen,
            SKColors.Turquoise,
            SKColors.LightSkyBlue,
            SKColors.SlateBlue,
            SKColors.Silver,
            SKColors.Peru,
            SKColors.Pink,
            SKColors.Plum,
            SKColors.PowderBlue,
            SKColors.Purple,
            SKColors.Red,
            SKColors.SkyBlue,
            SKColors.RosyBrown,
            SKColors.SaddleBrown,
            SKColors.Salmon,
            SKColors.SandyBrown,
            SKColors.SeaGreen,
            SKColors.SeaShell,
            SKColors.Sienna,
            SKColors.RoyalBlue,
            SKColors.LightSeaGreen,
            SKColors.LightSalmon,
            SKColors.LightPink,
            SKColors.Crimson,
            SKColors.Cyan,
            SKColors.DarkBlue,
            SKColors.DarkCyan,
            SKColors.DarkGoldenrod,
            SKColors.DarkGray,
            SKColors.Cornsilk,
            SKColors.DarkGreen,
            SKColors.DarkMagenta,
            SKColors.DarkOliveGreen,
            SKColors.DarkOrange,
            SKColors.DarkOrchid,
            SKColors.DarkRed,
            SKColors.DarkSalmon,
            SKColors.DarkKhaki,
            SKColors.DarkSeaGreen,
            SKColors.CornflowerBlue,
            SKColors.Chocolate,
            SKColors.AntiqueWhite,
            SKColors.Aqua,
            SKColors.Aquamarine,
            SKColors.Azure,
            SKColors.Beige,
            SKColors.Bisque,
            SKColors.Coral,
            SKColors.Black,
            SKColors.Blue,
            SKColors.BlueViolet,
            SKColors.Brown,
            SKColors.BurlyWood,
            SKColors.CadetBlue,
            SKColors.Chartreuse,
            SKColors.BlanchedAlmond,
            SKColors.Transparent,
            SKColors.DarkSlateBlue,
            SKColors.DarkTurquoise,
            SKColors.IndianRed,
            SKColors.Indigo,
            SKColors.Ivory,
            SKColors.Khaki,
            SKColors.Lavender,
            SKColors.LavenderBlush,
            SKColors.HotPink,
            SKColors.LawnGreen,
            SKColors.LightBlue,
            SKColors.LightCoral,
            SKColors.LightCyan,
            SKColors.LightGoldenrodYellow,
            SKColors.LightGray,
            SKColors.LightGreen,
            SKColors.LemonChiffon,
            SKColors.DarkSlateGray,
            SKColors.Honeydew,
            SKColors.Green,
            SKColors.DarkViolet,
            SKColors.DeepPink,
            SKColors.DeepSkyBlue,
            SKColors.DimGray,
            SKColors.DodgerBlue,
            SKColors.Firebrick,
            SKColors.GreenYellow,
            SKColors.FloralWhite,
            SKColors.Fuchsia,
            SKColors.Gainsboro,
            SKColors.GhostWhite,
            SKColors.Gold,
            SKColors.Goldenrod,
            SKColors.Gray,
            SKColors.ForestGreen
    }; } 
    ///<summary>
    ///Create a brush
    ///</summary>
    ///<param name="color"></param>
    ///<param name="fontSize"></param>
    ///<returns></returns>
    private static SKPaint CreatePaint(SKColor color, float fontSize)
    {
        SkiaSharp.SKTypeface font = SKTypeface.FromFamilyName(null, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
        SKPaint paint = new();
        paint.IsAntialias = true;
        paint.Color = color;
        paint.Typeface = font;
        paint.TextSize = fontSize;
        return paint;
    }
    /// <summary>
    /// 获取随机颜色
    /// </summary>
    /// <returns></returns>
    private static SKColor GetRandomColor()
    {
        Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
        // 对于C#的随机数，没什么好说的
        System.Threading.Thread.Sleep(RandomNum_First.Next(50));
        Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
        // 为了在白色背景上显示，尽量生成深色
        int int_Red = RandomNum_First.Next(256);
        int int_Green = RandomNum_Sencond.Next(256);
        int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
        int_Blue = (int_Blue > 255) ? 255 : int_Blue;
        return SKColor.FromHsv(int_Red, int_Green, int_Blue);
    }
    private static void AddForeNoisePoint(SKBitmap objBitmap, Random random)
    {
        for (int i = 0; i < objBitmap.Width * 2; i++)
        {
            objBitmap.SetPixel(random.Next(objBitmap.Width), random.Next(objBitmap.Height), GetRandomColor());
        }
    } 
    private static void AddBackgroundNoisePoint(SKBitmap objBitmap, SKCanvas objGraphics, Random random)
    {
        using (SKPaint objPen = CreatePaint(SKColors.Azure, 0))
        {
            for (int i = 0; i < objBitmap.Width * 2; i++)
            {

                //objGraphics.DrawRectangle(objPen, objRandom.Next(objBitmap.Width), objRandom.Next(objBitmap.Height), 1, 1);


                objGraphics.DrawRect(random.Next(objBitmap.Width), random.Next(objBitmap.Height), 1, 1, objPen);
            }
        }
        if (true)
        {
            //画图片的背景噪音线
            for (var i = 0; i < 12; i++)
            {
                var x1 = random.Next(objBitmap.Width);
                var x2 = random.Next(objBitmap.Width);
                var y1 = random.Next(objBitmap.Height);
                var y2 = random.Next(objBitmap.Height);

                objGraphics.DrawLine(x1, y1, x2, y2, CreatePaint(SKColors.Silver, 0));
            }
        }
    } 
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    public static byte[] GetCaptcha(string captchaText, int width, int height = 30, int lineNum = 1, int lineStrookeWidth = 1)
    {
        //Create bitmap bitmap
        using (SKBitmap image2d = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul))
        {
            //Create a brush
            using (SKCanvas canvas = new SKCanvas(image2d))
            {
                Random random = new Random();
                //填充白色背景
                //canvas.DrawColor(SKColors.White); 
                canvas.Clear(SKColors.AliceBlue);

                //噪点
                //AddForeNoisePoint(image2d, random);
                AddBackgroundNoisePoint(image2d, canvas, random);

                //   //Fill the background color as white
                //canvas.DrawColor(SKColors.White);
                ////Write the text on the canvas
                //using (SKPaint drawStyle = CreatePaint(SKColors.Black, height))
                //{
                //    canvas.DrawText(captchaText, 1, height - 1, drawStyle);
                //}
                ////Draw random interference lines
                //using (SKPaint drawStyle = new SKPaint())
                //{
                //    Random random = new Random();
                //    for (int i = 0; i < lineNum; i++)
                //    {
                //        drawStyle.Color = colors[random.Next(colors.Count)];
                //        drawStyle.StrokeWidth = lineStrookeWidth;
                //        canvas.DrawLine(random.Next(0, width), random.Next(0, height), random.Next(0, width), random.Next(0, height), drawStyle);
                //    }
                //}
                //将文字写到画布上 
                var drawStyle = new SKPaint
                {
                    IsAntialias = true,
                    TextSize = 24
                };
                char[] chars = captchaText.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    var font = SKTypeface.FromFamilyName("Verdana", SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);

                    //转动的度数
                    float angle = random.Next(-30, 30);

                    canvas.Translate(10, 10);


                    float px = ((i) * 24);
                    float py = (height) / 2;

                    canvas.RotateDegrees(angle, px, py);


                    drawStyle.Typeface = font;
                    drawStyle.Color = GetRandomColor();
                    //写字 (i + 1) * 16, 28
                    canvas.DrawText(chars[i].ToString(), px, py, drawStyle);


                    // canvas.DrawText(chars[i].ToString(), (i ) * SetFontSize, (SetHeight-1), drawStyle);

                    canvas.RotateDegrees(-angle, px, py);
                    canvas.Translate(-12, -12);
                }
                 
                //画随机干扰线
                using (SKPaint disturbStyle = new())
                {
                    for (int i = 0; i < lineNum; i++)
                    {
                        disturbStyle.Color = colors[random.Next(colors.Count)];
                        disturbStyle.StrokeWidth = lineStrookeWidth;
                        canvas.DrawLine(random.Next(0, width), random.Next(0, height), random.Next(0, width), random.Next(0, height), drawStyle);
                    }
                }
                //返回图片byte
                using SKImage img = SKImage.FromBitmap(image2d);
                using SKData p = img.Encode(SKEncodedImageFormat.Png, 100);
                return p.ToArray();
            }
        }
    }

    /// <summary>
    /// 生成带英文字符的验证码
    /// </summary>
    public static string RandomCode(int codeLength = 4)
    {
        var array = Letters.Split(new[] { ',' });
        var random = new Random();
        var temp = -1;
        var captcheCode = string.Empty;
        for (int i = 0; i < codeLength; i++)
        {
            if (temp != -1)
                random = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
            var index = random.Next(array.Length);
            if (temp != -1 && temp == index)
                return RandomCode(codeLength);
            temp = index;
            captcheCode += array[index];
        }
        return captcheCode;
    }


    /// <summary>
    /// 生成数字验证码
    /// </summary>
    /// <param name="Length">生成长度</param> 
    public static string Number(int Length = 4)
    { 
        string result = "";
        System.Random random = new Random();
        for (int i = 0; i < Length; i++)
        {
            result += random.Next(10).ToString();
        }
        return result;
    }


}
