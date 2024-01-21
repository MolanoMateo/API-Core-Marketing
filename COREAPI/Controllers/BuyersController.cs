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
    public class BuyersController : Controller
    {
        
            private readonly COREAPIContext _context;
            protected ResultadoApi _resultadoApi;

            public BuyersController(COREAPIContext context)
            {
                _context = context;
                _resultadoApi = new();
            }

            // GET: Usuarios
            [HttpGet]
            public async Task<IActionResult> GetBuyers()
            {
                var usu = await _context.Buyer.ToListAsync();

                if (usu != null)
                {
                    _resultadoApi.LBuyer = usu;
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
        [HttpGet("{filtro}")]
        public async Task<IActionResult> GetBuyersFiltrados(string filtro)
        {
            int edad = 0;
            string Gen = "";
            int Ingre = 0;
            string geo = "";
            //string cadena = "obj=x?canal=x?nombre=x?geo=x";
            string[] partes = filtro.Split('?');
            if (partes.Length <= 4)
            {
                edad = ObtenerValor("edad", partes) != null ? int.Parse(ObtenerValor("edad", partes)) : 0;
                Gen = ObtenerValor("Gen", partes) != null ? ObtenerValor("Gen", partes) : "a";
                Ingre = ObtenerValor("Ingre", partes) != null ? int.Parse(ObtenerValor("Ingre", partes)) : 0;
                geo = ObtenerValor("geo", partes) != null ? ObtenerValor("geo", partes) : "a";
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            List<Buyer> usu = await _context.Buyer.Where(x => x.Edad == edad || x.Genero == Gen || x.Ingresos == Ingre || x.Geografia == geo).ToListAsync();
            if (usu.Count != 0)
            {
                _resultadoApi.LBuyer = usu;
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
        }



        static string ObtenerValor(string clave, string[] partes)
        {
            // Buscar la parte que comienza con la clave y obtener el valor después del igual
            foreach (var parte in partes)
            {
                if (parte.StartsWith(clave + "="))
                {
                    return parte.Substring(clave.Length + 1); // +1 para omitir el igual
                }
            }
            return null; // Devolver null si no se encuentra la clave
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
            public async Task<IActionResult> PostBuyer([FromBody] Buyer usuario)
            {
            Buyer us = await _context.Buyer.FirstOrDefaultAsync(x => x.Id == usuario.Id);
                if (us == null)
                {
                    await _context.Buyer.AddAsync(usuario);
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
        }
    }
