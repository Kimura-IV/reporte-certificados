# Nombre de la Aplicación

DESARROLLO E INTEGRACIÓN DEL MÓDULO DE REPORTES PARA EL SISTEMA AUTOMATIZADO DE GENERACIÓN DE CERTIFICADOS DOCENTES EMITIDOS POR EL VICERRECTORADO ACADÉMICO DE LA UNIVERSIDAD DE GUAYAQUIL

## Requisitos Previos

Antes de desplegar la aplicación, asegúrate de tener instalados los siguientes requisitos:

- **Visual Studio** (v2022 o superior - https://visualstudio.microsoft.com/es/vs/community/)
- **Base de datos** (SQLserver v16.x o superior)
- **Gestor de Base de datos** (SQL Server Management Studio v20.x o superior)

## Instalación

Sigue estos pasos para instalar y configurar la aplicación en tu entorno local:

1. Clona el repositorio:
   ```bash
   git clone https://github.com/{USUARIO}/app-web-certificados.git
   cd tu-repositorio
   git checkout feature/produccion_certificados

2. Abrir el proyecto en Visual Studio
   ```bash
   - Abrir terminal de Visual Studio

   - Ejecutar los siguientes comandos una sola vez:
        - Add-Migration (nombre de la migracion)
        - Update database

3. Iniciar proyecto en Visual Studio
