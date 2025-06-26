alter procedure CrearTienda
	@UsuarioID int,
	@NombreTienda nvarchar(100),
	@Descripcion NVARCHAR(255)
as
begin
	begin try
		if not exists (select 1
						from Usuarios
						where UsuarioID = @UsuarioID)
		begin 
			raiserror('El usuario no existe' , 16 , 1)
			return
		end
		if exists (select 1
				from Tiendas
				where NombreTienda = @NombreTienda)
		begin
			raiserror('Nombre de tienda ocupado, coloque otro nombre' , 16 , 1)
			return
		end
		
		insert into Tiendas(UsuarioID , NombreTienda , Descripcion , FechaCreacion)
		values (@UsuarioID , @NombreTienda , @Descripcion , GETDATE())

		select SCOPE_IDENTITY() as TiendaID
	end try
	begin catch
        DECLARE @MensajeError NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@MensajeError, 16, 1);
        RETURN;
	end catch
end
