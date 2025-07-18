// DECLARACION DE VARIABLES GLOBALES

let idRolAEliminar = null;
let userInfoR = JSON.parse(localStorage.getItem('userInfo'));


/// DECLARACION DE FUNCIONES 
async function cargarDatosRoles() { 
    let response;
    try {
        const rolesResponse = await Utils.httpRequest(
            `${Utils.path}/rol/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        const roles = userInfoR.roles;
        setTimeout(() => {
            response = rolesResponse;
             
            const tablaBody = document.querySelector("#tabla-rol tbody");
             
            tablaBody.innerHTML = '';
             
            if (response.cod === Utils.COD_OK && response.data.length > 0) { 
                response.data.forEach(rol => {
                    const estadoTexto = rol.estado ? "Activo" : "Inactivo";
                    const usuarioIngreso = rol.usuarioIngreso || "No disponible";

                    const rolEsEditable = !roles.includes(rol.nombre);
                    const fila = `
                <tr>
                    <td>${rol.idRol}</td>
                    <td>${rol.nombre}</td>
                    <td>${rol.observacion}</td>
                    <td>${estadoTexto}</td>
                    <td>${usuarioIngreso}</td>  
                    <td>
                        <i class="bi bi-pencil-fill text-success me-3" style="cursor: pointer;" onclick="editarRol(${rol.idRol})" data-bs-toggle="tooltip" data-bs-placement="top" title="Editar Rol"></i>
                        <i class="bi bi-trash-fill text-danger" style="cursor: ${rolEsEditable ? 'pointer' : 'not-allowed'};" onclick="${rolEsEditable ? `eliminarRol(${rol.idRol})` : ''}" data-bs-toggle="tooltip" data-bs-placement="top" title="Eliminar Rol" ${rolEsEditable ? '' : 'disabled'}></i>

                    </td>
                </tr>
            `; 
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                let userInfo = JSON.parse(localStorage.getItem('userInfo'));
                document.getElementById("usuarioIngreso").value = `${userInfo.nombre}`;

                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-rol').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else { 
                tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron roles.</td></tr>';
                Utils.showToast('NO EXISTEN ROLES REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
 
 
  
 
async function agregarRol(event) { 

    event.preventDefault()
    if (!validarFormularioRol()) {
        return false; // Detener el envío
    };

    const bodyRequest = {
        Nombre: document.getElementById('rolNombre').value.trim(),
        Estado: document.getElementById('rolEstado').value === 'true',
        UsuarioIngreso: userInfoR.idUsuario,
        Observacion: document.getElementById('rolDescripcion').value,
    };

    try {
        const rolesResponse = await httpRequest(`${Utils.path}/rol/crear`, "POST", bodyRequest);

        if (rolesResponse.cod === Utils.COD_OK) {
            Utils.showToast('ROL REGISTRADO EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-rol')) {
                $('#tabla-rol').DataTable().clear().destroy();
            }

            const tablaTab = document.querySelector('#home-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();

            limpiarRol(event);
            cargarDatosRoles();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el rol:", 'danger');
    }
}


 
function limpiarRol(event) {
    event.preventDefault()
    document.getElementById('rolNombre').value = '';
    document.getElementById('rolEstado').value = '';
    document.getElementById('rolDescripcion').value = '';

    // Obtén el formulario
    const form = document.querySelector('.needs-validation');

    if (form) {
        // Elimina las clases relacionadas con la validación
        form.classList.remove('was-validated');
    }
}

function generarAccionesHtml(tipo, nombre) {
    return `
        <div class="text-end">
            <button class="btn btn-primary btn-sm" onclick="editarElemento('${tipo}', '${nombre}')">Editar</button>
            <button class="btn btn-danger btn-sm" onclick="eliminarElemento('${tipo}', '${nombre}')">Eliminar</button>
        </div>`;
}

function eliminarRol(idRol) {
    idRolAEliminar = idRol;
    
    const modal = new bootstrap.Modal(document.getElementById('confirmarEliminacionModal'));
    modal.show(); 
}
async function confirmarEliminacion( ) { 
    if (!idRolAEliminar) {
        Utils.showToast("ID de rol no válido", "danger");
        return;
    }

    try {
        const response = await httpRequest(`${Utils.path}/rol/eliminar`, "POST", { idRol: idRolAEliminar });

        if (response && response.cod === Utils.COD_OK) {
            Utils.showToast("Rol eliminado exitosamente", "success");

            if ($.fn.DataTable.isDataTable('#tabla-rol')) {
                $('#tabla-rol').DataTable().clear().destroy();
            }

            cargarDatosRoles();
        } else {
            const messageClient = response.message || "Error al eliminar el rol.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al realizar la eliminación", "danger");
    } finally {
        idRolAEliminar = null;
        const modal = bootstrap.Modal.getInstance(document.getElementById('confirmarEliminacionModal'));
        modal.hide();
    }
}
async function editarRol(idRol) { 
    try {
        const rol = await obtenerRolPorId(idRol);

        // Cargar los datos en el modal
        
        document.getElementById('rolIdEdit').value = rol.idRol;
        document.getElementById('rolNombreEdit').value = rol.nombre;
        document.getElementById('rolDescripcionEdit').value = rol.observacion;
        document.getElementById('rolEstadoEdit').value = rol.estado ? "true" : "false";
            
        const modal = new bootstrap.Modal(document.getElementById('editarRolModal'));
        modal.show();

    } catch (error) {
        console.error("Error al obtener los datos del rol:", error);
        Utils.showToast("Error al cargar los datos del rol", 'danger');
    }

}
async  function obtenerRolPorId(id) {

    try {
        const rolesResponse = await httpRequest(`${Utils.path}/rol/id`, "POST", { idRol: id });

        if (rolesResponse.cod === Utils.COD_OK) {
            return rolesResponse.data;
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el rol:", 'danger');
    }
}
async function confirmarEditar(event) {

    event.preventDefault();
    const form = event.target; 
    if (!form.checkValidity()) {
        form.classList.add('was-validated'); 
        return;  
    }

    try {
        const idRol = document.getElementById('rolIdEdit').value;
        const nombre = document.getElementById('rolNombreEdit').value;
        const estado = document.getElementById('rolEstadoEdit').value;
        const observacion = document.getElementById('rolDescripcionEdit').value;

        const data = {
            idRol: idRol,
            Nombre: nombre,
            Estado: estado === "true",
            Observacion: observacion,
            UsuarioActualizacion: userInfoR.idUsuario
        };


        const response = await httpRequest(`${Utils.path}/rol/modificar`, "POST", data);

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Rol actualizado con éxito", 'info');

            if ($.fn.DataTable.isDataTable('#tabla-rol')) {
                $('#tabla-rol').DataTable().clear().destroy();
            }

            const modal = bootstrap.Modal.getInstance(document.getElementById('editarRolModal'));
            modal.hide();

            cargarDatosRoles();
        } else {
            const messageClient = rolesResponse.message || "Ocurrió un error inesperado.";
            const messageTech = rolesResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al confirmar la edición", 'danger');
    } finally {
        const modal = new bootstrap.Modal(document.getElementById('editarRolModal'));
        modal.hide();
    }
}

async function buscarRol() {
    const idRol = document.getElementById('buscar-rol').value;
    const rol = await obtenerRolPorId(idRol);



    // Asignar valores a los campos
    document.getElementById('idRolFind').value = rol.idRol || "-";
    document.getElementById('rolNombreFind').value = rol.nombre || "-";
    document.getElementById('estadoRolFind').value = rol.estado ? "Activo" : "Inactivo";
    document.getElementById('usuarioIngresoFind').value = rol.usuarioIngreso;
    document.getElementById('fechaInicio').value = Utils.formatFecha(rol.fCreacion);
    document.getElementById('fechaActualizacion').value = Utils.formatFecha(rol.fModificacion);
    document.getElementById('usuarioActualizacion').value = rol.usuarioActualizacion || "-";
    document.getElementById('rolDescripcionFind').value = rol.observacion || "-";
}

function validarFormularioRol() {
    const nombre = document.getElementById("rolNombre").value.trim();
    const estado = document.getElementById("rolEstado").value;
    const descripcion = document.getElementById("rolDescripcion").value.trim();
     
    const inputs = [document.getElementById("rolNombre"), document.getElementById("rolEstado"), document.getElementById("rolDescripcion")];
    inputs.forEach(input => input.classList.remove("is-invalid"));

    let isValid = true;

    if (!nombre) {
        document.getElementById("rolNombre").classList.add("is-invalid");
         Utils.showErrorModal("El campo 'Nombre' es obligatorio.", "info");
        isValid = false;
    }

    if (!estado) {
        document.getElementById("rolEstado").classList.add("is-invalid");
         Utils.showErrorModal("Debes seleccionar un 'Estado'.", "info");
        isValid = false;
    }

    if (!descripcion) {
        document.getElementById("rolDescripcion").classList.add("is-invalid");
         Utils.showErrorModal("El campo 'Descripción' es obligatorio.", "info");
        isValid = false;
    }

    return isValid;
}

async function httpRequest(url, method, body = null) {
    Utils.showLoader();
    try {
        const options = {
            method, 
            headers: {
                "Content-Type": "application/json",
            },
        };
        if (body) {
            options.body = JSON.stringify(body);
        }

        const response = await fetch(url, options);

        return await response.json(); 
    } catch (error) {
        const messageClient = "Error en la petición";
        const messageTech = error.message || error;
        Utils.showErrorModal(messageClient, messageTech);
    } finally {
        setTimeout(() => Utils.hideLoader(), 2000);
    }
}