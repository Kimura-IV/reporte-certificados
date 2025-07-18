// Funcion para cargar datos de formatos
async function cargarDatosFormatos() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/formato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-formatos tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(formato => {
                    const fila = `
                        <tr>
                            <td>${formato.idFormato}</td>
                            <td>${formato.nombrePlantilla}</td>
                            <td><img src="data:image/png;base64,${formato.logoUG}" alt="Logo UG" class="img-thumbnail" style="width: 50px; height: 50px;"></td>
                            <td><img src="data:image/png;base64,${formato.lineaGrafica}" alt="Linea Gráfica" class="img-thumbnail" style="width: 50px; height: 50px;"></td>
                            <td>${formato.qr ? `<img src="data:image/png;base64,${formato.qr}" alt="QR" class="img-thumbnail" style="width: 50px; height: 50px;">` : "No disponible"}</td>
                            <td>${formato.usuarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarFormato(${formato.idFormato})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Formato"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
                //document.getElementById("usuario-ingreso-formato").value = userInfo.nombre;

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-formato').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="7" class="text-center">No se encontraron formatos.</td></tr>';
                Utils.showToast('NO EXISTEN FORMATOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarDecanatos();

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Funcion para cargar datos de formatos
async function cargarDatosCertificado() {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/formato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-formatos tbody");
            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(formato => {
                    const fila = `
                        <tr>
                            <td>${formato.idFormato}</td>
                            <td>${formato.nombrePlantilla}</td>
                            <td><img src="data:image/png;base64,${formato.logoUG}" alt="Logo UG" class="img-thumbnail" style="width: 50px; height: 50px;"></td>
                            <td><img src="data:image/png;base64,${formato.lineaGrafica}" alt="Linea Gráfica" class="img-thumbnail" style="width: 50px; height: 50px;"></td>
                            <td>${formato.qr ? `<img src="data:image/png;base64,${formato.qr}" alt="QR" class="img-thumbnail" style="width: 50px; height: 50px;">` : "No disponible"}</td>
                            <td>${formato.usuarioIngreso || 'No disponible'}</td>
                            <td>
                                <i class="bi bi-trash-fill text-danger" 
                                   style="cursor: pointer;" 
                                   onclick="eliminarFormato(${formato.idFormato})" 
                                   data-bs-toggle="tooltip" 
                                   data-bs-placement="top" 
                                   title="Eliminar Formato"></i>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });
                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
                //document.getElementById("usuario-ingreso-formato").value = userInfo.nombre;

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });

                $('#tabla-formato').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="7" class="text-center">No se encontraron formatos.</td></tr>';
                Utils.showToast('NO EXISTEN FORMATOS REGISTRADOS', 'info');
            }
        }, 150);

        await cargarDecanatos();

    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Funcion para agregar formato
async function handleAgregarFormato(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();

    try {
        // Convertir imágenes a Base64
        const lineaGrafica = document.getElementById('linea-grafica').files[0]
            ? await convertirABase64(document.getElementById('linea-grafica').files[0])
            : null;
            console.log(lineaGrafica)
        const logoUg = document.getElementById('logo-ug').files[0]
            ? await convertirABase64(document.getElementById('logo-ug').files[0])
            : null;
        const qr = document.getElementById('qr').files[0]
            ? await convertirABase64(document.getElementById('qr').files[0])
            : null;

        // Obtener valores de los campos
        const nombrePlantilla = document.getElementById('nombre-plantilla').value;
        const origen = document.getElementById('origen').value;
        const tipo = document.getElementById('tipo').value;
        const leyenda = document.getElementById('leyenda').value;

        const firma1Decanato = document.getElementById('firma1-decanato').value;
        const firma1Nombre = document.getElementById('firma1-nombre').value;

        const firma2Decanato = document.getElementById('firma2-decanato').value;
        const firma2Nombre = document.getElementById('firma2-nombre').value;

        const firma3Decanato = document.getElementById('firma3-decanato').value;
        const firma3Nombre = document.getElementById('firma3-nombre').value;

        // Validar usuario ingreso
        const userInfoConfig = JSON.parse(localStorage.getItem('userInfo'));
        if (!userInfoConfig || !userInfoConfig.idUsuario) {
            Utils.showToast('Usuario no encontrado. Inicie sesión nuevamente.', 'danger');
            return;
        }

        // Crear el diccionario formatoData
        const formatoData = {
            "NombrePlantilla": nombrePlantilla,
            "LineaGrafica": lineaGrafica || '',  // Asegúrate de que sea una cadena
            "LogoUG": logoUg || '',  // Asegúrate de que sea una cadena
            "Origen": origen,
            "Tipo": tipo,
            "Qr": qr || '',  // Asegúrate de que sea una cadena
            "Leyenda": leyenda,
            "CargoFirmanteUno": firma1Decanato,
            "NombreFirmanteUno": firma1Nombre,
            "CargoFirmanteDos": firma2Decanato,
            "NombreFirmanteDos": firma2Nombre,
            "CargoFirmanteTres": firma3Decanato,
            "NombreFirmanteTres": firma3Nombre,
            "UsuarioIngreso": userInfoConfig.idUsuario.toString(),
        };

        const response = await Utils.httpRequest(
            `${Utils.path}/formato/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(formatoData),
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('FORMATO REGISTRADO EXITOSAMENTE', 'success');

            if ($.fn.DataTable.isDataTable('#tabla-formato')) {
                $('#tabla-formato').DataTable().clear().destroy();
            }

            limpiarFormularioFormato();
            cargarDatosFormatos();

            // Cambiar a la pestaña de tabla
            const tablaTab = document.querySelector('#tabla-formatos-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el formato", 'danger');
    }
}


// Funcion para editar formato
async function editarFormato(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/formato/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idFormato: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const formato = response.data;
            document.getElementById('editar-id-formato').value = formato.idFormato;
            document.getElementById('editar-nombre-plantilla').value = formato.nombrePlantilla;
            document.getElementById('editar-usuario-ingreso').value = formato.usuarioIngreso;
            document.getElementById('editar-logo-ug-preview').src = formato.logoUg ? `data:image/png;base64,${formato.logoUg}` : '';
            document.getElementById('editar-linea-grafica-preview').src = formato.lineaGrafica ? `data:image/png;base64,${formato.lineaGrafica}` : '';
            document.getElementById('editar-qr-preview').src = formato.qr ? `data:image/png;base64,${formato.qr}` : '';

            // Mostrar el modal
            const modal = new bootstrap.Modal(document.getElementById('modal-editar-formato'));
            modal.show();
        } else {
            Utils.showToast("Error al cargar datos del formato", 'danger');
        }
    } catch (error) {
        Utils.showToast("Error al obtener los datos del formato", 'danger');
    }
}


// Funcion para guardar edición
async function handleEditarFormato(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    // Obtener datos del formulario
    const idFormato = document.getElementById('editar-id-formato').value;
    const nombrePlantilla = document.getElementById('editar-nombre-plantilla').value;
    const logoUG = await convertirABase64(document.getElementById('editar-logo-ug').files[0]);
    const lineaGrafica = await convertirABase64(document.getElementById('editar-linea-grafica').files[0]);
    const qr = await convertirABase64(document.getElementById('editar-qr').files[0]);
    const userInfo = JSON.parse(localStorage.getItem('userInfo'));

    const bodyRequest = {
        idFormato: idFormato,
        nombrePlantilla: logoUniversidad,
        logoUG: logoSecundario,
        lineaGrafica: marcarAgua,
        Qr: qr,
        UserModificacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/formato/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Formato actualizado exitosamente", 'info');
            cargarDatosFormatos();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar el formato", 'danger');
    }
}

// Funcion para eliminar formato
async function eliminarFormato(id) {
    if (!confirm('¿Está seguro que desea eliminar este formato?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/formato/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idFormato: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Formato eliminado exitosamente", 'success');

            if ($.fn.DataTable.isDataTable('#tabla-formato')) {
                $('#tabla-formato').DataTable().clear().destroy();
            }

            cargarDatosFormatos();
        } else {
            const messageClient = response.message || "Error al eliminar el formato.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el formato", 'danger');
    }
}

// Funcion para convertir archivo a Base64
function convertirABase64(file) {
    try {
        return new Promise((resolve, reject) => {
            if (!file) resolve('');
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result.split(',')[1]); // Obtener solo la parte Base64
            reader.onerror = error => reject(error);
            reader.readAsDataURL(file);
        });
    } catch (e) {
        console.error(e)
    }
}

// Funcion para limpiar formulario
function limpiarFormularioFormato() {
    console.log('taka')
    const form = document.getElementById('form-formato');
    if (form) {
        form.reset(); // Restablecer formulario
        form.classList.remove('was-validated'); // Eliminar validación
    }
}

// Inicializar validación
function habilitarValidacionFormato() {
    'use strict';
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            console.log('taka')
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
}

async function cargarDecanatos() {
    try {
        const decanatoResponse = await Utils.httpRequest(
            `${Utils.path}/decanato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true);
        if (decanatoResponse.cod === Utils.COD_OK && decanatoResponse.data.length > 0) {
            const selectDecanato = document.getElementById("origen");

            decanatoResponse.data.forEach(tipo => {
                const option = document.createElement("option");
                option.value = tipo.idDecanato;
                option.textContent = tipo.nombre;
                selectDecanato.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN DECANATOS REGISTRADOS', 'info');
        }
    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}
