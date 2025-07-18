// Función para cargar datos de docentes
async function cargarDatosDocentes() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-docente tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(docente => {
                    const fila = `
                        <tr>
                            <td>${docente.codigoDocente}</td>
                            <td>${docente.cedula}</td>
                            <td>${docente.titulo}</td>
                            <td>${docente.facultad}</td>
                            <td>${docente.carrera}</td>
                            <td>${docente.idEstado === 1 ? 'Activo' : 'Inactivo'}</td>
                            <td>${docente.usuarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-pencil-fill text-success me-3" 
                                   style="cursor: pointer;" 
                                   onclick="editarDocente('${docente.codigoDocente}')" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Editar Docente"></i>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarDocente('${docente.codigoDocente}')" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Docente"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                // Inicializar DataTables o reiniciarlo
                if ($.fn.DataTable.isDataTable("#tabla-docente")) {
                    $("#tabla-docente").DataTable().destroy();
                }
                $("#tabla-docente").DataTable({
                    language: {
                        url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
                    }
                });

                // Establecer usuario actual en el formulario
                document.getElementById("docente-usuarioingreso").value = userInfo.nombre;

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });
            } else {

                if ($.fn.DataTable.isDataTable("#tabla-docente")) {
                    $("#tabla-docente").DataTable().destroy();
                }

                $("#tabla-docente").DataTable({
                    language: {
                        url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
                    }
                });

                tablaBody.innerHTML = '<tr><td colspan="8" class="text-center">No se encontraron docentes.</td></tr>';
                Utils.showToast('NO EXISTEN DOCENTES REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Manejador para crear nuevo docente
async function handleAgregarDocente(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    const bodyRequest = {
        codigoDocente: document.getElementById('docente-codigo').value,
        cedula: document.getElementById('docente-cedula').value,
        titulo: document.getElementById('docente-titulo').value,
        facultad: document.getElementById('docente-facultad').value,
        carrera: document.getElementById('docente-carrera').value,
        estado: parseInt(document.getElementById('docente-estado').value),
        usuarioIngreso: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('DOCENTE REGISTRADO EXITOSAMENTE', 'info');
            cargarDatosDocentes();
            form.reset();
            form.classList.remove('was-validated');

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
        Utils.showToast("Error al agregar el docente", 'danger');
    }
}

// Función para editar docente
async function editarDocente(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ codigoDocente: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const docente = response.data;
            document.getElementById('docente-id-editar').value = docente.id;
            document.getElementById('docente-codigo-editar').value = docente.codigoDocente;
            document.getElementById('docente-cedula-editar').value = docente.cedula;
            document.getElementById('docente-titulo-editar').value = docente.titulo;
            document.getElementById('docente-facultad-editar').value = docente.facultad;
            document.getElementById('docente-carrera-editar').value = docente.carrera;
            document.getElementById('docente-estado-editar').value = docente.idEstado;
            document.getElementById('docente-usuarioingreso-editar').value = docente.usuarioIngreso;

            const modal = new bootstrap.Modal(document.getElementById('modal-editar-docente'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos del docente", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos del docente", 'danger');
    }
}

// Manejador para guardar edición
async function handleEditarDocente(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    const bodyRequest = {
        idDocente: document.getElementById('docente-id-editar').value,
        codigoDocente: document.getElementById('docente-codigo-editar').value,
        cedula: document.getElementById('docente-cedula-editar').value,
        titulo: document.getElementById('docente-titulo-editar').value,
        facultad: document.getElementById('docente-facultad-editar').value,
        carrera: document.getElementById('docente-carrera-editar').value,
        estado: parseInt(document.getElementById('docente-estado-editar').value),
        usuarioActualizacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Docente actualizado exitosamente", 'info');
            cargarDatosDocentes();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar-docente'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar el docente", 'danger');
    }
}

async function handleBuscarDocente(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    // Obtener los valores del formulario
    const cedula = document.getElementById('buscar-cedula').value;

    if (!cedula) {
        Utils.showToast("La cédula es obligatoria para la búsqueda", 'warning');
        return;
    }

    try {
        // Realizar la solicitud a la API para buscar el docente
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/cedula`,  // Ajusta la URL según tu API
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    cedula: cedula
                })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const data = response.data;

            // Mostrar los datos obtenidos en el formulario
            document.getElementById('docente-codigo-buscar').value = data.codigoDocente || '';
            document.getElementById('docente-cedula-buscar').value = data.cedula || '';
            document.getElementById('docente-titulo-buscar').value = data.titulo || '';
            document.getElementById('docente-facultad-buscar').value = data.facultad || '';
            document.getElementById('docente-carrera-buscar').value = data.carrera || '';
            document.getElementById('docente-fecha-buscar').value = data.fCreacion || '';
            document.getElementById('docente-estado-buscar').value = data.idEstado || '';
            document.getElementById('docente-usuario-buscar').value = data.usuarioIngreso || '';

            // Mostrar el formulario lleno
            document.getElementById('form-busqueda-docente').classList.remove('d-none');
        } else {
            const messageClient = response.message || "No se encontraron datos para el docente con esta cédula.";
            Utils.showToast(messageClient, 'warning');
        }
    } catch (error) {
        Utils.showToast("Error al buscar el docente", 'danger');
        console.error("Error en la búsqueda:", error);
    }
}


// Función para eliminar docente
async function eliminarDocente(id) {
    if (!confirm('¿Está seguro que desea eliminar este docente?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/docente/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idDocente: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Docente eliminado exitosamente", 'success');
            cargarDatosDocentes();
        } else {
            const messageClient = response.message || "Error al eliminar el docente.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el docente", 'danger');
    }
}

// Función para habilitar validación
function habilitarValidacionDocente() {
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
