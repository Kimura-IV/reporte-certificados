let userInfoCA = JSON.parse(localStorage.getItem('userInfo'));
let actaCalificacionEliminar;
async function inciarDatosActaCalificacion() {
    try {
        const eventosResponse = await Utils.httpRequest(
            `${Utils.path}/evento/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const response = eventosResponse;

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                const selectEventoActaAsistencia = document.getElementById("eventoActaAsistencia");
                selectEventoActaAsistencia.innerHTML = "";

                const defaultOptionActaCalificacion = document.createElement("option");
                defaultOptionActaCalificacion.value = "";
                defaultOptionActaCalificacion.textContent = "Seleccionar Temática";
                defaultOptionActaCalificacion.disabled = true;
                defaultOptionActaCalificacion.selected = true;
                selectEventoActaAsistencia.appendChild(defaultOptionActaCalificacion);

                response.data.forEach(evento => {
                    const option = document.createElement("option");
                    option.value = evento.idevento;
                    option.textContent = evento.tematica;
                    selectEventoActaAsistencia.appendChild(option);
                });

                const selectEventoActaCalificacionAll = document.getElementById("eventoActaAsistenciaAll");
                selectEventoActaCalificacionAll.innerHTML = "";

                const defaultOptionActaCalificacionAll = document.createElement("option");
                defaultOptionActaCalificacionAll.value = "";
                defaultOptionActaCalificacionAll.textContent = "Seleccione un evento";
                defaultOptionActaCalificacionAll.disabled = true;
                defaultOptionActaCalificacionAll.selected = true;
                selectEventoActaCalificacionAll.appendChild(defaultOptionActaCalificacionAll);

                response.data.forEach(evento => {
                    const option = document.createElement("option");
                    option.value = evento.idevento;
                    option.textContent = evento.tematica;
                    selectEventoActaCalificacionAll.appendChild(option);
                });

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                $('#tabla-actas-calificacion').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                Utils.showToast('NO EXISTEN EVENTOS REGISTRADOS', 'info');
            }
        }, 150);

        document.getElementById("nombreUsuario").value = `${userInfoCA.nombre}`;
        const fechaInput = document.getElementById("fecha");
        const hoy = new Date();
        const fechaHoy = hoy.toISOString().split("T")[0];
        fechaInput.value = fechaHoy;

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}


async function agregarActaCalificacion(event) {
    event.preventDefault();
    const idEvento = document.getElementById('eventoActaAsistencia').value;
    const usuarioIngreso = userInfoCA.idUsuario;
    const usuarioActualizacion = ''; // Por defecto vacío
    const archivoInput = document.getElementById('archivoAsistencia');
 
    if (!archivoInput.files.length) {
        Utils.showToast("Por favor, seleccione un archivo pdf", "warning");
        return;
    }


    try {
        const actaDocumentoBase64 = await Utils.convertirArchivoABase64(archivoInput.files[0]);
        const payload = {
            "IdEvento": `${parseInt(idEvento, 10)}`,
            "UsuarioIngreso": `${usuarioIngreso}`,
            "UsuarioActualizacion": `${usuarioActualizacion}`,
            "ActaDocumento": `${actaDocumentoBase64}`,
        };

        const responseCalificacion = await Utils.httpRequest(
            `${Utils.path}/calificacion/crear`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (responseCalificacion.cod === Utils.COD_OK) {
            Utils.showToast("Acta registrado exitosamente", "success");

            if ($.fn.DataTable.isDataTable('#tabla-actas-calificacion')) {
                $('#tabla-actas-calificacion').DataTable().clear().destroy();
            }

            limpiarCampos();
            inciarDatosActaCalificacion();

        } else {
            const messageClient = responseCalificacion.message || "Ocurrió un error inesperado.";
            const messageTech = responseCalificacion.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {

        Utils.showToast("Error al agregar el acta:", 'danger');
    }
}

function limpiarCampos() {
    document.getElementById('fecha').value = ""; // Reinicia la fecha
    document.getElementById('archivoAsistencia').value = "";
}

async function buscarActaCalificacion() {
    const idActaBuscar = document.getElementById('buscar-actaCalificacion').value;
    limpiarInputs();
    try {
        const payload = {
            "idActa": parseInt(idActaBuscar, 10)
        }
        const responseActaSearch = await Utils.httpRequest(
            `${Utils.path}/calificacion/id`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });
        if (responseActaSearch.cod == Utils.COD_OK) {
            const actaData = responseActaSearch.data;

            document.getElementById('dominioA').value = actaData.tevento.dominio || '';
            document.getElementById('tematicaA').value = actaData.tevento.tematica || '';
            document.getElementById('idEvento').value = actaData.idEvento || '';
            document.getElementById('actaId').value = actaData.idCalificacion || '';
            document.getElementById('usuarioIngresoA').value = actaData.usuarioIngreso || '';
            document.getElementById('usuarioActualizacionA').value = actaData.usuarioActualizacion || 'No actualizado';
            document.getElementById('fechaIngresoA').value = Utils.formatFecha(actaData.fCreacion);
            document.getElementById('fechaActualizacionA').value = Utils.formatFecha(actaData.fModificacion);
            // Manejo del botón y el nombre del documento
            const downloadButton = document.getElementById('btnDownload');
            const nombreDocumento = document.getElementById('nombreDocumento');

            if (actaData.actaDocumento) {
                const nombreDescargaD = new Date().getTime();
                downloadButton.disabled = false;
                nombreDocumento.value = `${nombreDescargaD}.pdf`; // Nombre dinámico del documento
                downloadButton.onclick = () => descargarActa(actaData.actaDocumento);
            } else {
                downloadButton.disabled = true;
                nombreDocumento.value = "Sin documento disponible";
            }

        } else {
            const messageClient = responseActaSearch.message || "Ocurrió un error inesperado.";
            const messageTech = responseActaSearch.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {
        console.log(error);
        Utils.showToast("Error al obtener  el acta:", 'danger');
    }
}
async function cargarActaCalificacion() {
    let hasData = false;

    try {
        const idActaReferencia = parseInt(document.getElementById('eventoActaAsistenciaAll').value, 10);

        const eventosAllResponse = await Utils.httpRequest(
            `${Utils.path}/calificacion/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );
        const tableBody = document.querySelector("#tabla-actas-calificacion tbody");
        tableBody.innerHTML = "";

        if (eventosAllResponse.cod === Utils.COD_OK) {
            eventosAllResponse.data.forEach(event => {

                if (parseInt(event.idEvento, 10) === idActaReferencia) {
                    hasData = true;

                    const row = document.createElement("tr");
                    row.innerHTML = `
                        <td>${event.idCalificacion}</td>
                        <td>${event.idEvento}</td>
                        <td>${event.tevento.dominio}</td>
                        <td>${event.fCreacion}</td>
                        <td>${event.usuarioIngreso}</td>
                        <td>
                        <i class="bi bi-file-earmark-pdf-fill text-danger me-3 fs-4" style="cursor: pointer;" onclick="descargarActa('${event.actaDocumento}')" 
                            data-bs-toggle="tooltip"  data-bs-placement="top"   title="Descargar PDF"></i>

                            <i class="bi bi-trash-fill text-danger  fs-4" style="cursor: pointer;" onclick="eliminarActaCalificacion(${event.idCalificacion})" data-bs-toggle="tooltip" data-bs-placement="top" title="Eliminar"></i>
                        </td>
                    `;

                    tableBody.appendChild(row);
                }
            });

            if (!hasData) {
                Utils.showToast("El evento no tiene ningún acta registrada", 'info');
            }
        } else {
            const messageClient = eventosAllResponse.message || "Ocurrió un error inesperado.";
            const messageTech = eventosAllResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.log('Error:', error);
        Utils.showToast("Error al buscar las actas del evento", 'danger');
    }
}

async function eliminarActaCalificacion(id) {
    actaCalificacionEliminar = id;

    const modal = new bootstrap.Modal(document.getElementById('confirmarEliminacionModal'));
    modal.show();

}
function limpiarInputs() {
    document.getElementById('dominioA').value = '';
    document.getElementById('tematicaA').value = '';
    document.getElementById('idEvento').value = '';
    document.getElementById('actaId').value = '';
    document.getElementById('usuarioIngresoA').value = '';
    document.getElementById('usuarioActualizacionA').value = '';
    document.getElementById('fechaIngresoA').value = '';
    document.getElementById('fechaActualizacionA').value = '';
    document.getElementById('nombreDocumento').value = '';
    document.getElementById('btnDownload').disabled = true;
}

async function confirmarEliminacionCalificacion() {
    if (!actaCalificacionEliminar) {
        Utils.showToast("ID de Acta no válido", "danger");
        return;
    }

    try {
        const data = {
            "idActa": actaCalificacionEliminar
        }
        const response = await Utils.httpRequest(
            `${Utils.path}/calificacion/eliminar`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        }
        );

        if (response && response.cod === Utils.COD_OK) {
            Utils.showToast("Documento eliminado exitosamente", "success");
            cargarActaCalificacion();
        } else {
            const messageClient = response.message || "Error al eliminar el documento.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el documento", "danger");
    } finally {
        actaCalificacionEliminar = null;
        const modal = bootstrap.Modal.getInstance(document.getElementById('confirmarEliminacionModal'));
        modal.hide();
    }
}
function descargarActa(actaDocumento) {
    const blob = new Blob([new Uint8Array(atob(actaDocumento).split("").map(char => char.charCodeAt(0)))], { type: "application/pdf" });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    const nombreDescarga = new Date().getTime();
    link.download = `${nombreDescarga}.pdf`
    link.click();
}