// Función para cargar datos de grupos
async function cargarDatosGrupos() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupo/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-grupo tbody");
            tablaBody.innerHTML = '';

            // Obtener el elemento select donde se listarán los grupos
            const selectGrupo = document.getElementById("persona-id-grupo");
            const selectGrupoIndividual = document.getElementById("persona-id-grupo-individual");

            // Limpiar las opciones actuales del select
            selectGrupo.innerHTML = '<option value="">Selecciona un grupo</option>';
            selectGrupoIndividual.innerHTML = '<option value="">Selecciona un grupo</option>';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(grupo => {

                    // Agregar el grupo al select
                    const option = document.createElement("option");
                    option.value = grupo.idGrupo;
                    option.textContent = grupo.nombre; // Mostrar el nombre del grupo
                    selectGrupo.appendChild(option);

                    const optionIndividual = document.createElement("option");
                    optionIndividual.value = grupo.idGrupo;
                    optionIndividual.textContent = grupo.nombre; // Mostrar el nombre del grupo
                    selectGrupoIndividual.appendChild(optionIndividual);

                    const fila = `
                        <tr>
                            <td>${grupo.idGrupo}</td>
                            <td>${grupo.nombre}</td>
                            <td>${grupo.cantidad}</td>
                            <td>${grupo.usuarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-pencil-fill text-success me-3" 
                                   style="cursor: pointer;" 
                                   onclick="editarGrupo(${grupo.idGrupo})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Editar Grupo"></i>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarGrupo(${grupo.idGrupo})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Grupo"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                // Establecer usuario actual en el formulario
                document.getElementById("grupo-usuario").value = userInfo.nombre;
                document.getElementById("persona-usuario-ingreso").value = userInfo.nombre;

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                // Inicializar DataTables o reiniciarlo
                $("#tabla-grupo").DataTable({
                    language: {
                        url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="4" class="text-center">No se encontraron grupos.</td></tr>';
                Utils.showToast('NO EXISTEN GRUPOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarDatosGrupoPersonas();

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Manejador para crear nuevo grupo
async function handleAgregarGrupo(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    const bodyRequest = {
        Nombre: document.getElementById('grupo-nombre').value.trim(),
        Cantidad: parseInt(document.getElementById('grupo-cantidad').value),
        UsuarioIngreso: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupo/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('GRUPO REGISTRADO EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-grupo')) {
                $('#tabla-grupo').DataTable().clear().destroy();
            }

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupos();
            limpiarFormularioGrupo();
            // Cambiar a la pestaña de la tabla
            const tablaTab = document.querySelector('#tabla-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el grupo", 'danger');
    }
}

// Función para editar grupo
async function editarGrupo(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupo/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idGrupo: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const grupo = response.data;
            document.getElementById('grupo-id-editar').value = grupo.idGrupo;
            document.getElementById('grupo-nombre-editar').value = grupo.nombre;
            document.getElementById('grupo-cantidad-editar').value = grupo.cantidad;
            document.getElementById('grupo-usuario-editar').value = grupo.usuarioIngreso;

            const modal = new bootstrap.Modal(document.getElementById('modal-editar'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos del grupo", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos del grupo", 'danger');
    }
}

// Manejador para guardar edición
async function handleEditarGrupo(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    const bodyRequest = {
        idGrupo: document.getElementById('grupo-id-editar').value,
        Nombre: document.getElementById('grupo-nombre-editar').value,
        Cantidad: parseInt(document.getElementById('grupo-cantidad-editar').value),
        UsuarioActualizacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupo/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Grupo actualizado exitosamente", 'info');

            if ($.fn.DataTable.isDataTable('#tabla-grupo')) {
                $('#tabla-grupo').DataTable().clear().destroy();
            }

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupos();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar el grupo", 'danger');
    }
}

// Función para eliminar grupo
async function eliminarGrupo(id) {
    if (!confirm('¿Está seguro que desea eliminar este grupo?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupo/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idGrupo: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Grupo eliminado exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-grupo')) {
                $('#tabla-grupo').DataTable().clear().destroy();
            }

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupos();
        } else {
            const messageClient = response.message || "Error al eliminar el grupo.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el grupo", 'danger');
    }
}

// METODOS GRUPO PERSONA
// Funcion para cargar los datos grupo persona
async function cargarDatosGrupoPersonas() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/listar`,
            { method: "GET", headers: { "Content-Type": "application/json" } },
            true
        );

        const tablaBody = document.querySelector("#tabla-grupo-personas tbody");
        tablaBody.innerHTML = "";

        if (response.cod === Utils.COD_OK && response.data.length > 0) {
            response.data.forEach(persona => {
                const fila = `
                    <tr>
                        <td>${persona.idGrupoPersona}</td>
                        <td>${persona.idGrupo}</td>
                        <td>${persona.cedula}</td>
                        <td>${persona.estado}</td>
                        <td>
                            <i class="bi bi-trash-fill text-danger" 
                               style="cursor: pointer;"
                               onclick="eliminarGrupoPersona(${persona.idGrupoPersona},'${persona.cedula}')" 
                               data-bs-toggle="tooltip" 
                               title="Eliminar Persona"></i>
                        </td>
                    </tr>`;
                tablaBody.insertAdjacentHTML("beforeend", fila);
            });

            Utils.showToast("DATOS GRUPO PERSONAS CARGADOS EXITOSAMENTE", "success");

            $("#tabla-grupo-personas").DataTable({
                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
                }
            });

        } else {
            tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron grupo personas.</td></tr>';
            Utils.showToast("NO EXISTEN GRUPO DE PERSONAS REGISTRADAS", "info");
        }
    } catch (error) {
        Utils.showToast("Error cargando datos de grupo persona", "error");
    }
}

// Manejador para crear nuevo grupo persona
async function handleAgregarGrupoPersonas(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    const grupoId = document.getElementById('persona-id-grupo').value;
    const archivoCSV = document.getElementById('persona-cedulas-csv').files[0];
    const usuarioIngreso = userInfo.idUsuario;

    if (!grupoId || !archivoCSV || !usuarioIngreso) {
        Utils.showToast("Todos los campos son obligatorios", 'warning');
        return;
    }

    // Leer el archivo CSV
    const cedulas = await leerArchivoCSV(archivoCSV);
    if (!cedulas || cedulas.length === 0) {
        Utils.showToast("El archivo CSV está vacío o no contiene cédulas válidas", 'warning');
        return;
    }

    const bodyRequest = {
        IdGrupo: grupoId,
        Cedulas: cedulas,
        UsuarioIngreso: usuarioIngreso.toString()
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('GRUPO DE PERSONAS REGISTRADO EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupoPersonas();
            limpiarFormularioGrupoPersonas();
            // Cambiar a la pestaña de la tabla si es necesario
            const tablaTab = document.querySelector('#tabla-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el grupo personas", 'danger');
    }
}

// Manejador para crear nuevo grupo persona individual
async function handleAgregarGrupoPersonasIndividual(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    const grupoId = document.getElementById('persona-id-grupo-individual').value;
    const cedulaIndividual = document.getElementById('cedula-persona-individual').value;
    const usuarioIngreso = userInfo.idUsuario;

    if (!grupoId || !cedulaIndividual || !usuarioIngreso) {
        Utils.showToast("Todos los campos son obligatorios", 'warning');
        return;
    }

    const bodyRequest = {
        IdGrupo: grupoId,
        Cedulas: [cedulaIndividual],
        UsuarioIngreso: usuarioIngreso.toString()
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('GRUPO DE PERSONAS REGISTRADO EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupoPersonas();
            limpiarFormularioGrupoIndividual();
            // Cambiar a la pestaña de la tabla si es necesario
            const tablaTab = document.querySelector('#listar-personas-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el grupo personas", 'danger');
    }
}


async function eliminarGrupoPersona(idGrupoPersona, cedula) {
    if (!confirm('¿Está seguro que desea eliminar este grupo?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idGrupoPersona, cedula })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Grupo eliminado exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-grupo')) {
                $('#tabla-grupo').DataTable().clear().destroy();
            }

            if ($.fn.DataTable.isDataTable('#tabla-grupo-personas')) {
                $('#tabla-grupo-personas').DataTable().clear().destroy();
            }

            cargarDatosGrupos();
        } else {
            const messageClient = response.message || "Error al eliminar el grupo.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el grupo", 'danger');
    }
}

async function handleBuscarGrupoCedula(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    // Obtener los valores del formulario
    const idGrupo = document.getElementById('buscar-id-grupo-cedula').value;
    const cedula = document.getElementById('buscar-cedula').value;

    if (!idGrupo || !cedula) {
        Utils.showToast("Todos los campos son obligatorios", 'warning');
        return;
    }

    try {
        // Realizar la solicitud a la API
        const response = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/buscarPersona`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    idGrupo: idGrupo,
                    cedula: cedula
                })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const data = response.data;

            // Mostrar los datos obtenidos en el formulario
            document.getElementById('id-grupo-persona').value = data.idGrupoPersona || '';
            document.getElementById('id-grupo').value = data.idGrupo || '';
            document.getElementById('cedula').value = data.cedula || '';
            document.getElementById('estado').value = data.estado || '';
            document.getElementById('fecha-creacion').value = data.fCreacion || '';
            document.getElementById('usuario-ingreso').value = data.ususarioIngreso || '';

            // Mostrar el formulario lleno
            document.getElementById('form-datos-grupo-cedula').classList.remove('d-none');
        } else {
            const messageClient = response.message || "No se encontraron datos para la búsqueda.";
            Utils.showToast(messageClient, 'warning');
        }
    } catch (error) {
        Utils.showToast("Error al buscar el grupo persona", 'danger');
        console.error("Error en la búsqueda:", error);
    }
}


// Funcion para leer el archivo CSV y obtener las cedulas
function leerArchivoCSV(archivo) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = function (event) {
            const contenido = event.target.result;
            const lineas = contenido.split('\n');
            const cedulas = lineas.map(linea => linea.trim()).filter(linea => linea.length > 0); // Eliminar espacios y líneas vacías
            resolve(cedulas);
        };
        reader.onerror = function (error) {
            reject("Error al leer el archivo CSV");
        };
        reader.readAsText(archivo);
    });
}

// Función para limpiar formulario
function limpiarFormularioGrupo() {
    document.getElementById('grupo-nombre').value = '';
    document.getElementById('grupo-cantidad').value = '';
    document.getElementById('grupo-usuario').value = userInfo.nombre;

    const form = document.querySelector('.needs-validation');
    if (form) {
        form.classList.remove('was-validated');
    }
}

// Función para habilitar validación
function habilitarValidacion() {
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

function limpiarFormularioGrupoPersonas() {
    // Restablecer los campos del formulario
    document.getElementById('persona-id-grupo').value = '';
    document.getElementById('persona-cedulas-csv').value = '';
    document.getElementById('persona-usuario-ingreso').value = userInfo.nombre;

    // Eliminar la validación visual (si es necesario)
    const form = document.getElementById('form-grupo-personas');
    form.classList.remove('was-validated');
}

function limpiarFormularioGrupoIndividual() {
    const form = document.getElementById('form-crear-gpindividual');
    if (form) {
        form.reset(); // Restablecer formulario
        form.classList.remove('was-validated'); // Eliminar validación
    }
}
