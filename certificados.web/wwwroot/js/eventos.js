
let userInfoEv = JSON.parse(localStorage.getItem('userInfo'));

async function cargarDatosEventos() {
     
    try {
        const eventosResponse = await Utils.httpRequest(
            `${Utils.path}/evento/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        setTimeout(() => {
            const response = eventosResponse;
            const tablaBody = document.querySelector("#tabla-evento tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(evento => {
                    const fila = `
                <tr>
                    <td>${evento.idevento}</td>
                    <td>${evento.tematica}</td>
                    <td>${evento.dominio}</td>
                    <td>${Utils.formatFecha(evento.fechaInicio)} -${Utils.formatFecha(evento.fechaFin)}  </td>
                    <td>${evento.tmodalidad.nombre}</td>  
                    <td>${evento.lugar}</td>  
                    <td>
                        <i class="bi bi-pencil-fill text-success me-3" style="cursor: pointer;" onclick="editarevento(${evento.idevento})" data-bs-toggle="tooltip" data-bs-placement="top" title="Editar evento"></i>
                        <i class="bi bi-trash-fill text-danger me-3" style="cursor: pointer;" onclick="eliminarEvento(${evento.idevento})" data-bs-toggle="tooltip" data-bs-placement="top" title="Eliminar evento"></i>
                    </td>
                </tr>
            `; 
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                $('#tabla-evento').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

                cargarDatosTipoEvento();
            } else {
                tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron eventos.</td></tr>';
                Utils.showToast('NO EXISTEN ROLES REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }

}
async function cargarDatosTipoEvento() {
    try {
        const tipoResponse = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        setTimeout(() => {
            const response = tipoResponse;
            const tablaBody = document.querySelector("#tabla-tipoevento tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(tipo => {
                    const fila = `  
                <tr>
                    <td>${tipo.idtipoevento}</td>
                    <td>${tipo.nombre}</td>
                    <td>${tipo.descripcion}</td>
                    <td>${Utils.formatFecha(tipo.fCreacion)} </td>
                    <td>${Utils.formatFecha(tipo.fModificacion)} </td>
                    <td>${tipo.usuarioIngreso}</td>  
                    <td>${tipo.usuarioActualizacion ?? 'No Actualizado'}</td>  
                    <td>
                        <i class="bi bi-pencil-fill text-success me-3" style="cursor: pointer;" onclick="editarTipoEvento(${tipo.idtipoevento})" data-bs-toggle="tooltip" data-bs-placement="top" title="Editar evento"></i> 
                    </td>
                </tr>
            `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                $('#tabla-tipoevento').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron eventos.</td></tr>';
                Utils.showToast('NO EXISTEN ROLES REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function crearTipoEvento(event) {
    event.preventDefault(); 

    if (!Utils.validarFormulario('formAgregarTipo')) {
        Utils.showToast("Corrige los errores en el formulario", "warning");
        return;
    }
    const ModalEdit = document.getElementById('modalAgregarTipo');
    const modalBootstrap = bootstrap.Modal.getInstance(ModalEdit);
    try {
        const payloadCreate = {
            "Nombre": `${document.getElementById('nombreTipoC').value}`,
            "Descripcion": `${document.getElementById('descripcionTipoC').value}`,
            "usuarioIngreso": `${userInfoEv.idUsuario}`
        }
        const createEvent = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadCreate)
            },
            true);
        if (createEvent.cod == Utils.COD_OK) {
            Utils.showToast("Evento creado", "success");

            if ($.fn.DataTable.isDataTable('#tabla-tipoevento')) {
                $('#tabla-tipoevento').DataTable().clear().destroy();
            }

            cargarDatosTipoEvento();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {
        console.log(error);
        Utils.showToast("Error al crear el tipo", "danger");
    } finally {
        if (modalBootstrap) {
            modalBootstrap.hide();
        }
    }

}
async function editarevento(id) {
    try {

        const requestEdit = await getEventoById(id);
        const ModalEdit = document.getElementById('modalEditarEvento');
        const modalBootstrap = new bootstrap.Modal(ModalEdit);
        if (requestEdit && requestEdit.data) {
            const data = requestEdit.data;
            document.getElementById('idEventoE').value = data.idevento;
            document.getElementById('idmodalidadE').value = data.idModalidad;
            document.getElementById('ttipoEvento').value = data.idTipoEvento;
            document.getElementById('idDecanatoE').value = data.idDecanato;
            document.getElementById('idGrupoE').value = data.idGrupo;


            document.getElementById('tematicaEvento').value = data.tematica;
            document.getElementById('dominioEvento').value = data.dominio;
            document.getElementById('FechaInicio').value = new Date(data.fechaInicio).toISOString().split('T')[0];
            document.getElementById('FechaFin').value = new Date(data.fechaFin).toISOString().split('T')[0];
            document.getElementById('Horas').value = data.horas;
            document.getElementById('lugarEvento').value = data.lugar;
            document.getElementById('grupoParticipante').value = data.tgrupo.nombre;
            document.getElementById('periodoEvento').value = data.periodo;
            document.getElementById('conCertificado').value = data.conCertificado ? 'true' : 'false';
            document.getElementById('uActualizacion').value = userInfoEv.nombre;

            modalBootstrap.show();
        } else {
            Utils.showToast("No se encontraron datos para el evento seleccionado", "warning");
        }
    } catch (error) {
        console.error(error);
        Utils.showToast("Error al cargar el evento", "error");
    }

}

async function confirmarEditarEvento(event) {
    event.preventDefault();

    if (!Utils.validarFormulario('formEditarEvento')) {
        Utils.showToast("Corrige los errores en el formulario", "warning");
        return;
    } 
    const ModalEdit = document.getElementById('modalEditarEvento');
    const modalBootstrap = bootstrap.Modal.getInstance(ModalEdit);
    try {
        const form = document.getElementById("formEditarEvento");
        const payloadUpdate = {
            "idevento": `${parseInt(form.querySelector("#idEventoE").value, 10)}`,
            "FechaInicio": `${new Date(form.querySelector("#FechaInicio").value).toISOString()}`,
            "FechaFin": `${new Date(form.querySelector("#FechaFin").value).toISOString()}`,
            "Horas": `${parseInt(form.querySelector("#Horas").value, 10)}`,
            "Lugar": `${form.querySelector("#lugarEvento").value}`,
            "ConCertificado": `${form.querySelector("#conCertificado").value === "true" ? 1 : 0}`,
            "Periodo": `${form.querySelector("#periodoEvento").value}`,
            "Tematica": `${form.querySelector("#tematicaEvento").value}`,
            "Dominio": `${form.querySelector("#dominioEvento").value}`,
            "IdGrupo": `${parseInt(form.querySelector("#idGrupoE").value, 10)}`,
            "IdModalidad": `${parseInt(form.querySelector("#idmodalidadE").value, 10)}`,
            "IdTipoEvento": `${parseInt(form.querySelector("#ttipoEvento").value, 10)}`,
            "IdDecanato": `${parseInt(form.querySelector("#idDecanatoE").value, 10)}`,
            "UsuarioActualizacion": `${userInfoEv.idUsuario}`
        };

        const updateEvent = await Utils.httpRequest(
            `${Utils.path}/evento/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadUpdate)
            },
            true);

        if (updateEvent.cod == Utils.COD_OK) {
            Utils.showToast("Evento Actualizado", "success");

            if ($.fn.DataTable.isDataTable('#tabla-evento')) {
                $('#tabla-evento').DataTable().clear().destroy();
            }
            if ($.fn.DataTable.isDataTable('#tabla-tipoevento')) {
                $('#tabla-tipoevento').DataTable().clear().destroy();
            }

            cargarDatosEventos();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.log(error);
        Utils.showToast("Error al actualizar el evento", "danger");
    } finally {
        if (modalBootstrap) {
            modalBootstrap.hide();
        }
    }

}
async function editarTipoEvento(idTipo) {
    try {
        const modalElement = document.getElementById('modalEditarEventoTipo');
        const modalBootstrap = new bootstrap.Modal(modalElement);

        const payloadEdit = {
            idtipoevento: idTipo
        }
        const EditResponse = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadEdit)
            },
            true);
        if (EditResponse.cod == Utils.COD_OK) {
            const data = EditResponse.data;
            document.getElementById('idTipoEventoE').value = data.idtipoevento;
            document.getElementById('nombreTipoEvento').value = data.nombre;
            document.getElementById('descripcionTipoEvento').value = data.descripcion;
            document.getElementById('usuarioActualizacion').value = userInfoEv.nombre;

            modalBootstrap.show();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {

        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function confirmarEditarTipoEvento(event) {
    event.preventDefault();

    const form = document.getElementById('formEditarTipoEvento');

    if (!Utils.validarFormulario('formEditarTipoEvento')) {
        Utils.showToast("Corrige los errores en el formulario", "warning");
        return;
    } 
    const ModalEdit = document.getElementById('modalEditarEventoTipo');
    const modalBootstrap = bootstrap.Modal.getInstance(ModalEdit);

    const payloadUpdate = {
        "Idtipoevento": `${parseInt(form.querySelector("#idTipoEventoE").value, 10)}`,
        "Nombre": `${form.querySelector("#nombreTipoEvento").value}`,
        "Descripcion": `${form.querySelector("#descripcionTipoEvento").value}`,
        "UsuarioActualizacion": `${userInfoEv.idUsuario}`,
    };
    try {
        const updateEvent = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadUpdate)
            },
            true);
        if (updateEvent.cod == Utils.COD_OK) {
            Utils.showToast("Evento Actualizado", "success");

            if ($.fn.DataTable.isDataTable('#tabla-tipoevento')) {
                $('#tabla-tipoevento').DataTable().clear().destroy();
            }

            cargarDatosTipoEvento();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.log(error);
        Utils.showToast("Error al actualizar el evento", "danger");
    } finally {
        if (modalBootstrap) {
            modalBootstrap.hide();
        }
    }

}
async function buscarEvento() { 
    const idEventoB = document.getElementById('buscarevento').value;
    console.log(idEventoB)

    const getEvento = await getEventoById(idEventoB);
    if (getEvento.cod == Utils.COD_OK && getEvento.data != null) {
        const evento = getEvento.data;

        // Asignar valores a los campos
        document.getElementById("lugarE").value = evento.lugar || '';
        document.getElementById("tematicaE").value = evento.tematica || '';
        document.getElementById("dominioE").value = evento.dominio || '';
        document.getElementById("modalidadE").value = evento.tmodalidad.nombre || '';
        document.getElementById("tipoEventoE").value = evento.ttipoEvento.nombre || '';
        document.getElementById("decanatoE").value = evento.tdecanato.nombre || '';
        document.getElementById("horasEvento").value = evento.horas || '';
        document.getElementById("fechaInicioE").value = evento.fechaInicio ? new Date(evento.fechaInicio).toLocaleString() : '';
        document.getElementById("fechaFinE").value = evento.fechaFin ? new Date(evento.fechaFin).toLocaleString() : '';
        document.getElementById("conCertificado").value = evento.conCertificado ? "Sí" : "No";
        document.getElementById("uIngresoE").value = evento.usuarioIngreso || 'Admin';
        document.getElementById("uActualizacionE").value = evento.usuarioActualizacion || 'No Actualizado';
        document.getElementById("fIngresoEvento").value = Utils.formatFecha(evento.fCreacion);
        document.getElementById("fActualizacionEvento").value = Utils.formatFecha(evento.fModificacion);

    } else {
        Utils.showToast("No existe el evento", 'info');
    }
}
async function getEventoById(idEvento) {
    try {
        const payloadEdit = {
            idEvento: idEvento
        }
        const EditResponse = await Utils.httpRequest(
            `${Utils.path}/evento/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadEdit)
            },
            true);
        if (EditResponse.cod == Utils.COD_OK) {
            return EditResponse;
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {

        console.log(error);
        Utils.showToast("Error cargando datos de busqueda", 'danger');
    }

}


async function getTipoEventoById(idEvento) {
    try {
        const payloadEdit = {
            idtipoevento: idEvento
        }
        const EditResponse = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloadEdit)
            },
            true);
        if (EditResponse.cod == Utils.COD_OK) {
            return EditResponse;
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {

        console.log(error);
        Utils.showToast("Error cargando datos de busqueda", 'danger');
    }

}

function eliminarEvento(id) {
    document.getElementById('modalEliminarEvento').dataset.idEvento = id;
    new bootstrap.Modal(document.getElementById('modalEliminarEvento')).show();
}
function eliminarTipoEvento(id) {
    document.getElementById('modalEliminarTipoEvento').dataset.idTipoEvento = id;
    new bootstrap.Modal(document.getElementById('modalEliminarTipoEvento')).show();
}
async function confirmareliminarTipoEvento() {
}


