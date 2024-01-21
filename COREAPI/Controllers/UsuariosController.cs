using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COREAPI.Data;
using COREAPI.Models;
using System.Net;

namespace COREAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private readonly COREAPIContext _context;
        protected ResultadoApi _resultadoApi;

        public UsuariosController(COREAPIContext context)
        {
            _context = context;
            _resultadoApi = new();
        }

        // GET: Usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usu = await _context.Usuario.ToListAsync();

            if (usu != null)
            {
                _resultadoApi.LUsuario = usu;
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            return Ok(_resultadoApi);
        }

        // GET: Usuarios/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioId(int? id)
        {
            Usuario usu = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);
            if (usu != null)
            {
                _resultadoApi.NUsuario = usu;
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            Usuario us = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == usuario.Id);
            if (us == null)
            {
                usuario.Roles = us.Roles;
                await _context.Usuario.AddAsync(usuario);
                await _context.SaveChangesAsync();
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString().ToUpper();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString().ToUpper();
                return BadRequest(_resultadoApi);
            }
        }

        // GET: Usuarios/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUsuario(int id, [FromBody] Usuario usuario)
        {
            Usuario usunew = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);

            if (usunew != null)
            {
                usunew.Nombre = usuario.Nombre != null ? usuario.Nombre : usunew.Nombre;
                usunew.Correo = usuario.Correo != null ? usuario.Correo : usunew.Correo;
                usunew.Clave = usuario.Clave != null ? usuario.Clave : usunew.Clave;
                usunew.Roles = usuario.Roles != null ? usuario.Roles : usunew.Roles;

                _context.Update(usunew);
                await _context.SaveChangesAsync();
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
        }



        // GET: Usuarios/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int? id)
        {
            Usuario us = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);
            if (us != null)
            {
                _context.Usuario.Remove(us);
                await _context.SaveChangesAsync();
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
        }
    }
}
