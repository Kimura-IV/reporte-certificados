
let userInfoPl = JSON.parse(localStorage.getItem('userInfo'));

// Función para cargar datos de planificación
async function cargarDatosPlanificacion() {
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
            const hoy = new Date();

            const response = eventosResponse;
            const tablaBody = document.querySelector("#tabla-evento tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                // Filtra los eventos que estén en curso y tengan el estado "ACT"
                const eventosEnCurso = response.data.filter(evento =>
                    new Date(evento.fechaFin) > hoy && evento.estado === "ACT"
                );

                if (eventosEnCurso.length > 0) {
                    eventosEnCurso.forEach(evento => {
                        const fila = `
                            <tr>
                                <td>${evento.idevento}</td>
                                <td>${evento.tematica}</td>
                                <td>${evento.dominio}</td>
                                <td>${Utils.formatFecha(evento.fechaInicio)} - ${Utils.formatFecha(evento.fechaFin)}</td>
                                <td>${evento.tmodalidad.nombre}</td>
                                <td>${evento.idGrupo}</td>
                                <td>
                                    <i class="bi bi-pencil-fill text-success me-3"
                                           style="cursor: pointer;"
                                           onclick="editarPlanificacion(${evento.idevento})" 
                                           data-bs-toggle="tooltip" 
                                           data-bs-placement="top" 
                                           title="Editar Planificacion"></i>
                                    <i class="bi bi-trash-fill text-danger" 
                                        style="cursor: pointer;" 
                                        onclick="eliminarPlanificacion(${evento.idevento})" 
                                        data-bs-toggle="tooltip" 
                                        data-bs-placement="top" 
                                        title="Eliminar Planificacion"></i>
                                </td>
                            </tr>
                        `;
                        tablaBody.insertAdjacentHTML("beforeend", fila);
                    });
                    Utils.showToast('EVENTOS CARGADOS EXITOSAMENTE', 'success');

                    $('#tabla-evento').DataTable({
                        language: {
                            url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                        }
                    });

                } else {
                    tablaBody.innerHTML = '<tr><td colspan="7" class="text-center">No hay eventos en curso con estado activo.</td></tr>';
                    Utils.showToast('NO EXISTEN EVENTOS EN CURSO CON ESTADO ACTIVO', 'info');
                }
            } else {
                tablaBody.innerHTML = '<tr><td colspan="7" class="text-center">No se encontraron eventos.</td></tr>';
                Utils.showToast('NO EXISTEN EVENTOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarGrupos();
        await cargarModalidades();
        await cargarFacilitadores();
        await cargarCiclos();
        await cargarTipoEvento();
        await cargarFacultad();

    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}


async function cargarTipoEvento() {
    try {
        const tipoResponse = await Utils.httpRequest(
            `${Utils.path}/tipoEvento/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        if (tipoResponse.cod === Utils.COD_OK && tipoResponse.data.length > 0) {
            const selectTipoEvento = document.getElementById("tipo-evento");
            const selectTipoEventoEditar = document.getElementById("tipo-evento-editar");

              
            tipoResponse.data.forEach(tipo => {
                const option = document.createElement("option");
                option.value = tipo.idtipoevento;  
                option.textContent = tipo.nombre;  
                selectTipoEvento.appendChild(option);

                const optionEditar = document.createElement("option");
                optionEditar.value = tipo.idtipoevento;
                optionEditar.textContent = tipo.nombre;
                selectTipoEventoEditar.appendChild(optionEditar);
            });
        } else {  
           Utils.showToast('NO EXISTEN TIPO DE EVENTOS REGISTRADOS', 'info');
        }
    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
async function cargarFacultad() {
    try {
        const decanatoResponse = await Utils.httpRequest(
            `${Utils.path}/decanato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        if (decanatoResponse.cod === Utils.COD_OK && decanatoResponse.data.length > 0) {
            const selectDecanato = document.getElementById("decanatoAll");
            const selectDecanatoEditar = document.getElementById("decanato-editar");

            decanatoResponse.data.forEach(tipo => {
                const option = document.createElement("option");
                option.value = tipo.idDecanato;
                option.textContent = tipo.nombre;
                selectDecanato.appendChild(option);

                const optionEditar = document.createElement("option");
                optionEditar.value = tipo.idDecanato;
                optionEditar.textContent = tipo.nombre;
                selectDecanatoEditar.appendChild(optionEditar);
            });
        } else {
            Utils.showToast('NO EXISTEN DECANATOS REGISTRADOS', 'info');
        }
    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
async function cargarCiclos() {
    try {
        // Realizar la solicitud HTTP para obtener los ciclos
        const responseCiclos = await Utils.httpRequest(
            `${Utils.path}/ciclo/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        // Verificar si la respuesta es válida y contiene datos
        if (responseCiclos.cod === Utils.COD_OK && responseCiclos.data.length > 0) {
            const selectCiclo = document.getElementById("ciclo-evento");
            const selectCicloEditar = document.getElementById("ciclo-editar");

            // Limpiar los selectores antes de agregar nuevas opciones
            selectCiclo.innerHTML = '';
            selectCicloEditar.innerHTML = '';

            // Agregar opciones a los selectores
            responseCiclos.data.forEach(ciclo => {
                const optionText = ciclo.nombre;

                // Crear opción para el primer selector
                const option = document.createElement("option");
                option.value = ciclo.idCiclo;
                option.textContent = optionText;
                selectCiclo.appendChild(option);

                // Crear opción para el segundo selector
                const optionEditar = document.createElement("option");
                optionEditar.value = ciclo.idCiclo;
                optionEditar.textContent = optionText;
                selectCicloEditar.appendChild(optionEditar);
            });
        } else {
            // Mostrar mensaje si no se encuentran ciclos
            Utils.showToast('NO EXISTEN CICLOS REGISTRADOS', 'info');
        }
    } catch (error) {
        // Manejar errores
        console.error("Error cargando ciclos:", error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function cargarFacilitadores() {
    try {
        console.log('taka')
        const condition = {
            estado: 'ACT'
        };

        // Realizar la solicitud HTTP
        const expositorResponse = await Utils.httpRequest(
            `${Utils.path}/personas/all`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(condition)
            },
            true
        );

        // Verificar si la respuesta es válida y contiene datos
        if (expositorResponse.cod === Utils.COD_OK && expositorResponse.data.length > 0) {
            const selectElement = document.getElementById('facilitadorAll');
            const selectFacilitadorEditar = document.getElementById('facilitador-editar');

            // Limpiar los selectores antes de agregar nuevas opciones
            selectElement.innerHTML = '';
            selectFacilitadorEditar.innerHTML = '';

            // Filtrar y agregar facilitadores a los selectores
            expositorResponse.data.forEach(persona => {
                if (persona.mDatos.rol.includes('FACILITADOR')) {
                    const optionText = `${persona.cedula} - ${persona.nombres} - ${persona.apellidos}`;

                    // Crear opción para el primer selector
                    const option = document.createElement('option');
                    option.value = persona.cedula;
                    option.textContent = optionText;
                    selectElement.appendChild(option);

                    // Crear opción para el segundo selector
                    const optionEditar = document.createElement('option');
                    optionEditar.value = persona.cedula;
                    optionEditar.textContent = optionText;
                    selectFacilitadorEditar.appendChild(optionEditar);
                }
            });
        } else {
            // Mostrar mensaje si no se encuentran facilitadores
            Utils.showToast("No se encontraron facilitadores.", 'info');
        }
    } catch (error) {
        // Manejar errores
        console.error("Error cargando facilitadores:", error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
async function cargarModalidades() {
    try {
        const responseModalidades = await Utils.httpRequest(
            `${Utils.path}/modalidad/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );
        if (responseModalidades.cod === Utils.COD_OK && responseModalidades.data.length > 0) {
            const selectModalidad = document.getElementById("modalidadAll"); 
            const selectModalidadEditar = document.getElementById("modalidad-editar"); 

            responseModalidades.data.forEach(mod => {
                const option = document.createElement("option");
                option.value = mod.idModalidad;
                option.textContent = mod.nombre;
                selectModalidad.appendChild(option);

                const optionEditar = document.createElement("option");
                optionEditar.value = mod.idModalidad;
                optionEditar.textContent = mod.nombre;
                selectModalidadEditar.appendChild(optionEditar);
            });
        } else {
            Utils.showToast('NO EXISTEN MODALIDADES REGISTRADAS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function cargarGrupos() {
    try {
        const responseGrupos = await Utils.httpRequest(
            `${Utils.path}/grupo/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        if (responseGrupos.cod === Utils.COD_OK && responseGrupos.data.length > 0) {
            const selectGrupo = document.getElementById("grupoAll"); 
            const selectGrupoEditar = document.getElementById("grupo-editar"); 

            responseGrupos.data.forEach(grupo => {
                const option = document.createElement("option");
                option.value = grupo.idGrupo;
                option.textContent = grupo.nombre;
                selectGrupo.appendChild(option);

                const optionEditar = document.createElement("option");
                optionEditar.value = grupo.idGrupo;
                optionEditar.textContent = grupo.nombre;
                selectGrupoEditar.appendChild(optionEditar);
            });
        } else {
            Utils.showToast('NO EXISTEN GRUPOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
// Manejador para crear nueva planificación
async function handleAgregarPlanificacion(event) {
    const form = event.target.closest("form");

    // Validación del formulario
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    // Construir el cuerpo de la solicitud (JSON) 
    const bodyRequest = {
        "FechaInicio": `${document.getElementById('fecha-inicio').value}T08:30:00`,
        "FechaFin": `${document.getElementById('fecha-fin').value}T12:30:00`,
        "Horas": `${parseInt(document.getElementById('horas').value, 10)}`,
        "Lugar": null,
        "ConCertificado": `${document.getElementById('con-certificado').value}`,
        "Periodo": `${document.getElementById('ciclo-evento').value}`,
        "Facilitador": `${document.getElementById('facilitadorAll').value}`,
        "Estado": "ACT",
        "Tematica": `${document.getElementById('tematica').value}`,
        "Dominio": `${document.getElementById('dominioAll').value.trim()}`,
        "IdGrupo": `${parseInt(document.getElementById('grupoAll').value, 10)}`,
        "IdModalidad": `${parseInt(document.getElementById('modalidadAll').value, 10)}`,
        "IdTipoEvento": `${parseInt(document.getElementById('tipo-evento').value, 10)}`,
        "IdDecanato": `${parseInt(document.getElementById('decanatoAll').value, 10)}`,
        "UsuarioIngreso": `${userInfoPl.idUsuario}`
    };


    try { 
        const responseCreate = await Utils.httpRequest(
            `${Utils.path}/evento/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );
         
        if (responseCreate.cod === Utils.COD_OK) {
            Utils.showToast('PLANIFICACIÓN REGISTRADA EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-evento')) {
                $('#tabla-evento').DataTable().clear().destroy();
            }

            cargarDatosPlanificacion();
            limpiarFormularioPlanificacion(); 

            const tablaTab = document.querySelector('#listar-planificacion-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();

        } else {
            const messageClient = responseCreate.message || "Ocurrió un error inesperado.";
            const messageTech = responseCreate.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.log(error)
        Utils.showToast("Error al agregar la planificación", 'danger');
    }
}

// Función para limpiar el formulario
function limpiarFormularioPlanificacion() {
    const form = document.getElementById('form-planificacion');
    form.reset(); // Restablecer todos los inputs
    form.classList.remove('was-validated'); // Eliminar clases de validación
}

// Función para editar planificación
async function editarPlanificacion(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/evento/id`, 
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idEvento: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const planificacion = response.data;

            document.getElementById('planificacion-id-editar').value = planificacion.idevento;
            document.getElementById('tematica-editar').value = planificacion.tematica;
            document.getElementById('dominio-editar').value = planificacion.dominio;
            document.getElementById('fecha-inicio-editar').value = Utils.formatearFecha(planificacion.fechaInicio);
            document.getElementById('fecha-fin-editar').value = Utils.formatearFecha(planificacion.fechaFin);
            document.getElementById('horas-editar').value = planificacion.horas;

            asignarSelectEditarPlanificacion(planificacion);

            // Mostrar el modal
            const modal = new bootstrap.Modal(document.getElementById('modal-editar-planificacion'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos de la planificación", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos de la planificación", 'danger');
    }
}

async function handleEditarPlanificacion(event) {
    const form = event.target.closest("form");

    // Validar el formulario
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    // Crear el cuerpo de la solicitud para la actualización
    const bodyRequest = {
        "Idevento": `${document.getElementById('planificacion-id-editar').value}`,
        "Tematica": `${document.getElementById('tematica-editar').value}`,
        "Dominio": `${document.getElementById('dominio-editar').value}`,
        "IdTipoEvento": `${parseInt(document.getElementById('tipo-evento-editar').value, 10)}`,
        "Periodo": `${document.getElementById('ciclo-editar').value}`,
        "Facilitador": `${document.getElementById('facilitador-editar').value}`,
        "IdModalidad": `${parseInt(document.getElementById('modalidad-editar').value, 10)}`,
        "ConCertificado": `${document.getElementById('con-certificado-editar').value}`,
        "IdGrupo": `${parseInt(document.getElementById('grupo-editar').value, 10)}`,
        "IdDecanato": `${parseInt(document.getElementById('decanato-editar').value, 10)}`,
        "FechaInicio": `${document.getElementById('fecha-inicio-editar').value}T08:30:00`,
        "FechaFin": `${document.getElementById('fecha-fin-editar').value}T12:30:00`,
        "Horas": `${parseInt(document.getElementById('horas-editar').value, 10)}`,
        "UsuarioActualizacion": `${userInfoPl.idUsuario}`,
        "Estado": "ACT",
        "Lugar": null,
    };

    try {
        // Enviar la solicitud de actualización
        const response = await Utils.httpRequest(
            `${Utils.path}/evento/modificar`, // Asegúrate de que la URL sea correcta
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            // Mostrar mensaje de éxito
            Utils.showToast("Planificación actualizada exitosamente", 'info');

            // Si estás usando DataTable, puedes actualizar la tabla después de la modificación
            if ($.fn.DataTable.isDataTable('#tabla-evento')) {
                $('#tabla-evento').DataTable().clear().destroy();
            }

            cargarDatosPlanificacion();
            // Cerrar el modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar-planificacion'));
            modal.hide();
        } else {
            // Si hay un error, mostrar el mensaje de error
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        // Si ocurre un error en la solicitud
        Utils.showToast("Error al actualizar la planificación", 'danger');
    }
}



// Función para eliminar planificación
async function eliminarPlanificacion(id) {
    if (!confirm('¿Está seguro que desea eliminar esta planificación?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/evento/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idEvento: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Planificación eliminada exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-evento')) {
                $('#tabla-evento').DataTable().clear().destroy();
            }

            cargarDatosPlanificacion();
        } else {
            const messageClient = response.message || "Error al eliminar la planificación.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar la planificación", 'danger');
    }
}

function asignarSelectEditarPlanificacion(planificacion) {
    const selects = {
        'tipo-evento-editar': planificacion.ttipoEvento.idtipoevento,
        'ciclo-editar': planificacion.periodo,
        'facilitador-editar': planificacion.facilitador,
        'modalidad-editar': planificacion.tmodalidad.idModalidad,
        'con-certificado-editar': planificacion.conCertificado,
        'grupo-editar': planificacion.tgrupo.idGrupo,
        'decanato-editar': planificacion.tdecanato.idDecanato
    };

    Object.entries(selects).forEach(([id, value]) => {
        const select = document.getElementById(id);
        if (select) {
            select.value = value;
        }
    });
}


// Funcion para habilitar validación
function habilitarValidacionPlanificacion() {
    'use strict';
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
}
