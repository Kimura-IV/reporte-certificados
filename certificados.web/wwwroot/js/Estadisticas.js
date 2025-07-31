let estadisticaFilter = {
    FechaInicio : null,
    FechaFin: null,
    Plantilla: 0,
    Firmante: '',
    Tipo: '',
    Creador: '',
    Estado: null
}
myChartPlantilla = null
myChartLineaTiempo = null
myChartFirmante = null
let filtroCargadoEstadistica = null;
responseGraficas = null
async function CargarEstadistica() {
    if (filtroCargado == null) {
        resetObject()
    }
    await inicializarPantallaEstadisticas()
        await CargarEstadisticas();
}


async function CargarEstadisticas() {
    
    const response = await Utils.httpRequest(
        `${Utils.path}/certificado/Estadistica`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(estadisticaFilter)
        },
        true
    );
    responseGraficas = response
    const data = response.data;
    const dataPlantilla = data.plantillas;

    if (myChartPlantilla) {
        myChartPlantilla.destroy();
    }
    console.log(data)
    
    const ctxPlantilla = document.getElementById('plantillaEstadisticas');

    myChartPlantilla = new Chart(ctxPlantilla, {
        type: 'pie',
        data: {
            labels: dataPlantilla.map(x => x.nombrePlantilla),
            datasets: [{
                label: 'Certificados por Plantilla',
                data: dataPlantilla.map(x => x.count),
                backgroundColor: dataPlantilla.map((_, i) => {
                    const colors = [
                        '#FF6384', '#36A2EB', '#FFCE56',
                        '#4BC0C0', '#9966FF', '#FF9F40',
                        '#B8E986', '#D72638', '#3F88C5'
                    ];
                    return colors[i % colors.length];
                }),
                borderColor: 'white',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                datalabels: {
                    color: 'white',
                    font: {
                        weight: 'bold',
                        size: 14
                    },
                    formatter: (value, context) => {
                        const data = context.chart.data.datasets[0].data;
                        const total = data.reduce((a, b) => a + b, 0);
                        const percentage = (value / total * 100).toFixed(1);
                        return `${percentage}%`;
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            const total = context.chart.data.datasets[0].data.reduce((a, b) => a + b, 0);
                            const value = context.parsed;
                            const percentage = ((value / total) * 100).toFixed(2);
                            return `${context.label}: ${value} (${percentage}%)`;
                        }
                    }
                }
            }
        },
        plugins: [ChartDataLabels]
    });

    const dataFirmates = data.firmantes;
    if (myChartFirmante) {
        myChartFirmante.destroy();
    }
    const ctxFirmante = document.getElementById('firmanteEstadisticas');

    myChartFirmante = new Chart(ctxFirmante, {
        type: 'bar',
        data: {
            labels: dataFirmates.map(x => x.nombreFirmante),
            datasets: [{
                label: 'Firmantes',
                data: dataFirmates.map(x => x.count),
                backgroundColor: dataPlantilla.map((_, i) => {
                    const colors = [
                        '#FF6384', '#36A2EB', '#FFCE56',
                        '#4BC0C0', '#9966FF', '#FF9F40',
                        '#B8E986', '#D72638', '#3F88C5'
                    ];
                    return colors[i % colors.length];
                }),
                borderColor: 'white',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                datalabels: {
                    color: 'white',
                    font: {
                        weight: 'bold',
                        size: 14
                    },
                    formatter: (value, context) => {
                        const data = context.chart.data.datasets[0].data;
                        const total = data.reduce((a, b) => a + b, 0);
                        const percentage = (value / total * 100).toFixed(1);
                        return `${percentage}%`;
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            const label = context.dataset.label || ''; 
                            const value = context.raw; 
                            const data = context.chart.data.datasets[0].data;
                            const total = data.reduce((a, b) => a + b, 0);
                            const percentage = total ? ((value / total) * 100).toFixed(2) : '0.00';
                            return `${label}: ${value} (${percentage}%)`;
                        }
                    }
                }
            }
        },
        plugins: [ChartDataLabels]
    });


    GenerateGraficoTiempo()

  
}

async function inicializarPantallaEstadisticas() {
    if (filtroCargadoEstadistica == null) {
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
            const selectPlantilla = $("#plantillaEstadistica")
            const selectFirmante = $("#firmanteEstadistica")
            const selectTipo = $("#tipoEstadistica")
            const selectCreador = $("#creadorEstadistica")
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
        filtroCargadoEstadistica = true;
    }

    function debounceEstadistica(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        }
    }

    const ejecutarBusquedaEstadistica = debounceEstadistica(function () {

        estadisticaFilter = {
            FechaInicio: $("#fechaInicio").val() ? new Date($("#fechaInicio").val()).toISOString() : null,
            FechaFin: $("#fechaFin").val() ? new Date($("#fechaFin").val()).toISOString() : null,
            Plantilla: parseInt($("#plantillaEstadistica").val()) || 0,
            Firmante: $("#firmanteEstadistica").val() || null,
            Tipo: $("#tipoEstadistica").val() || null,
            Creador: $("#creadorEstadistica").val() || null,
            Estado: (() => {
                const val = $("#estadoEstadistica").val();
                if (val === "true") return true;
                if (val === "false") return false;
                return null;
            })()
        };
        CargarEstadisticas()
    });

    function limpiarFiltrosEstadistica() {
        $("#fechaInicio").val('');
        $("#fechaFin").val('');
        $("#plantillaEstadistica").val('');
        $("#firmanteEstadistica").val('');
        $("#tipoEstadistica").val('');
        $("#creadorEstadistica").val('');
        $("#estadoEstadistica").val('');
    }

    function iniciarFiltrosEstadistica() {
        $("#fechaInicio").off('input change').on('input change', ejecutarBusquedaEstadistica);
        $("#fechaFin").off('input change').on('input change', ejecutarBusquedaEstadistica);
        $("#plantillaEstadistica").off('change').on('change', ejecutarBusquedaEstadistica);
        $("#firmanteEstadistica").off('change').on('change', ejecutarBusquedaEstadistica);
        $("#tipoEstadistica").off('change').on('change', ejecutarBusquedaEstadistica);
        $("#creadorEstadistica").off('change').on('change', ejecutarBusquedaEstadistica);
        $("#estadoEstadistica").off('change').on('change', ejecutarBusquedaEstadistica);
        $("#tipoTiempoEstadistica").off('change').on('change', GenerateGraficoTiempo);

        $("#btnLimpiarEstadistica").off('click').on('click', function () {
            limpiarFiltrosEstadistica();
            ejecutarBusquedaEstadistica();
        });
    }

    iniciarFiltrosEstadistica();
}


function resetObjectEstadistica() {
    objectFilter = {
        FechaInicio: null,
        FechaFin: null,
        Plantilla: 0,
        Firmante: '',
        Tipo: '',
        Creador: '',
        Estado: null
    };
}

function GenerateGraficoTiempo() {
    const value = document.getElementById("tipoTiempoEstadistica").value;
    const ctxLinea = document.getElementById('lineaTiempoEstadisticas');

    const dataResponse = responseGraficas.data;

    if (myChartLineaTiempo) {
        myChartLineaTiempo.destroy();
    }

    switch (value){
        case "dia":
            GenerarGraficaDia(ctxLinea, dataResponse.lapsoDias)
            break;
        case "semana":
            GenerarGraficaSemana(ctxLinea, dataResponse.lapsoSemanas)
            break;
        case "anio":
            GenerarGraficaAnio(ctxLinea, dataResponse.lapsoAnios)

            break;
    }   
}


function GenerarGraficaAnio(contextEstadistica, dataEstadisitca) {
    myChartLineaTiempo = new Chart(contextEstadistica, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Cantidad por Año',
                data: dataEstadisitca.map(x => ({
                    x: x.anio,
                    y: x.count
                })),
                fill: false,
                borderColor: '#4bc0c0',
                backgroundColor: '#4bc0c0',
                tension: 0.3,
                pointRadius: 5,
                pointHoverRadius: 7
            }]
        },
        options: {
            responsive: true,
            parsing: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'year',
                        tooltipFormat: 'yyyy-MM-dd',
                        displayFormats: {
                            day: 'yyyy-MM-dd'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Año'
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Cantidad'
                    }
                }
            },
            plugins: {
                tooltip: {
                    mode: 'index',
                    intersect: false
                },
                legend: {
                    display: true,
                    position: 'top'
                }
            }
        }
    });
}
function GenerarGraficaSemana(contextEstadistica, dataEstadisitca) {
    myChartLineaTiempo = new Chart(contextEstadistica, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Cantidad por semana',
                data: dataEstadisitca.map(x => ({
                    x: x.semanaInicio,
                    y: x.count
                })),
                fill: false,
                borderColor: '#4bc0c0',
                backgroundColor: '#4bc0c0',
                tension: 0.3,
                pointRadius: 5,
                pointHoverRadius: 7
            }]
        },
        options: {
            responsive: true,
            parsing: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'week',
                        tooltipFormat: 'yyyy-MM-dd',
                        displayFormats: {
                            day: 'yyyy-MM-dd'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Semana'
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Cantidad'
                    }
                }
            },
            plugins: {
                tooltip: {
                    mode: 'index',
                    intersect: false
                },
                legend: {
                    display: true,
                    position: 'top'
                }
            }
        }
    });
}
function GenerarGraficaDia(contextEstadistica, dataEstadisitca) {
    myChartLineaTiempo = new Chart(contextEstadistica, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Cantidad por día',
                data: dataEstadisitca.map(x => ({
                    x: x.dia,
                    y: x.count
                })),
                fill: false,
                borderColor: '#4bc0c0',
                backgroundColor: '#4bc0c0',
                tension: 0.3,
                pointRadius: 5,
                pointHoverRadius: 7
            }]
        },
        options: {
            responsive: true,
            parsing: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        tooltipFormat: 'yyyy-MM-dd',
                        displayFormats: {
                            day: 'yyyy-MM-dd'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Fecha'
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Cantidad'
                    }
                }
            },
            plugins: {
                tooltip: {
                    mode: 'index',
                    intersect: false
                },
                legend: {
                    display: true,
                    position: 'top'
                }
            }
        }
    });
}

