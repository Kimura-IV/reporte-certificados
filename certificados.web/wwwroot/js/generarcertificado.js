// Cargar los grupos disponibles para generar certificados
async function cargarDatosGenerarCertificados() {
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
            const selectGrupo = document.getElementById("grupo");
            selectGrupo.innerHTML = '<option value="">Selecciona un grupo</option>';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(grupo => {
                    const option = document.createElement("option");
                    option.value = grupo.idGrupo;
                    option.textContent = grupo.nombre;
                    selectGrupo.appendChild(option);
                });

                if ($.fn.DataTable.isDataTable('#tablaParticipantes')) {
                    $('#tablaParticipantes').DataTable().clear().destroy();
                }

                // Inicializar DataTable para la tabla de participantes
                $('#tablaParticipantes').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

                const selectGrupoEvent = document.querySelector("#grupo");
                selectGrupoEvent.addEventListener("change", async (event) => {
                    const idGrupoSeleccionado = event.target.value;
                    if (idGrupoSeleccionado) {
                        await listarIntegrantesAprobados(idGrupoSeleccionado);
                    }
                });

            } else {
                Utils.showToast('NO EXISTEN GRUPOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarFormatosGenerar();
        await cargarDecanatosGenerar();
        await cargarRolesGenerarCertificado();

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Listar los integrantes aprobados del grupo seleccionado
async function listarIntegrantesAprobados(id) {
    const beneficiario = document.getElementById("generar-beneficiario").value;

    try {
        const payload = {
            idGrupo: id,
            estado: false, // Solo los aprobados
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

        const tablaBody = document.querySelector("#tablaParticipantes tbody");
        tablaBody.innerHTML = ''; // Limpiar tabla antes de añadir nuevos datos

        if (requestIntegrantes.cod === Utils.COD_OK && requestIntegrantes.data.length > 0) {
            requestIntegrantes.data.forEach(persona => {
                const asistenciaMarcada = true;  // Por defecto está marcado
                const calificacionMarcada = true;  // Por defecto está marcado
                const estado = asistenciaMarcada && calificacionMarcada ? "Aprobado" : "Pendiente";

                const fila = `
                    <tr>
                        <td>${persona.tpersona.cedula}</td>
                        <td>${persona.tpersona.nombres} ${persona.tpersona.apellidos}</td>
                        <td>
                            <div class="form-check">
                                <input class="form-check-input asistencia" type="checkbox" checked disabled>
                            </div>
                        </td>
                        <td>
                            <div class="form-check">
                                <input class="form-check-input calificacion" type="checkbox" checked disabled>
                            </div>
                        </td>
                        <td class="estado">${estado}</td>
                        <td>
                            <i class="i bi-file-earmark-pdf-fill text-primary me-3"
                            style="cursor: pointer;font-size: 1.4rem;"
                            onclick="generarCertificado('${persona.tpersona.cedula}')" 
                            data-bs-toggle="tooltip" 
                            data-bs-placement="top" 
                            title="Generar Certificado"></i>
                            <i class="i bi bi-envelope-arrow-up-fill text-secondary me-3"
                            style="cursor: pointer;font-size: 1.4rem;"
                            onclick="enviarCertificado('${persona.tpersona.cedula}')" 
                            data-bs-toggle="tooltip" 
                            data-bs-placement="top" 
                            title="Enviar Certificado"></i>
                        </td>
                    </tr>
                `;
                tablaBody.insertAdjacentHTML("beforeend", fila);
            });

            // Resetear la condición
            reseteoSeleccionCertificado();
            escucharCambiosCheckboxesCert();

        } else {
            Utils.showToast('NO EXISTEN INTEGRANTES EN ESTE GRUPO', 'info');
        }
    } catch (error) {
        console.log('ERROR -->', error);
        Utils.showToast("ERROR AL CARGAR DATOS", "error");
    }
}

// Función para resetear la selección de condición
function reseteoSeleccionCertificado() {
    const selectCondicion = document.querySelector("#condicion");
    if (selectCondicion) {
        selectCondicion.value = "1";  // Por defecto, selecciona "Ambos"
    }
}

// Función para escuchar cambios en los checkboxes de asistencia y calificación
function escucharCambiosCheckboxesCert() {
    const checkboxes = document.querySelectorAll(".asistencia, .calificacion");
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("change", () => {
            actualizarEstadosCertificado();
        });
    });
}

// Actualizar los estados según la condición seleccionada (asistencia/calificación)
function actualizarEstadosCertificado() {
    const condicion = document.querySelector("#condicion").value;
    const filas = document.querySelectorAll("#tablaParticipantes tbody tr");

    filas.forEach(fila => {
        const asistenciaMarcada = fila.querySelector(".asistencia").checked;
        const calificacionMarcada = fila.querySelector(".calificacion").checked;
        let estado;

        // Determinar el estado según la condición seleccionada
        if (condicion === "1") {  // Ambos
            estado = (asistenciaMarcada && calificacionMarcada) ? "Aprobado" : "No Aprobado";
        } else if (condicion === "2") {  // Solo Calificación
            estado = calificacionMarcada ? "Aprobado" : "No Aprobado";
        } else if (condicion === "3") {  // Solo Asistencia
            estado = asistenciaMarcada ? "Aprobado" : "No Aprobado";
        }

        fila.querySelector(".estado").textContent = estado;
    });
}

// Habilitar o deshabilitar checkboxes según la condición seleccionada
function manejarHabilitacionCheckboxesCert(condicion) {
    const checkboxesAsistencia = document.querySelectorAll(".asistencia");
    const checkboxesCalificacion = document.querySelectorAll(".calificacion");

    if (condicion === "2") {
        checkboxesAsistencia.forEach(cb => cb.disabled = true);
        checkboxesCalificacion.forEach(cb => cb.disabled = false);
    } else if (condicion === "3") {
        checkboxesAsistencia.forEach(cb => cb.disabled = false);
        checkboxesCalificacion.forEach(cb => cb.disabled = true);
    } else {  // Ambos
        checkboxesAsistencia.forEach(cb => cb.disabled = false);
        checkboxesCalificacion.forEach(cb => cb.disabled = false);
    }
}

// Obtener los datos de los aprobados para su procesamiento
function obtenerDatosAprobados() {
    const selectGrupo = document.getElementById("grupo");
    const idGrupo = selectGrupo.value;

    if (!idGrupo) {
        Utils.showToast("Por favor selecciona un grupo antes de continuar", "info");
        return null;
    }

    const tablaBody = document.querySelector("#tablaParticipantes tbody");
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

    const datosAprobados = {
        IdGrupo: parseInt(idGrupo, 10),
        Aprobados: cedulasAprobadas,
        usuarioActualizacion: `${userInfo.idUsuario}`
    };

    return datosAprobados;
}

// Generar certificados para los aprobados
async function generarCertificado(cedula) {
    try {
        // Obtener el valor del formato seleccionado
        const formato = document.getElementById('formato').value;
        const decanato = document.getElementById('generar-decanato').value;

        // Validar si el formato está seleccionado
        if (!formato || formato === "Selecciona Formato") {
            Utils.showToast('Por favor, selecciona un formato.', 'info');
            return;
        }
        if (!decanato || decanato === "Seleccionar Decanato") {
            Utils.showToast('Por favor, selecciona un decanato.', 'info');
            return;
        }

        // Validar si la cédula es válida
        if (!cedula) {
            Utils.showToast('Cédula no válida.', 'error');
            return;
        }

        // Crear el cuerpo de la solicitud con cédula y formato
        const requestApr = await Utils.httpRequest(
            `${Utils.path}/certificado/generarcertificado`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    cedula: cedula,
                    idFormato: formato,
                    idDecanato: decanato,
                    isEnviarEmail: false,
                    idUsuario: userInfoCA.idUsuario
                })
            },
            true
        );

        // Manejar la respuesta
        if (requestApr.cod === Utils.COD_OK) {
            const data = requestApr.data;

            // Mostrar el PDF en el modal
            const pdfViewer = document.getElementById('pdfViewer');
            const downloadLink = document.getElementById('downloadLink');

            // Convertir base64 a blob
            const byteCharacters = atob(data);
            const byteNumbers = new Array(byteCharacters.length);
            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            const byteArray = new Uint8Array(byteNumbers);
            const blob = new Blob([byteArray], { type: 'application/pdf' });

            // Crear una URL para el blob
            const url = URL.createObjectURL(blob);

            // Mostrar el PDF en el iframe
            pdfViewer.src = url;

            // Configurar el enlace de descarga
            downloadLink.href = url;

            // Mostrar el modal
            const pdfModal = new bootstrap.Modal(document.getElementById('pdfModal'));
            pdfModal.show();

            Utils.showToast('DATOS PROCESADOS EXITOSAMENTE', 'info');

        } else {
            const messageClient = requestApr.message || "Ocurrió un error inesperado.";
            const messageTech = requestApr.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {
        Utils.showToast("Error al procesar la solicitud", "error");
    }
}

async function enviarCertificado(cedula) {
    try {
        // Obtener el valor del formato seleccionado
        const formato = document.getElementById('formato').value;
        const decanato = document.getElementById('generar-decanato').value;

        // Validar si el formato está seleccionado
        if (!formato || formato === "Selecciona Formato") {
            Utils.showToast('Por favor, selecciona un formato.', 'info');
            return;
        }
        if (!decanato || decanato === "Seleccionar Decanato") {
            Utils.showToast('Por favor, selecciona un decanato.', 'info');
            return;
        }

        // Validar si la cédula es válida
        if (!cedula) {
            Utils.showToast('Cédula no válida.', 'error');
            return;
        }

        // Crear el cuerpo de la solicitud con cédula y formato
        const requestApr = await Utils.httpRequest(
            `${Utils.path}/certificado/generarcertificado`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    cedula: cedula,
                    idFormato: formato,
                    idDecanato: decanato,
                    isEnviarEmail: true,
                    idUsuario: userInfoCA.idUsuario

                })
            },
            true
        );

        // Manejar la respuesta
        if (requestApr.cod === Utils.COD_OK) {
            const mensaje = requestApr.message;
            Utils.showToast(mensaje, 'info');

        } else {
            const messageClient = requestApr.message || "Ocurrió un error inesperado.";
            const messageTech = requestApr.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }

    } catch (error) {
        Utils.showToast("Error al procesar la solicitud", "error");
    }
}
async function cargarFormatosGenerar() {
    try {
        const responseFormato = await Utils.httpRequest(
            `${Utils.path}/formato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );
        if (responseFormato.cod === Utils.COD_OK && responseFormato.data.length > 0) {
            const selectFormato = document.getElementById("formato");

            responseFormato.data.forEach(format => {
                const option = document.createElement("option");
                option.value = format.idFormato;
                option.textContent = format.nombrePlantilla;
                selectFormato.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN FORMATOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function cargarDecanatosGenerar() {
    try {
        const responseDecanato = await Utils.httpRequest(
            `${Utils.path}/decanato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );
        if (responseDecanato.cod === Utils.COD_OK && responseDecanato.data.length > 0) {
            const selectDecanto = document.getElementById("generar-decanato");

            responseDecanato.data.forEach(dec => {
                const option = document.createElement("option");
                option.value = dec.idDecanato;
                option.textContent = dec.nombre;
                selectDecanto.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN DECANATOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function limpiarFrmGenerarCertificado() {
    document.getElementById('formParticipante').reset();
}

async function cargarRolesGenerarCertificado() {
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
            const selectRol = document.getElementById("generar-beneficiario");

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