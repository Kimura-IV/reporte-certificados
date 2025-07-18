// Función para cargar datos de modalidades
async function cargarDatosModalidades() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-modalidad tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(modalidad => {
                    const fila = `
                        <tr>
                            <td>${modalidad.idModalidad}</td>
                            <td>${modalidad.nombre}</td>
                            <td>${modalidad.descripcion}</td>
                            <td>${modalidad.ususarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-pencil-fill text-success me-3" 
                                   style="cursor: pointer;" 
                                   onclick="editarModalidad(${modalidad.idModalidad})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Editar Modalidad"></i>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarModalidad(${modalidad.idModalidad})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Modalidad"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                // Establecer usuario actual en el formulario
                document.getElementById("modalidad-usuario").value = userInfo.nombre;

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-modalidad').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron modalidades.</td></tr>';
                Utils.showToast('NO EXISTEN MODALIDADES REGISTRADAS', 'info');
            }
        }, 150);

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Manejador para crear nueva modalidad
async function handleAgregarModalidad(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    const bodyRequest = {
        Nombre: document.getElementById('modalidad-nombre').value.trim(),
        Descripcion: document.getElementById('modalidad-descripcion').value,
        UsuarioIngreso: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('MODALIDAD REGISTRADA EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-modalidad')) {
                $('#tabla-modalidad').DataTable().clear().destroy();
            }

            cargarDatosModalidades();
            limpiarFormularioModalidad();
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
        Utils.showToast("Error al agregar la modalidad", 'danger');
    }
}

// Función para editar modalidad
async function editarModalidad(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idModalidad: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const modalidad = response.data;
            document.getElementById('modalidad-id-editar').value = modalidad.idModalidad;
            document.getElementById('modalidad-nombre-editar').value = modalidad.nombre;
            document.getElementById('modalidad-descripcion-editar').value = modalidad.descripcion;
            document.getElementById('modalidad-usuario-editar').value = modalidad.ususarioIngreso;

            const modal = new bootstrap.Modal(document.getElementById('modal-editar'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos de la modalidad", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos de la modalidad", 'danger');
    }
}

// Manejador para guardar edición
async function handleEditarModalidad(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    const bodyRequest = {
        idModalidad: document.getElementById('modalidad-id-editar').value,
        Nombre: document.getElementById('modalidad-nombre-editar').value,
        Descripcion: document.getElementById('modalidad-descripcion-editar').value,
        UsuarioActualizacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Modalidad actualizada exitosamente", 'info');

            if ($.fn.DataTable.isDataTable('#tabla-modalidad')) {
                $('#tabla-modalidad').DataTable().clear().destroy();
            }

            cargarDatosModalidades();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar la modalidad", 'danger');
    }
}

// Función para eliminar modalidad
async function eliminarModalidad(id) {
    if (!confirm('¿Está seguro que desea eliminar esta modalidad?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idModalidad: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Modalidad eliminada exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-modalidad')) {
                $('#tabla-modalidad').DataTable().clear().destroy();
            }

            cargarDatosModalidades();
        } else {
            const messageClient = response.message || "Error al eliminar la modalidad.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar la modalidad", 'danger');
    }
}

async function handleBuscarModalidad(event) {
    const form = event.target.closest("form");

    // Validar el formulario
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    // Obtener el valor del ID Modalidad
    const idModalidad = document.getElementById('buscar-id').value;

    if (!idModalidad) {
        Utils.showToast("El campo ID Modalidad es obligatorio", 'warning');
        return;
    }

    try {
        // Realizar la solicitud a la API
        const response = await Utils.httpRequest(
            `${Utils.path}/modalidad/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idModalidad })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const data = response.data;

            // Mostrar los datos obtenidos en el formulario
            document.getElementById('id-modalidad').value = data.idModalidad || '';
            document.getElementById('nombre-modalidad').value = data.nombre || '';
            document.getElementById('descripcion-modalidad').value = data.descripcion || '';
            document.getElementById('fcreacion-modalidad').value = data.fCreacion || '';
            document.getElementById('usuario-ingreso-modalidad').value = data.usuarioIngreso || '';

            // Mostrar el formulario lleno
            document.getElementById('form-resultado-busqueda').classList.remove('d-none');
        } else {
            const messageClient = response.message || "No se encontraron datos para la búsqueda.";
            Utils.showToast(messageClient, 'warning');
        }
    } catch (error) {
        Utils.showToast("Error al buscar la modalidad", 'danger');
        console.error("Error en la búsqueda:", error);
    }
}

// Función para limpiar formulario
function limpiarFormularioModalidad() {
    document.getElementById('modalidad-nombre').value = '';
    document.getElementById('modalidad-descripcion').value = '';
    document.getElementById('modalidad-usuario').value = userInfo.nombre;

    const form = document.querySelector('.needs-validation');
    if (form) {
        form.classList.remove('was-validated');
    }
}

// Función para habilitar validación
function habilitarValidacionModalidad() {
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
