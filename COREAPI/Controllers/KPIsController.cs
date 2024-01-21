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
    public class KPIsController : Controller
    {
        
            private readonly COREAPIContext _context;
            protected ResultadoApi _resultadoApi;

            public KPIsController(COREAPIContext context)
            {
                _context = context;
                _resultadoApi = new();
            }

            // GET: Usuarios
            [HttpGet]
            public async Task<IActionResult> GetKPIs()
            {
                var usu = await _context.KPI.ToListAsync();

                if (usu != null)
                {
                    _resultadoApi.LKPI = usu;
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
            public async Task<IActionResult> GetKPIId(int? id)
            {
                KPI usu = await _context.KPI.FirstOrDefaultAsync(x => x.Id == id);
                if (usu != null)
                {
                    _resultadoApi.NKPI = usu;
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
            public async Task<IActionResult> PostKPI([FromBody] KPI usuario)
            {
                KPI us = await _context.KPI.FirstOrDefaultAsync(x => x.Id == usuario.Id);
                if (us == null)
                {
                    await _context.KPI.AddAsync(usuario);
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
            public async Task<IActionResult> EditKPI(int id, [FromBody] KPI usuario)
            {
            KPI usunew = await _context.KPI.FirstOrDefaultAsync(x => x.Id == id);

                if (usunew != null)
                {
                    usunew.Nombre = usuario.Nombre != null ? usuario.Nombre : usunew.Nombre;
                    usunew.Descripcion = usuario.Descripcion != null ? usuario.Descripcion : usunew.Descripcion;

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
            public async Task<IActionResult> DeleteKPI(int? id)
            {
            KPI us = await _context.KPI.FirstOrDefaultAsync(x => x.Id == id);
                if (us != null)
                {
                    _context.KPI.Remove(us);
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
