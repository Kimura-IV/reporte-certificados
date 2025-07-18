// Función para cargar datos de ciclos
async function cargarDatosCiclos() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-ciclo tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(ciclo => {
                    const fila = `
                        <tr>
                            <td>${ciclo.idCiclo}</td>
                            <td>${ciclo.nombre}</td>
                            <td>${ciclo.descripcion}</td>
                            <td>${ciclo.usuarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-pencil-fill text-success me-3" 
                                   style="cursor: pointer;" 
                                   onclick="editarCiclo(${ciclo.idCiclo})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Editar Ciclo"></i>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarCiclo(${ciclo.idCiclo})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Ciclo"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                // Establecer usuario actual en el formulario
                document.getElementById("ciclo-usuario").value = userInfo.nombre;

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-ciclo').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="4" class="text-center">No se encontraron ciclos.</td></tr>';
                Utils.showToast('NO EXISTEN CICLOS REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Manejador para crear nuevo ciclo
async function handleAgregarCiclo(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();
    let userInfoConfig = JSON.parse(localStorage.getItem('userInfo'));
    const bodyRequest = {
        Nombre: document.getElementById('ciclo-nombre').value.trim(),
        Descripcion: document.getElementById('ciclo-descripcion').value,
        UsuarioIngreso: userInfoConfig.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('CICLO REGISTRADO EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-ciclo')) {
                $('#tabla-ciclo').DataTable().clear().destroy();
            }

            cargarDatosCiclos();
            limpiarFormularioCiclo();
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
        Utils.showToast("Error al agregar el ciclo", 'danger');
    }
}

// Función para editar ciclo
async function editarCiclo(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idCiclo: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const ciclo = response.data;
            document.getElementById('ciclo-id-editar').value = ciclo.idCiclo;
            document.getElementById('ciclo-nombre-editar').value = ciclo.nombre;
            document.getElementById('ciclo-descripcion-editar').value = ciclo.descripcion;
            document.getElementById('ciclo-usuario-editar').value = ciclo.usuarioIngreso;

            const modal = new bootstrap.Modal(document.getElementById('modal-editar'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos del ciclo", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos del ciclo", 'danger');
    }
}

// Manejador para guardar edición
async function handleEditarCiclo(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    const bodyRequest = {
        idCiclo: document.getElementById('ciclo-id-editar').value,
        Nombre: document.getElementById('ciclo-nombre-editar').value,
        Descripcion: document.getElementById('ciclo-descripcion-editar').value,
        UserModificacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Ciclo actualizado exitosamente", 'info');

            if ($.fn.DataTable.isDataTable('#tabla-ciclo')) {
                $('#tabla-ciclo').DataTable().clear().destroy();
            }

            cargarDatosCiclos();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar el ciclo", 'danger');
    }
}

// Función para eliminar ciclo
async function eliminarCiclo(id) {
    if (!confirm('¿Está seguro que desea eliminar este ciclo?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idCiclo: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Ciclo eliminado exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-ciclo')) {
                $('#tabla-ciclo').DataTable().clear().destroy();
            }

            cargarDatosCiclos();
        } else {
            const messageClient = response.message || "Error al eliminar el ciclo.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el ciclo", 'danger');
    }
}

async function handleBuscarCiclo(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    // Obtener el valor del formulario
    const idCiclo = document.getElementById('buscar-id').value;

    if (!idCiclo) {
        Utils.showToast("El campo ID Ciclo es obligatorio", 'warning');
        return;
    }

    try {
        // Realizar la solicitud a la API
        const response = await Utils.httpRequest(
            `${Utils.path}/ciclo/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idCiclo: idCiclo })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const data = response.data;

            // Mostrar los datos obtenidos en el formulario de resultados
            document.getElementById('id-ciclo').value = data.idCiclo || '';
            document.getElementById('nombre-ciclo').value = data.nombre || '';
            document.getElementById('descripcion-ciclo').value = data.descripcion || '';
            document.getElementById('fcreacion-ciclo').value = data.fCreacion || '';
            document.getElementById('usuario-ingreso-ciclo').value = data.usuarioIngreso || '';

            // Mostrar el formulario de resultados
            document.getElementById('form-resultado-busqueda').classList.remove('d-none');
        } else {
            const messageClient = response.message || "No se encontraron datos para la búsqueda.";
            Utils.showToast(messageClient, 'warning');
        }
    } catch (error) {
        Utils.showToast("Error al buscar el ciclo", 'danger');
        console.error("Error en la búsqueda:", error);
    }
}


// Función para limpiar formulario
function limpiarFormularioCiclo() {
    document.getElementById('ciclo-nombre').value = '';
    document.getElementById('ciclo-descripcion').value = '';

    const form = document.querySelector('.needs-validation');
    if (form) {
        form.classList.remove('was-validated');
    }
}

// Función para habilitar validación
function habilitarValidacionCiclo() {
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
