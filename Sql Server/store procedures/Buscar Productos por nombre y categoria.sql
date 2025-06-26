create procedure BuscarProducto
    @BUSQUEDA NVARCHAR(255)
AS
BEGIN
	--obtiene los datos necesarios para el frontend
    SELECT p.ProductoID, p.NombreProducto, p.Descripcion, p.Precio, p.Stock, p.FechaPublicacion, c.NombreCategoria
    FROM Productos p
    INNER JOIN Categorias AS c 
	ON p.CategoriaID = c.CategoriaID
    WHERE (p.NombreProducto LIKE '%' + @BUSQUEDA + '%' 
    OR c.NombreCategoria LIKE '%' + @BUSQUEDA + '%')    --busca el texto ingresado en los nombres de los productos y las categoriaas
    AND p.Estado = 'Activo'								--solo devuelve los que estan con nestado "Activo"
END;