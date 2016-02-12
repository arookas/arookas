namespace arookas {
	public delegate T Parse<T>(string value);
	public delegate bool TryParse<T>(string value, out T result);
}
