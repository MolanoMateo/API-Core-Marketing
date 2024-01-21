using COREAPI.Data;
using COREAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;

namespace COREAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogicController : Controller
    {
        private readonly COREAPIContext _context;
        protected ResultadoApi _resultadoApi;

        public LogicController(COREAPIContext context)
        {
            _context = context;
            _resultadoApi = new();
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
        [HttpGet("{id, filtro}")]
        
        public async Task<IActionResult> LOGIC(int id, string filtro)
        {
            int presupuesto = 0;
            int edad = 0;
            int ingre = 0;
            string obj = "";
            string genero = "";
            string geo = "";
            string KPI = "";
            DateTime fechaI = DateTime.Parse("1001-01-01");
            DateTime fechaV = DateTime.Parse("1001-01-01");

            //string cadena = "obj=x?presupuesto=x?edad=x?geo=x?genero=x?ingre=x?KPI=x?fechaI=x?fechaV=x";
            string[] partes = filtro.Split('?');
            if (partes.Length <= 4)
            {
                presupuesto = ObtenerValor("presupuesto", partes) != null ? int.Parse(ObtenerValor("presupuesto", partes)) : 0;
                ingre = ObtenerValor("ingre", partes) != null ? int.Parse(ObtenerValor("ingre", partes)) : 0;
                edad = ObtenerValor("edad", partes) != null ? int.Parse(ObtenerValor("edad", partes)) : 0;

                obj = ObtenerValor("obj", partes) != null ? ObtenerValor("obj", partes) : "a";
                genero = ObtenerValor("genero", partes) != null ? ObtenerValor("genero", partes) : "a";
                KPI = ObtenerValor("KPI", partes) != null ? ObtenerValor("KPI", partes) : "a";
                geo = ObtenerValor("geo", partes) != null ? ObtenerValor("geo", partes) : "a";
                fechaI = ObtenerValor("fechaI", partes) != null ? DateTime.Parse(ObtenerValor("fechaI", partes)) : DateTime.Parse("1001-01-01");
                fechaV = ObtenerValor("fechaV", partes) != null ? DateTime.Parse(ObtenerValor("fechaV", partes)) : DateTime.Parse("1001-01-01");
            }
            else
            {
                _resultadoApi.httpResponseCode = HttpStatusCode.BadRequest.ToString();
                return BadRequest(_resultadoApi);
            }
            string[] kpiselect = KPI.Split('-');
            List<Campana> usu = await _context.Campana.Where(x => x.Objetivo == obj || x.Presupuesto == presupuesto || x.fechaInicio == fechaI || x.fechaVencimiento == fechaV).ToListAsync();
            usu = usu.Where(x => x.UserID == id).ToList();
            List<KPI> Lkpi = new List<KPI>();
            List<Buyer> Lbuy = await _context.Buyer.Where(x=>x.Edad==edad||x.Genero==genero||x.Ingresos==ingre||x.Geografia==geo).ToListAsync();
            Lbuy = Lbuy.Where(x => x.UserID == id).ToList();
            foreach (string kpc in kpiselect)
            {
                Lkpi.Add(await _context.KPI.FirstAsync(x=>x.Nombre==kpc));
            }
            if (usu == null)
            {
                usu = await _context.Campana.Where(x => x.UserID == id).ToListAsync();
            }
            if (Lbuy!=null)
            {
                List<Campana> auxfilt = new List<Campana>();
                foreach (var item in Lbuy)
                {
                    List <Campana> temp= usu.Where(x=>x.BuyerID==item.Id).ToList();
                    auxfilt = auxfilt.Concat(temp).ToList();
                }
                usu = auxfilt;
            }
            List<KPIXCampana> kpcc = new List<KPIXCampana>();
            if (Lkpi != null)
            {
                foreach (var item in Lkpi)
                {
                    kpcc = kpcc.Concat(await _context.KPIXCampana.Where(x =>x.KPIID==item.Id).ToListAsync()).ToList();
                }
            }
            int kpifilt = 0;
            if(kpcc == null)
            {
                kpcc=await _context.KPIXCampana.ToListAsync();
                kpifilt = 1;
            }
            List<Logic> lista = new List<Logic>();
            foreach (var item in usu)
                {
                    List<KPIXCampana> kpres = kpcc.Where(x => x.CampanaID == item.Id).ToList();
                List<ResultsCampana> resultsCampanas = new List<ResultsCampana>();
                foreach (KPIXCampana kp in kpres)
                {
                    KPI kpii=await _context.KPI.FirstAsync(x=>x.Id==kp.KPIID);
                    resultsCampanas.Add(new ResultsCampana { KPI=kpii,Result=kp.Resultado});
                }
                Buyer bb = await _context.Buyer.FirstAsync(x => x.Id==item.BuyerID);
                if (kpifilt == 1)
                {
                    lista.Add(new Logic { campana = item, buyer = bb, ResultsCampana = resultsCampanas });
                }
                else if(kpifilt == 0 && kpres!=null)
                {
                    lista.Add(new Logic { campana = item, buyer = bb, ResultsCampana = resultsCampanas });
                }
                else
                {

                }
                }
            
            if (lista.Count != 0)
            {


                if (Lkpi != null)
                {
                    foreach (KPI kpi in Lkpi)
                    {
                        List<int> analitic=new List<int>();
                        int res;
                        foreach (Logic g in lista)
                        {
                            ResultsCampana resc=g.ResultsCampana.Where(x=>x.KPI==kpi).First();
                            if (resc!=null) { analitic.Add(int.Parse(resc.Result)); }
                        }
                        if (kpi.flag == 0)
                        {
                            //result menor
                            res = analitic.Min();
                        }
                        else
                        {
                            //result mayor
                            res = analitic.Max();
                        }
                        foreach (Logic g in lista)
                        {

                            ResultsCampana resc = g.ResultsCampana.Where(x => x.KPI == kpi).First();
                            if (resc != null) {
                                if (int.Parse(resc.Result)!=res)
                                {
                                    g.ResultsCampana.Remove(resc);
                                }
                            }
                        }
                    }
                    //remove empty
                    foreach (Logic g in lista)
                    {
                        if (g.ResultsCampana.Count==0)
                        {
                            lista.Remove(g);
                        }
                    }
                }
                
                _resultadoApi.LLogic = lista;
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
