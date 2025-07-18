let userInfoPerfil = JSON.parse(localStorage.getItem('userInfo'));
async function cargarDatosPerfil() {
    try {
        const payloiad = {
            cedula: userInfoPerfil.cedula
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
                document.getElementById("genero").value = persona.genero;
                document.getElementById("usuarioIngreso").value = persona.usuarioIngreso;
                document.getElementById("usuarioActualizacion").value = persona.usuarioActualizacion || "NO ACTUALIZADO";
                document.getElementById("fechaCreacion").value = Utils.formatFecha(persona.fechaCreacion);
                document.getElementById("fechaModificacion").value = Utils.formatFecha(persona.fechaModificacion);

            } else {
                Utils.showToast('NO PUDIMOS OBTENER TU INFORMACION', 'info');
            }
        }, 120);
    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }
}