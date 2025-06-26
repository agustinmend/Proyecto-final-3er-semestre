alter procedure confirmarcompra
	@UsuarioID int,
	@DireccionID int,
	@Carrito Carrito Readonly
as
begin
	begin transaction
	begin try
		--Verificar que exista la Direccion para ese usuario
		if not exists (
			select 1
			from Direcciones
			where DireccionID = @DireccionID and UsuarioID = @UsuarioID
		)
		begin
			raiserror('La dirreccion no existe o no pertenece al usuario' , 16 , 1)
			rollback
			return
		end
		--verificar que la compra no sea mayor al stock disponible
		if exists (
			select 1
			from @Carrito as c
			inner join Productos as p
			on c.ProductoID = p.ProductoID
			where c.Cantidad > p.Stock
		)
		begin
			raiserror('Stock insuficiente', 16, 1)
			rollback
			return
		end
		--Realizar compra
		insert into Pedidos (UsuarioID , DireccionID , FechaPedido , Estado)
		values(@UsuarioID , @DireccionID , GETDATE() , 'Pendiente')
		declare @PedidoID int = scope_identity()
		insert into DetallePedido(PedidoID , ProductoID , Cantidad , PrecioUnitario)
		select @PedidoID , ProductoID , Cantidad , PrecioUnitario 
		from @Carrito
		update p
		set p.Stock = p.Stock - c.Cantidad
		from Productos as p
		inner join @Carrito as c
		on p.ProductoID = c.ProductoID
		commit
		select @PedidoID as PedidoID
	end try
	begin catch
		rollback
		raiserror('Error al confirmar pedido', 16 , 1)
	end catch
end
--Carrito en sqlserver
create type Carrito as table(
	ProductoID int,
	Cantidad int,
	PrecioUnitario decimal(10, 2)
)
