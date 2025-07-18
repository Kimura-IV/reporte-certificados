$(function () {
    inicializarTabla('#tabla-grupo');
    inicializarTabla('#tabla-decanato');
    inicializarTabla('#tabla-modalidad');
    inicializarTabla('#tabla-evento');
    inicializarTabla('#tabla-ciclo');
});

function inicializarTabla(selector) {
    $(selector).DataTable({
        language: {
            sProcessing: 'Procesando...',
            sLengthMenu: 'Mostrar _MENU_ registros',
            sZeroRecords: 'No se encontraron resultados',
            sEmptyTable: 'Ningún dato disponible en esta tabla',
            sInfo: 'Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros',
            sInfoEmpty: 'Mostrando registros del 0 al 0 de un total de 0 registros',
            sInfoFiltered: '(filtrado de un total de _MAX_ registros)',
            sSearch: 'Buscar:',
            sLoadingRecords: 'Cargando...',
            oPaginate: {
                sFirst: 'Primero',
                sPrevious: 'Anterior',
                sNext: 'Siguiente',
                sLast: 'Último',
            },
        },
    });
}

let datos = {
    grupos: [],
    decanatos: [],
    modalidad: [],
    evento: [],
    ciclo: []
};

function agregarElemento(tipo) {
    $(`#${tipo.toLowerCase()}-nombre`).val('');
    $('#modal-editar-label').text(`Agregar Nuevo ${tipo}`);
    $('#modal-editar').modal('show');

    $('#btn-guardar-cambios').off('click').on('click', () => guardarCambios(tipo));
}

function editarElemento(tipo, nombre) {
    const item = obtenerElemento(tipo, nombre);

    if (item) {
        $(`#${tipo.toLowerCase()}-nombre`).val(item.nombre);
        $('#modal-editar-label').text(`Editar ${tipo}`);
        $('#modal-editar').modal('show');

        $('#btn-guardar-cambios').off('click').on('click', () => guardarCambios(tipo, item));
    }
}

function obtenerElemento(tipo, nombre) {
    return datos[tipo.toLowerCase()].find(item => item.nombre === nombre);
}

function guardarCambios(tipo, itemEditado = null) {
    const nombre = $(`#${tipo.toLowerCase()}-nombre`).val().trim();

    if (!nombre) {
        alert(`El nombre del ${tipo.toLowerCase()} es obligatorio.`);
        return;
    }

    const tableSelector = `#tabla-${tipo.toLowerCase()}`;
    const dataType = tipo.toLowerCase();

    const tiposValidos = ['grupo', 'decanato', 'modalidad', 'evento', 'ciclo'];
    if (!tiposValidos.includes(dataType)) {
        alert('Tipo inválido.');
        return;
    }

    if (!Array.isArray(datos[dataType])) {
        datos[dataType] = [];
    }

    if ($('#modal-editar-label').text() === `Agregar Nuevo ${tipo}`) {
        const nuevoElemento = { nombre };
        datos[dataType].push(nuevoElemento);

        $(tableSelector)
            .DataTable()
            .row.add([nombre, generarAccionesHtml(tipo, nombre)])
            .draw();

        alert(`Nuevo ${tipo.toLowerCase()} agregado: ${nombre}`);
    } else if (itemEditado) {
        itemEditado.nombre = nombre;

        const table = $(tableSelector).DataTable();
        table.rows().every(function () {
            const data = this.data();
            if (data[0] === itemEditado.nombre) {
                data[0] = itemEditado.nombre;
                data[1] = generarAccionesHtml(tipo, itemEditado.nombre);
                this.data(data);
            }
        });
        table.draw();

        alert(`Cambios guardados para: ${nombre}`);
    }

    $('#modal-editar').modal('hide');
}

function eliminarElemento(tipo, nombre) {
    if (confirm(`¿Estás seguro de que deseas eliminar el ${tipo.toLowerCase()}: ${nombre}?`)) {
        const dataType = tipo.toLowerCase();
        const index = datos[dataType].findIndex(item => item.nombre === nombre);

        if (index !== -1) {
            datos[dataType].splice(index, 1);

            const tableSelector = `#tabla-${tipo.toLowerCase()}`;
            const table = $(tableSelector).DataTable();
            table.rows().every(function () {
                const data = this.data();
                if (data[0] === nombre) {
                    this.remove();
                }
            });
            table.draw();

            alert(`${tipo} eliminado: ${nombre}`);
        }
    }
}

function generarAccionesHtml(tipo, nombre) {
    return `<div class="text-end">
        <button class="btn btn-primary btn-sm" onclick="editarElemento('${tipo}', '${nombre}')">Editar</button>
        <button class="btn btn-danger btn-sm" onclick="eliminarElemento('${tipo}', '${nombre}')">Eliminar</button>
    </div>`;
}
