let userInfoConfig = JSON.parse(localStorage.getItem('userInfo'));
async function cargarDatosConfiguracion() {
    try {
        const payloiad = {
            cedula: userInfoConfig.cedula
        }
        const response = await Utils.httpRequest(
            `${Utils.path}/personas/perfil`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payloiad)
            },
            true);

        setTimeout(() => {
            if (response.cod === Utils.COD_OK && response.data != null) {

                const { usuario, persona, rol } = response.data;

                // Llenar campos del Tab Usuario
                document.getElementById("email").value = usuario.email;

                const roles = response.data.rol; // Array de roles
                document.getElementById("rolesP").value = roles.join(", ");



                document.getElementById("password").value = usuario.clave;

                // Llenar campos del Tab Persona
                document.getElementById("nombres").value = persona.nombres;
                document.getElementById("apellidos").value = persona.apellidos;
                document.getElementById("cedula").value = persona.cedula;
                document.getElementById("edad").value = persona.edad;
                document.getElementById("genero").value = persona.genero == "M" ? "Masculino" :"Femenino"; 
                document.getElementById("persona-rol").value = usuario.trol.idRol;
                document.getElementById("usuarioIngreso").value = usuario.ususarioIngreso;
            } else {
                Utils.showToast('NO PUDIMOS OBTENER TU INFORMACION', 'info');
            }
        }, 120);
    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}

function habilitarEdicion() { 
    document.getElementById("email").disabled = false;
    document.getElementById("password").disabled = false; 
    document.getElementById("repetirPasswordContainer").classList.remove("d-none"); 
    document.getElementById("habilitarBtn").classList.add("d-none");
    document.getElementById("guardarBtn").classList.remove("d-none");  
    document.getElementById("password").value = ''; 
    document.getElementById("repetirPassword").value = '';
}       
function habilitarEdicionPersona() {
    document.getElementById("nombres").disabled = false;
    document.getElementById("apellidos").disabled = false;
    document.getElementById("edad").disabled = false; 
    document.getElementById("habilitarBtnPersona").classList.add("d-none");
    document.getElementById("guardarBtnPersona").classList.remove("d-none");
}


async function guardarUsuario() { 
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const repetirPassword = document.getElementById("repetirPassword").value;
     
    if (password !== repetirPassword) {
        Utils.showToast("Las contraseñas no coinciden.", "danger");
        return;
    }
    document.getElementById("email").disabled = true;
    document.getElementById("password").disabled = true;
    document.getElementById("repetirPasswordContainer").classList.add("d-none");
    document.getElementById("guardarBtn").classList.add("d-none");
    document.getElementById("habilitarBtn").classList.remove("d-none");
    await confirmarEditarUsuarios();
}
async function confirmarEditarUsuarios(  ) {
    const cedulaE = document.getElementById("cedula").value.trim();
    const nombresE = document.getElementById("nombres").value.trim();
    const apellidosE = document.getElementById("apellidos").value.trim();
    const edadE = parseInt(document.getElementById("edad").value.trim(), 10);
    const generoSelectE = document.getElementById("generoSelect");
    const generoE = generoSelectE.options[generoSelectE.selectedIndex].value;
    const emailE = document.getElementById("email").value.trim();
    const claveE = document.getElementById("password").value.trim();
    const userIngresoE = document.getElementById("usuarioIngreso").value.trim();
    const idRolE = parseInt(document.getElementById("persona-rol").value, 10);
    const estadoEditE = 'ACT';
     
    if (!cedulaE || !nombresE || !apellidosE || isNaN(edadE) || !generoE || !emailE || !claveE) {
        Utils.showToast("Todos los campos son Obligatorios.", "danger");
        return;
    } 
    const payloadEdit = {
        "cedula": `${cedulaE}`,
        "nombres": `${nombresE}`,
        "apellidos": `${apellidosE}`,
        "edad": `${edadE}`,
        "genero": `${generoE}`,
        "usuarioIngreso": `${userIngresoE}`,
        "usuarioActualizacion": `${userInfoConfig.idUsuario}`,
        "email": `${emailE}`,
        "clave": `${claveE}`,
        "idRol": `${idRolE}`,
        "estado": `${estadoEditE}`
    };

    try {
        const responseEdit = await Utils.httpRequest(
            `${Utils.path}/personas/modificar/perfil`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(payloadEdit),
            }, true);


        if (responseEdit && responseEdit.cod === Utils.COD_OK) {
            Utils.showToast("Persona actualizada correctamente", "success");
            // Ocultar el botón de guardar y mostrar el de habilitar edición
            document.getElementById("guardarBtnPersona").classList.add("d-none");
            document.getElementById("habilitarBtnPersona").classList.remove("d-none");
            blockInputs();
            cargarDatosConfiguracion();
        } else {
            const messageClient = responseEdit.message || "Error al actualizar la persona.";
            const messageTech = responseEdit.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        Utils.showToast("Error al realizar la actualización", "danger");
    } 
}
function blockInputs() {
    // Deshabilitar todos los campos
    document.getElementById("email").disabled = true;
    document.getElementById("rolesP").disabled = true;
    document.getElementById("password").disabled = true;
    document.getElementById("nombres").disabled = true;
    document.getElementById("apellidos").disabled = true;
    document.getElementById("cedula").disabled = true;
    document.getElementById("edad").disabled = true;
    document.getElementById("generoSelect").disabled = true;
    document.getElementById("genero").disabled = true;
}