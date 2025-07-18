// Función para cargar datos de grupos 
let userInfoEmitir = JSON.parse(localStorage.getItem('userInfo'));
async function cargarDatosCertificadosEmicion() {
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
 
            const selectGrupo = document.getElementById("persona-id-grupo"); 
             
            selectGrupo.innerHTML = '<option value="">Selecciona un grupo</option>'; 

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(grupo => {
                     
                    const option = document.createElement("option");
                    option.value = grupo.idGrupo;
                    option.textContent = grupo.nombre;  
                    selectGrupo.appendChild(option); 
                }); 
                const selectGrupoEvent = document.querySelector("#persona-id-grupo");
                selectGrupoEvent.addEventListener("change", async (event) => {
                    const idGrupoSeleccionado = event.target.value;
                    if (idGrupoSeleccionado) {
                        await listarIntegrantes(idGrupoSeleccionado);
                    }
                });


                $('#tabla-certificado').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });
                 
            } else { 
                
                Utils.showToast('NO EXISTEN GRUPOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarRolesCertificado();
         
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
 
async function listarIntegrantes(id) {
    const beneficiario = document.getElementById("beneficiario").value;

    try {
        const payload = {
            idGrupo: id,
            idRol: beneficiario
        };

        const requestIntegrantes = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/pendientes`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            },
            true
        );

        const tablaBody = document.querySelector("#tabla-certificado tbody");
        tablaBody.innerHTML = '';

        if (requestIntegrantes.cod === Utils.COD_OK && requestIntegrantes.data.length > 0) {
            requestIntegrantes.data.forEach(persona => {
                const asistenciaMarcada = true; // Por defecto marcado
                const calificacionMarcada = true; // Por defecto marcado
                const estado = asistenciaMarcada && calificacionMarcada ? "Aprobado" : "Pendiente";

                const fila = `
                    <tr>
                        <td>${persona.tpersona.cedula}</td>
                        <td>${persona.tpersona.nombres} ${persona.tpersona.apellidos}</td>
                        <td>
                            <div class="form-check">
                                <input class="form-check-input asistencia" type="checkbox" ${asistenciaMarcada ? 'checked' : ''}>
                            </div>
                        </td>
                        <td>
                            <div class="form-check">
                                <input class="form-check-input calificacion" type="checkbox" ${calificacionMarcada ? 'checked' : ''}>
                            </div>
                        </td>
                        <td class="estado">${estado}</td>
                    </tr>
                `;
                tablaBody.insertAdjacentHTML("beforeend", fila);
            });

            reseteoSeleccion();
            const selectCondicion = document.getElementById("condicion");
            selectCondicion.onchange = () => {
                actualizarEstados();
                manejarHabilitacionCheckboxes(selectCondicion.value);
            };
            escucharCambiosCheckboxes();
        } else {
            Utils.showToast('NO EXISTEN INTEGRANTES EN ESTE GRUPO', 'info');
        }
    } catch (error) {
        console.error('ERROR -->', error);
        Utils.showToast("ERROR AL CARGAR DATOS", "error");
    }
}

function reseteoSeleccion() {
    const selectCondicion = document.querySelector("#condicion");
    if (selectCondicion) {
        selectCondicion.value = "1"; 
    }
}

function escucharCambiosCheckboxes() {
    const checkboxes = document.querySelectorAll(".asistencia, .calificacion");

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("change", () => {
            actualizarEstados();  // Actualiza los estados inmediatamente
        });
    });
}

// Función para actualizar los estados según la condición seleccionada
function actualizarEstados() {
    const condicion = document.querySelector("#condicion").value;
    const filas = document.querySelectorAll("#tabla-certificado tbody tr");

    filas.forEach(fila => {
        const asistenciaMarcada = fila.querySelector(".asistencia").checked;
        const calificacionMarcada = fila.querySelector(".calificacion").checked;
        let estado;

        // Determinar el estado según la condición seleccionada
        if (condicion === "1") {  // Ambos
            estado = (asistenciaMarcada && calificacionMarcada) ? "Aprobado" : "No Aprobado";
        } else if (condicion === "2") {  // Calificación
            estado = calificacionMarcada ? "Aprobado" : "No Aprobado";
        } else if (condicion === "3") {  // Asistencia
            estado = asistenciaMarcada ? "Aprobado" : "No Aprobado";
        }

        fila.querySelector(".estado").textContent = estado;  
    });
}

function manejarHabilitacionCheckboxes(condicion) {
    const checkboxesAsistencia = document.querySelectorAll(".asistencia");
    const checkboxesCalificacion = document.querySelectorAll(".calificacion");

    if (condicion === "2") {   
        checkboxesAsistencia.forEach(cb => cb.disabled = true);   
        checkboxesCalificacion.forEach(cb => cb.disabled = false);   
    } else if (condicion === "3") {  
        checkboxesAsistencia.forEach(cb => cb.disabled = false);   
        checkboxesCalificacion.forEach(cb => cb.disabled = true);   
    } else {  // AMBOS
        checkboxesAsistencia.forEach(cb => cb.disabled = false);   
        checkboxesCalificacion.forEach(cb => cb.disabled = false);  
    }
}

function obtenerDatosAprobadosC() { 
    const selectGrupo = document.getElementById("persona-id-grupo");
    const selectBeneficiario = document.getElementById("beneficiario");
    const idGrupo = selectGrupo.value;
    const idRol = selectBeneficiario.value;

    if (!idRol) {
        Utils.showToast("Por favor selecciona un beneficiario antes de continuar", "info");
        return null;
    }

    if (!idGrupo) {
        Utils.showToast("Por favor selecciona un grupo antes de continuar", "info");
        return null;  
    }
     
    const tablaBody = document.querySelector("#tabla-certificado tbody");
    const filas = tablaBody.querySelectorAll("tr");
    const cedulasAprobadas = [];

    filas.forEach(fila => {
        const estado = fila.querySelector(".estado").textContent.trim();
        if (estado === "Aprobado") {
            const cedula = fila.querySelector("td:nth-child(1)").textContent.trim();
            cedulasAprobadas.push(cedula);
        }
    });

    if (cedulasAprobadas.length === 0) {
        Utils.showToast("No hay integrantes con estado 'Aprobado'", "info");
        return null;
    }

    // Estructura del payload
    const datosAprobados = {
        IdGrupo: parseInt(idGrupo, 10),
        Aprobados: cedulasAprobadas,
        usuarioActualizacion: `${userInfoEmitir.idUsuario}`
    };

    return datosAprobados;
} 
async function enviarDatosAprobados() {
    const datosApr = obtenerDatosAprobadosC();
    if (!datosApr) return;
    try {
        const requestApr = await Utils.httpRequest(
            `${Utils.path}/grupoPersona/aprobar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(datosApr)
            },
            true
        );
        if (requestApr.cod === Utils.COD_OK) {
            Utils.showToast('DATOS PROCESADOS EXITOSAMENTE', 'info');

            if ($.fn.DataTable.isDataTable('#tabla-certificado')) {
                $('#tabla-certificado').DataTable().clear().destroy();
            }

            limpiarTabla();
            cargarDatosCertificadosEmicion();
        } else {
            const messageClient = requestApr.message || "Ocurrió un error inesperado.";
            const messageTech = requestApr.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {
        console.error("Error al enviar los datos aprobados:", error);
        Utils.showToast("Error al procesar la solicitud", "error");
    }

}
async function cargarRolesCertificado() {
    try {
        const responseRol = await Utils.httpRequest(
            `${Utils.path}/rol/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        if (responseRol.cod === Utils.COD_OK && responseRol.data.length > 0) {
            const selectRol = document.getElementById("beneficiario");

            // Agregar la opción por defecto
            selectRol.innerHTML = '<option value="">Seleccionar Beneficiarios</option>';

            responseRol.data.forEach(rol => {
                // Verificar si el nombre del rol es "Participantes" o "Facilitadores"
                if (rol.nombre === "Participante" || rol.nombre === "Facilitador") {
                    const option = document.createElement("option");
                    option.value = rol.idRol;
                    option.textContent = rol.nombre;
                    selectRol.appendChild(option);
                }
            });
        } else {
            Utils.showToast('NO EXISTEN MODALIDADES REGISTRADAS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

function limpiarTabla() {
    const tablaBody = document.querySelector("#tabla-certificado tbody");
    if (tablaBody) {
        tablaBody.innerHTML = ''; // Vacía la tabla
    }
}