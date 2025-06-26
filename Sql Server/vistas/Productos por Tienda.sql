create view MostrarProductosPorTienda as
select t.TiendaID,
		t.NombreTienda,
		p.NombreProducto,
		p.Precio,
		p.Stock,
		p.Estado,
		p.FechaPublicacion
from Tiendas as t
inner join Productos as p
on t.TiendaID = p.TiendaID
