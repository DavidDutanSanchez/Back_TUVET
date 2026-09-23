using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tuvet.Setting
{
    // =========================================================
    // CATEGORIAS
    // =========================================================
    public class CategoriasConfiguration : IEntityTypeConfiguration<Categorias>
    {
        public void Configure(EntityTypeBuilder<Categorias> builder)
        {
            builder.HasKey(e => e.IdCategoria);

            builder.ToTable("categorias");

            builder.HasIndex(e => e.IdCategoria, "PRIMARY");

            builder.HasIndex(e => e.NombreCategoria, "nombreCategoria");

            builder.Property(e => e.IdCategoria)
                .HasColumnType("char(36)")
                .HasColumnName("idCategoria")
                .IsRequired();

            builder.Property(e => e.NombreCategoria)
                .HasMaxLength(255)
                .HasColumnName("nombreCategoria")
                .IsRequired();
        }
    }


    // =========================================================
    // COLORES
    // =========================================================
    public class ColoresConfiguration : IEntityTypeConfiguration<Colores>
    {
        public void Configure(EntityTypeBuilder<Colores> builder)
        {
            builder.HasKey(e => e.IdColor);

            builder.ToTable("colores");

            builder.HasIndex(e => e.IdColor, "PRIMARY");

            builder.HasIndex(e => e.NombreColor, "nombreColor");

            builder.Property(e => e.IdColor)
                .HasColumnType("char(36)")
                .HasColumnName("idColor")
                .IsRequired();

            builder.Property(e => e.NombreColor)
                .HasMaxLength(255)
                .HasColumnName("nombreColor")
                .IsRequired();
        }
    }


    // =========================================================
    // ESPECIES
    // =========================================================
    public class EspeciesConfiguration : IEntityTypeConfiguration<Especies>
    {
        public void Configure(EntityTypeBuilder<Especies> builder)
        {
            builder.HasKey(e => e.IdEspecies);

            builder.ToTable("especies");

            builder.HasIndex(e => e.IdEspecies, "PRIMARY");

            builder.HasIndex(e => e.NombreEspecie, "nombreEspecie");

            builder.Property(e => e.IdEspecies)
                .HasColumnType("char(36)")
                .HasColumnName("idEspecies")
                .IsRequired();

            builder.Property(e => e.NombreEspecie)
                .HasMaxLength(255)
                .HasColumnName("nombreEspecie")
                .IsRequired();
        }
    }


    // =========================================================
    // INVENTARIOS
    // =========================================================
    public class InventariosConfiguration : IEntityTypeConfiguration<Inventarios>
    {
        public void Configure(EntityTypeBuilder<Inventarios> builder)
        {
            builder.HasKey(e => e.IdInventario);

            builder.ToTable("inventarios");

            builder.HasIndex(e => e.IdInventario, "PRIMARY");

            builder.HasIndex(
                e => e.Id_Producto,
                "fk_Inventarios_Productos_idx"
            );

            builder.Property(e => e.IdInventario)
                .HasColumnType("char(36)")
                .HasColumnName("idInventario")
                .IsRequired();

            builder.Property(e => e.Id_Producto)
                .HasColumnType("char(36)")
                .HasColumnName("Id_Producto")
                .IsRequired();

            builder.Property(e => e.FechaMovimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaMovimiento")
                .IsRequired();

            builder.Property(e => e.TipoMovimiento)
                .HasColumnName("tipoMovimiento")
                .IsRequired();

            builder.Property(e => e.Cantidad)
                .HasColumnName("cantidad")
                .IsRequired();

            builder.Property(e => e.FechaCaducidad)
                .HasColumnType("date")
                .HasColumnName("fechaCaducidad");

            builder.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.Id_Producto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


    // =========================================================
// MASCOTAS
// =========================================================
public class MascotasConfiguration : IEntityTypeConfiguration<Mascotas>
{
    public void Configure(EntityTypeBuilder<Mascotas> builder)
    {
        builder.HasKey(e => e.IdMascota);

        builder.ToTable("mascotas");

        builder.HasIndex(
            e => e.IdMascota,
            "PRIMARY"
        );

        builder.HasIndex(
            e => e.Id_Persona,
            "fk_Mascotas_Personas_idx"
        );

        builder.HasIndex(
            e => e.Id_Color,
            "fk_Mascotas_Colores_idx"
        );

        builder.HasIndex(
            e => e.Id_Especie,
            "fk_Mascotas_Especies_idx"
        );

        builder.HasIndex(
            e => e.Id_Raza,
            "fk_Mascotas_Razas_idx"
        );

        // =====================================================
        // ID MASCOTA
        // =====================================================
        builder.Property(e => e.IdMascota)
            .HasColumnType("char(36)")
            .HasColumnName("idMascota")
            .IsRequired();

        // =====================================================
        // PROPIETARIO
        // =====================================================
        builder.Property(e => e.Id_Persona)
            .HasColumnType("char(36)")
            .HasColumnName("Id_Persona")
            .IsRequired();

        // =====================================================
        // NOMBRE
        // =====================================================
        builder.Property(e => e.Nombre)
            .HasMaxLength(255)
            .HasColumnName("nombre")
            .IsRequired();

        // =====================================================
        // COLOR
        // =====================================================
        builder.Property(e => e.Id_Color)
            .HasColumnType("char(36)")
            .HasColumnName("Id_Color")
            .IsRequired();

        // =====================================================
        // SEXO
        // false = Hembra
        // true  = Macho
        // =====================================================
        builder.Property(e => e.Sexo)
            .HasColumnName("sexo")
            .IsRequired();

        // =====================================================
        // ESPECIE
        // =====================================================
        builder.Property(e => e.Id_Especie)
            .HasColumnType("char(36)")
            .HasColumnName("Id_Especie")
            .IsRequired();

        // =====================================================
        // RAZA
        // =====================================================
        builder.Property(e => e.Id_Raza)
            .HasColumnType("char(36)")
            .HasColumnName("Id_Raza")
            .IsRequired();

        // =====================================================
        // FECHA DE NACIMIENTO
        // NULL cuando solamente conocemos la edad aproximada.
        // NO colocar IsRequired().
        // =====================================================
        builder.Property(e => e.FechaDeNacimiento)
            .HasColumnType("date")
            .HasColumnName("fechaDeNacimiento");

        // =====================================================
        // EDAD APROXIMADA
        // NULL cuando conocemos la fecha exacta de nacimiento.
        // =====================================================
        builder.Property(e => e.EdadAproximada)
            .HasColumnName("EdadAproximada");

        // =====================================================
        // INDICA SI LA EDAD ES APROXIMADA
        // false = se utiliza FechaDeNacimiento
        // true  = se utiliza EdadAproximada
        // =====================================================
        builder.Property(e => e.EdadEsAproximada)
            .HasColumnName("EdadEsAproximada")
            .IsRequired();

        // =====================================================
        // ESTERILIZADO
        // En tu BD:
        // false (0) = Sí
        // true  (1) = No
        // =====================================================
        builder.Property(e => e.Esterilizado)
            .HasColumnName("esterilizado")
            .IsRequired();

        // =====================================================
        // MICROCHIP
        // OPCIONAL
        // =====================================================
        builder.Property(e => e.CodigoMicrochip)
            .HasMaxLength(255)
            .HasColumnName("codigoMicrochip");

        // =====================================================
        // ENFERMEDADES PREEXISTENTES
        // OPCIONAL
        // =====================================================
        builder.Property(e => e.EnfermedadesPreexistentes)
            .HasColumnType("text")
            .HasColumnName("EnfermedadesPreexistentes");

        // =====================================================
        // RELACIÓN PERSONA -> MASCOTAS
        // =====================================================
        builder.HasOne(e => e.Persona)
            .WithMany(e => e.Mascotas)
            .HasForeignKey(e => e.Id_Persona)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================
        // RELACIÓN COLOR -> MASCOTAS
        // =====================================================
        builder.HasOne(e => e.Color)
            .WithMany(e => e.Mascotas)
            .HasForeignKey(e => e.Id_Color)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================
        // RELACIÓN ESPECIE -> MASCOTAS
        // =====================================================
        builder.HasOne(e => e.Especie)
            .WithMany(e => e.Mascotas)
            .HasForeignKey(e => e.Id_Especie)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================
        // RELACIÓN RAZA -> MASCOTAS
        // =====================================================
        builder.HasOne(e => e.Raza)
            .WithMany(e => e.Mascotas)
            .HasForeignKey(e => e.Id_Raza)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

    // =========================================================
    // PERSONAS
    // =========================================================
    public class PersonasConfiguration : IEntityTypeConfiguration<Personas>
    {
        public void Configure(EntityTypeBuilder<Personas> builder)
        {
            builder.HasKey(e => e.IdPersona);

            builder.ToTable("personas");

            builder.HasIndex(e => e.IdPersona, "PRIMARY");

            builder.HasIndex(
                e => e.NumeroIdentificacion,
                "numeroIdentificacion"
            );

            builder.HasIndex(e => e.Nombres, "nombres");

            builder.HasIndex(e => e.Apellidos, "apellidos");

            builder.Property(e => e.IdPersona)
                .HasColumnType("char(36)")
                .HasColumnName("idPersona")
                .IsRequired();

            builder.Property(e => e.TipoIdentificacion)
                .HasColumnName("tipoIdentificacion")
                .IsRequired();

            builder.Property(e => e.NumeroIdentificacion)
                .HasMaxLength(13)
                .HasColumnName("numeroIdentificacion")
                .IsRequired();

            builder.Property(e => e.Nombres)
                .HasMaxLength(255)
                .HasColumnName("nombres")
                .IsRequired();

            builder.Property(e => e.Apellidos)
                .HasMaxLength(255)
                .HasColumnName("apellidos")
                .IsRequired();

            builder.Property(e => e.TipoTelefono)
                .HasColumnName("tipoTelefono")
                .IsRequired();

            builder.Property(e => e.Telefono)
                .HasMaxLength(15)
                .HasColumnName("telefono")
                .IsRequired();

            builder.Property(e => e.CorreoElectronico)
                .HasMaxLength(255)
                .HasColumnName("correoElectronico")
                .IsRequired();

            builder.Property(e => e.Direccion)
                .HasColumnType("text")
                .HasColumnName("direccion");
        }
    }


    // =========================================================
    // PRODUCTOS
    // =========================================================
    public class ProductosConfiguration : IEntityTypeConfiguration<Productos>
    {
        public void Configure(EntityTypeBuilder<Productos> builder)
        {
            builder.HasKey(e => e.IdProductos);

            builder.ToTable("productos");

            builder.HasIndex(e => e.IdProductos, "PRIMARY");

            builder.HasIndex(
                e => e.NombreProducto,
                "nombreProducto"
            );

            builder.HasIndex(
                e => e.CodigProducto,
                "codigoProducto"
            );

            builder.HasIndex(
                e => e.Id_Categoria,
                "fk_Productos_Categorias_idx"
            );

            builder.Property(e => e.IdProductos)
                .HasColumnType("char(36)")
                .HasColumnName("idProductos")
                .IsRequired();

            builder.Property(e => e.NombreProducto)
                .HasMaxLength(255)
                .HasColumnName("nombreProducto")
                .IsRequired();

            // IMPORTANTE:
            // La columna real de la BD se llama CodigProducto
            // no codigoProducto.
            builder.Property(e => e.CodigProducto)
                .HasMaxLength(255)
                .HasColumnName("CodigProducto");

            builder.Property(e => e.DescripcionProducto)
                .HasColumnType("text")
                .HasColumnName("descripcionProducto");

            // La BD fue modificada a DECIMAL(10,2)
            builder.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10,2)")
                .HasColumnName("precioVenta")
                .IsRequired();

            builder.Property(e => e.StockMinimo)
                .HasColumnName("stockMinimo")
                .IsRequired();

            builder.Property(e => e.Unidad)
                .HasColumnName("unidad")
                .IsRequired();

            builder.Property(e => e.Id_Categoria)
                .HasColumnType("char(36)")
                .HasColumnName("Id_Categoria")
                .IsRequired();

            builder.HasOne(e => e.Categoria)
                .WithMany(e => e.Productos)
                .HasForeignKey(e => e.Id_Categoria)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


    // =========================================================
    // RAZAS
    // =========================================================
    public class RazasConfiguration : IEntityTypeConfiguration<Razas>
    {
        public void Configure(EntityTypeBuilder<Razas> builder)
        {
            builder.HasKey(e => e.IdRaza);

            builder.ToTable("razas");

            builder.HasIndex(e => e.IdRaza, "PRIMARY");

            builder.HasIndex(e => e.NombreRaza, "nombreRaza");

            builder.Property(e => e.IdRaza)
                .HasColumnType("char(36)")
                .HasColumnName("idRaza")
                .IsRequired();

            builder.Property(e => e.NombreRaza)
                .HasMaxLength(255)
                .HasColumnName("nombreRaza")
                .IsRequired();
        }
    }


    // =========================================================
    // SERVICIOS
    // =========================================================
    public class ServiciosConfiguration : IEntityTypeConfiguration<Servicios>
    {
        public void Configure(EntityTypeBuilder<Servicios> builder)
        {
            builder.HasKey(e => e.IdServicios);

            builder.ToTable("servicios");

            builder.HasIndex(e => e.IdServicios, "PRIMARY");

            builder.HasIndex(
                e => e.NombreServicio,
                "nombreServicio"
            );

            builder.Property(e => e.IdServicios)
                .HasColumnType("char(36)")
                .HasColumnName("idServicios")
                .IsRequired();

            builder.Property(e => e.NombreServicio)
                .HasMaxLength(255)
                .HasColumnName("nombreServicio")
                .IsRequired();

            builder.Property(e => e.DescripcionServicio)
                .HasMaxLength(255)
                .HasColumnName("descripcionServicio");

            // La BD fue modificada a DECIMAL(10,2)
            builder.Property(e => e.PreciosServicio)
                .HasColumnType("decimal(10,2)")
                .HasColumnName("preciosServicio")
                .IsRequired();

            builder.Property(e => e.IncluyeIva)
                .HasColumnName("incluyeIva")
                .IsRequired();

            builder.Property(e => e.DescuentoServicio)
                .HasColumnName("descuentoServicio")
                .IsRequired();
        }
    }


    // =========================================================
    // USUARIOS
    // =========================================================
    public class UsuariosConfiguration : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.HasKey(e => e.IdUsuario);

            builder.ToTable("usuarios");

            builder.HasIndex(e => e.IdUsuario, "PRIMARY");

            builder.HasIndex(
                e => e.NombreUsuario,
                "nombreUsuario"
            );

            builder.HasIndex(
                e => e.Id_Persona,
                "fk_Usuarios_Personas_idx"
            );

            builder.Property(e => e.IdUsuario)
                .HasColumnType("char(36)")
                .HasColumnName("idUsuario")
                .IsRequired();

            builder.Property(e => e.NombreUsuario)
                .HasMaxLength(255)
                .HasColumnName("nombreUsuario")
                .IsRequired();

            builder.Property(e => e.Contrasenia)
                .HasColumnType("blob")
                .HasColumnName("contrasenia")
                .IsRequired();

            builder.Property(e => e.Permisos)
                .HasMaxLength(255)
                .HasColumnName("permisos")
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .IsRequired();

            builder.Property(e => e.Id_Persona)
                .HasColumnType("char(36)")
                .HasColumnName("Id_Persona")
                .IsRequired();

            builder.HasOne(e => e.Persona)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.Id_Persona)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }


    // =========================================================
    // AGENDAMIENTOS
    // =========================================================
    public class AgendamientosConfiguration
        : IEntityTypeConfiguration<Agendamientos>
    {
        public void Configure(EntityTypeBuilder<Agendamientos> builder)
        {
            builder.HasKey(e => e.IdAgendamiento);

            builder.ToTable("agendamientos");

            builder.HasIndex(
                e => e.IdAgendamiento,
                "PRIMARY"
            );

            builder.HasIndex(
                e => e.Id_Mascota,
                "FK_agendamiento_mascota_idx"
            );

            builder.HasIndex(
                e => e.Id_Persona,
                "FK_agendamiento_persona_idx"
            );

            builder.Property(e => e.IdAgendamiento)
                .HasColumnType("char(36)")
                .HasColumnName("IdAgendamiento")
                .IsRequired();

            builder.Property(e => e.FechaAgendamiento)
                .HasColumnType("datetime")
                .HasColumnName("FechaAgendamiento")
                .IsRequired();

            builder.Property(e => e.TipoAgendamiento)
                .HasColumnName("TipoAgendamiento")
                .IsRequired();

            builder.Property(e => e.Id_Mascota)
                .HasColumnType("char(36)")
                .HasColumnName("Id_Mascota")
                .IsRequired();

            builder.Property(e => e.Id_Persona)
                .HasColumnType("char(36)")
                .HasColumnName("Id_Persona")
                .IsRequired();

            builder.Property(e => e.EstadoAgendamiento)
                .HasColumnName("EstadoAgendamiento")
                .IsRequired();

            // Relación con MASCOTAS
            builder.HasOne(e => e.Mascota)
                .WithMany()
                .HasForeignKey(e => e.Id_Mascota)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con PERSONAS
            builder.HasOne(e => e.Persona)
                .WithMany()
                .HasForeignKey(e => e.Id_Persona)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}