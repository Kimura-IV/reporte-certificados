using certificados.models;
using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.auditoria;
using certificados.models.Entitys.dbo;
using certificados.models.Helper;
using certificados.services.Utils;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace certificados.dal.DataAccess
{
    public class TcertificadoDA(AppDbContext appDbContext)
    {
        private readonly AppDbContext context = appDbContext;

        public ResponseApp InsertarCertificado(Tcertificado tcertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    tcertificado.TformatoCertificado = context.TformatoCertificado.Local.FirstOrDefault(e => e.idFormato == tcertificado.TformatoCertificado.idFormato)
                                                        ?? context.TformatoCertificado.Find(tcertificado.IdFormato);


                    context.Tcertificado.Add(tcertificado);
                    context.SaveChanges();

                    var certifiAudit = AuditHelper.ConvertToAudit<Tcertificado, TcertificadoAuditoria>(tcertificado);
                    certifiAudit.IdCertificado = 0;
                    context.TcertificadoAuditoria.Add(certifiAudit);
                    context.SaveChanges();

                    transaction.Commit();
                    response = Utils.OkResponse(tcertificado);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse("Error al insertar CERTIFICADO");

                    throw new Exception($"ERROR AL PROCESAR LA INSERACION DE UN CERTIFICADO {ex.Message}");
                }
                return response;
            }


        }

        public ResponseApp ModificarCertificado(Tcertificado certificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Buscar el certificado existente
                    var certificadoExistente = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == certificado.IdCertificado);

                    if (certificadoExistente != null)
                    {


                        certificadoExistente.TformatoCertificado = context.TformatoCertificado.Find(certificado.TformatoCertificado.idFormato) ?? certificado.TformatoCertificado;

                        // Actualizar los campos del certificado
                        certificadoExistente.Titulo = certificado.Titulo;
                        certificadoExistente.Imagen = certificado.Imagen;
                        certificadoExistente.IdFormato = certificado.IdFormato;
                        certificadoExistente.Tipo = certificado.Tipo;
                        certificadoExistente.Estado = certificado.Estado;
                        certificadoExistente.FModificacion = Utils.timeParsed(DateTime.Now);
                        certificadoExistente.UsuarioActualizacion = certificado.UsuarioActualizacion;

                        // Guardar los cambios
                        context.SaveChanges();
                        transaction.Commit();
                        response = Utils.OkResponse(certificadoExistente);
                    }
                    else
                    {
                        // Si el certificado no existe
                        response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = Utils.BadResponse($"ERROR AL MODIFICAR CERTIFICADO: {ex.Message}");
                    throw new Exception($"ERROR AL MODIFICAR CERTIFICADO: {ex.Message}");
                }
                return response;
            }

        }

        public ResponseApp EliminarCertificado(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el certificado existente
                var certificadoExistente = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == idCertificado);

                if (certificadoExistente != null)
                {
                    // Eliminar el certificado
                    context.Tcertificado.Remove(certificadoExistente);
                    context.SaveChanges();

                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(certificadoExistente);
                }
                else
                {
                    // Si el certificado no existe
                    response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL ELIMINAR CERTIFICADO: {ex.Message}");
                throw new Exception($"ERROR AL ELIMINAR CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarCertificados()
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado.ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp ListarCertificadosPorEvento(int idEvento)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado
                    .Where(c => c.IdCertificado == idEvento)
                    .ToList();

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }
        public ResponseApp CertificadosById(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {

                var listaCertificados = context.Tcertificado
                    .Include(gp => gp.TformatoCertificado)

                    .Where(c => c.IdCertificado == idCertificado)
                    .FirstOrDefault();
                if (listaCertificados == null)
                    return Utils.BadResponse("NO EXISTE EL CERTIDICADO");

                // Retornar respuesta exitosa con la lista
                response = Utils.OkResponse(listaCertificados);
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL LISTAR CERTIFICADOS: {ex.Message}");
                throw new Exception($"ERROR AL listar CERTIFICADO: {ex.Message}");
            }
            return response;
        }

        public ResponseApp BuscarCertificado(int idCertificado)
        {
            ResponseApp response = Utils.BadResponse(null);
            try
            {
                // Buscar el certificado por su IdCertificado
                var certificado = context.Tcertificado.FirstOrDefault(c => c.IdCertificado == idCertificado);

                if (certificado != null)
                {
                    // Retornar respuesta exitosa
                    response = Utils.OkResponse(certificado);
                }
                else
                {
                    // Si el certificado no existe
                    response = Utils.BadResponse("CERTIFICADO NO EXISTE");
                }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL BUSCAR CERTIFICADO: {ex.Message}");
                throw new Exception($"ERROR AL BUSCAR CERTIFICADO: {ex.Message}");
            }
            return response;
        }
        public FiltroCertificado GetFiltros()
        {
            FiltroCertificado filtros = new FiltroCertificado();
            try
            {
                filtros.Plantillas = context.TformatoCertificado.Select(x => new TformatoCertificado
                {
                    idFormato = x.idFormato,
                    NombrePlantilla = x.NombrePlantilla
                }).ToList();
                filtros.Tipos = context.Tcertificado.Where(x => !string.IsNullOrEmpty(x.Tipo)).Select(x => x.Tipo!).Distinct().ToList();
                filtros.Firmantes = context.TformatoCertificado
                    .Select(x => new { x.CargoFirmanteDos, x.CargoFirmanteTres, x.CargoFirmanteUno })
                    .AsEnumerable()
                    .SelectMany(x => new[] { x.CargoFirmanteDos, x.CargoFirmanteTres, x.CargoFirmanteUno })
                    .Where(nombre => !string.IsNullOrEmpty(nombre))
                    .Select(nombre => nombre!).Distinct()
                    .ToList();
                filtros.Personas = (
                from c in context.Tcertificado
                join u in context.Tusuario on c.UsuarioIngreso equals u.idUsuario.ToString()
                join p in context.Tpersona on u.Cedula equals p.Cedula
                select new Tpersona
                {
                    Cedula = u.idUsuario.ToString(),
                    Nombres = p.Nombres + " " + p.Apellidos
                }
                ).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR AL CARGAR LOS FILTROS: {ex.Message}");
            }
            return filtros;
        }
        public EstadisticaModel GetEstadistica(FiltroEstadistica filtro)
        {
            EstadisticaModel estadistica = new EstadisticaModel();
            var certificadoIQueryable = context.Tcertificado.AsQueryable();
            var predicate = PredicateBuilder.New<Tcertificado>(true);

            if (filtro.FechaInicio != null)
            {
                predicate = predicate.And(x => x.FCreacion >= filtro.FechaInicio);
            }
            if (filtro.FechaFin != null)
            {
                predicate = predicate.And(x => x.FCreacion <= filtro.FechaFin);
            }

            if (!string.IsNullOrEmpty(filtro.Tipo))
            {
                var tipoUpper = filtro.Tipo.ToUpper();
                predicate = predicate.And(x => !string.IsNullOrEmpty(x.Tipo) && x.Tipo.ToUpper() == tipoUpper);
            }

            if (filtro.Estado != null)
            {
                predicate = predicate.And(x => x.Estado == filtro.Estado);
            }

            if (filtro.Plantilla != 0)
            {
                predicate = predicate.And(x => x.IdFormato == filtro.Plantilla);
            }

            if (!string.IsNullOrEmpty(filtro.Creador))
            {
                var creadorUpper = filtro.Creador.ToUpper();
                predicate = predicate.And(x => !string.IsNullOrEmpty(x.UsuarioIngreso) && x.UsuarioIngreso == creadorUpper);
            }
            if (!string.IsNullOrEmpty(filtro.Firmante))
            {
                var firmanteUpper = filtro.Firmante.ToUpper();
                predicate = predicate.And(x =>
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteUno) && x.TformatoCertificado.CargoFirmanteUno.ToUpper().Equals(firmanteUpper) ||
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteDos) && x.TformatoCertificado.CargoFirmanteDos.ToUpper().Equals(firmanteUpper) ||
                !string.IsNullOrEmpty(x.TformatoCertificado.CargoFirmanteTres) && x.TformatoCertificado.CargoFirmanteTres.ToUpper().Equals(firmanteUpper));
            }
            certificadoIQueryable = certificadoIQueryable.AsExpandable().Where(predicate);


            estadistica.Plantillas = certificadoIQueryable.Include(x => x.TformatoCertificado).GroupBy(x => x.TformatoCertificado.NombrePlantilla).Select(x => new Plantilla
            {
                NombrePlantilla = x.Key,
                Count = x.Count()

            }).ToList();

            estadistica.Estados = certificadoIQueryable.GroupBy(x => x.Estado).Select(x => new Estado
            {
                Tipo = x.Key ? "Activo" : "Inactivo",
                Count = x.Count()
            }).ToList();

            estadistica.LapsoDias = certificadoIQueryable.GroupBy(x => x.FCreacion.Date).Select(x => new LapsoDia
            {
                Dia = x.Key,
                Count = x.Count()
            }).ToList();

            estadistica.LapsoSemanas = certificadoIQueryable
             .AsEnumerable() 
             .GroupBy(x => FirstDayOfWeek(x.FCreacion)) 
             .Select(g => new LapsoSemana
             {
                 SemanaInicio = g.Key,
                 Count = g.Count()
             })
             .OrderBy(x => x.SemanaInicio)
             .ToList();

            estadistica.LapsoAnios = certificadoIQueryable.GroupBy(x => x.FCreacion.Date.Year).Select(x => new LapsoAnio
            {
                Anio = x.Key,
                Count = x.Count()
            }).ToList();
            estadistica.Firmantes = certificadoIQueryable.Select(x => new { x.TformatoCertificado.NombreFirmanteUno, x.TformatoCertificado.NombreFirmanteDos, x.TformatoCertificado.NombreFirmanteTres }).AsEnumerable()
                .SelectMany(x => new[] { x.NombreFirmanteUno, x.NombreFirmanteDos, x.NombreFirmanteTres }).Where(x => !string.IsNullOrEmpty(x)).GroupBy(x => x).Select(x => new Firmante
                {
                    NombreFirmante = x.Key,
                    Count = x.Count()
                }).ToList();



            return estadistica;
        }
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            return date.Date.AddDays(-1 * diff);
        }
        private void DetachIfTracked<T>(T entity, int id) where T : class
        {
            if (entity != null)
            {
                var trackedEntity = context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == entity);
                if (trackedEntity != null)
                {
                    trackedEntity.State = EntityState.Detached;
                }
            }
        }
    }
}
