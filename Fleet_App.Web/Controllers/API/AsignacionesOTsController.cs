using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Web.Data;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fleet_App.Common.Codigos;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionesOTsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AsignacionesOTsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static int?[] CodigosOcultar = new int?[]
       {
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            30,
       };

        [HttpPost]
        [Route("GetRemotes/{UserID}")]
        public async Task<IActionResult> GetRemotes(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => (o.UserID == UserID) && (o.PROYECTOMODULO == "Otro") && ((o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC")))
            .Where(o => !CodigosOcultar.Contains(o.CodigoCierre))
           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new
           {
               r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.PROVINCIA,
               r.ReclamoTecnicoID,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.FechaInicio,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,
               r.FechaCita,
               r.MedioCita,
               r.NroSeriesExtras,
               r.Evento1,
               r.FechaEvento1,
               r.Evento2,
               r.FechaEvento2,
               r.Evento3,
               r.FechaEvento3,
               r.Evento4,
               r.FechaEvento4,
               r.Observacion




           })
           .Select(g => new
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO = g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               FechaInicio = g.Key.FechaInicio,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               FechaCita = g.Key.FechaCita,
               MedioCita = g.Key.MedioCita,
               NroSeriesExtras = g.Key.NroSeriesExtras,
               Evento1=g.Key.Evento1,
               FechaEvento1 = g.Key.FechaEvento1,
               Evento2 = g.Key.Evento2,
               FechaEvento2 = g.Key.FechaEvento2,
               Evento3 = g.Key.Evento3,
               FechaEvento3 = g.Key.FechaEvento3,
               Evento4 = g.Key.Evento4,
               FechaEvento4 = g.Key.FechaEvento4,
               Observacion=g.Key.Observacion,

               CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Controles Remotos para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetCables/{UserID}")]
        public async Task<IActionResult> GetCables(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var codigosMostrar = new List<int?> { 2, 3, 5, 6, 7, 8, 9, 10, 11, 13, 14, 30 };

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o =>
                    o.UserID == UserID
                    && (o.PROYECTOMODULO == "Cable")
                    && ((o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC")))
           //&& codigosMostrar.Contains(o.CodigoCierre) ))
           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new
           {
               r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,

               r.PROVINCIA,
               r.ReclamoTecnicoID,
               r.Motivos,
               r.FechaCita,
               r.MedioCita,
               r.NroSeriesExtras,
               r.Evento1,
               r.FechaEvento1,
               r.Evento2,
               r.FechaEvento2,
               r.Evento3,
               r.FechaEvento3,
               r.Evento4,
               r.FechaEvento4,
               r.Observacion,
               r.TelefAlternativo1,
               r.TelefAlternativo2,
               r.TelefAlternativo3,
               r.TelefAlternativo4

           })
           .Select(g => new
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO = g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               Motivos = g.Key.Motivos,
               FechaCita = g.Key.FechaCita,
               MedioCita = g.Key.MedioCita,
               NroSeriesExtras = g.Key.NroSeriesExtras,
               Evento1 = g.Key.Evento1,
               FechaEvento1 = g.Key.FechaEvento1,
               Evento2 = g.Key.Evento2,
               FechaEvento2 = g.Key.FechaEvento2,
               Evento3 = g.Key.Evento3,
               FechaEvento3 = g.Key.FechaEvento3,
               Evento4 = g.Key.Evento4,
               FechaEvento4 = g.Key.FechaEvento4,
               Observacion = g.Key.Observacion,
               TelefAlternativo1 = g.Key.TelefAlternativo1,
               TelefAlternativo2 = g.Key.TelefAlternativo2,
               TelefAlternativo3 = g.Key.TelefAlternativo3,
               TelefAlternativo4 = g.Key.TelefAlternativo4,
           



            CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Recuperos de Cablevisión para este Usuario.");
            }

            return Ok(orders);
        }
        
        [HttpPost]
        [Route("GetTasas/{UserID}")]
        public async Task<IActionResult> GetTasas(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
               
           .Where(o => (o.UserID == UserID) 
                        && (o.PROYECTOMODULO == "Tasa") 
                        && (o.ESTADOGAOS == "PEN" || o.ESTADOGAOS == "INC" && o.CodigoCierre != 44 && o.CodigoCierre <= 50 && o.CodigoCierre > 40)
                        )
           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new
           {
               r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.CodigoCierre,
               //r.ObservacionCaptura,
               r.Novedades,
               r.PROVINCIA,
               r.ReclamoTecnicoID,
               //r.Motivos,
               r.FechaCita,
               r.MedioCita,
               r.NroSeriesExtras,
               r.Evento1,
               r.FechaEvento1,
               r.Evento2,
               r.FechaEvento2,
               r.Evento3,
               r.FechaEvento3,
               r.Evento4,
               r.FechaEvento4,
               //r.MODELO,
               r.Observacion,
               
               r.TelefAlternativo1,
               r.TelefAlternativo2,
               r.TelefAlternativo3,
               r.TelefAlternativo4
           })
           .Select(g => new
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO = g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               CodigoCierre = g.Key.CodigoCierre,
               //ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               //Motivos = g.Key.Motivos,
               FechaCita = g.Key.FechaCita,
               MedioCita = g.Key.MedioCita,
               NroSeriesExtras = g.Key.NroSeriesExtras,
               Evento1 = g.Key.Evento1,
               FechaEvento1 = g.Key.FechaEvento1,
               Evento2 = g.Key.Evento2,
               FechaEvento2 = g.Key.FechaEvento2,
               Evento3 = g.Key.Evento3,
               FechaEvento3 = g.Key.FechaEvento3,
               Evento4 = g.Key.Evento4,
               FechaEvento4 = g.Key.FechaEvento4,
               //Modelo = g.Modelo,
               Observacion=g.Key.Observacion,
               TelefAlternativo1 = g.Key.TelefAlternativo1,
               TelefAlternativo2 = g.Key.TelefAlternativo2,
               TelefAlternativo3 = g.Key.TelefAlternativo3,
               TelefAlternativo4 = g.Key.TelefAlternativo4,
              

               CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Recuperos de Tasa para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetDevoluciones/{UserID}")]
        public async Task<IActionResult> GetDevoluciones(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => (o.UserID == UserID)
                        && ((o.CodigoCierre == 60) || (o.CodigoCierre == 13))
                         && (o.Recupero == 0)
                         && (o.PasaDefinitiva == 0)
                         && (o.IDCABECERACERTIF == 0)
                         && (o.ESTADO == "")
                         && (o.PROYECTOMODULO == "Tasa")
                            )
           .OrderBy(o => o.PROYECTOMODULO)
           .GroupBy(r => new
           {
               r.PROYECTOMODULO,
               
           })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               Cantidad = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("Este Usuario no tiene Equipos Recuperados para devolver.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetAlls/{UserID}")]
        public async Task<IActionResult> GetAlls(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)

           .Where(o => 
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Tasa") && (o.ESTADOGAOS == "PEN")
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Cable") && (o.ESTADOGAOS == "PEN")
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Otro") && (o.ESTADOGAOS == "PEN")
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Teco") && (o.ESTADOGAOS == "PEN")
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "DTV") && (o.ESTADOGAOS == "PEN")
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Tasa") && (o.ESTADOGAOS == "INC") //&& (o.CodigoCierre <= 50) && (o.CodigoCierre > 40)//
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Otro") && (o.ESTADOGAOS == "INC") //&& (o.CodigoCierre <= 13) && (o.CodigoCierre > 0)
                       ||
                       (o.UserID == UserID) && (o.PROYECTOMODULO == "Cable") && (o.ESTADOGAOS == "INC") && (o.CodigoCierre <= 13) && (o.CodigoCierre > 0)
                       ) 


           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new
           {
               r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,
               r.Observacion,
               r.PROVINCIA,
               r.ReclamoTecnicoID,
               r.Motivos,
               r.FechaCita,
               r.MedioCita,
               r.NroSeriesExtras,
               r.Evento1,
               r.FechaEvento1,
               r.Evento2,
               r.FechaEvento2,
               r.Evento3,
               r.FechaEvento3,
               r.Evento4,
               r.FechaEvento4
           })
           .Select(g => new
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO = g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               Motivos = g.Key.Motivos,
               FechaCita = g.Key.FechaCita,
               MedioCita = g.Key.MedioCita,
               NroSeriesExtras = g.Key.NroSeriesExtras,
               Observacion = g.Key.Observacion,
               Evento1 = g.Key.Evento1,
               FechaEvento1 = g.Key.FechaEvento1,
               Evento2 = g.Key.Evento2,
               FechaEvento2 = g.Key.FechaEvento2,
               Evento3 = g.Key.Evento3,
               FechaEvento3 = g.Key.FechaEvento3,
               Evento4 = g.Key.Evento4,
               FechaEvento4 = g.Key.FechaEvento4,


               CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay tareas pendientes para este Usuario.");
            }

            return Ok(orders);
        }



        // PUT: api/AsignacionesOTs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsignacionesOT([FromRoute] int id, [FromBody] AsignacionesOT asignacionesOT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asignacionesOT.IDREGISTRO)
            {
                return BadRequest();
            }

            var oldasignacionesOT = await _dataContext.AsignacionesOTs.FindAsync(asignacionesOT.IDREGISTRO);
            if (oldasignacionesOT == null)
            {
                return BadRequest("El registro no existe.");
            }


            var imageUrlDNI = oldasignacionesOT.UrlDni;
            if (asignacionesOT.ImageArrayDni != null && asignacionesOT.ImageArrayDni.Length > 0)
            {
                var stream = new MemoryStream(asignacionesOT.ImageArrayDni);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\DNI";                 //Alt126 -->  ~
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrlDNI = fullPath;
                }
            }
            
            var imageUrlFirma = oldasignacionesOT.UrlDni;
            if (asignacionesOT.ImageArrayFirma != null && asignacionesOT.ImageArrayFirma.Length > 0)
            {
                var stream = new MemoryStream(asignacionesOT.ImageArrayFirma);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.png";
                var folder = "wwwroot\\images\\Firmas";                 //Alt126 -->  ~
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrlFirma = fullPath;
                }
            }

            oldasignacionesOT.ESTADOGAOS = asignacionesOT.ESTADOGAOS;
            oldasignacionesOT.UrlDni = imageUrlDNI;
            oldasignacionesOT.UrlFirma = imageUrlFirma;
            oldasignacionesOT.CodigoCierre = asignacionesOT.CodigoCierre;
            oldasignacionesOT.FECHACUMPLIDA = asignacionesOT.FECHACUMPLIDA;
            oldasignacionesOT.HsCumplidaTime = asignacionesOT.HsCumplidaTime;
            oldasignacionesOT.DECO1 = asignacionesOT.DECO1;
            oldasignacionesOT.ESTADO2 = asignacionesOT.ESTADO2;
            oldasignacionesOT.ESTADO3 = asignacionesOT.ESTADO3;
            oldasignacionesOT.Observacion = asignacionesOT.Observacion;
            oldasignacionesOT.FechaCita = asignacionesOT.FechaCita;
            oldasignacionesOT.MedioCita = asignacionesOT.MedioCita;
            oldasignacionesOT.NroSeriesExtras = asignacionesOT.NroSeriesExtras;
            oldasignacionesOT.Evento1 = asignacionesOT.Evento1;
            oldasignacionesOT.FechaEvento1 = asignacionesOT.FechaEvento1;
            oldasignacionesOT.Evento2 = asignacionesOT.Evento2;
            oldasignacionesOT.FechaEvento2 = asignacionesOT.FechaEvento2;
            oldasignacionesOT.Evento3 = asignacionesOT.Evento3;
            oldasignacionesOT.FechaEvento3 = asignacionesOT.FechaEvento3;
            oldasignacionesOT.Evento4 = asignacionesOT.Evento4;
            oldasignacionesOT.FechaEvento4 = asignacionesOT.FechaEvento4;

            _dataContext.AsignacionesOTs.Update(oldasignacionesOT);
            await _dataContext.SaveChangesAsync();
            return Ok(asignacionesOT);
        }

        [HttpPost]
        [Route("GetTrabajos")]
        public async Task<IActionResult> GetTrabajos(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
           .Include(m => m.ControlesEquivalencia)
           .Where(o => (o.UserID == trabajosRequest.UserID) 
           && (o.FechaAsignada >= trabajosRequest.Desde) 
           && (o.FechaAsignada <= trabajosRequest.Hasta) 
           && (o.PROYECTOMODULO == trabajosRequest.Proyecto))
           .OrderBy(o => o.PROYECTOMODULO)
           .GroupBy(r => new
                        {
                            r.PROYECTOMODULO,
                            r.ESTADOGAOS
                            
                         })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               Cant = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetTrabajos2")]
        public async Task<IActionResult> GetTrabajos2(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => (o.UserID == trabajosRequest.UserID) && (o.FECHACUMPLIDA >= trabajosRequest.Desde) && (o.FECHACUMPLIDA <= trabajosRequest.Hasta) && (o.PROYECTOMODULO == trabajosRequest.Proyecto))
           .OrderBy(o => o.PROYECTOMODULO)
           .GroupBy(r => new
           {
               r.PROYECTOMODULO,
               r.ESTADOGAOS,
           })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               Cant = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetTrabajos3")]
        public async Task<IActionResult> GetTrabajos3(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Where(o => (
                                o.UserID == trabajosRequest.UserID) 
                            && (o.PROYECTOMODULO == trabajosRequest.Proyecto)
                            && (o.HsCumplidaTime >= trabajosRequest.Desde) 
                            && (o.HsCumplidaTime <= trabajosRequest.Hasta) 
                            )
           .GroupBy(r => new
           {
               r.PROYECTOMODULO,
               r.CodigoCierre,
           })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               CodigoCierre = g.Key.CodigoCierre,
               Cant = g.Select(x => x.DOMICILIO).Distinct().Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetTrabajos4")]
        public async Task<IActionResult> GetTrabajos4(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => 
                    (o.UserID == trabajosRequest.UserID) 
                    && (o.FechaAsignada >= trabajosRequest.Desde) 
                    && (o.FechaAsignada <= trabajosRequest.Hasta) 
                    && (o.PROYECTOMODULO == trabajosRequest.Proyecto)
                    //&& (o.CodigoCierre == 60)
                    )
           .OrderBy(o => o.PROYECTOMODULO)
           .GroupBy(r => new
           {
               r.PROYECTOMODULO,
           })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               Cant = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetTrabajos5")]
        public async Task<IActionResult> GetTrabajos5(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
                .Where(o => (o.UserID == trabajosRequest.UserID) 
                            && (o.FECHACUMPLIDA >= trabajosRequest.Desde) 
                            && (o.FECHACUMPLIDA <= trabajosRequest.Hasta) 
                            && (o.PROYECTOMODULO == trabajosRequest.Proyecto))
                .OrderBy(o => o.PROYECTOMODULO)
                .GroupBy(r => new
           {
               r.PROYECTOMODULO,
               r.FechaInicio,
               r.FechaAsignada,
               r.FECHACUMPLIDA,
           })
           .Select(g => new
           {
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               FechaInicio = g.Key.FechaInicio,
               FechaAsignada = g.Key.FechaAsignada,
               FECHACUMPLIDA = g.Key.FECHACUMPLIDA,
               Cant = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetModelosTotales")]
        public async Task<IActionResult> GetModelosTotales(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
                .Where(o => (o.UserID == trabajosRequest.UserID)
                            && (o.FECHACUMPLIDA >= trabajosRequest.Desde)
                            && (o.FECHACUMPLIDA <= trabajosRequest.Hasta)
                            && (o.PROYECTOMODULO == trabajosRequest.Proyecto)
                            && (o.CodigoCierre == 60)
                            )
                .OrderBy(o => o.PROYECTOMODULO)
                .GroupBy(r => new
                {
                    r.PROYECTOMODULO,
                    r.DECO1,
                })
                .Select(g => new ModelosResponse()
                {
                    Modelo = g.Key.DECO1,
                    ProyectoModulo = g.Key.PROYECTOMODULO,
                    Cant = g.Count(),
                }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }


        [HttpPost]
        [Route("GetAdicionalesTotales")]
        public async Task<IActionResult> GetAdicionalesTotales(TrabajosRequest trabajosRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            var orders = await _dataContext.AsignacionesOtsEquiposExtras
                .Where(o => (o.IDTECNICO == trabajosRequest.UserID)
                            && (o.FECHACARGA >= trabajosRequest.Desde)
                            && (o.FECHACARGA <= trabajosRequest.Hasta))
                .GroupBy(r => new
                {
                    r.CODDECO1,
                })
                .Select(g => new ModelosResponse()
                {
                    Modelo = g.Key.CODDECO1,
                    ProyectoModulo = null,
                    Cant = g.Count(),
                }).ToListAsync();

            
            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }
    }
}
