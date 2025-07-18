//VARIABLES
let idPersonaAEliminarP = null;
let userInfoPersona = JSON.parse(localStorage.getItem('userInfo'));
let modalEliminarPersona;
let roles; 
async function cargarDatospersonas() {
    let response;
    try {
        const condition = {
            estado: 'ACT'
        }
        const personaResponse = await Utils.httpRequest(
            `${Utils.path}/personas/all`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(condition)
            },
            true);

        setTimeout(() => {
            response = personaResponse;

            const tablaBody = document.querySelector("#tabla-persona tbody");

            tablaBody.innerHTML = '';

            if (response.cod === Utils.COD_OK && response.data.length > 0) {

                response.data.forEach(persona => {
                    if (persona.mDatos.estado != 'ACT') {
                        return;
                    }
                    const genero = persona.genero == 'M'
                        ? '<i class="bi bi-person-standing text-primary" data-bs-toggle="tooltip" data-bs-placement="top" title="Masculino"></i>'
                        : '<i class="bi bi-person-standing-dress text-danger" data-bs-toggle="tooltip" data-bs-placement="top" title="Femenino"></i>';


                    const roles = Array.isArray(persona.mDatos?.rol) ? persona.mDatos.rol.join(', ') : 'Sin rol';
                  
                    const fila = `
                <tr>
                    <td>${persona.cedula.toString() }</td>
                    <td>${persona.nombres} ${persona.apellidos} </td>
                    <td>${persona.edad}</td>
                    <td>${genero}</td>
                    <td>${persona.mDatos.email}</td>
                    <td>${roles}</td>
                    <td>
                        <i class="bi bi-pencil-fill text-success me-3" style="cursor: pointer;" onclick="editarpersona('${persona.cedula}')" data-bs-toggle="tooltip" data-bs-placement="top" title="Editar persona"></i>
                        <i class="bi bi-trash-fill text-danger " style="cursor: pointer;" onclick="eliminarPersona('${persona.cedula}')" data-bs-toggle="tooltip" data-bs-placement="top" title="Eliminar persona"></i>

                    </td>
                </tr>
            `;
                    tablaBody.insertAdjacentHTML("beforeend", fila);
                });

                Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
                const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                tooltipTriggerList.forEach(function (tooltipTriggerEl) {
                    new bootstrap.Tooltip(tooltipTriggerEl);
                });
                //Cargamos los Roles;
                modalEliminarPersona = new bootstrap.Modal(document.getElementById('confirmarEliminacionModal'));

                userInfoPersona = JSON.parse(localStorage.getItem('userInfo'));
                document.getElementById("usuarioIngreso").value = `${userInfoPersona.nombre}`;
                document.getElementById("nombres").addEventListener("input", generarClave);
                document.getElementById("apellidos").addEventListener("input", generarClave);

                $('#tabla-persona').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.21/i18n/Spanish.json'
                    }
                });

            } else {
                tablaBody.innerHTML = '<tr><td colspan="5" class="text-center">No se encontraron personas.</td></tr>';
                Utils.showToast('NO EXISTEN personaES REGISTRADOS', 'info');
            }
        }, 120);

        await cargarRoles();
        habilitarValidacioPersonas();

    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }

}
async function cargarRoles() {
    try {
        // Realiza la petición para obtener los roles
        const rolesResponse = await Utils.httpRequest(
            `${Utils.path}/rol/all`,
            {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            },
            false
        );

        if (rolesResponse.cod === Utils.COD_OK && Array.isArray(rolesResponse.data)) {
            const roles = rolesResponse.data.filter((rol) => rol.estado);

            // Selecciona ambos selects por sus IDs
            const selectAgregar = document.getElementById("idRol"); // Select del formulario
            const selectEditar = document.getElementById("editar-persona-rol"); // Select del modal

            // Limpia las opciones existentes en ambos selects
            [selectAgregar, selectEditar].forEach((select) => {
                if (select) {
                    select.innerHTML = '<option value="">Seleccione un rol</option>';
                    // Agrega las opciones de roles
                    roles.forEach((rol) => {
                        const option = document.createElement("option");
                        option.value = rol.idRol;
                        option.textContent = rol.nombre;
                        select.appendChild(option);
                    });
                } else {
                    console.error("No se encontró el select:", select);
                }
            });
        } else {
            Utils.showToast("No se pudieron obtener los roles", "error");
        }
    } catch (error) {
        console.error("Error al cargar los roles:", error);
        Utils.showToast("Error al cargar los roles", "error");
    }
}

function eliminarPersona(cedula) {
    
    esAdmin = userInfoPersona.cedula;
    if (String(esAdmin) == String(cedula)) {
        Utils.showToast('No puede eliminar al Administrador', 'warning');
        return;
    }
    modalEliminarPersona.show();
    idPersonaAEliminarP = String( cedula);
}

async function editarpersona(cedula) {
    try {
        // Realizar la solicitud HTTP para obtener los datos de la persona
        const response = await httpRequest(`${Utils.path}/personas/id`, "POST", { id: cedula });

        // Cargar los roles disponibles
        await cargarRoles();

        // Verificar si la respuesta es válida
        if (!response || response.cod !== Utils.COD_OK) {
            const messageClient = response?.message || "Error al cargar los datos de la persona.";
            const messageTech = response?.data || null;
            Utils.showErrorModal(messageClient, messageTech);
            return;
        }

        const persona = response.data;

        // Mapear los campos del formulario con los datos de la persona
        const campos = {
            "editar-persona-cedula": persona.cedula || "",
            "editar-persona-nombres": persona.nombres || "",
            "editar-persona-apellidos": persona.apellidos || "",
            "editar-persona-edad": persona.edad || "",
            "editar-persona-genero": persona.genero || "",
            "editar-persona-email": persona.mDatos?.email || "",
            "editar-userIngreso": persona.usuarioIngreso || "",
            "editar-userActualizar": userInfoPersona.nombre || "",
        };

        // Llenar los campos del formulario
        Object.entries(campos).forEach(([id, value]) => {
            const element = document.getElementById(id);
            if (element) element.value = value;
        });

        // Seleccionar el rol correspondiente en el dropdown
        const selectRol = document.getElementById("editar-persona-rol");
        if (selectRol) {
            // Extraer el primer elemento del arreglo de roles (asumiendo que solo hay un rol)
            const rolUsuario = persona.mDatos?.rol?.[0]?.toLowerCase() || "";

            // Buscar la opción que coincida con el rol
            const optionMatch = Array.from(selectRol.options).find(
                (option) =>
                    option.textContent.toLowerCase() === rolUsuario ||
                    option.value.toLowerCase() === rolUsuario
            );

            if (optionMatch) {
                selectRol.value = optionMatch.value;
            } else {
                console.warn("No se encontró un rol que coincida con:", rolUsuario);
            }
        }

        // Mostrar el modal de edición
        const modalEditarPersona = new bootstrap.Modal(document.getElementById("modal-editar-persona"));
        modalEditarPersona.show();
    } catch (error) {
        console.error("Error al cargar los datos de la persona:", error);
        Utils.showToast("Error al cargar los datos", "danger");
    }
}


async function confirmarEliminacionPersona() {
    if (!idPersonaAEliminarP) {
        Utils.showToast("ID de rol no válido", "danger");
        return;
    }
    const cedulaEliminar = idPersonaAEliminarP.toString();
    try {
        const response = await httpRequest(`${Utils.path}/personas/eliminar`, "POST", { cedula: cedulaEliminar });

        if (response && response.cod === Utils.COD_OK) {
            Utils.showToast("Usuario eliminado exitosamente", "success");

            if ($.fn.DataTable.isDataTable('#tabla-persona')) {
                $('#tabla-persona').DataTable().clear().destroy();
            }

            cargarDatospersonas();
        } else {
            const messageClient = response.message || "Error al eliminar el Persona.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al realizar la eliminación", "danger");
    } finally {
        idPersonaAEliminarP = null; 
        modalEliminarPersona.hide();
    }

}
async function confirmarEditarPersona() {
    // Obtener valores de los campos del formulario
    const cedulaE = document.getElementById("editar-persona-cedula").value.trim();
    const nombresE = document.getElementById("editar-persona-nombres").value.trim();
    const apellidosE = document.getElementById("editar-persona-apellidos").value.trim();
    const edadE = parseInt(document.getElementById("editar-persona-edad").value.trim(), 10);
    const generoE = document.getElementById("editar-persona-genero").value.trim();
    const emailE = document.getElementById("editar-persona-email").value.trim();
    const claveE = "DEFECTO"; // Opcional, agrega si es necesario
    const rolSelectE = document.getElementById("editar-persona-rol");
    const estadoEditE = document.getElementById("editar-estadopersona").value;
    const userIngresoE = document.getElementById("editar-userIngreso").value.trim();
    const idRolE = parseInt(rolSelectE.value, 10);

    // Validar campos obligatorios
    if (!cedulaE || !nombresE || !apellidosE || !emailE || isNaN(edadE) || isNaN(idRolE)) {
        Utils.showToast("Por favor, complete todos los campos obligatorios.", "warning");
        return;
    }

    // Crear el payload para la solicitud
    const payloadEdit = {
        "cedula": `${cedulaE}`,
        "nombres": `${nombresE}`,
        "apellidos": `${apellidosE}`,
        "edad": `${edadE}`,
        "genero": `${generoE}`,
        "usuarioIngreso": `${userIngresoE}`,
        "usuarioActualizacion": `${userInfoPersona.idUsuario}`,
        "email": `${emailE}`,
        "clave": `${claveE}`,
        "idRol": `${idRolE}`,
        "estado": `${estadoEditE}`
    };

    const modalE = bootstrap.Modal.getInstance(document.getElementById('modal-editar-persona'));

    try {
        const responseEdit = await Utils.httpRequest(
            `${Utils.path}/personas/modificar`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(payloadEdit),
            }, true);

        if (responseEdit && responseEdit.cod === Utils.COD_OK) {
            Utils.showToast("Persona actualizada correctamente", "success");
            cerrarModalYRecargarTabla(modalE);
        } else {
            const messageClient = responseEdit.message || "Error al actualizar la persona.";
            const messageTech = responseEdit.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.error("Error al actualizar la persona:", error);
        Utils.showToast("Error al realizar la actualización", "danger");
    } finally {
        modalE.hide();
    }
}

// Funcion para cerrar el modal y recargar la tabla
function cerrarModalYRecargarTabla(modal) {
    modal.hide();

    if ($.fn.DataTable.isDataTable('#tabla-persona')) {
        $('#tabla-persona').DataTable().clear().destroy();
    }

    cargarDatospersonas();
}

// Función para validar la entrada del formulario
function validarEntrada({ cedula, nombres, apellidos, edad, genero, email, idRol }) {
    console.log(cedula)
    if (!cedula || cedula.trim() === "") {
        Utils.showToast("La cédula es obligatoria", "warning");
        return false;
    }
    if (!nombres || nombres.trim() === "") {
        Utils.showToast("Los nombres son obligatorios", "warning");
        return false;
    }
    if (!apellidos || apellidos.trim() === "") {
        Utils.showToast("Los apellidos son obligatorios", "warning");
        return false;
    }
    if (isNaN(edad) || edad <= 0) {
        Utils.showToast("La edad debe ser un número válido", "warning");
        return false;
    }
    if (!genero || genero.trim() === "") {
        Utils.showToast("El género es obligatorio", "warning");
        return false;
    }
    if (!email || !validarEmail(email)) {
        Utils.showToast("El correo electrónico no es válido", "warning");
        return false;
    }
    if (idRol <= 0 || isNaN(idRol)) {
        Utils.showToast("Debes seleccionar un rol válido", "warning");
        return false;
    }
    return true;
}

//Funcion para Buscar una persona
async function BuscarPersona() {
    const cedulaABuscar = document.getElementById('buscar-persona').value;

    if (!cedulaABuscar) {
        Utils.showToast("Por favor, ingresa una cédula válida", "danger");
        return;
    }

    try {
        const buscarPersona = await httpRequest(`${Utils.path}/personas/id`, "POST", { id: cedulaABuscar });

        if (buscarPersona.cod === "OK" && buscarPersona.data) {
            const persona = buscarPersona.data;

            document.getElementById("persona-nombres").value = persona.nombres || "";
            document.getElementById("persona-apellidos").value = persona.apellidos || "";
            document.getElementById("persona-cedula").value = persona.cedula || "";
            document.getElementById("persona-edad").value = persona.edad || "";
            document.getElementById("persona-genero").value = persona.genero === "M" ? "Masculino" : "Femenino";
            document.getElementById("persona-email").value = persona.mDatos?.email || "";
            document.getElementById("persona-rol").value = persona.mDatos?.rol || "";
            document.getElementById("persona-usuario-ingreso").value = persona.usuarioIngreso || "";
            document.getElementById("persona-usuario-actualizacion").value = persona.usuarioActualizacion || "No Actualizado";
            document.getElementById("persona-fecha-creacion").value = Utils.formatFecha(persona.fechaCreacion);
            document.getElementById("persona-fecha-modificacion").value = Utils.formatFecha(persona.fechaModificacion);
        } else {
            Utils.showToast(buscarPersona.message || "No se encontraron datos para esta cédula", "warning");
        }
    } catch (error) {
        console.error("Error al buscar la persona:", error);
        Utils.showToast("Error al buscar la persona", "danger");
    }
}

function validarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}


async function savePersona(event) {
    event.preventDefault();
    if (!validarFormularioPersona()) {
 
        return false; 
    }
    const cedula = document.getElementById("cedula").value;
    const nombres = document.getElementById("nombres").value;
    const apellidos = document.getElementById("apellidos").value;
    const edad = parseInt(document.getElementById("edad").value, 10);
    const genero = document.getElementById("genero").value;
    const email = document.getElementById("email").value;
    const clave = document.getElementById("clave").value;
    const rolSelect = document.getElementById("idRol");
    const idRol = parseInt(rolSelect.value, 10);

    if (!idRol) {
        Utils.showToast("Debe seleccionar un rol", "warning");
        return;
    }
    if (!validarEntrada({ cedula, nombres, apellidos, edad, genero, email, clave })) {
        return;
    }

    const persona = {
        "cedula": `${cedula}`,
        "nombres": `${nombres}`,
        "apellidos": `${apellidos}`,
        "edad": `${edad}`,
        "genero": `${genero}`,
        "email": `${email}`,
        "clave": `${clave}`,
        "idRol": `${idRol}`,
        "usuarioIngreso": `${userInfoPersona.idUsuario}`,
        "usuarioActualizacion": `${userInfoPersona.idUsuario}`,
    };

    try {
        const responseRequest = await Utils.httpRequest(
            `${Utils.path}/personas/crear`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(persona),
            },
            true
        );

        if (responseRequest.cod === Utils.COD_OK) {
            Utils.showToast("Persona guardada con éxito", "success");

            if ($.fn.DataTable.isDataTable('#tabla-persona')) {
                $('#tabla-persona').DataTable().clear().destroy();
            }

            const tablaTab = document.querySelector('#home-tab');
            const tab = new bootstrap.Tab(tablaTab);
            tab.show();

            resetPersonaForm(event);
            cargarDatospersonas();
        } else {
            Utils.showToast(responseRequest.message || "Error al guardar la persona", "danger");
        }
    } catch (error) {
        console.log(error)
        Utils.showToast("Error al guardar la persona", "danger");
    }
}

//Valdidaciones
function validarEntrada({ cedula, nombres, apellidos, edad, genero, email }) {
    if (!/^\d{10}$/.test(cedula)) {
        Utils.showToast("La cédula debe contener exactamente 10 dígitos", "warning");
        return false;
    }

    if (!nombres || nombres.length > 35) {
        Utils.showToast("Los nombres no deben estar vacíos ni superar los 35 caracteres", "warning");
        return false;
    }

    if (!apellidos || apellidos.length > 35) {
        Utils.showToast("Los apellidos no deben estar vacíos ni superar los 35 caracteres", "warning");
        return false;
    }

    if (isNaN(edad) || edad < 0 || edad > 120) {
        Utils.showToast("La edad debe ser un número entre 0 y 120", "warning");
        return false;
    }

    if (!genero || (genero !== "M" && genero !== "F")) {
        Utils.showToast("Debe seleccionar un género válido", "warning");
        return false;
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        Utils.showToast("El email no tiene un formato válido", "warning");
        return false;
    }

    return true; // Todas las validaciones pasaron
}
//Genera contraseña automaticamente:
function generarClave() {
    const nombres = document.getElementById("nombres").value.trim();
    const apellidos = document.getElementById("apellidos").value.trim();
    const claveField = document.getElementById("clave");

    if (nombres.length >= 3 && apellidos.length >= 3) {
        const fecha = new Date();
        const hora = String(fecha.getHours()).padStart(2, "0"); 

        const claveGenerada =
            nombres.substring(0, 3).toUpperCase() +
            apellidos.substring(0, 3).toUpperCase() +
            'CER'+
            hora;

        claveField.value = claveGenerada;
    } else {
        claveField.value = "PORDEFECTO2025";  
    }
}

function habilitarValidacioPersonas() {

    (() => {
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
    })();

}
//Validar entrada
function resetPersonaForm(event) {
    event.preventDefault();
    document.getElementById("personaForm").reset();
    const campos = ["cedula", "edad", "genero", "nombres", "apellidos", "email", "idRol"];
    campos.forEach(campo => document.getElementById(campo).classList.remove("is-invalid"));
}


// Generar HTML para los botones de acciones (editar y eliminar)
function generarAccionesHtml(tipo, cedula) {
    return `
        <div class="text-end">
            <button class="btn btn-primary btn-sm" onclick="editarElemento('${tipo}', '${cedula}')">Editar</button>
            <button class="btn btn-danger btn-sm" onclick="eliminarElemento('${tipo}', '${cedula}')">Eliminar</button>
        </div>`;
}

function validarFormularioPersona() {
    const cedula = document.getElementById("cedula").value.trim();
    const edad = document.getElementById("edad").value.trim();
    const genero = document.getElementById("genero").value;
    const nombres = document.getElementById("nombres").value.trim();
    const apellidos = document.getElementById("apellidos").value.trim();
    const email = document.getElementById("email").value.trim();
    const rol = document.getElementById("idRol").value;
     
    const campos = ["cedula", "edad", "genero", "nombres", "apellidos", "email", "idRol"];
    campos.forEach(campo => document.getElementById(campo).classList.remove("is-invalid"));

    let isValid = true;

    if (!cedula || !/^\d{10}$/.test(cedula)) {
        document.getElementById("cedula").classList.add("is-invalid");
        Utils.showErrorModal("La cédula debe tener exactamente 10 dígitos numéricos.");
        return false;
    }

    if (!edad || isNaN(edad) || edad < 0 || edad > 120) {
        document.getElementById("edad").classList.add("is-invalid");
         Utils.showErrorModal("La edad debe estar entre 0 y 120.");
        return false;
    }

    if (!genero) {
        document.getElementById("genero").classList.add("is-invalid");
         Utils.showErrorModal("Selecciona un género.");
         return false;
    }

    if (!nombres) {
        document.getElementById("nombres").classList.add("is-invalid");
         Utils.showErrorModal("El campo 'Nombres' es obligatorio.");
         return false;
    }

    if (!apellidos) {
        document.getElementById("apellidos").classList.add("is-invalid");
         Utils.showErrorModal("El campo 'Apellidos' es obligatorio.");
         return false;
    }

    if (!email || !/\S+@\S+\.\S+/.test(email)) {
        document.getElementById("email").classList.add("is-invalid");
         Utils.showErrorModal("El correo electrónico no es válido.");
         return false;
    }

    if (!rol) {
        document.getElementById("idRol").classList.add("is-invalid");
         Utils.showErrorModal("Selecciona un rol.");
         return false;
    }

    return isValid;
}
async function procesarArchivoCSV(event) {
    event.preventDefault();

    const inputFile = document.getElementById("inputCsvFile");
    if (!inputFile.files.length) {
        Utils.showToast("Debe seleccionar un archivo CSV", "warning");
        return;
    }

    const archivo = inputFile.files[0];
    if (archivo.type !== "text/csv") {
        Utils.showToast("El archivo debe ser un CSV", "warning");
        return;
    }

    const matriculadosExitosos = [];
    const matriculadosErrores = [];

    try {
        const texto = await archivo.text();
        const filas = texto.split("\n").slice(1); // Omitir encabezado
        for (const fila of filas) {
            if (fila.trim() === "") continue; // Ignorar filas vacías

            const columnas = fila.split(";");

            if (columnas.length < 8) {
                matriculadosErrores.push({ fila, error: "Datos incompletos" });
                continue;
            }

            const [cedula, edad, genero, nombres, apellidos, email, clave, idRol] = columnas.map(col => col.trim());

            // Validación de datos básicos
            if (!cedula || !nombres || !apellidos || !edad || !genero || !email || !clave || !idRol) {
                matriculadosErrores.push({ fila, error: "Datos inválidos" });
                continue;
            }

            const persona = {
                "cedula": `${cedula}`,
                "nombres": `${nombres}`,
                "apellidos": `${apellidos}`,
                "edad": `${edad}`,
                "genero": `${genero}`,
                "email": `${email}`,
                "clave": `${clave}`,
                "idRol": `${idRol}`,
                "usuarioIngreso": `${userInfoPersona.idUsuario}`,
                "usuarioActualizacion": `${userInfoPersona.idUsuario}`,
            };

            try {
                const responseRequest = await Utils.httpRequest(
                    `${Utils.path}/personas/crear`,
                    {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(persona),
                    },
                    true
                );

                if (responseRequest.cod === Utils.COD_OK) {
                    matriculadosExitosos.push(persona);
                } else {
                    matriculadosErrores.push({ fila, error: responseRequest.message || "Error desconocido" });
                }
            } catch (error) {
                matriculadosErrores.push({ fila, error: "Error en la solicitud al servidor" });
            } finally {

                if ($.fn.DataTable.isDataTable('#tabla-persona')) {
                    $('#tabla-persona').DataTable().clear().destroy();
                }

                cargarDatospersonas();
            }
        }

        llenarModalResultados(matriculadosExitosos, matriculadosErrores);
    } catch (error) {
        console.error(error);
        Utils.showToast("Error al procesar el archivo CSV", "danger");
    }
}

function llenarModalResultados(exitosos, errores) {
    const listaExitosos = document.getElementById("listaExitosos");
    const listaErrores = document.getElementById("listaErrores");

    listaExitosos.innerHTML = exitosos
        .map(e => `<li>${e.nombres} ${e.apellidos} (${e.email})</li>`)
        .join("");
    listaErrores.innerHTML = errores
        .map(e => `<li>Fila: ${e.fila} - Error: ${e.error}</li>`)
        .join("");

    new bootstrap.Modal(document.getElementById("modalResultados")).show();
}
