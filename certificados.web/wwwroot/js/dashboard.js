let usuarioLogeado = '';
let userInfo;
function toggleCollapse(submenuId) {
    console.log("PRECIONANDO.....")
    const submenu = document.getElementById(submenuId);
    const arrow = submenu.previousElementSibling.querySelector('.arrow');

    if (submenu.style.display === "block") {
        submenu.style.display = "none";
        arrow.classList.remove("rotate");
    } else {
        submenu.style.display = "block";
        arrow.classList.add("rotate");
    }
}

function logOut() {
    Utils.backToIndex();

}
function RolesByUsuarios() {
    const userInfo = JSON.parse(localStorage.getItem('userInfo'));
    const rolesUsuario = userInfo?.roles || [];
    const permisosUsuario = new Set();

    const rolesByUsuarios = {
        "admin": ["inicio", "mantenimiento", "planificacion", "facilitador", "actas", "certificacion", "consultas"],
        "facilitador": ["inicio", "facilitador", "actas"],
        "decano": ["inicio", "mantenimiento", "planificacion", "certificacion", "consultas"],
        "vicerrector": ["inicio", "consultas"],
    };
     
    rolesUsuario.forEach(rol => {
        const rolNormalizado = rol.trim().toLowerCase();
        if (rolesByUsuarios[rolNormalizado]) {
            rolesByUsuarios[rolNormalizado].forEach(modulo => {
                permisosUsuario.add(modulo.toLowerCase());
            });
        }
    });
     
    document.querySelectorAll(".nav-item").forEach(item => {
        // Ignorar elementos en el header
        if (item.closest("header.main-header")) {
            return;
        }
         
        const isSubmenuItem = item.closest('.submenu') !== null;
        if (isSubmenuItem) {
            return;  
        }

        let tienePermiso = false;
         
        const moduloEnItem = [...item.classList].find(clase =>
            clase.startsWith("modulo-")
        );
        if (moduloEnItem) {
            const nombreModulo = moduloEnItem.replace("modulo-", "").toLowerCase();
            tienePermiso = permisosUsuario.has(nombreModulo);
        }
         
        if (!tienePermiso) {
            const iconos = item.querySelectorAll("i[class*='modulo-']");
            iconos.forEach(icono => {
                const moduloEnIcono = [...icono.classList].find(clase =>
                    clase.startsWith("modulo-")
                );
                if (moduloEnIcono) {
                    const nombreModulo = moduloEnIcono.replace("modulo-", "").toLowerCase();
                    if (permisosUsuario.has(nombreModulo)) {
                        tienePermiso = true;
                    }
                }
            });
        }
         
        item.style.display = tienePermiso ? "" : "none";
         
        if (tienePermiso) {
            const submenu = item.querySelector('.submenu');
            if (submenu) {
                const submenuItems = submenu.querySelectorAll('.nav-item');
                submenuItems.forEach(submenuItem => {
                    submenuItem.style.display = "";
                });
            }
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    Utils.cleanRoute();
    const submenuLinks = document.querySelectorAll('.nav-link[data-view]');
    usuarioLogeado = document.getElementById("userLog");
    userInfo = JSON.parse(localStorage.getItem('userInfo'));
    //No hay Información
    if (!userInfo) {
        Utils.backToIndex();
    }
    usuarioLogeado.innerHTML = `${userInfo.nombre}`;

    // Generar y mostrar las iniciales del usuario
    const nombre = userInfo.nombre || '';
    const apellido = userInfo.apellido || '';
    const inicialNombre = nombre.charAt(0).toUpperCase();
    const inicialApellido = apellido.charAt(0).toUpperCase();
    const iniciales = inicialNombre + inicialApellido;

    const userInitial = document.getElementById('userInitial');
    if (userInitial) {
        userInitial.innerHTML = `<div style="width: 40px; height: 40px; border-radius: 50%; background-color: #007bff; color: white; display: flex; align-items: center; justify-content: center; font-weight: bold;">${iniciales}</div>`;
    }

    const defaultView = "Inicio";
    cargarVista(defaultView);

    const defaultLink = document.getElementById('link-inicio');
    if (defaultLink) {
        defaultLink.classList.add('active');
    }

    submenuLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const viewName = link.getAttribute('data-view');

            submenuLinks.forEach(link => link.classList.remove('active'));
            link.classList.add('active');

            cargarVista(viewName);
        });
    });

    function cargarVista(viewName) { 
        filtroCargado = null
        fetch(`/Dashboard/${viewName}`)
            .then(response => {
                if (response.ok) {
                    return response.text();
                } else {
                    throw new Error('No se pudo cargar la vista');
                }
            })
            .then(html => {
                document.getElementById('dynamic-content').innerHTML = html;
            })
            .catch(error => {
                console.error('Error al cargar la vista:', error);
            });
    } 
    RolesByUsuarios();
    const dropdownButton = document.getElementById('dropdownUser');
    const dropdownMenu = document.getElementById('submenu9');

    dropdownButton.addEventListener('click', function () {
        const isVisible = dropdownMenu.classList.contains('show');
        if (isVisible) {
            dropdownMenu.classList.remove('show');
        } else {
            dropdownMenu.classList.add('show');
        }
    });
     
    document.addEventListener('click', function (event) {
        if (!dropdownMenu.contains(event.target) && event.target !== dropdownButton) {
            dropdownMenu.classList.remove('show');
        }
    });

});

