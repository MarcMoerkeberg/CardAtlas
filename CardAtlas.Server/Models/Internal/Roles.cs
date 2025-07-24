namespace CardAtlas.Server.Models.Internal;

public static class Roles
{
	public const string Admin = "Admin";
	public const string Moderator = "Moderator";
	public const string User = "User";
	public const string Guest = "Guest";
	public static readonly string[] DefaultRoles = [User];
	public static readonly string[] AllRoles = [Admin, Moderator, User, Guest];
}
