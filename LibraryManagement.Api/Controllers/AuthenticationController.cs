using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.DTOs.Users;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilter<RegisterMemberUserDto>))]
    [ProducesResponseType(typeof(ReadMemberDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReadMemberDto>> RegisterMember(RegisterMemberUserDto registerDto)
    {
        var result = await _authenticationService.RegisterMemberAsync(registerDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return CreatedAtRoute("GetMemberById", new { result.Value.Id }, result.Value);
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpPost("register-librarian")]
    [ServiceFilter(typeof(ValidationFilter<RegisterLibrarianUserDto>))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> RegisterLibrarian(RegisterLibrarianUserDto registerDto)
    {
        var result = await _authenticationService.RegisterLibrarianAsync(registerDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Created();
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilter<LoginDto>))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult<string>> Login(LoginDto registerDto)
    {
        var result = await _authenticationService.LoginAsync(registerDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }
}
