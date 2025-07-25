namespace WPFProject01.Helper;

public class ApiResponse<T> {
	public bool Success { get; set; }
	public string Message { get; set; }
	public int Code { get; set; }
	public T Data { get; set; }
}