
let userInfoPersonaFac = JSON.parse(localStorage.getItem('userInfo'));

async function cargarFacilitador() {
    try {
        const personaResponse = await Utils.httpRequest(
            `${Utils.path}/expositor/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        const tablaBody = document.querySelector("#tabla-facilitador tbody");
        tablaBody.innerHTML = ''; // Limpiar la tabla antes de cargar nuevos datos.

        if (personaResponse.cod === "OK" && personaResponse.data.length > 0) {
            personaResponse.data.forEach((registro) => {
                const persona = registro.tpersona || {};
                const genero = persona.genero === 'M'
                    ? '<i class="bi bi-person-standing text-primary" data-bs-toggle="tooltip" data-bs-placement="top" title="Masculino"></i>'
                    : '<i class="bi bi-person-standing-dress text-danger" data-bs-toggle="tooltip" data-bs-placement="top" title="Femenino"></i>';

                const fila = `
                    <tr>
                        <td>${registro.idExpositor}</td>
                        <td>${persona.cedula || 'Sin cédula'}</td>
                        <td>${persona.nombres || 'N/A'} ${persona.apellidos || ''}</td>
                        <td>${persona.edad || 'N/A'}</td>
                        <td>${genero}</td>
                        <td>${ Utils.formatFecha(registro.fCreacion) || 'N/A'}</td>
                        <td>${registro.ususarioIngreso || 'N/A'}</td>
                    </tr>
                `;
                tablaBody.insertAdjacentHTML("beforeend", fila);
            });

            Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
             
            const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            tooltipTriggerList.forEach((tooltipTriggerEl) => {
                new bootstrap.Tooltip(tooltipTriggerEl);
            });

        } else {
            tablaBody.innerHTML = '<tr><td colspan="8" class="text-center">No se encontraron registros.</td></tr>';
            Utils.showToast('NO EXISTEN REGISTROS', 'info');
        }
         
        userInfoPersona = JSON.parse(localStorage.getItem('userInfo'));
        await cargarRoles();

    } catch (error) {
        console.error(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function buscarFacilitador() {
    const idActaBuscar = document.getElementById('buscar-facilitador').value;
    limpiarInputs(); // Limpiar inputs antes de cargar nueva información
    try {
        const payload = {
            "idExpositor": idActaBuscar
        };
        const responseFaciSearch = await Utils.httpRequest(
            `${Utils.path}/expositor/id`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });
        if (responseFaciSearch.cod == Utils.COD_OK) {
            const facilitador = responseFaciSearch.data;
            if (facilitador) {
                // Rellenar inputs con la información obtenida
                document.getElementById('idExpositorFacilitador').value = facilitador.idExpositor;
                document.getElementById('cedulaFacilitador').value = facilitador.cedula;
                document.getElementById('nombresFacilitador').value = facilitador.tpersona.nombres;
                document.getElementById('apellidosFacilitador').value = facilitador.tpersona.apellidos;
                document.getElementById('edadFacilitador').value = facilitador.tpersona.edad;
                document.getElementById('generoFacilitador').value = facilitador.tpersona.genero;
                document.getElementById('fechaCreacionFacilitador').value = facilitador.tpersona.fechaCreacion;
                document.getElementById('usuarioIngresoFacilitador').value = facilitador.tpersona.usuarioIngreso;
                document.getElementById('usuarioActualizacionFacilitador').value = facilitador.tpersona.usuarioActualizacion || 'N/A';
                document.getElementById('fechaModificacionFacilitador').value = facilitador.tpersona.fechaModificacion || 'N/A';
                document.getElementById('fCreacionFacilitador').value = facilitador.fCreacion;
            } else {
                Utils.showToast("No se encontraron datos", 'warning');
            }
        } else {
            const messageClient = responseFaciSearch.message || "Ocurrió un error inesperado.";
            const messageTech = responseFaciSearch.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.log(error);
        Utils.showToast("Error al obtener el documento", 'danger');
    }
}

function limpiarInputs() {
    // Limpiar todos los inputs de la tarjeta
    const inputIds = [
        'idExpositorFacilitador', 'cedulaFacilitador', 'nombresFacilitador',
        'apellidosFacilitador', 'edadFacilitador', 'generoFacilitador',
        'fechaCreacionFacilitador', 'usuarioIngresoFacilitador',
        'usuarioActualizacionFacilitador', 'fechaModificacionFacilitador',
        'fCreacionFacilitador'
    ];
    inputIds.forEach(id => {
        const inputElement = document.getElementById(id);
        if (inputElement) {
            inputElement.value = '';
        }
    });
}
