
let userInfoDecanato = JSON.parse(localStorage.getItem('userInfo'));
async function cargarDatosDecanatos() {
    let response;

    try {
        const decanatoResponse = await Utils.httpRequest(
            `${Utils.path}/decanato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            response = decanatoResponse;
            const tablaBody = document.querySelector("#tabla-decanato tbody");
            tablaBody.innerHTML = '';
            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(decanato => {
                    const usuarioActualizacion = decanato.usuarioActualizacion || "No disponible";
                    const fila = `
                        <tr>
                            <td>${decanato.idDecanato}</td>
                            <td>${decanato.nombre}</td>
                            <td>${Utils.formatFecha(decanato.fCreacion)}</td >
                            <td>${Utils.formatFecha(decanato.fModificacion) || 'No disponible'}</td>
                            <td>${decanato.usuarioIngreso}</td>
                            <td>${usuarioActualizacion || 'No Actualizado'}</td>
                            <td>
                                <i class="bi bi-pencil-fill text-success me-3" style="cursor: pointer;" onclick="editarDecanato(${decanato.idDecanato})" data-bs-toggle="tooltip" data-bs-placement="top" title="Editar Decanato"></i>
                                <i class="bi bi-trash-fill text-danger" style="cursor: pointer;" onclick="mostrarConfirmacionEliminar(${decanato.idDecanato})" data-bs-toggle="tooltip" data-bs-placement="top" title="Eliminar Decanato"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                document.getElementById('decanato-usuario-ingreso').value = userInfoDecanato.nombre;
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-decanato').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="6" class="text-center">No se encontraron decanatos.</td></tr>';
                Utils.showToast('NO EXISTEN DECANATOS REGISTRADOS', 'info');
            }
        }, 150);
    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function editarDecanato(idDecanato) {

    const decanato = await obtenerDecanatoPorId(idDecanato);

    if (decanato) { 
        document.getElementById("idDecanatoEdit").value = idDecanato;
        document.getElementById("nombreEdit").value = decanato.nombre;
        document.getElementById("fCreacionEdit").value = Utils.formatFecha(decanato.fCreacion);
        document.getElementById("usuarioActualizacionEdit").value = userInfoDecanato.nombre; 
         
        const myModal = new bootstrap.Modal(document.getElementById('editarDecanatoModal'));
        myModal.show();
    }

}
async function guardarCambiosDecanato() {
    const nombre = document.getElementById("nombreEdit").value;
    const usuarioActualizacion = userInfoDecanato.idUsuario;
    const idDecanato = parseInt(document.getElementById("idDecanatoEdit").value, 10);

    const data = {
        "Nombre": `${nombre}`,
        "UsuarioActualizacion": `${usuarioActualizacion}`,
        "IdDecanato": `${idDecanato}`
    };
    const myModal = bootstrap.Modal.getInstance(document.getElementById('editarDecanatoModal'));
    
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/decanato/modificar`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        }
        );

        if (response.cod === "OK") {
            Utils.showToast("Decanato actualizado exitosamente", "success");

            if ($.fn.DataTable.isDataTable('#tabla-decanato')) {
                $('#tabla-decanato').DataTable().clear().destroy();
            }

            cargarDatosDecanatos();
        } else {
            Utils.showToast("Error al actualizar el decanato", "danger");
        }
    } catch (error) {
        Utils.showToast("Error al guardar los cambios", "danger");
    } finally {
        myModal.hide();
    }
}

async function crearDecanato(event) {
    event.preventDefault();
    const nombre = document.getElementById("decanato-nombre").value.trim();
    const usuarioIngreso = userInfoDecanato.idUsuario;

    const data = { 
        "nombre": `${nombre}`,
        "usuarioIngreso": `${usuarioIngreso}`
    };
    
    try {
        const responseCrear = await Utils.httpRequest(
            `${Utils.path}/decanato/crear`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
             }
        );

        if (responseCrear.cod === "OK") {
            Utils.showToast("Decanato actualizado exitosamente", "success");
            document.getElementById("decanato-nombre").value = "";

            if ($.fn.DataTable.isDataTable('#tabla-decanato')) {
                $('#tabla-decanato').DataTable().clear().destroy();
            }

            const tablaTab = document.querySelector('#home-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();

            cargarDatosDecanatos();  
        } else {
            const messageClient = responseCrear.message || "Error al actualizar la persona.";
            const messageTech = responseCrear.data || null;
            Utils.showErrorModal(messageClient, messageTech);
            Utils.showToast("Error al actualizar el decanato", "danger");
        }
    } catch (error) {
        Utils.showToast("Error al guardar los cambios", "danger");
    }
}


async function mostrarConfirmacionEliminar(idDecanato) {
    const modalEliminar = new bootstrap.Modal(document.getElementById('confirmarEliminarModal'));
    const confirmarBtn = document.getElementById('confirmarEliminarBtn');

    // Limpiamos cualquier evento previo para evitar duplicaciones
    confirmarBtn.onclick = () => eliminarDecanato(idDecanato);

    modalEliminar.show();
}
async function eliminarDecanato(idDecanato) {


    const data = {
        "idDecanato": `${idDecanato}`,
    }; 
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/decanato/eliminar`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        }
        );

        if (response.cod === "OK") {
            Utils.showToast("Decanato actualizado exitosamente", "success");

            if ($.fn.DataTable.isDataTable('#tabla-decanato')) {
                $('#tabla-decanato').DataTable().clear().destroy();
            }

            cargarDatosDecanatos();
        } else {
            Utils.showToast("Error al actualizar el decanato", "danger");
        }
    } catch (error) {
        Utils.showToast("Error al guardar los cambios", "danger");
    } finally {
        const modalEliminar = bootstrap.Modal.getInstance(document.getElementById('confirmarEliminarModal'));
        modalEliminar.hide();
    }
     
}
async function BuscarDecanato() {
    const idAbuscar = document.getElementById('buscar-decanato').value;
    if (!idAbuscar) {
        Utils.showToast("Por favor ingresa el ID del Decanato", 'warning');
        return;
    }

    const decanato = await obtenerDecanatoPorId(idAbuscar);
    if (decanato) {
        document.getElementById('nombreDecanato').value = decanato.nombre;
        document.getElementById('fCreacion').value = Utils.formatFecha(decanato.fCreacion);
        document.getElementById('fModificacion').value = Utils.formatFecha(decanato.fModificacion) || 'No disponible';
        document.getElementById('usuarioIngreso').value = decanato.usuarioIngreso;
        document.getElementById('usuarioActualizacion').value = decanato.usuarioActualizacion || 'No disponible';
    } else { 
            Utils.showToast("No se encontró el Decanato con ese ID.", 'info');
    }
}
async function obtenerDecanatoPorId(idDecanato) {

    try {
        const DecanatoResponse = await httpRequest(`${Utils.path}/decanato/id`, "POST", { idDecanato: idDecanato });

        if (DecanatoResponse.cod === Utils.COD_OK) {
            return DecanatoResponse.data;
        } else {
            const messageClient = DecanatoResponse.message || "Ocurrió un error inesperado.";
            const messageTech = DecanatoResponse.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al obtener el Decanato:", 'danger');
    }
}