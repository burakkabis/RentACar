using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class BaseController:ControllerBase
{
    private IMediator? _mediator;
    //Daha once mediator injetke edilmisse olnu dondur.Yoksa injeksiyon ortaminda bana IMediator un injeksinini ver demis oluyoruz.
    protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


}
