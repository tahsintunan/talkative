using Microsoft.AspNetCore.Mvc;
using server.Model;

namespace server.Interface;

public interface IHandlerService
{
    Task<IActionResult> Handle();
}