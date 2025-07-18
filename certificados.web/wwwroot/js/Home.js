
document.addEventListener("DOMContentLoaded", function () {
    const  userDash = JSON.parse(localStorage.getItem('userInfo'));
    if (userDash && userDash.nombre && userDash.nombre.trim().length > 0) {
        setTimeout(() => {
            cargarDatosDashboard();
        }, 100);
    }
 
});
async function cargarDatosDashboard() {
    let userDashboardd = JSON.parse(localStorage.getItem('userInfo'));
    try {
        const requestData = await Utils.httpRequest(`${Utils.path}/dashboard/all`, {
            method: "GET",
            headers: { "Content-Type": "application/json" },
        }, true);

        if (requestData.cod === Utils.COD_OK && requestData.data.length > 1) {
            const data = requestData.data;
             
            const usuariosRegistrados = data.find(item => item.USUARIOS_REGISTRADOS)?.USUARIOS_REGISTRADOS || 0;
            const distribucionRoles = data.filter(item => item.ROL).map(item => ({
                rol: item.ROL,
                total: item.TOTAL_USUARIOS
            }));
            const eventos = data.filter(item => item.IDEVENTO);
             
            const now = new Date(); 
            const dashboardWelcome = document.getElementById('dashboard-init');

            const lastLogin = `Último acceso: Hoy ${now.getHours()}:${String(now.getMinutes()).padStart(2, '0')} ${now.getHours() >= 12 ? 'PM' : 'AM'}`;
            dashboardWelcome.textContent = "Dashboard, " + now.getFullYear();
  
            document.querySelector(".username-dashboard").textContent = userDashboardd.nombre;  
            document.querySelector(".last-login").textContent = lastLogin;

            document.querySelector(".registered-people").textContent = usuariosRegistrados;
            document.querySelector(".events-count").textContent = eventos.length;
             
            const totalCertificados = eventos.filter(evento => evento.CONCERTIFICADO === 1).length;
            document.querySelector(".certificates-count").textContent = totalCertificados;
             
            const updateProgressAndStatus = (type, progress, status) => {
                document.querySelector(`.progress-${type}`).style.width = `${progress}%`;
                document.querySelector(`.status-${type}`).className =
                    `status-${type} mt-2 d-block ${status.class}`;
            };

            updateProgressAndStatus('registered', 75, { icon: "arrow-up", class: "text-success" });
            updateProgressAndStatus('events', 45, { icon: "clock",  class: "text-warning" }); 
            updateProgressAndStatus('certificates', 30, { icon: "clock", class: "text-danger" });

            processTimeSeriesData(data);
            
            const pieChartCtx = document.getElementById('pieChart').getContext('2d');
            new Chart(pieChartCtx, {
                type: 'doughnut',
                data: {
                    labels: distribucionRoles.map(item => item.rol),
                    datasets: [{
                        data: distribucionRoles.map(item => item.total),
                        backgroundColor: ['#0d6efd', '#198754', '#ffc107']
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        } else {
            Utils.showToast('ERROR AL OBTENER DATOS DEL DASHBOARD', 'info');
        }
    } catch (error) { 
        //Utils.showToast("Error cargando datos iniciales", 'error');
    }

}

function processTimeSeriesData(data) {
    // Obtener registros relevantes
    const usuariosConFecha = data.filter(item => item.FCREACION && item.IDUSUARIO);
    const eventos = data.filter(item => item.FECHAINICIO && item.IDEVENTO);

    // Crear objetos para almacenar todos los meses posibles del año actual
    const mesesUsuarios = {};
    const mesesEventos = {};
    const mesesNombres = [
        'Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'
    ];

    // Inicializar todos los meses con 0 para ambas series
    mesesNombres.forEach(mes => {
        mesesUsuarios[mes] = 0;
        mesesEventos[mes] = 0;
    });

    // Contar usuarios por mes
    usuariosConFecha.forEach(usuario => {
        const fecha = new Date(usuario.FCREACION);
        const nombreMes = mesesNombres[fecha.getMonth()];
        mesesUsuarios[nombreMes]++;
    });

    // Contar eventos por mes
    eventos.forEach(evento => {
        const fecha = new Date(evento.FECHAINICIO);
        const nombreMes = mesesNombres[fecha.getMonth()];
        mesesEventos[nombreMes]++;
    });

    // Preparar datos para el gráfico
    const chartData = {
        labels: mesesNombres,
        datasets: [
            {
                label: 'Usuarios Registrados',
                data: Object.values(mesesUsuarios),
                borderColor: '#0d6efd',
                tension: 0.4,
                fill: false
            },
            {
                label: 'Eventos',
                data: Object.values(mesesEventos),
                borderColor: '#dc3545',
                tension: 0.4,
                fill: false
            }
        ]
    };

    // Crear el gráfico
    const mainChartCtx = document.getElementById('mainChart').getContext('2d');
    new Chart(mainChartCtx, {
        type: 'line',
        data: chartData,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        usePointStyle: true,
                        padding: 20
                    }
                },
                tooltip: {
                    mode: 'index',
                    intersect: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            },
            interaction: {
                mode: 'nearest',
                axis: 'x',
                intersect: false
            }
        }
    });
}