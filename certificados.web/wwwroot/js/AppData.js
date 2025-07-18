const AppData = (() => {
    const storage = {
        roles: [],
        usuarios: [],
    };

    const setData = (key, data) => {
        if (storage.hasOwnProperty(key)) {
            storage[key] = data;
        } else {
            Utils.showToast('NO EXISTE DATOS REGISTRADOS', 'info');
        }
    };

    const getData = (key) => {
        if (storage.hasOwnProperty(key)) {
            return storage[key];
        } else {
            Utils.showToast('NO EXISTE DATOS REGISTRADOS', 'info');
            return null;
        }
    };

    const fetchAndStoreData = async () => {
        try {
            const rolesResponse = await Utils.httpRequest(
                `${Utils.path}/rol/all`,
                {
                    method: "GET",
                    headers: {  "Content-Type": "application/json" },
                },
                true);
            if (rolesResponse.cod === Utils.COD_OK) { 
                setData("roles", rolesResponse.data);
            }
 

            const usuariosResponse = await Utils.httpRequest(`${Utils.path}/personas/all`,
                {
                    method: "POST",
                    headers: {  "Content-Type": "application/json" },
                    body: JSON.stringify({ "estado": "ACTi" }),
                }, true);
            if (usuariosResponse.cod === Utils.COD_OK) { 
                setData("usuarios", usuariosResponse.data);
            }

            Utils.showToast('DATOS CARGADOS EXITOSAMENTE', 'success');
        } catch (error) {
            Utils.showToast("Error cargando datos iniciales", 'error');
        }
    };

    // Llamar este método al iniciar el dashboard
    const obtenerDatos = () => {
        fetchAndStoreData();
    };

    return {
        obtenerDatos,
        getData,
    };
})();