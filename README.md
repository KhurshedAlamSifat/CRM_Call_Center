# ACCESO WEB
user: admin@accroachcode.com
pass: oQEx8Q8vSG$1^hwFT^Ku



-----------------------------------------------------------------------------------
# Database

## Database information (for this particular project)
````
host:169.62.240.75
user:fet_ecuador
pass:qhgz5etbxvfukocwmpsj
db:fet


## Tipo de pago
DEPOSITO (CUANDO PAGO CON DEPOSITO)
TRANSFERENCIA (CUANDO PAGO CON TRANSFERENCIA)
TARJETACREDITO (CUANDO PAGO CON PLACE TO PAY)
FREE (CUANDO LA AFILIACION ES GRATOS)
````

## Database initialization (Initialize Clean Project)
Run InitDb.sql in Database.
It creates the following  tables:
    DocumentType
    User
    Role
    User_Role_Mapping
    System_Log
    Settings
    Language
    LocaleStringResources
    Permissions
    PermissionRoleMapping
    GenericAttribute
    GuestUser
    EmailAccount
    EmailTemplate
The default created user is
user:jmendez@accroachcode.com
pass:juancarlos

## Where to edit database connection
Connection string at Base.Data.BaseModels.basesystemContext.cs and recompile
In production it is also edited in appsettings.json->ConnectionStrings->BaseConnectionString


## Update database ORM Model
Comando para actualizar Database en PackageManagerConsole en Base.Data
````
NUGET PACKAGE CONSOLE
scaffold-DbContext "Server=dbserver; Database=fet; user id=fet_ecuador; password=qhgz5etbxvfukocwmpsj;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir BaseModels -force -v

CLI
dotnet ef dbcontext scaffold "Data Source=DESKTOP-3BPP5J9\SQLEXPRESS;Initial Catalog=basesystem; user id=sa; password=juancarlos;" Microsoft.EntityFrameworkCore.SqlServer  --output-dir BaseModels --force -v

````
Luego de este comando en Base.Data.BaseDbContext.cs cambiar la referencia de basesystemContext a [NOMBRE DE DB GENERADA]Context.cs dentro de la carpeta BaseModels/



-----------------------------------------------------------------------------------
# Maintenance
- Erase data in table "GuestUser"
- Erase data in table "SystemLog"
- Erase data in table "GenericAttribute" where KeyGroup=GuestUserItem



-----------------------------------------------------------------------------------
# Dev Documentation

## Configuraciones Generales
Base.Core.Configuration.BaseConfig.cs

## Configuraciones de Cookies 
Base.Services.CookieDefaults.cs

## Consideraciones Generales
1. Siempre mantener el estilo wwwroot/css/base.css y wwwroot/secure/css/base.css en los layouts tanto 
P�blicos como de Secure ya que estos ayudan a una correcta visualizaci�n de las validaciones para 
campos en asp net core
2. Siempre mantener el estilo wwwroot/secure/css/fixes.css en los layouts de p�ginas en donde se quiera usar
Telerik Kendo
3. Siempre mantener el Javascript file wwwroot/secure/js/secure-site.js en el layout de Secure ya
que hace que los tweaks dela interfaz gr�fica se ejecuten como por ejemplo los collapsable panels


### Lenguaje/Localizaci�n
[AutoDetectLanguage = true/false] si queremos que se detecte el lenguaje del browser



## P�ginas de Errores
Para efectos de desarrollo est� habilitado poder ver el detalle de los errores, pero cuando
se publique no es recomendable ver este detalle, para esto tenemos una p�gina de Errores
gen�ricos en /Views/Common/Error
Habilitamos que se vea esto por defecto en Startup.cs descomentando la siguiente l�nea:
````
    if (env.IsDevelopment())
    {
        app.UseExceptionHandler("/error"); //Descomentar
    }
    else
    {
        ...
    }
````


# Localizaci�n

### Database
Los recursos de idioma est�n en la tabla [LocaleStringResource]
El campo [ResourceName] es el que tiene la comparaci�n para seleccionar ese recurso

### Mostrar en una Vista
En el View se extrae el recurso mediante su ResourceName as�
````
@T("Secure.Login.Title")
````

### Usar en un Controlador
Asegurarse de usar como dependencia el servicio ILocalizationService y dentro del m�todo:
````
string localized = _localizationService.GetResource("Secure.Login.Title");
````



## Permisos

### Database
Los permisos deben agregarse primero en la tabla [Permissions]
Campo: SystemName
Ej: Root.User.List

### Mostrar en Sidebar como men�
Si necesitamos que  salga  en el sidebar modificamos 
/Secure/Views/Shared/Components/SidebarMenuWidget/Default.cshtml
y  preguntamos as�
````
    @if (Model.Contains("Root.User.list"))
    {
        <li class="nav-item">
            <a href="/Secure/User/List" class="nav-link">Usuarios</a>
        </li>
    }
````

### Da los permisos en la lista
ViewBag.Roles = _permissionService.GetAuthenticatedUserPermissions(); // da los permisos en la vista

### Mostrar una pantalla o no de acuerdo al permiso
Necesitamos colocar al comienzo del m�todo de la acci�n del controlador esto:
````
    if (!_permissionService.Authorize("Root.User.Edit"))
        return AccessDeniedView();
````

###  Mostrar una acci�n o bot�n o link en el View dependiendo el permiso
En �l m�todo de la acci�n del controlador luego de la l�gica del negocio
isntanciar una variable viewbag o de modelo as�:
````
    ViewBag.AllowEdit = _permissionService.Authorize("Root.User.Edit");
````
Y en la Vista se puede validar as�:
````
    @ViewBag.AllowEdit{
        <a>ACCION</a>
    }
````



## UI

### Paneles de BUSQUEDA en CRUD
En los List de los CRUD, en el panel de b[usqueda] si no se quiere ver por defecto desplegada la misma quitar el atributo 'data-mostrado'
***Bug, se muestran dos flechas en el panel



## ESTADOS
PE = PENDIENTE
PR = RECIBO SUBIDO
CA = PAGADO
EL = ELIMINADO
