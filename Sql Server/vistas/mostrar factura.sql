--Descripcion: Muestra los datos relevantes del pedido anadir where si se quiere ser especifico
create view mostrar_detalle_venta as
select p.PedidoID as Nro_factura,
		p.FechaPedido,
		p.Estado as Estado_pedido,
		cliente.UsuarioID as ID_Cliente,
		cliente.Nombre as Nombre_cliente,
		d.DireccionDetalle + ', ' + d.Ciudad + ', ' + d.Pais as Direccion_envio,
		pr.ProductoID,
		pr.NombreProducto,
		dp.Cantidad,
		dp.PrecioUnitario,
		dp.Cantidad * dp.PrecioUnitario as Subtotal,
		t.TiendaID,
		t.NombreTienda as Tienda

from Pedidos as p
inner join Usuarios as cliente
on p.UsuarioID = cliente.UsuarioID
inner join Direcciones as d
on p.DireccionID = d.DireccionID
inner join DetallePedido as dp
on p.PedidoID = dp.PedidoID
inner join Productos as pr
on dp.ProductoID = pr.ProductoID
inner join Tiendas as t
on pr.TiendaID = t.TiendaID
inner join Usuarios as vendedor
on vendedor.UsuarioID = t.UsuarioID
