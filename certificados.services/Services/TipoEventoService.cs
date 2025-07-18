using certificados.dal.DataAccess;
using certificados.models.Entitys.dbo;
using certificados.models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class TipoEventoService
    {
        public readonly TtipoEventoDA ttipoEventoDA;
        public TipoEventoService(TtipoEventoDA ttipoEventoDA) { 
        
            this.ttipoEventoDA = ttipoEventoDA;
        }
        // Insertar Tipo de Evento
        public ResponseApp InsertarTipoEvento(TtipoEvento evento)
        {
            return ttipoEventoDA.insertarTipoEvento(evento);
        }

        // Modificar Tipo de Evento
        public ResponseApp ActualizarTipoEvento(TtipoEvento evento)
        {
            return ttipoEventoDA.ModificarTipoEvento(evento);
        }

        // Eliminar Tipo de Evento
        public ResponseApp EliminarTipoEvento(int idEvento)
        {
            return ttipoEventoDA.EliminarTipoEvento(idEvento);
        }

        // Consultar Tipo de Evento por ID
        public ResponseApp ConsultarTipoEvento(int idEvento)
        {
            return ttipoEventoDA.ConsultarTipoEventoId(idEvento);
        }

        public ResponseApp listarTiposEvento()
        {
            return ttipoEventoDA.ListarTipoEvento();
        }

    }
}
