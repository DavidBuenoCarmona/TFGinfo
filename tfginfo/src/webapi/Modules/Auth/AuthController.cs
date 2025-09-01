using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

/// <summary>
/// Controlador para autenticación y gestión de usuarios.
/// </summary>
[Route("/auth")]
[ApiController]
public class AuthController : BaseController
{
    /// <summary>
    /// Constructor del controlador de autenticación.
    /// </summary>
    public AuthController(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration) { }

    /// <summary>
    /// Inicia sesión con credenciales de usuario.
    /// </summary>
    /// <param name="credentails">Credenciales de acceso (usuario y contraseña).</param>
    /// <returns>Token JWT y datos del usuario si la autenticación es correcta.</returns>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginCredentials credentails)
    {
        try
        {
            AuthManager manager = new AuthManager(context, configuration);
            return Ok(manager.Login(credentails));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    /// <summary>
    /// Cambia la contraseña de un usuario autenticado.
    /// </summary>
    /// <param name="request">Datos para el cambio de contraseña.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            AuthManager manager = new AuthManager(context, configuration);
            return Ok(manager.ChangePassword(request));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    /// <summary>
    /// Crea un usuario administrador.
    /// Requiere credenciales válidas.
    /// </summary>
    /// <param name="credentials">Credenciales del nuevo administrador.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("create-admin")]
    public IActionResult CreateAdmin([FromBody] LoginCredentials credentials)
    {
        try
        {
            AuthManager manager = new AuthManager(context, configuration);
            manager.CreateAdmin(credentials);
            return Ok();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    /// <summary>
    /// Verifica la validez de un token JWT.
    /// </summary>
    /// <param name="token">Objeto con el token a verificar.</param>
    /// <returns>Resultado de la verificación (válido o no).</returns>
    [HttpPost("check-token")]
    public IActionResult CheckToken([FromBody] TokenObject token)
    {
        try
        {
            AuthManager manager = new AuthManager(context, configuration);
            return Ok(manager.CheckToken(token.token));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }
}