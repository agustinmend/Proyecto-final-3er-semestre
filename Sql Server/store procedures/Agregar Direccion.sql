alter procedure AgregarDireccion
	@UsuarioID int,
	@Pais nvarchar(100),
	@Ciudad nvarchar(100),
	@CodigoPostal nvarchar(20),
	@DireccionDetalle nvarchar(255)
as
begin
	begin try
		--Verificar que exista el usuario
		if not exists (select 1
						from Usuarios
						where UsuarioID = @UsuarioID)
		begin
			raiserror('El usuario no existe' , 16 , 1)
			return
		end
		--realizar la insercion en la tabla
		insert into Direcciones (UsuarioID , Pais , Ciudad , CodigoPostal , DireccionDetalle)
		values (@UsuarioID , @Pais , @Ciudad , @CodigoPostal , @DireccionDetalle)
		--Devolver el nuevo DireccionID creado
		select SCOPE_IDENTITY() as DireccionID
	end try
	begin catch
		raiserror('No se pudo agregar direccion' , 16 , 1)
	end catch
end