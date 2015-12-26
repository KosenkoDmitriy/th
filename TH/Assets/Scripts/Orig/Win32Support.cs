using System;
using System.Runtime.InteropServices;

public class Win32Support
{
    public enum Bool
    {
        False = 0,
        True
    };
    public enum TRO //TernaryRasterOperations
    {
        SRCCOPY = 0x00CC0020,// dest = source             
        SRCPAINT = 0x00EE0086,// dest = source OR dest     
        SRCAND = 0x008800C6,// dest = source AND dest    
        SRCINVERT = 0x00660046,// dest = source XOR dest        
        SRCERASE = 0x00440328,// dest = source AND (NOT dest )  
        NOTSRCCOPY = 0x00330008,// dest = (NOT source)          
        NOTSRCERASE = 0x001100A6,// dest = (NOT src) AND (NOT dest) 
        MERGECOPY = 0x00C000CA,// dest = (source AND pattern)     
        MERGEPAINT = 0x00BB0226,// dest = (NOT source) OR dest     
        PATCOPY = 0x00F00021,// dest = pattern                  
        PATPAINT = 0x00FB0A09,// dest = DPSnoo                   
        PATINVERT = 0x005A0049,// dest = pattern XOR dest         
        DSTINVERT = 0x00550009,// dest = (NOT dest)               
        BLACKNESS = 0x00000042,// dest = BLACK                    
        WHITENESS = 0x00FF0062,// dest = WHITE                    
    };
    //The API is your friend
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]//used to send messages through the API
    public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);//use the API SendMessage routine
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]//used to send messages through the API
    public static extern int PostMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);//use the API SendMessage routine
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TRO dwRop);
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern Bool DeleteDC(IntPtr hdc);
    [DllImport("gdi32.dll", ExactSpelling = true)]
    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern Bool DeleteObject(IntPtr hObject);
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);
    [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnString, int nSize, string lpFilename);
}

