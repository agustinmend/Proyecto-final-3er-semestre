alter trigger ProductoAgotado
on Productos
after update
as
begin
	--si el producto llega a 0 stock se le cambia el estado a Agotado
    update p
    set Estado = 'Agotado'
    from Productos as p
    inner join inserted as i 
	on p.ProductoID = i.ProductoID
    where i.Stock <= 0 and p.Estado <> 'Agotado';
end