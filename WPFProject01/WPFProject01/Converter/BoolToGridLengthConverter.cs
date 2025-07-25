﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFProject01.Converter;

public class BoolToGridLengthConverter : IValueConverter {
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
		bool isVisible = (bool)value;
		return isVisible ? new GridLength(140) : new GridLength(0);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
		return null;
	}
}