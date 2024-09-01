namespace SimpleModManager_cli.Utils;

public class ConsoleUtil
{
    public static string ResetStr = "\u001b[0m";

    // foreground color
    public static string BlackStr = "\u001b[30m";
    public static string RedStr = "\u001b[31m";
    public static string GreenStr = "\u001b[32m";
    public static string YellowStr = "\u001b[33m";
    public static string BlueStr = "\u001b[34m";
    public static string MagentaStr = "\u001b[35m";
    public static string CyanStr = "\u001b[36m";
    public static string WhiteStr = "\u001b[37m";

    public static string BrightBlackStr = "\u001b[30;1m";
    public static string BrightRedStr = "\u001b[31;1m";
    public static string BrightGreenStr = "\u001b[32;1m";
    public static string BrightYellowStr = "\u001b[33;1m";
    public static string BrightBlueStr = "\u001b[34;1m";
    public static string BrightMagentaStr = "\u001b[35;1m";
    public static string BrightCyanStr = "\u001b[36;1m";
    public static string BrightWhiteStr = "\u001b[37;1m";


    // background color
    public static string BackgroundBlackStr = "\u001b[40m";
    public static string BackgroundRedStr = "\u001b[41m";
    public static string BackgroundGreenStr = "\u001b[42m";
    public static string BackgroundYellowStr = "\u001b[43m";
    public static string BackgroundBlueStr = "\u001b[44m";
    public static string BackgroundMagentaStr = "\u001b[45m";
    public static string BackgroundCyanStr = "\u001b[46m";
    public static string BackgroundWhiteStr = "\u001b[47m";

    public static string BackgroundBrightBlackStr = "\u001b[40;1m";
    public static string BackgroundBrightRedStr = "\u001b[41;1m";
    public static string BackgroundBrightGreenStr = "\u001b[42;1m";
    public static string BackgroundBrightYellowStr = "\u001b[43;1m";
    public static string BackgroundBrightBlueStr = "\u001b[44;1m";
    public static string BackgroundBrightMagentaStr = "\u001b[45;1m";
    public static string BackgroundBrightCyanStr = "\u001b[46;1m";
    public static string BackgroundBrightWhiteStr = "\u001b[47;1m";


    // decorations
    public static string BoldStr = "\u001b[1m";
    public static string UnderlineStr = "\u001b[4m";
    public static string ReversedStr = "\u001b[7m";


    // Foreground color methods
    public static string Black(string text)
    {
        return BlackStr + text + ResetStr;
    }

    public static string Red(string text)
    {
        return RedStr + text + ResetStr;
    }

    public static string Green(string text)
    {
        return GreenStr + text + ResetStr;
    }

    public static string Yellow(string text)
    {
        return YellowStr + text + ResetStr;
    }

    public static string Blue(string text)
    {
        return BlueStr + text + ResetStr;
    }

    public static string Magenta(string text)
    {
        return MagentaStr + text + ResetStr;
    }

    public static string Cyan(string text)
    {
        return CyanStr + text + ResetStr;
    }

    public static string White(string text)
    {
        return WhiteStr + text + ResetStr;
    }

    // Bright foreground color methods
    public static string BrightBlack(string text)
    {
        return BrightBlackStr + text + ResetStr;
    }

    public static string BrightRed(string text)
    {
        return BrightRedStr + text + ResetStr;
    }

    public static string BrightGreen(string text)
    {
        return BrightGreenStr + text + ResetStr;
    }

    public static string BrightYellow(string text)
    {
        return BrightYellowStr + text + ResetStr;
    }

    public static string BrightBlue(string text)
    {
        return BrightBlueStr + text + ResetStr;
    }

    public static string BrightMagenta(string text)
    {
        return BrightMagentaStr + text + ResetStr;
    }

    public static string BrightCyan(string text)
    {
        return BrightCyanStr + text + ResetStr;
    }

    public static string BrightWhite(string text)
    {
        return BrightWhiteStr + text + ResetStr;
    }

    // Background color methods
    public static string BackgroundBlack(string text)
    {
        return BackgroundBlackStr + text + ResetStr;
    }

    public static string BackgroundRed(string text)
    {
        return BackgroundRedStr + text + ResetStr;
    }

    public static string BackgroundGreen(string text)
    {
        return BackgroundGreenStr + text + ResetStr;
    }

    public static string BackgroundYellow(string text)
    {
        return BackgroundYellowStr + text + ResetStr;
    }

    public static string BackgroundBlue(string text)
    {
        return BackgroundBlueStr + text + ResetStr;
    }

    public static string BackgroundMagenta(string text)
    {
        return BackgroundMagentaStr + text + ResetStr;
    }

    public static string BackgroundCyan(string text)
    {
        return BackgroundCyanStr + text + ResetStr;
    }

    public static string BackgroundWhite(string text)
    {
        return BackgroundWhiteStr + text + ResetStr;
    }

    // Bright background color methods
    public static string BackgroundBrightBlack(string text)
    {
        return BackgroundBrightBlackStr + text + ResetStr;
    }

    public static string BackgroundBrightRed(string text)
    {
        return BackgroundBrightRedStr + text + ResetStr;
    }

    public static string BackgroundBrightGreen(string text)
    {
        return BackgroundBrightGreenStr + text + ResetStr;
    }

    public static string BackgroundBrightYellow(string text)
    {
        return BackgroundBrightYellowStr + text + ResetStr;
    }

    public static string BackgroundBrightBlue(string text)
    {
        return BackgroundBrightBlueStr + text + ResetStr;
    }

    public static string BackgroundBrightMagenta(string text)
    {
        return BackgroundBrightMagentaStr + text + ResetStr;
    }

    public static string BackgroundBrightCyan(string text)
    {
        return BackgroundBrightCyanStr + text + ResetStr;
    }

    public static string BackgroundBrightWhite(string text)
    {
        return BackgroundBrightWhiteStr + text + ResetStr;
    }

    // Decoration methods
    public static string Bold(string text)
    {
        return BoldStr + text + ResetStr;
    }

    public static string Underline(string text)
    {
        return UnderlineStr + text + ResetStr;
    }

    public static string Reversed(string text)
    {
        return ReversedStr + text + ResetStr;
    }

    public static string BoldUnderline(string text)
    {
        return UnderlineStr + BoldStr + text + ResetStr;
    }
}