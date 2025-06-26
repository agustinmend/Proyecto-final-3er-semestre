alter trigger Estado_Producto_sin_stock
on productos
after insert
as
begin
	--si el producto llega a stock 0, se lo vuelve Inactivo
	update p
	set Estado = 'Inactivo'
	from Productos as p
	inner join inserted as i
	on i.ProductoID = p.ProductoID
	where i.Stock = 0 and i.Estado = 'Activo'
end
