using Asp.Versioning;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardAtlas.Server.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/[controller]")]
	public class CardsController : ControllerBase
	{
		private readonly IScryfallIngestionService _scryfallInjectionService;

		public CardsController(IScryfallIngestionService scryfallIngestionService)
		{
			_scryfallInjectionService = scryfallIngestionService;
		}

		[HttpGet]
		public async Task<IEnumerable<Card>> Get()
		{
			await _scryfallInjectionService.UpsertCardCollectionAsync();
			throw new NotImplementedException();
		}

		[HttpGet]
		[Route("{cardId:Guid}")]
		public string Get(Guid cardId)
		{
			throw new NotImplementedException();
		}

		[HttpPost]
		public string Create(object card)
		{
			throw new NotImplementedException();
		}

		[HttpPut]
		public string Update(object card)
		{
			throw new NotImplementedException();
		}

		[HttpDelete]
		[Route("{cardId:Guid}")]
		public string Delete(Guid cardId)
		{
			throw new NotImplementedException();
		}
	}
}
