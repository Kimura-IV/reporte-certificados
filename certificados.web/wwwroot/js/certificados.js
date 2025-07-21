let objectFilter = {
    Emision: '',
    Plantilla: 0,
    Firmante: '',
    Tipo: '',
    Creador: '',
    Estado: ''
};
let filtroCargado = null;
let myChart = null;
function abrirModal(base64) {
    $('#largeModal').modal('show');

    $('#modalIFrame').attr('src', `data:application/pdf;base64,${base64}`);
}

function cerrarModal() {
    $('#largeModal').modal('hide');
}

async function cargarDatosCertificados() {
    if (filtroCargado == null) {
        resetObject()
    }
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/obtener`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(objectFilter)
            },
            true
        );

        const ctx = document.getElementById('myChart');


        const data = response.data

        const activo = data.filter(x => x.estado);
        const inactivo = data.filter(x => !x.estado);

        if (myChart) {
            myChart.destroy();
        }
        myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Activos', 'Inactivos'],
                datasets: [
                    {
                        label: 'Certificados activos',
                        data: [activo.length, 0],
                        borderWidth: 1,
                        borderColor: 'black',
                        backgroundColor: 'rgb(85, 164, 196)'
                    },
                    {
                        label: 'Certificados inactivos',
                        data: [0, inactivo.length],
                        borderWidth: 1,
                        borderColor: 'black',
                        backgroundColor: 'rrgb(196, 185, 163)'
                    }
                ]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });

        setTimeout(() => {
            const tablaBody = document.querySelector("#tabla-certificados tbody");
            tablaBody.innerHTML = ''
            if (response.cod === Utils.COD_OK && response.data.length > 0) {
                response.data.forEach(certificado => {
                    const fila = `
                        <tr>
                            <td>${certificado.idCertificado}</td>
                            <td>${certificado.titulo}</td>
                            <td>${certificado.fCreacion}</td>
                            <td>${certificado.estado ? 'Activo' : 'Inactivo'}</td>
                            <td>
                              <button class="btn btn-success" onclick="abrirModal('${certificado.pdfBase64}')">Ver certificado</button>
                            </td>
                            <td>
                                <a class="btn btn-primary" href="data:application/pdf;base64,${certificado.pdfBase64}" download="${certificado.idCertificado}.pdf">Descargar</a>
                            </td>
                        </tr>
                    `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                const body = document.getElementById('injectModal')
                body.innerHTML += `
                        <!-- Modal -->
    <div class="modal fade" id="largeModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Ver certificado</h5>
                </div>
                <div class="modal-body">
                    <iframe id="modalIFrame" width="100%" height="700px"></iframe>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-danger" onclick="cerrarModal()">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
                `

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');

                // Inicializar tooltips
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });
            } else {
                tablaBody.innerHTML = '<tr><td colspan="7" class="text-center">No se encontraron certificados.</td></tr>';
                Utils.showToast('NO EXISTEN CERTIFICADOS REGISTRADOS', 'info');
            }
        }, 150);

    } catch (error) {
        console.log(error)
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
    inicializarPantallaCertificados();
}

// Manejador para crear nuevo certificado
async function handleAgregarCertificado(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();
    let userInfoConfig = JSON.parse(localStorage.getItem('userInfo'));
    const fileInput = document.getElementById('certificado-imagen');
    const file = fileInput.files[0];

    try {

        const imagenBase64 = await convertirImagenABase64(file);
        const base64Data = imagenBase64.split(",")[1];


        const bodyRequest = {
            Titulo: document.getElementById('certificado-titulo').value,
            Imagen: base64Data,
            IdEvento: parseInt(document.getElementById('certificado-id-evento').value),
            IdFormato: parseInt(document.getElementById('certificado-id-formato').value),
            Tipo: document.getElementById('certificado-tipo').value,
            Estado: document.getElementById('certificado-estado').value,
            UsuarioIngreso: userInfoConfig.idUsuario
        };


        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('CERTIFICADO REGISTRADO EXITOSAMENTE', 'info');
            cargarDatosCertificados();
            limpiarFormularioCertificado();
            const tablaTab = document.querySelector('#tabla-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al agregar el certificado", 'danger');
    }
}

// Función para editar certificado
async function editarCertificado(id) {
    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/id`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idCertificado: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            const certificados = response.data;
            const certificado = certificados.find(cert => cert.idCertificado === id);

            if (certificado) {
                // Asignar valores a los campos del formulario
                document.getElementById('certificado-id-editar').value = certificado.idCertificado || '';
                document.getElementById('certificado-titulo-editar').value = certificado.titulo || '';
                document.getElementById('certificado-id-evento-editar').value = certificado.idEvento || '';
                document.getElementById('certificado-id-formato-editar').value = certificado.idFormato || '';
                document.getElementById('certificado-tipo-editar').value = certificado.tipo || '';
                document.getElementById('certificado-estado-editar').value = certificado.estado;

                // Mostrar el modal
                const modal = new bootstrap.Modal(document.getElementById('modal-editar-certificado'));
                modal.show();
            } else {
                Utils.showToast("No se encontró el certificado especificado", 'warning');
            }
        } else {
            Utils.showToast("Error al cargar datos del certificado", 'danger');
        }
    } catch (error) {
        console.error(error);
        Utils.showToast("Error al obtener los datos del certificado", 'danger');
    }
}


// Manejador para guardar edición
async function handleEditarCertificado(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    event.preventDefault();

    const bodyRequest = {
        idCertificado: document.getElementById('certificado-id-editar').value,
        Titulo: document.getElementById('certificado-titulo-editar').value,
        IdEvento: parseInt(document.getElementById('certificado-id-evento-editar').value),
        IdFormato: parseInt(document.getElementById('certificado-id-formato-editar').value),
        Tipo: document.getElementById('certificado-tipo-editar').value,
        Estado: document.getElementById('certificado-estado-editar').value,
        UserModificacion: userInfo.idUsuario
    };

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Certificado actualizado exitosamente", 'info');
            cargarDatosCertificados();
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar-certificado'));
            modal.hide();
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al actualizar el certificado", 'danger');
    }
}

// Función para eliminar certificado
async function eliminarCertificado(id) {
    if (!confirm('¿Está seguro que desea eliminar este certificado?')) return;

    try {
        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/eliminar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idCertificado: id })
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Certificado eliminado exitosamente", 'success');
            cargarDatosCertificados();
        } else {
            const messageClient = response.message || "Error al eliminar el certificado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al eliminar el certificado", 'danger');
    }
}

// Función para limpiar formulario
function limpiarFormularioCertificado() {
    document.getElementById('certificado-titulo').value = '';
    document.getElementById('certificado-imagen').value = '';
    document.getElementById('certificado-id-evento').value = '';
    document.getElementById('certificado-id-formato').value = '';
    document.getElementById('certificado-tipo').value = '';
    document.getElementById('certificado-estado').checked = false;

    const form = document.querySelector('.needs-validation');
    if (form) {
        form.classList.remove('was-validated');
    }
}

// Habilitar validación de formularios
function habilitarValidacionCertificado() {
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
async function cargarEventos() {
    try {
        const responseEventos = await Utils.httpRequest(
            `${Utils.path}/evento/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        if (responseEventos.cod === Utils.COD_OK && responseEventos.data.length > 0) {
            const selectEvento = document.getElementById("certificado-id-evento");

            responseEventos.data.forEach(evento => {
                const option = document.createElement("option");
                option.value = evento.idevento;
                option.textContent = evento.tematica;
                selectEvento.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN EVENTOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

async function cargarFormatos() {
    try {
        const responseFormatos = await Utils.httpRequest(
            `${Utils.path}/formato/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        if (responseFormatos.cod === Utils.COD_OK && responseFormatos.data.length > 0) {
            const selectFormato = document.getElementById("certificado-id-formato");

            responseFormatos.data.forEach(formato => {
                const option = document.createElement("option");
                option.value = formato.idFormato;
                option.textContent = formato.idFormato;
                selectFormato.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN FORMATOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

// Metodo para convertir una imagen a base64
function convertirImagenABase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result); // Obtener el resultado base64
        reader.onerror = () => reject(new Error("Error al convertir la imagen a base64"));
        reader.readAsDataURL(file); // Leer el archivo como DataURL
    });
}

async function handleGenerarCertificados(event) {
    const form = event.target.closest("form");

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }
    event.preventDefault();
    let userInfoConfig = JSON.parse(localStorage.getItem('userInfo'));
    const fileInput = document.getElementById('generar-docentes');
    const file = fileInput.files[0];

    try {
        const csvContent = await file.text();
        const docentes = csvContent.split('\n').map(line => line.trim()).filter(line => line);

        const bodyRequest = {
            idCertificado: parseInt(document.getElementById('generar-id-certificado').value),
            docentes: docentes
        };

        const response = await Utils.httpRequest(
            `${Utils.path}/certificado/emitir`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(bodyRequest)
            },
            true
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast('CERTIFICADOS GENERADOS EXITOSAMENTE', 'info');
            limpiarFormGeneraCertificado();
            // Procesar los datos de respuesta
            mostrarResultados(response.data);
        } else {
            const messageClient = response.message || "Ocurrió un error inesperado.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al generar los certificados", 'danger');
    }
}

async function cargarCertificados() {
    try {
        const responseCertificados = await Utils.httpRequest(
            `${Utils.path}/certificado/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            true
        );

        if (responseCertificados.cod === Utils.COD_OK && responseCertificados.data.length > 0) {
            const selectGrupo = document.getElementById("generar-id-certificado");

            responseCertificados.data.forEach(certificado => {
                const option = document.createElement("option");
                option.value = certificado.idCertificado;
                option.textContent = certificado.titulo;
                selectGrupo.appendChild(option);
            });
        } else {
            Utils.showToast('NO EXISTEN CERTIFICADOS REGISTRADOS', 'info');
        }
    } catch (error) {
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

function mostrarResultados(data) {
    const { Notificados, NoNotificados } = data;

    // Crear contenedores con tarjetas para los resultados
    let resultadosHTML = `
        <div class="mt-4">
            <h4 class="text-center">Resultados del Proceso</h4>
            <div class="row justify-content-center">
                <!-- Tarjeta de Notificados -->
                <div class="col-md-5 mb-4">
                    <div class="card border-success shadow-sm">
                        <div class="card-header bg-success text-white">
                            <h5 class="card-title mb-0">Notificados</h5>
                        </div>
                        <div class="card-body">
                            <p class="card-text">
                                <strong>Cantidad:</strong> ${Notificados.length}
                            </p>
                            <p class="card-text">
                                <strong>Listado:</strong> 
                                ${Notificados.length > 0 ? Notificados.join(", ") : "No hay notificados."}
                            </p>
                        </div>
                    </div>
                </div>
                <!-- Tarjeta de No Notificados -->
                <div class="col-md-5 mb-4">
                    <div class="card border-danger shadow-sm">
                        <div class="card-header bg-danger text-white">
                            <h5 class="card-title mb-0">No Notificados</h5>
                        </div>
                        <div class="card-body">
                            <p class="card-text">
                                <strong>Cantidad:</strong> ${NoNotificados.length}
                            </p>
                            <p class="card-text">
                                <strong>Listado:</strong> 
                                ${NoNotificados.length > 0 ? NoNotificados.join(", ") : "No hay no notificados."}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Buscar el contenedor de resultados o crearlo dinámicamente si no existe
    const resultadosContenedor = document.getElementById('resultados-certificados');
    if (resultadosContenedor) {
        resultadosContenedor.innerHTML = resultadosHTML;
    } else {
        // Si no existe el contenedor, crear uno dinámicamente
        const tabGenerar = document.getElementById('generar');
        const divResultados = document.createElement('div');
        divResultados.id = 'resultados-certificados';
        divResultados.innerHTML = resultadosHTML;
        tabGenerar.appendChild(divResultados);
    }
}

function limpiarFormGeneraCertificado() {
    document.getElementById('generar-id-certificado').value = '';
    document.getElementById('generar-docentes').value = '';

    const form = document.querySelector('.needs-validation');
    if (form) {
        form.classList.remove('was-validated');
    }
}
async function inicializarPantallaCertificados() {
    if (filtroCargado == null) {
            const responseFilters = await Utils.httpRequest(
                `${Utils.path}/certificado/GetFiltrosCertificados`,
                {
                    method: "GET",
                    headers: { "Content-Type": "application/json" },
                },
                true
            );
            const data = responseFilters.data;
            if (responseFilters.cod === Utils.COD_OK) {
                const selectPlantilla = $("#plantilla")
                const selectFirmante = $("#firmante")
                const selectTipo = $("#tipo")
                const selectCreador = $("#creador")
                data.personas.forEach(x => {
                    const option = document.createElement("option");
                    option.value = x.cedula;
                    option.text = x.nombres;
                    selectCreador.append(option)
                })
                data.plantillas.forEach(x => {
                    const option = document.createElement("option");
                    option.value = x.idFormato;
                    option.text = x.nombrePlantilla;
                    selectPlantilla.append(option)
                })
    
                data.tipos.forEach(x => {
                    const option = document.createElement("option");
                    option.value = x;
                    option.text = x;
                    selectTipo.append(option)
                })
    
                data.firmantes.forEach(x => {
                    const option = document.createElement("option");
                    option.value = x;
                    option.text = x;
                    selectFirmante.append(option)
                })
            }
        filtroCargado = true;
    }

    function debounce(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        }
    }
    
    const ejecutarBusqueda = debounce(function () {

        objectFilter = {
            Emision: $("#emision").val() ? new Date($("#emision").val()).toISOString() : null,
            Plantilla: parseInt($("#plantilla").val()) || 0,
            Firmante: $("#firmante").val() || null,
            Tipo: $("#tipo").val() || null,
            Creador: $("#creador").val() || null,
            Estado: (() => {
                const val = $("#estado").val();
                console.log(val)
                if (val === "true") return true;
                if (val === "false") return false;
                return null;
            })()
        };
        cargarDatosCertificados()
    });

    function limpiarFiltros() {
        $("#emision").val('');
        $("#plantilla").val('');
        $("#firmante").val('');
        $("#tipo").val('');
        $("#creador").val('');
        $("#estado").val('');
    }

    function iniciarFiltros() {
        $("#emision").off('input change').on('input change', ejecutarBusqueda);
        $("#plantilla").off('change').on('change', ejecutarBusqueda);
        $("#firmante").off('change').on('change', ejecutarBusqueda);
        $("#tipo").off('change').on('change', ejecutarBusqueda);
        $("#creador").off('change').on('change', ejecutarBusqueda);
        $("#estado").off('change').on('change', ejecutarBusqueda);

        $("#btnLimpiar").off('click').on('click', function () {
            limpiarFiltros();
            ejecutarBusqueda();
        });
    }

    iniciarFiltros();
}
function resetObject() {
    objectFilter = {
        Emision: '',
        Plantilla: 0,
        Firmante: '',
        Tipo: '',
        Creador: '',
        Estado: ''
    };
}