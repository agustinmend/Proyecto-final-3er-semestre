alter procedure agregarproducto
	@TiendaID int,
	@CategoriaID int,
	@NombreProducto nvarchar(100),
	@Descripcion nvarchar(255),
	@Precio Decimal (10 , 2),
	@Stock int,
	@Estado nvarchar(50)
as
begin
	begin try
		--verificar	que exista la tienda
		if not exists (
						select 1
						from Tiendas
						where TiendaID = @TiendaID)
		begin
			raiserror('La tienda no existe' , 16 , 1)
			return
		end
		--verificar que exista la categoria
		if not exists (
						select 1
						from Categorias
						where CategoriaID = @CategoriaID)
		begin
			raiserror('La categoria no existe' , 16 , 1)
			return
		end
		--El precio al ingresar tiene que ser mayor igual a cero
		if @Precio <= 0
		begin 
			raiserror('El precio tiene que ser mayor a 0' , 16 , 1)
			return
		end
		--Controla que el stock no sea negativo
		if @Stock <= 0
		begin
			raiserror('Para agregar producto el stock debe ser mayor a 0',16 , 1)
			return
		end
		--insercion
		insert into Productos (TiendaID , CategoriaID, NombreProducto , Descripcion , Precio , Stock , Estado , FechaPublicacion)
		values (@TiendaID , @CategoriaID , @NombreProducto , @Descripcion , @Precio , @Stock , @Estado , GETDATE())
		--Devuelve el ProductoID creado
		select SCOPE_IDENTITY() as ProductoID
	end try
	begin catch
		DECLARE 
			@ErrorMessage NVARCHAR(4000),
			@ErrorSeverity INT,
			@ErrorState INT

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE()

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
	end catch
end
