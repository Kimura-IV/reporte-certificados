using certificados.dal.DataAccess;
using certificados.models.Context;
using certificados.models.Entitys;
using certificados.models.Entitys.dbo;
using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace certificados.services.Services
{

    public class PersonaService
    {
        private readonly TpersonaDA personaDataAcces;
        private readonly UsuarioService usuarioService;

        public PersonaService(TpersonaDA _personaDataAcces, UsuarioService usuarioService)
        {
            this.personaDataAcces = _personaDataAcces;
            this.usuarioService = usuarioService;
        }

        public ResponseApp CrearPersona(Tpersona personaU, Tusuario usuario) {

            ResponseApp response = Utils.Utils.BadResponse(null);

            try
            {

                if (personaDataAcces.BuscarPersona(personaU.Cedula).Cod.Equals(CONSTANTES.COD_OK))
                {
                    response.Message = "USUARIO YA ESTA REGISTRADO";
                    return response;
                }
                response = personaDataAcces.InsertarPersona(personaU, usuario);
            }
            catch (Exception ex) {
                response.Message = $"ERROR AL CREAR PERSONA: {personaU.Cedula}";
                throw new Exception($"ERROR AL CREAR PERSONA: {ex.Message}", ex);
            }

            return response;

        }
        //Buscar una persona por medio de la CEDULA
        public ResponseApp ObtenerPersona(String cedula) { 
        
            return personaDataAcces.BuscarPersona(cedula);
        }
        //Buscar una persona por medio de la CEDULA COMPLETO
        public ResponseApp ObtenerPersonaCompleta(String cedula)
        {

            return personaDataAcces.BuscarPersonaEntidadCompleta(cedula);
        }

        //Elimina una persona por medio de su ID
        public ResponseApp ELiminarPersona(String cedula) {
            ResponseApp response = Utils.Utils.BadResponse(null);
            try
            {
                var buscarUsuario = personaDataAcces.BuscarPersonaEntidad(cedula);
                if (!buscarUsuario.Cod.Equals(CONSTANTES.COD_OK))
                {
                    return buscarUsuario;
                }
                if (buscarUsuario.Data !=null) {
                    var jsonString = JsonSerializer.Serialize(buscarUsuario.Data);

                    Tpersona personaResponse = JsonSerializer.Deserialize<Tpersona>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true  
                    });

                    var eliminarUsuario = usuarioService.EliminarUsuario(personaResponse.Cedula);
                    if (!eliminarUsuario.Cod.Equals(CONSTANTES.COD_OK))
                    {
                        response.Message = "ERROR AL ELIMINAR USUARIO";
                        return response;  
                    }
                        response = eliminarUsuario;
                }
                else {
                    response.Message = "LA PERSONA NO ES VÁLIDA O NO SE ENCONTRÓ";
                }

            }
            catch (Exception ex) {
                response.Message = $"ERROR AL ELIMINAR PERSONA: {ex.Message}";
                throw new Exception($"ERROR AL ELIMINAR PERSONA: {ex.Message}", ex);
            }
            return response;    
        }
        //Modifica los Datos de una persona
        public ResponseApp ModificarPersona(Tpersona persona, Tusuario usuario) {
            ResponseApp response = Utils.Utils.BadResponse(null);

            try
            {
                response = personaDataAcces.ModificarPersona(persona, usuario);

            }
            catch (Exception ex) {
                response.Message = $"ERROR AL MODIFICAR PERSONA: {ex.Message}";
                throw new Exception($"ERROR AL MODIFICAR PERSONA: {ex.Message}", ex);
            }

            return response;
        }
        //Modifica los Datos de una persona
        public ResponseApp ModificarPerfil(Tpersona persona, Tusuario usuario)
        {
            ResponseApp response = Utils.Utils.BadResponse(null);

            try
            {
                response = personaDataAcces.ModificarPerfil(persona, usuario);

            }
            catch (Exception ex)
            {
                response.Message = $"ERROR AL MODIFICAR PERSONA: {ex.Message}";
                throw new Exception($"ERROR AL MODIFICAR PERSONA: {ex.Message}", ex);
            }

            return response;
        }
        public ResponseApp ListarPersonas(string estado) {
            ResponseApp response = Utils.Utils.BadResponse(null);
            try
            {
                response = personaDataAcces.ListarPersonas(estado);
            }
            catch (Exception ex) {
                response.Message = $"ERROR AL LISTAR PERSONA: {ex.Message}";
                throw new Exception($"ERROR AL LISTAR PERSONA: {ex.Message}", ex);
            }
            return response;
        }

        public ResponseApp buscarPersonaPorCedula(String cedula)
        {

            return personaDataAcces.buscarObjPersona(cedula);
        }
    }
}
