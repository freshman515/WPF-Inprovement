using System.ComponentModel.DataAnnotations;

namespace WPFProject01.Dtos;

public class UpdateProductDto {
	[MaxLength(200)]
	public string Name { get; set; }

	public string Description { get; set; }

	[Range(0, 9999999)]
	public decimal Price { get; set; }

	[Range(0, int.MaxValue)]
	public int Stock { get; set; }

	public string? CategoryName { get; set; }  // CategoryId 可以为空
}