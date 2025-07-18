
const Utils = (() => {
     /***
      * Metodo que crea un Loader que se muestra en cada peticion HTTP 
      */
    const createLoader = () => {
        const loaderHTML = `
        <div id="globalLoader" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.5); z-index: 1050; align-items: center; justify-content: center;">
            <img src="../img/loader.gif" alt="Cargando..." style="width: 100px; height: 100px;">
        </div>`;
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = loaderHTML.trim();
        document.body.appendChild(tempDiv.firstChild);
    };

    const showLoader = () => {
        const loader = document.getElementById('globalLoader');
        if (loader) loader.style.display = 'flex';
    };

    const hideLoader = () => {
        const loader = document.getElementById('globalLoader');
        if (loader) loader.style.display = 'none';
    };

    /**
     * Metodo que crea un modal si la peticion HTTP fue un ERROR 
     * @param {any} messageClient
     * @param {any} messageTech
     */
    const createErrorModal = (messageClient, messageTech) => {
        const modalHTML = `
        <div class="modal fade" id="errorModal" tabindex="-1" aria-labelledby="errorModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="errorModalLabel">Error</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                    
                        <p><strong>Mensaje: </strong>${messageClient}</p>
                        ${messageTech ? `<p><strong>Detalle técnico: </strong>${messageTech}</p>` : ''}
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" id="cerrarBtn">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>`;
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = modalHTML.trim();
        const modalElement = tempDiv.firstChild
        document.body.appendChild(modalElement);

        modalElement.querySelector("#cerrarBtn").addEventListener('click', () => {
            closeModal(modalElement);
        });
        modalElement.addEventListener('click', (event) => {
            if (event.target === modalElement) {
                closeModal(modalElement);
            }
        });
        modalElement.querySelector('#cerrarBtn').addEventListener('click', () => {
            closeModal(modalElement);
        });

    };
    const closeModal = (modalElement) => {
        modalElement.style.display = 'none';
        modalElement.remove();
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) {
            backdrop.remove();
        }
    }
    const showErrorModal = (messageClient, messageTech) => {
        createErrorModal(messageClient, messageTech);
        const modal = new bootstrap.Modal(document.getElementById('errorModal'));
        modal.show();
    };

    const createSuccessRequest = () => {
        const alertHtml = `
            <div class="toast align-items-center text-bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        OPERACIÓN REALIZADA CON ÉXITO.
                    </div>
                    <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>`;
        const tempDivSuccess = document.createElement('div');
        tempDivSuccess.innerHTML = alertHtml.trim();
        document.body.appendChild(tempDivSuccess);

        const toast = new bootstrap.Toast(tempDivSuccess.firstChild);
        //toast.show();

        tempDivSuccess.firstChild.addEventListener('hidden.bs.toast', () => {
            tempDivSuccess.remove();
        });
    };
    /**
     * METODO QUE REALIZA PETICIONES HTTP
     * @param {any} url
     * @param {any} options
     * @returns
     */
    const httpRequest = async (url, options = {}, showAlert = true) => {
        showLoader();  
        try {

            const response = await fetch(url, options);
            const result = await response.json();

            if (result.cod === Utils.COD_OK) {
                if (showAlert) createSuccessRequest();
                return result;
            } else {
                const messageClient = result.message || "Ocurrió un error inesperado.";
                const messageTech = result.data || null;
                showErrorModal(messageClient, messageTech);
            }
        } catch (error) {
            const messageClient = "Error en la peticion";
            const messageTech = error;
            hideLoader();  
            showErrorModal(messageClient, messageTech);
    
        } finally {
            setTimeout(() => {
                hideLoader();  
            }, 2000)
          
        }
    };
        
    /**
     * Metodo que muestra la Notificacion 
     * @param {any} message
     * @param {any} type
     */
    const showToast = (message, type = 'primary', duration = 5000) => {
        try {
            // Crear o reutilizar el contenedor de toasts
            const toastContainerId = "toastContainer";
            let toastContainer = document.getElementById(toastContainerId);

            if (!toastContainer) {
                toastContainer = document.createElement("div");
                toastContainer.id = toastContainerId;
                toastContainer.style.position = "fixed";
                toastContainer.style.bottom = "20px";
                toastContainer.style.left = "50%";
                toastContainer.style.transform = "translateX(-50%)";
                toastContainer.style.zIndex = "1060";
                toastContainer.style.maxWidth = "90%"; // Limitar el ancho máximo
                toastContainer.style.width = "fit-content";
                document.body.appendChild(toastContainer);
            }

            // Crear el toast
            const toastHtml = `
            <div class="toast align-items-center text-bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true" style="max-width: 100%; width: fit-content; height: auto; min-height: 60px; font-size: 1.2rem; padding: 10px;">
                <div class="d-flex justify-content-between align-items-center">
                <div class="toast-body text-center flex-grow-1" style="word-wrap: break-word; overflow: hidden; text-overflow: ellipsis;">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2" data-bs-dismiss="toast" aria-label="Cerrar"></button>
                </div>
            </div>
    `;

            const tempDiv = document.createElement("div");
            tempDiv.innerHTML = toastHtml.trim();
            const toastElement = tempDiv.firstChild;

            // Añadir el toast al contenedor
            toastContainer.appendChild(toastElement);

            // Inicializar el toast de Bootstrap
            const toast = new bootstrap.Toast(toastElement, {
                autohide: true, // Ocultar automáticamente
                delay: duration, // Duración personalizada
            });

            // Mostrar el toast
            toast.show();

            // Eliminar el toast del DOM después de que se oculte
            toastElement.addEventListener("hidden.bs.toast", () => {
                toastElement.remove();
            });

        } catch (error) {
            console.error("Error al mostrar el toast:", error);
        }
    };
    const path = 'api';
    const COD_OK = 'OK';
    //Limpia el LocalStorage
    const limpiarLocalStorage = () => { 
        localStorage.clear();
    }
    //Limpia la navegacion 
    const cleanRoute = () => {
        history.replaceState(null, null, '/');
        history.pushState(null, null, '/');
    }
    //Vuelve al Inicio 
    const backToIndex = () => {
        cleanRoute();
        window.location.href = "/Home/Index";
        window.location.reload(true);  
    }
    //Formatea Fechas
    const formatFecha = (fecha) => {
        if (!fecha || fecha.startsWith("0001")) return "Sin modificación";
        const [year, month, day] = fecha.split("T")[0].split("-");
        const monthNames = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];
        const monthName = monthNames[parseInt(month, 10) - 1];  
        return `${year}/${monthName}/${day}`;
    }; 

    const validarFormulario = (formId)  => {
        const form = document.getElementById(formId);
        let isValid = true;
         
        form.querySelectorAll('.invalid-feedback').forEach(feedback => feedback.remove());
        form.querySelectorAll('.is-invalid').forEach(input => input.classList.remove('is-invalid'));
         
        form.querySelectorAll('[required]').forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                marcarError(input, "Este campo es obligatorio");
            }
        });

        form.querySelectorAll('[pattern]').forEach(input => {
            const pattern = new RegExp(input.getAttribute('pattern'));
            if (!pattern.test(input.value.trim())) {
                isValid = false;
                marcarError(input, "Formato inválido");
            }
        });

        return isValid;
    }
    //Marca error algun campo del fomrulario
    const  marcarError = (input, mensaje)  => {
        input.classList.add('is-invalid');
        const feedback = document.createElement('div');
        feedback.classList.add('invalid-feedback');
        feedback.textContent = mensaje;
        input.parentElement.appendChild(feedback);
    }
    //Convierte en base64 los archivos
    const  convertirArchivoABase64 = async (file)  => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result.split(',')[1]);
            reader.onerror = (error) => reject(error);
            reader.readAsDataURL(file);
        });
    }

    const rolesByUsuario = {
        "admin": ["inicio", "mantenimiento", "planificacion", "facilitador", "actas", "certificacion", "consultas"],
        "facilitador": ["inicio", "facilitador", "actas"],
        "decano": ["inicio", "mantenimiento", "planificacion", "certificacion", "consultas"],
    }
    const  obtenerRolesUsuario = () => {
        const userInfo = JSON.parse(localStorage.getItem('userInfo'));
        return userInfo?.roles || [];
    }

    const formatearFecha = (fechaISO) => {
        // Convertir la fecha ISO a un objeto Date
        const fecha = new Date(fechaISO);

        // Obtener mes, día y año
        const mes = String(fecha.getMonth() + 1).padStart(2, '0'); // Mes (0-11) + 1
        const dia = String(fecha.getDate()).padStart(2, '0'); // Día del mes
        const anio = fecha.getFullYear(); // Año

        return `${anio}-${mes}-${dia}`;
    };


    createLoader(); // Crea el loader al inicializar

    return {
        httpRequest,
        path,
        COD_OK,
        showToast,
        limpiarLocalStorage,
        backToIndex,
        cleanRoute,
        closeModal, 
        showErrorModal,
        createSuccessRequest,
        hideLoader,
        showLoader,
        formatFecha,
        marcarError,
        validarFormulario,
        convertirArchivoABase64,
        rolesByUsuario,
        obtenerRolesUsuario,
        formatearFecha
 
    };
})();
