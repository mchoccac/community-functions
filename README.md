# Azure Functions Demo

Este repositorio contiene el código de ejemplo utilizado en la charla de [Arquitectura Serverless](https://www.meetup.com/Azure-Guatemala/events/268574054/) de la comunidad [Azure Guatemala](https://www.facebook.com/azuregt/).

# La presentación

Las diapositivas de la charla puedes descargarlas de aquí:

- [Arquitectura Serverless.pptx](https://1drv.ms/p/s!AnSpkxOg_w2Ck5tAhA3FOb8_p5MThQ?e=xNdUlD)

# Requisitos

Para ejecutar este ejemplo en tu maquina (Linux, Windows o Mac) necesitas tener instalado lo siguiente:

- [NodeJs v10](https://nodejs.org/en/download/)
- [Azure Functions Core Tools v3](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows#v2)
- [Azurite](https://github.com/Azure/Azurite#npm)
- [DotNet Core v3](https://dotnet.microsoft.com/download/dotnet-core/3.0)
- [Visual Studo Code](https://code.visualstudio.com/#alt-downloads)
- [Azure Functions Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) for VsCode.
- [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) for VsCode.

> **Nota:** todas estas herramientas pueden correr opcionalmente sobre la [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install-win10), yo lo recomiendo trabajarlo así ya que se tiene una experiencia de desarrollo bastante agradable.

# Base de datos

Para el ejemplo utilicé una base de datos SQL Server en azure con una única tabla (puedes usar una base de datos en tu propio servidor):

```sql
CREATE TABLE dbo.Users
(
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    DisplayName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    SessionsCount INT NOT NULL DEFAULT 0
)
```

> si quieres utilizar otra base de datos tendrías que cambiar el proveedor de conexion.

# Ejecución del proyecto:

**Primero** debes agregar un archivo de configuración en la raíz del proyecto llamado `local.settings.json` con la siguiente estructura:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    },
    "ConnectionStrings": {
        "UsersDb": "Server={nombre-o-ip-del-servidor};Initial Catalog={base-de-datos};Persist Security Info=False;User ID={usuario};Password={clave};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    }
}
```

**Segundo**, puedes ejecutar `azurite` para hacer funcionar la funcion cuyo trigger es un timer:

```bash
# en linux:
azurite --silent --location ~/data/azurite --debug ~/data/azurite/debug.log

# en windows:
azurite --silent --location C:\data\azurite --debug C:\data\azurite\debug.log
```

**Tercero**, mientras está ejecutandose `azurite` en otra consola puedes iniciar las Azure Functions desde la consola:

```bash
func start
```

# Errores en el repo

Si encuentras algun error en el código (o alguna sugerencia) o si encuentras que algunas de las instrucciones para ejecutar el proyecto está mal, no dudes en compartirla con la comunidad [registrando un issue](https://github.com/miguelerm/community-functions/issues/new) en este mismo repositorio.

# Notas finales

Algunos comandos para recordar de la charla:

- `func init --source-control=true --worker-runtime=dotnet --language=C# --docker=true` permite crear en la carpeta actual un proyecto de Azure Functions App en C#, inicializando el repositorio de git y creando al mismo tiempo un dockerfile listo para ejecutar nuestras funciones en azure.
- `func new --language=C# --template=TimerTrigger --name=UpdateSessionsCount` crea una nueva Azure Function en el proyecto cuyo trigger será un timer.
- `func new --language=C# --template=HttpTrigger --name=GetAllUsers` crea una nueva Azure Function en el proyecto cuyo trigger será un request HTTP.

si quieres hacer requests a tus funciones http desde vscode, te recomeniendo la extensión [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) de vscode y con eso podras ejecutar los [ejemplos http](./test-requests.http) que están en este repo.

