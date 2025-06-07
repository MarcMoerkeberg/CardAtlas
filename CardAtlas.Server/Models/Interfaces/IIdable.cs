using System.Numerics;

namespace CardAtlas.Server.Models.Interfaces;

public interface IIdable<T>
	where T : INumber<T>
{
	T Id { get; set; }
}
