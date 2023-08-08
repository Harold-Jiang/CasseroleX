namespace CasseroleX.Application.Utils;
public static class RandomProvider
{
    private static readonly string Letters = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";

    /// <summary>
    /// Obtain a 32-bit guid string that does not contain the '-' sign
    /// </summary> 
    public static string GetGuid32ToString(bool isUpper = false)
    {
        var guid = Guid.NewGuid().ToString("N");
        return isUpper == true ? guid.ToUpper() : guid;
    }

    /// <summary>
    /// Obtain a 16 bit string based on the Guid
    /// </summary>
    public static string GetGuidToString()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        string tempStr = string.Format("{0:x}", i - DateTime.Now.Ticks);
        if (tempStr.Length != 16)
        {
            tempStr += "0";
        }
        return tempStr;
    }

    /// <summary>
    /// Obtain 19 digits based on the Guid
    /// </summary>
    public static string GetGuidToNumber()
    {
        byte[] buffer = Guid.NewGuid().ToByteArray();
        return BitConverter.ToInt64(buffer, 0).ToString();
    }

    /// <summary>
    /// Generate random numbers
    /// </summary>
    public static string Number(int Length)
    {
        return Number(Length, false);
    }

    /// <summary>
    /// Generate random numbers
    /// </summary>
    /// <param name="Length">Length</param>
    /// <param name="Sleep">Do you want to block the current thread before generating to avoid duplication</param>
    public static string Number(int Length, bool Sleep)
    {
        if (Sleep)
             Thread.Sleep(3);
        string result = "";
        System.Random random = new Random();
        for (int i = 0; i < Length; i++)
        {
            result += random.Next(10).ToString();
        }
        return result;
    }

    /// <summary>
    /// Generate a random letter string (mixed with numbers and letters)
    /// </summary> 
    public static string GetCheckCode(int Length)
    {
        string str = string.Empty;
        int rep = 0;
        long num2 = DateTime.Now.Ticks + rep;
        rep++;
        Random random = new(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
        for (int i = 0; i < Length; i++)
        {
            char ch;
            int num = random.Next();
            ch = (num % 2) == 0 ? (char)(0x30 + ((ushort)(num % 10))) : (char)(0x41 + ((ushort)(num % 0x1a)));
            str += ch.ToString();
        }
        return str;
    }


    /// <summary>
    /// Returns the current millisecond timestamp
    /// </summary>
    public static string Msectime()
    {
        long timeTicks = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        return timeTicks.ToString();
    }


    /// <summary>
    /// Generate verification codes with English characters
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
}
