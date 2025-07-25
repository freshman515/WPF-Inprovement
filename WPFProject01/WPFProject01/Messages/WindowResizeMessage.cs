namespace WPFProject01.Messages;

public class WindowResizeMessage {
	public int Width { get; }
	public int Height { get; }

	public WindowResizeMessage(int width, int height)
	{
		Width = width;
		Height = height;
	}
}