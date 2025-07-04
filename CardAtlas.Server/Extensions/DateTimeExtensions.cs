namespace CardAtlas.Server.Extensions;

public static class DateTimeExtensions
{
	/// <summary>
	/// Truncates the datetime to the specified <paramref name="scale"/>.
	/// </summary>
	/// <param name="scale">Determines how far to truncate the <paramref name="target"/>. Use TimeSpan.FromXXX for ease of declaration.</param>
	/// <returns>A new <see cref="DateTime"/> object truncted to the specified <paramref name="scale"/>.</returns>
	public static DateTime Truncate(this DateTime target, TimeSpan scale)
	{
		if (scale <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(scale), "Must be greater than zero.");

		return target.AddTicks(-(target.Ticks % scale.Ticks));
	}
}
