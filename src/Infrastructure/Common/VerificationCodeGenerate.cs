using SkiaSharp;

namespace CasseroleX.Infrastructure.Common;

/// <summary>
/// Verification code Helper class
/// </summary>
public class VerificationCodeGenerate
{
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
    /// Get Random Colors
    /// </summary>
    /// <returns></returns>
    private static SKColor GetRandomColor()
    {
        Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
        
        System.Threading.Thread.Sleep(RandomNum_First.Next(50));
        Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
        // To display on a white background, try to generate dark colors as much as possible
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
            //Draw the background noise line of the image
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
    /// Generate verification code image
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
                //Fill with white background
                //canvas.DrawColor(SKColors.White); 
                canvas.Clear(SKColors.AliceBlue);

                //Noise
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
                //Write text on the canvas 
                var drawStyle = new SKPaint
                {
                    IsAntialias = true,
                    TextSize = 24
                };
                char[] chars = captchaText.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    var font = SKTypeface.FromFamilyName("Verdana", SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);

                    //Degrees of rotation
                    float angle = random.Next(-30, 30);

                    canvas.Translate(10, 10);


                    float px = ((i) * 24);
                    float py = (height) / 2;

                    canvas.RotateDegrees(angle, px, py);


                    drawStyle.Typeface = font;
                    drawStyle.Color = GetRandomColor(); 
                    canvas.DrawText(chars[i].ToString(), px, py, drawStyle);


                    // canvas.DrawText(chars[i].ToString(), (i ) * SetFontSize, (SetHeight-1), drawStyle);

                    canvas.RotateDegrees(-angle, px, py);
                    canvas.Translate(-12, -12);
                }

                //Draw random interference lines
                using (SKPaint disturbStyle = new())
                {
                    for (int i = 0; i < lineNum; i++)
                    {
                        disturbStyle.Color = colors[random.Next(colors.Count)];
                        disturbStyle.StrokeWidth = lineStrookeWidth;
                        canvas.DrawLine(random.Next(0, width), random.Next(0, height), random.Next(0, width), random.Next(0, height), drawStyle);
                    }
                }
                //Return image byte
                using SKImage img = SKImage.FromBitmap(image2d);
                using SKData p = img.Encode(SKEncodedImageFormat.Png, 100);
                return p.ToArray();
            }
        }
    } 
     
}
