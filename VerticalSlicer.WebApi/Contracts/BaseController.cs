using Microsoft.AspNetCore.Mvc;

namespace VerticalSlicer.WebApi.Contracts;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
    
}