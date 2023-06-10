using System;
namespace Fiorello.Services.Interfaces
{
	public interface ISettingService
	{
        Dictionary<string, string> GetAll();
    }
}

