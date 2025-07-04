using CardAtlas.Server.Extensions;

namespace CardAtlas.UnitTests.ExtensionTests;

class DateTimeExtensionsTests
{
	private readonly DateTime _baseTime = new DateTime(
		year: 2025,
		month: 01,
		day: 02,
		hour: 12,
		minute: 34,
		second: 56
	).AddTicks(1234567);// Milliseconds: 123 / Microseconds: 456 / Nanoseconds: 7 milliseconds

	[Test]
	public void Truncate_ToNanoseconds_ShouldNotAlterObject()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromTicks(1)); // 10 ticks = 1 microsecond

		Assert.That(
			truncatedObject,
			Is.EqualTo(_baseTime),
			"Should be equivalent to target, when truncating using nanoseconds as scale."
		);
	}

	[Test]
	public void Truncate_ToMicroseconds_ShouldRemoveLessThanMicroseconds()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromTicks(10)); // 10 ticks = 1 microsecond

		Assert.That(
			_baseTime.Nanosecond,
			Is.EqualTo(700),
			"Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0),
			"Should not have any nanoseconds, when truncating using microseconds as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(456),
			"Should not alter microseconds, when truncating using microseconds as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(123),
			"Should not alter milliseconds, when truncating using microseconds as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(56),
			"Should not alter seconds, when truncating using microseconds as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(34),
			"Should not alter minutes, when truncating using microseconds as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(12),
			"Should not alter hours, when truncating using microseconds as scale."
		);
	}

	[Test]
	public void Truncate_ToMilliseconds_ShouldRemoveMicroSecondsAndBelow()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromMilliseconds(1));

		Assert.That(
			_baseTime.Millisecond,
			Is.EqualTo(123),
			"Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0),
			"Should not contain nanoseconds, when truncating using milliseconds as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(0),
			"Should not contain microseconds, when truncating using milliseconds as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(123),
			"Should not alter milliseconds, when truncating using milliseconds as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(56),
			"Should not alter seconds, when truncating using milliseconds as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(34),
			"Should not alter minutes, when truncating using milliseconds as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(12),
			"Should not alter hours, when truncating using milliseconds as scale."
		);
	}

	[Test]
	public void Truncate_ToSeconds_ShouldRemoveMilliSecondsAndBelow()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromSeconds(1));

		Assert.That(
			_baseTime.Second,
			Is.EqualTo(56), "Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0), "Should not contain nanoseconds, when truncating using seconds as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(0), "Should not contain microseconds, when truncating using seconds as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(0), "Should not contain milliseconds, when truncating using seconds as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(56), "Should not alter seconds, when truncating using seconds as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(34), "Should not alter minutes, when truncating using seconds as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(12), "Should not alter hours, when truncating using seconds as scale."
		);
	}

	[Test]
	public void Truncate_ToMinutes_ShouldRemoveSecondsAndBelow()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromMinutes(1));

		Assert.That(
			_baseTime.Minute,
			Is.EqualTo(34), "Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0), "Should not contain nanoseconds, when truncating using minutes as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(0), "Should not contain microseconds, when truncating using minutes as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(0), "Should not contain milliseconds, when truncating using minutes as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(0), "Should not contain seconds, when truncating using minutes as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(34), "Should not alter minutes, when truncating using minutes as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(12), "Should not alter hours, when truncating using minutes as scale."
		);
	}

	[Test]
	public void Truncate_ToHours_ShouldRemoveSecondsAndBelow()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromHours(1));

		Assert.That(
			_baseTime.Hour,
			Is.EqualTo(12), "Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0), "Should not contain nanoseconds, when truncating using hours as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(0), "Should not contain microseconds, when truncating using hours as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(0), "Should not contain milliseconds, when truncating using hours as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(0), "Should not contain seconds, when truncating using hours as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(0), "Should not minutes, when truncating using hours as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(12), "Should contain alter hours, when truncating using hours as scale."
		);
	}

	[Test]
	public void Truncate_ToDays_ShouldRemoveTimeFromObject()
	{
		DateTime truncatedObject = _baseTime.Truncate(TimeSpan.FromDays(1));

		Assert.That(
			_baseTime.Hour,
			Is.EqualTo(12), "Should not alter the target datetime object."
		);
		Assert.That(
			truncatedObject.Nanosecond,
			Is.EqualTo(0), "Should not contain nanoseconds, when truncating using days as scale."
		);
		Assert.That(
			truncatedObject.Microsecond,
			Is.EqualTo(0), "Should not contain microseconds, when truncating using days as scale."
		);
		Assert.That(
			truncatedObject.Millisecond,
			Is.EqualTo(0), "Should not contain milliseconds, when truncating using days as scale."
		);
		Assert.That(
			truncatedObject.Second,
			Is.EqualTo(0), "Should not contain seconds, when truncating using days as scale."
		);
		Assert.That(
			truncatedObject.Minute,
			Is.EqualTo(0), "Should not minutes, when truncating using days as scale."
		);
		Assert.That(
			truncatedObject.Hour,
			Is.EqualTo(0), "Should not contain hours, when truncating using days as scale."
		);
	}

	[Test]
	public void Truncate_ZeroScale_ShouldThrowException()
	{
		Func<DateTime> truncateMethod = () => _baseTime.Truncate(TimeSpan.Zero);

		Assert.That(
			truncateMethod,
			Throws.TypeOf<ArgumentOutOfRangeException>(),
			"Should throw an ArgumentOutOfRangeException, when using zero for scale."
		);
	}

	[Test]
	public void Truncate_NegativeScale_ShouldThrowException()
	{
		Func<DateTime> truncateMethod = () => _baseTime.Truncate(TimeSpan.FromMinutes(-1));

		Assert.That(
			truncateMethod,
			Throws.TypeOf<ArgumentOutOfRangeException>(),
			"Should throw an ArgumentOutOfRangeException, when using a negative value for scale."
		);
	}
}
