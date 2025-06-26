alter view PedidosPorTienda as
select t.TiendaID,
		t.NombreTienda,
		pe.PedidoID,
		pe.UsuarioID as CompradorID,
		u.Nombre + ' ' + u.Apellido as Nombre_Cliente,
		pe.DireccionID,
		pe.FechaPedido,
		pe.Estado as Estado_Pedido,
		dp.ProductoID,
		p.NombreProducto,
		dp.Cantidad,
		dp.PrecioUnitario,
		dp.Cantidad * dp.PrecioUnitario as TotalProducto
from Tiendas as t
inner join Productos as p 
on t.TiendaID = p.TiendaID
inner join DetallePedido as dp
on p.ProductoID = dp.ProductoID
inner join Pedidos as pe
on dp.PedidoID = pe.PedidoID
inner join Usuarios as u
on pe.UsuarioID = u.UsuarioID


