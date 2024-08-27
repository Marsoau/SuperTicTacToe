using Microsoft.AspNetCore.Mvc;
using SuperTicTacToe.API.Controllers;
using System.ComponentModel;

namespace SuperTicTacToe.API.Extensions
{
    public static class ControllerBaseExtensions
    {
		public static T GetRequiredQueryValue<T>(this ControllerBase controller, string key){
			var value = controller.Request.Query[key].FirstOrDefault()
				?? throw new Exception($"No required \"{key}\" found in query");

			var converter = TypeDescriptor.GetConverter(typeof(T));

			return (T)(converter.ConvertFromInvariantString(value) ?? throw new Exception($"Cant convert value \"{value}\" to \"{typeof(T).Name}\""));
		}
		public static T? GetQueryValue<T>(this ControllerBase controller, string key) where T : class {
			var value = controller.Request.Query[key].FirstOrDefault();
			if (value is null) return null;

			var converter = TypeDescriptor.GetConverter(typeof(T));

			try {
                return (T?)converter.ConvertFromInvariantString(value);
			}
			catch {
				return null;
			}
		}
    }
}
