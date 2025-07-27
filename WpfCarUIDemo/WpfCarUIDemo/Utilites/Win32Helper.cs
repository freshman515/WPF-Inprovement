using System.Runtime.InteropServices;

namespace WpfCarUIDemo.Utilites {
	public static class Win32Helper {
		[DllImport("user32.dll")]
		public static extern bool SystemParametersInfo(int uAction, int uParam, ref RECT lpvParam, int fuWinIni);

		public const int SPI_GETWORKAREA = 0x0030;

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}

}