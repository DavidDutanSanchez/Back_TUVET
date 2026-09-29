
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using System.Security.Cryptography.X509Certificates;

using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Service;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Service;


// =========================================================
// 1) CREAR BUILDER
// =========================================================

var builder = WebApplication.CreateBuilder(args);


// =========================================================
// 2) CONFIGURACIÓN DEL CERTIFICADO HTTPS - TUVET
// =========================================================

// Certificado generado en Windows para TuVet.

var thumbprint =
    "4FA027954ABE1AEEFF480D7C55F94CA123F18B01";


// Abrir almacén de certificados de Windows.

using var store = new X509Store(
    StoreName.My,
    StoreLocation.LocalMachine
);

store.Open(OpenFlags.ReadOnly);


// Buscar el certificado mediante su huella digital.

var certificados = store.Certificates.Find(
    X509FindType.FindByThumbprint,
    thumbprint,
    validOnly: false
);


// Validar que exista.

if (certificados.Count == 0)
{
    throw new InvalidOperationException(
        "No se encontró el certificado HTTPS de TuVet " +
        "en el almacén de certificados de Windows."
    );
}


// Obtener certificado.

var certificado = certificados[0];


// Verificar que tenga clave privada.

if (!certificado.HasPrivateKey)
{
    throw new InvalidOperationException(
        "El certificado HTTPS de TuVet " +
        "no tiene clave privada."
    );
}


// Verificar que todavía esté vigente.

if (
    DateTime.Now < certificado.NotBefore ||
    DateTime.Now > certificado.NotAfter
)
{
    throw new InvalidOperationException(
        "El certificado HTTPS de TuVet " +
        "no se encuentra dentro de su periodo de validez."
    );
}


// =========================================================
// 3) CONFIGURACIÓN KESTREL - HTTP Y HTTPS
// =========================================================

builder.WebHost.ConfigureKestrel(options =>
{

    // -----------------------------------------------------
    // HTTP - BACKEND ACTUAL
    // -----------------------------------------------------

    options.ListenAnyIP(5001);


    // -----------------------------------------------------
    // HTTPS - BACKEND TUVET
    // -----------------------------------------------------

    options.ListenAnyIP(
        5002,
        listenOptions =>
        {
            listenOptions.UseHttps(
                certificado
            );
        }
    );

});


// =========================================================
// 4) CONFIGURACIÓN DE CONEXIÓN A MYSQL
// =========================================================

var connectionString =
    builder.Configuration.GetConnectionString(
        "TuVet"
    );


if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'TuVet' not found."
    );
}


// Registrar contexto de base de datos.

builder.Services.AddDbContext<TuVetContext>(
    options =>
        options.UseMySQL(
            connectionString
        )
);


// =========================================================
// 5) CONTROLLERS
// =========================================================

builder.Services.AddControllers();


// =========================================================
// 6) INYECCIÓN DE DEPENDENCIAS
// =========================================================


// ---------------------------------------------------------
// PERSONAS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorPersonas,
    PersonasService
>();


// ---------------------------------------------------------
// MASCOTAS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorMascotas,
    MascotasService
>();


// ---------------------------------------------------------
// ESPECIES
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorEspecies,
    EspeciesService
>();


// ---------------------------------------------------------
// RAZAS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorRazas,
    RazaService
>();


// ---------------------------------------------------------
// COLORES
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorColores,
    ColoresService
>();


// ---------------------------------------------------------
// PRODUCTOS
// ---------------------------------------------------------

// Se conserva el nombre actual de tu clase.

builder.Services.AddScoped<
    IControladorProductos,
    ProdcutosService
>();


// ---------------------------------------------------------
// CATEGORÍAS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorCategorias,
    CategoriasService
>();


// ---------------------------------------------------------
// SERVICIOS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorServicios,
    ServiciosService
>();


// ---------------------------------------------------------
// CASAS COMERCIALES
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorCasasComerciales,
    CasasComercialesService
>();


// ---------------------------------------------------------
// AGENDAMIENTOS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorAgendamientos,
    AgendamientosService
>();


// ---------------------------------------------------------
// USUARIOS
// ---------------------------------------------------------

builder.Services.AddScoped<
    IControladorUsuario,
    UsuariosService
>();


builder.Services.AddScoped<
    IControladorHistorialClinico,
    HistorialClinicoService
>();

builder.Services.AddScoped<
    IControladorHospitalizaciones,
    HospitalizacionesService
>();


// =========================================================
// 7) SWAGGER
// =========================================================

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "TuVet API",

            Version = "v1",

            Description =
                "API para la gestión de la clínica veterinaria TuVet"
        }
    );
});


// =========================================================
// 8) OPENAPI
// =========================================================

builder.Services.AddOpenApi();


// =========================================================
// 9) CORS - FRONTEND LOCAL Y AZURE
// =========================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        policy =>
        {
            policy
                .WithOrigins(

                    // React + Vite local
                    "http://localhost:5173",

                    // Vite Preview
                    "http://localhost:4173",

                    // Frontend oficial en Azure
                    "https://tuvet-front-csfhenhmg3huemdf.westus-01.azurewebsites.net"

                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});


// =========================================================
// 10) CREAR APLICACIÓN
// =========================================================

var app = builder.Build();


// =========================================================
// 11) SWAGGER
// =========================================================

app.UseSwagger();


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "TuVet API v1"
    );

    options.RoutePrefix = "swagger";
});


// =========================================================
// 12) OPENAPI EN DESARROLLO
// =========================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// =========================================================
// 13) MIDDLEWARE
// =========================================================

app.UseRouting();


// Permitir frontend local y Azure.

app.UseCors(
    "AllowSpecificOrigin"
);


// Si posteriormente implementas autenticación formal:
//
// app.UseAuthentication();
//
// app.UseAuthorization();


// =========================================================
// 14) MAPEAR CONTROLADORES
// =========================================================

app.MapControllers();


// =========================================================
// 15) PRUEBA DE CONEXIÓN MYSQL
// =========================================================

// Endpoint de diagnóstico disponible únicamente
// cuando el entorno es Development.

if (app.Environment.IsDevelopment())
{
    app.MapGet(
        "/test-db",
        async (TuVetContext context) =>
        {
            try
            {
                var conectado =
                    await context.Database.CanConnectAsync();

                if (!conectado)
                {
                    return Results.Problem(
                        detail:
                            "No se pudo conectar a la base de datos MySQL.",

                        title:
                            "Error de conexión"
                    );
                }

                var cantidadPersonas =
                    await context.Personas.CountAsync();

                return Results.Ok(
                    new
                    {
                        conexion = true,

                        mensaje =
                            "Conexión a MySQL correcta",

                        personas =
                            cantidadPersonas
                    }
                );
            }
            catch (Exception)
            {
                return Results.Problem(
                    detail:
                        "No se pudo verificar la conexión a MySQL.",

                    title:
                        "Error de conexión a MySQL"
                );
            }
        }
    );
}


// =========================================================
// 16) INICIAR APLICACIÓN
// =========================================================

app.Run();