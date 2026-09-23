using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Service;


var builder = WebApplication.CreateBuilder(args);


// =========================================================
// 1) CONFIGURACIÓN DE LA CONEXIÓN A MYSQL
// =========================================================

var connectionString =
    builder.Configuration.GetConnectionString("TuVet");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'TuVet' not found."
    );
}

builder.Services.AddDbContext<TuVetContext>(options =>
    options.UseMySQL(connectionString)
);


// =========================================================
// 2) CONTROLLERS
// =========================================================

builder.Services.AddControllers();


// =========================================================
// 3) INYECCIÓN DE DEPENDENCIAS
// =========================================================

// PERSONAS
builder.Services.AddScoped<
    IControladorPersonas,
    PersonasService
>();

// MASCOTAS
builder.Services.AddScoped<
    IControladorMascotas,
    MascotasService
>();

// ESPECIES
builder.Services.AddScoped<
    IControladorEspecies,
    EspeciesService
>();

// RAZAS
builder.Services.AddScoped<
    IControladorRazas,
    RazaService
>();

// COLORES
builder.Services.AddScoped<
    IControladorColores,
    ColoresService
>();

// PRODUCTOS
// Tu clase actualmente se llama ProdcutosService.
builder.Services.AddScoped<
    IControladorProductos,
    ProdcutosService
>();

// CATEGORÍAS
builder.Services.AddScoped<
    IControladorCategorias,
    CategoriasService
>();

builder.Services.AddScoped<
    IControladorServicios,
    ServiciosService
>();

builder.Services.AddScoped<
    IControladorCasasComerciales,
    CasasComercialesService
>();

builder.Services.AddScoped<
    IControladorAgendamientos,
    AgendamientosService
>();

builder.Services.AddScoped<IControladorUsuario, UsuariosService>();

// =========================================================
// 4) SWAGGER
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
// 5) OPENAPI
// =========================================================

builder.Services.AddOpenApi();


// =========================================================
// 6) CORS
// =========================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});


// =========================================================
// 7) CREAR APLICACIÓN
// =========================================================

var app = builder.Build();


// =========================================================
// 8) SWAGGER
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
// 9) OPENAPI EN DEVELOPMENT
// =========================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// =========================================================
// 10) MIDDLEWARE
// =========================================================

app.UseRouting();

app.UseCors("AllowSpecificOrigin");


// Cuando posteriormente agreguemos autenticación:
// app.UseAuthentication();
// app.UseAuthorization();


// =========================================================
// 11) CONTROLLERS
// =========================================================

app.MapControllers();


// =========================================================
// 12) PRUEBA DE CONEXIÓN A MYSQL
// =========================================================

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
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                title:
                    "Error de conexión a MySQL"
            );
        }
    }
);


// =========================================================
// 13) PUERTO DE LA API
// =========================================================

app.Urls.Add(
    "http://localhost:5151"
);


// =========================================================
// 14) EJECUTAR APLICACIÓN
// =========================================================

app.Run();