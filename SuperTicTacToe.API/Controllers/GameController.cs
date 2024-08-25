using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperTicTacToe.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GameController : ControllerBase
	{
		//[NO USER REQUIRED]

		//Search [Request]
		//Create [GameName] [Password?] -> PlayerToken
		//Join [GameId] [Password?] -> PlayerToken

		//[USER REQUIRED]

		//Leave
		//PlaceAt [GameX] [GameY] [PosX] [PosY] -> IsSuccess
		//BecomeSpectator
		//BecomePlayer [PrefferedChar]
	}
}

