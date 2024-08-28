using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using SuperTicTacToe.API.Extensions;
using SuperTicTacToe.API.Repositories;
using SuperTicTacToe.API.Attributes;
using SuperTicTacToe.API.Modules;
using System.Reflection;
using System.Text.Json;
using SuperTicTacToe.API.Model;

namespace SuperTicTacToe.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class GameController : ControllerBase
	{
		private JsonSerializerOptions _jsonOptions;

		private readonly GameRoomsRepository _rooms;

		public GameController(GameRoomsRepository rooms) {
			_rooms = rooms;
			_jsonOptions = new JsonSerializerOptions() {
				IncludeFields = true,
			};
		}

		[HttpGet("Create")]
		public IActionResult Create() {
			var gameName = ControllerBaseExtensions.GetRequiredQueryValue<string>(this, "name");
			var password = ControllerBaseExtensions.GetQueryValue<string>(this, "password");

			var room = _rooms.Create(gameName, password);
			var player = room.AddPlayer();

			return Ok(new {
                playerToken = player.Token,
				roomId = player.Room.Id,
            });
		}
		[HttpGet("Join")]
		public IActionResult Join() {
			var gameId = ControllerBaseExtensions.GetRequiredQueryValue<int>(this, "id");
			var password = ControllerBaseExtensions.GetQueryValue<string>(this, "password");
			var qPlayerToken = Request.Query["player-token"].FirstOrDefault();
			Guid? playerToken = null;
			if (qPlayerToken is not null && qPlayerToken.Length > 0) {
				if (Guid.TryParse(qPlayerToken, out var token)) {
					playerToken = token;
				}
				else return BadRequest("Invalid player token format");
			}

			var room = _rooms.GetRequired(gameId);

			Player? player = null;

			if (playerToken is not null) {
				player = room.GetPlayer(playerToken.Value);
			}
			if (player is null) {
                if (room.Password?.Length > 0 && room.Password != password)
					return Unauthorized("Password incorrect");
                player = room.AddPlayer();
			}

			return Ok(new {
				playerId = player.Id,
                playerToken = player.Token,
				room = player.Room,
            });
		}
		[HttpGet("Search")]
		public IActionResult Search() {
			throw new NotImplementedException();
		}
		[HttpGet("Command")]
		public async Task<IActionResult> Command() {
			var queriedCommand = Request.Query["name"].FirstOrDefault();
			if (queriedCommand is null) {
				return BadRequest("Command name in querry \"name\" required");
			}

			var queriedToken = Request.Headers["player-token"].FirstOrDefault();
			if (queriedToken is null) {
				return BadRequest("Player token required");
			}

			if (!Guid.TryParse(queriedToken, out Guid playerToken)) {
				return BadRequest("Wrong player token format");
			}

			var player = _rooms.GetRequiredGamePlayer(playerToken);

			var command = typeof(GameCommandsModule).GetMethod(queriedCommand);
			if (command is null || !command.CustomAttributes.Any(attr => attr.AttributeType == typeof(GameCommandAttribute))) {
				return NotFound($"Cant find command named \"{queriedCommand}\"");
			}

			var argsInfo = command.GetParameters();
			var args = new object?[argsInfo.Length];

			string? argKey;
			string? argStringValue;
			object? argValue;
            for (int i = 0; i < argsInfo.Length; i++) {
				argKey = argsInfo[i].Name;
				if (argKey is null) {
					continue;
				}

				argStringValue = Request.Query[argKey].FirstOrDefault();
				if (argStringValue is null) {
					return BadRequest($"Cant find argument {argsInfo[i].Name} in query");
				}
				try {
					argValue = TypeDescriptor.GetConverter(argsInfo[i].ParameterType).ConvertFromInvariantString(argStringValue);
				}
				catch {
					return BadRequest($"Cant convert \"{argsInfo[i].Name}\" argument \"{argStringValue}\" value to \"{argsInfo[i].ParameterType}\" type");
				}

				args[i] = argValue;
			}

            Console.Write($"[{player}] Executing network game command /{command.Name} ");
            for (int i = 0; i < argsInfo.Length; i++) {
                Console.Write($"{argsInfo[i].Name}: {args[i]}");
                if (i != argsInfo.Length - 1) {
                    Console.Write(", ");
                }
            }
            Console.WriteLine();

            try {
                if (command.ReturnType == typeof(Task)) {
                    return Ok(await command.InvokeAsync(player.Commands, args));
                }
                return Ok(command.Invoke(player.Commands, args));
            }
			catch (TargetInvocationException tie) {
				return BadRequest($"Execution of command interrupted by exception, message: {tie.InnerException?.Message}");
			}
		}

		[HttpGet("Events/Global")]
		public async Task GlobalEvents() {
            Response.Headers.ContentType = "text/event-stream";
            Response.Headers.CacheControl = "no-cache";
            //Response.Headers.Connection = "keep-alive";
            await Response.Body.FlushAsync();

            _rooms.Events.BindResponse(Response);

            await Task.Delay(-1);
        }
        [HttpGet("Events/Game")]
		public async Task GameEvents() {
			var gameId = ControllerBaseExtensions.GetRequiredQueryValue<int>(this, "id");
			var password = ControllerBaseExtensions.GetQueryValue<string>(this, "password");

			var room = _rooms.GetRequired(gameId);
			if (room.Password?.Length > 0 && room.Password != password) {
				throw new Exception("Wrong room password");
			}

            Response.Headers.ContentType = "text/event-stream";
            Response.Headers.CacheControl = "no-cache";
            //Response.Headers.Connection = "keep-alive";
            await Response.Body.FlushAsync();

            room.Events.BindResponse(Response);

            await Task.Delay(-1);
		}

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

