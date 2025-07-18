// Funcion para cargar los grupos y tipos de registro desde las APIs
async function cargarDatos() {
    /*
    try {
        // Llamadas paralelas a las APIs para obtener los grupos y los tipos de registro
        const [grupoResponse, tipoRegistroResponse] = await Promise.all([
            fetch('https://api.example.com/grupos'),
            fetch('https://api.example.com/tipos-registro')
        ]);

        // Verificacion del estado de las respuestas
        if (!grupoResponse.ok || !tipoRegistroResponse.ok) {
            throw new Error('Error al obtener los datos de las APIs');
        }

        // Obtener los datos en formato JSON
        const grupos = await grupoResponse.json();
        const tiposRegistro = await tipoRegistroResponse.json();

        // Rellenar el select de grupos con los datos de la API
        const grupoSelect = document.getElementById('grupo');
        if (grupoSelect) {
            grupos.forEach(grupo => {
                const option = document.createElement('option');
                option.value = grupo.id;
                option.textContent = grupo.nombre;
                grupoSelect.appendChild(option);
            });
        }

        // Rellenar el select de tipos de registro con los datos de la API
        const tipoRegistroSelect = document.getElementById('tipo-registro');
        if (tipoRegistroSelect) {
            tiposRegistro.forEach(tipo => {
                const option = document.createElement('option');
                option.value = tipo.id;
                option.textContent = tipo.nombre;
                tipoRegistroSelect.appendChild(option);
            });
        }
    } catch (error) {
        console.error('Error al cargar los datos:', error);
  
    }*/
}

// Llamar a la funcion al cargar la pagina
document.addEventListener('DOMContentLoaded', cargarDatos);

// Funcion para buscar los datos por cedula
async function buscarPorCedula() {
    const cedula = document.getElementById('cedula').value;

    // Validacion basica de la cedula
    if (!cedula || !/^\d+$/.test(cedula)) {
        alert('Por favor, ingrese una cédula válida');
        return;
    }

    try {
        // Hacer la peticion a la API con la cedula
        const response = await fetch(`https://api.example.com/persona?cedula=${cedula}`);

        // Verificacion del estado de la respuesta
        if (!response.ok) {
            throw new Error('No se pudo obtener la persona');
        }

        const persona = await response.json();

        // Rellenar los campos con los datos de la persona
        if (persona && persona.nombres && persona.apellidos) {
            document.getElementById('nombres').value = persona.nombres;
            document.getElementById('apellidos').value = persona.apellidos;
            document.getElementById('correo').value = persona.correo;
            document.getElementById('celular').value = persona.celular;
        } else {
           // alert('No se encontró la persona con esa cédula');
        }
    } catch (error) {
        console.error('Error al buscar la persona:', error);
    }
}

//// Llamar a la funcion de buscar cuando se haga clic en el boton "Buscar"
//document.getElementById('buscarBtn').addEventListener('click', buscarPorCedula);
