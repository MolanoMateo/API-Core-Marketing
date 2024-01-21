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
    public class CampanasController : Controller
    {
        
            private readonly COREAPIContext _context;
            protected ResultadoApi _resultadoApi;

            public CampanasController(COREAPIContext context)
            {
                _context = context;
                _resultadoApi = new();
            }

            // GET: Usuarios
            [HttpGet("{id}")]
            public async Task<IActionResult> GetCampanas(int id)
            {
            Usuario usuario=await _context.Usuario.FirstAsync(x=>x.Id==id);
            if (usuario==null)
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            List<Campana> usu = await _context.Campana.Where(x=>x.UserID==id).ToListAsync();
            if (usu.Count != 0)
            {
                List<CreCampana> ncr = new List<CreCampana>();
                foreach (Campana cm in usu)
                {
                    List<ResultsCampana> resultsCampanas = new List<ResultsCampana>();
                    List<KPIXCampana> kc = await _context.KPIXCampana.Where(x => x.CampanaID == cm.Id).ToListAsync();
                    foreach (KPIXCampana kpc in kc)
                    {
                        KPI kk = await _context.KPI.FirstAsync(x => x.Id == kpc.KPIID);
                        string r = kpc.Resultado;
                        resultsCampanas.Add(new ResultsCampana { KPI = kk, Result = r });
                    }
                    ncr.Add(new CreCampana { Campana = cm, ResultsCampana = resultsCampanas });
                }
                _resultadoApi.LCreCampana = ncr;
                _resultadoApi.httpResponseCode = HttpStatusCode.OK.ToString();
                return Ok(_resultadoApi);
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
        }

            // GET: Usuarios/Details/5
            [HttpGet("{id, filtro}")]
            public async Task<IActionResult> GetCampanasFiltradas(int id, string filtro)
            {
            string obj = "";
            string canal = "";
            string nombre = "";
            //string cadena = "obj=mensaje?canal=directo?nombre=juan";
            string[] partes = filtro.Split('?');
            if (partes.Length <= 3)
            {
                nombre = ObtenerValor("nombre", partes) != null ? ObtenerValor("nombre", partes) : "a";
                obj = ObtenerValor("obj", partes) != null ? ObtenerValor("obj", partes) : "a";
                canal = ObtenerValor("canal", partes) != null ? ObtenerValor("canal", partes) : "a";
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            Buyer baux = await _context.Buyer.FirstAsync(x => x.Nombre == nombre && x.UserID==id);
            Usuario usuario = await _context.Usuario.FirstAsync(x => x.Id == id);
            if (usuario == null)
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            
            List<Campana> usu = await _context.Campana.Where(x => x.Objetivo==obj || x.Canal == canal || x.BuyerID == baux.Id).ToListAsync();
            usu = usu.Where(x => x.UserID == id).ToList();
            if (usu.Count!=0)
                {
                
                List<CreCampana> ncr = new List<CreCampana>();
                foreach (Campana cm in usu)
                {
                    List<ResultsCampana> resultsCampanas = new List<ResultsCampana>();
                    List<KPIXCampana> kc = await _context.KPIXCampana.Where(x => x.CampanaID == cm.Id).ToListAsync();
                    foreach(KPIXCampana kpc in kc)
                    {
                        KPI kk = await _context.KPI.FirstAsync(x => x.Id==kpc.KPIID);
                        string r = kpc.Resultado;
                        resultsCampanas.Add(new ResultsCampana { KPI=kk,Result=r});
                    }
                    ncr.Add(new CreCampana { Campana=cm,ResultsCampana=resultsCampanas});
                }
                    _resultadoApi.LCreCampana = ncr;
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

            [HttpPost]
            public async Task<IActionResult> PostCampana([FromBody] CreCampana usuario)
            {
            Campana us = await _context.Campana.FirstOrDefaultAsync(x => x.Id == usuario.Campana.Id);
                if (us == null)
                {
                    await _context.Campana.AddAsync(usuario.Campana);
                List <ResultsCampana> a= usuario.ResultsCampana;
                foreach (ResultsCampana k in a)
                {
                    KPIXCampana kp = new KPIXCampana { KPIID=k.KPI.Id,CampanaID=usuario.Campana.Id, Resultado=k.Result};
                    await _context.KPIXCampana.AddAsync(kp);
                }
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

            // GET: Usuarios/Delete/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCampana(int? id)
            {
            Campana us = await _context.Campana.FirstOrDefaultAsync(x => x.Id == id);
            List<KPIXCampana> kc = await _context.KPIXCampana.Where(x => x.CampanaID == id).ToListAsync();
            if (us != null)
                {
                foreach (KPIXCampana c in kc)
                {
                    _context.KPIXCampana.Remove(c);
                }
                    _context.Campana.Remove(us);
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
