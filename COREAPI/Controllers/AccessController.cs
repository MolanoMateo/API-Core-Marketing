using COREAPI.Data;
using COREAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace COREAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : Controller
    {
        private readonly COREAPIContext _context;
        protected ResultadoApi _resultadoApi;

        public AccessController(COREAPIContext context)
        {
            _context = context;
            _resultadoApi = new();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Credenciales credendiales)
        {
            Usuario usu = await _context.Usuario.FirstOrDefaultAsync(x => x.Correo == credendiales.username && x.Clave == credendiales.password);
            if (usu != null)
            {
                _resultadoApi.NUsuario = usu;
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString().ToUpper();
                return BadRequest(_resultadoApi);
            }
        }
    }
}
