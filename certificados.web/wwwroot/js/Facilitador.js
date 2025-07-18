
let userInfoFacilitador = JSON.parse(localStorage.getItem('userInfo'));



async function cargarDatosFacilitadores() {
    try {
        console.log('taka');
        const condition = {
            estado: 'ACT'
        }
        const expositorResponse = await Utils.httpRequest(
            `${Utils.path}/personas/all`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(condition)
            },
            true);

        expositorResponse

        setTimeout(() => {
            console.log(expositorResponse)
            if (expositorResponse.cod === Utils.COD_OK && expositorResponse.data.length > 0) {
                const selectElement = document.getElementById('facilitadorAll');
                selectElement.innerHTML = '';  

                expositorResponse.data.forEach(persona => {
                    console.log(persona);
                    const option = document.createElement('option');
                    option.value = persona.cedula;  
                    option.textContent = `${persona.cedula} - ${persona.nombres} - ${persona.apellidos}`;  
                    selectElement.appendChild(option);
                });
            } else {
                Utils.showToast("No se encontraron usuarios activos.", 'warning');
            }
        }, 120);
        userInfoPersona = JSON.parse(localStorage.getItem('userInfo'));

    } catch (error) {
        console.log(error);
        Utils.showToast("Error cargando datos iniciales", 'error');
    }

}


async function RegistrarFacilitador() {
    const selectElement = document.getElementById('facilitadorAll');
    const cedulaSeleccionada = selectElement.value; // Obtiene la cédula seleccionada

    if (!cedulaSeleccionada) {
        Utils.showToast("Seleccione un facilitador antes de registrar.", 'warning');
        return;
    }

    try {
        const payload = {
            "Cedula": `${cedulaSeleccionada}`,
            "UsuarioIngreso": `${userInfoFacilitador.idUsuario}`
        };

        const response = await Utils.httpRequest(
            `${Utils.path}/expositor/crear`, 
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            }
        );

        if (response.cod === Utils.COD_OK) {
            Utils.showToast("Facilitador registrado con éxito.", 'success');
        } else {
            const messageClient = response.message || "Error al registrar el facilitador.";
            const messageTech = response.data || null;
            Utils.showErrorModal(messageClient, messageTech);
        }
    } catch (error) {
        console.error(error);
        Utils.showToast("Error al registrar facilitador.", 'error');
    }
}