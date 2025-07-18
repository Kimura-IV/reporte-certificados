$(document).ready(function () {

    // Validación de formulario
    $('#eventForm').on('submit', function (e) {
        let isValid = true;

        $('#decanato, #tipoEvento').each(function () {
            if ($(this).val() === '') {
                isValid = false;
                $(this).addClass('is-invalid');
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        if (!isValid) {
            e.preventDefault();
            alert('Por favor complete todos los campos.');
        }
    });

    // Filtro en tabla
    $('#searchInput').on('keyup', function () {
        let filter = $(this).val().toLowerCase();
        $('#eventosTable tbody tr').each(function () {
            const match = $(this).text().toLowerCase().indexOf(filter) > -1;
            $(this).toggle(match);
            if (match) {
                $(this).addClass('table-warning').delay(300).queue(function (next) {
                    $(this).removeClass('table-warning');
                    next();
                });
            }
        });
    });

    // Mostrar tab y cargar spinner
    $('#tipoEvento').on('change', function () {
        let tipoEvento = $(this).val();
        $('#tabEventos').text(tipoEvento);

        $('#eventosTable tbody').fadeOut(function () {
            $('#spinner').fadeIn(); // Mostrar spinner
            setTimeout(() => {
                $('#spinner').fadeOut();
                $('#eventosTable tbody').fadeIn(); // Mostrar tabla
            }, 1000); // Simula carga de datos
        });
    });

    // Descargar Excel
    $('#exportExcel').on('click', function () {
        $('#eventosTable').tableExport({
            type: 'excel',
            fileName: 'eventos',
            ignoreColumn: [7], // Ignorar columna de Certificado
            escape: false
        });
    });
});