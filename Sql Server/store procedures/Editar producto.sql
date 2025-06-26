alter procedure editarproducto
	@ProductoID int,
	@NombreProducto nvarchar(100),
	@Descripcion nvarchar(255),
	@Precio Decimal(10 , 2),
	@Stock int,
	@Estado nvarchar(50),
	@CategoriaID int
as
begin
	begin try
		--Verifica que el producto no se halla vendido
        if exists (
            select 1
            from DetallePedido
            where ProductoID = @ProductoID
        )
        begin
            raiserror('No se puede editar el producto porque ya ha sido vendido.', 16, 1);
            return;
        end
		--Verifica que exista el producto
		if not exists (select 1
						from Productos
						where ProductoID = @ProductoID)
		begin
			raiserror('El producto no existe' , 16 , 1)
			return
		end
		--Verifica que exista la categoria
		if not exists(select 1 
						from Categorias
						where CategoriaID = @CategoriaID)
		begin
			raiserror('La categoria no existe.' , 16 , 1)
			return
		end
		--Verifica que el precio sea mayor a 0
		if @Precio <= 0
		begin
			raiserror('Precio tiene que ser mayor a 0' , 16 , 1)
			return
		end
		--No stock negativo
		if @Stock < 0
		begin
			raiserror('Stock tiene que ser mayor a 0' , 16 ,1 )
			return
		end
		update Productos
		set
			NombreProducto = @NombreProducto,
			Descripcion = @Descripcion,
			Precio = @Precio,
			Stock = @Stock,
			Estado = @Estado,
			CategoriaID = @CategoriaID
		where ProductoID = @ProductoID
	end try
	begin catch
		raiserror('No se pudo actualizar producto' , 16 , 1)
		return
	end catch
end
