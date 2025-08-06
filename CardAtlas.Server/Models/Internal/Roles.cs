namespace CardAtlas.Server.Models.Internal;

public static class Roles
{
	public const string Admin = "Admin";
	public const string Moderator = "Moderator";
	public const string User = "User";
	public const string Guest = "Guest";

	private const string AdminId = "120a6a6d-9ba8-4291-8852-2c886425d0d1";
	private const string ModeratorId = "95007ffa-933b-44bd-8312-12322e26fcd4";
	private const string UserId = "1f3e819c-0f30-4b36-9533-64486b282b88";
	private const string GuestId = "a9eb88a6-590b-4863-8c47-eacc8e76fc91";

	private static IReadOnlyDictionary<string, string> _roleIdLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		[Admin] = AdminId,
		[Moderator] = ModeratorId,
		[User] = UserId,
		[Guest] = GuestId,
	};

	public static readonly string[] DefaultRoles = [User];
	public static readonly string[] AllRoles = [Admin, Moderator, User, Guest];

	public static string GetRoleId(string roleName)
	{
		if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentNullException(nameof(roleName));
		if (!_roleIdLookup.TryGetValue(roleName, out string? roleId)) throw new ArgumentException($"Role '{roleName}' is not associated with an id.", nameof(roleName));

		return roleId;
	}
}
