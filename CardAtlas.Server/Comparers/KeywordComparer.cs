using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class KeywordComparer : IEqualityComparer<Keyword>
{
	public bool Equals(Keyword? x, Keyword? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& x.SourceId == y.SourceId
			&& string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(x.ReminderText, y.ReminderText)
			&& string.Equals(x.Description, y.Description);
	}

	public int GetHashCode(Keyword obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.Name);
		hash.Add(obj.ReminderText);
		hash.Add(obj.Description);

		return hash.ToHashCode();
	}
}
