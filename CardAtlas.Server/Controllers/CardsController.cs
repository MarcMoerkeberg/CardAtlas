using Asp.Versioning;
using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
using Microsoft.AspNetCore.Mvc;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/[controller]")]
	public class CardsController : ControllerBase
	{
		private readonly IScryfallApi _scryfallApi;

		public CardsController(IScryfallApi scryfallApi)
		{
			_scryfallApi = scryfallApi;
		}

		[HttpGet]
		public async Task<IEnumerable<Card>> Get()
		{
			await foreach(ApiCard card in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
			{
				var mappedResult = CardMapper.MapFromScryfallApi(card);
			}

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
