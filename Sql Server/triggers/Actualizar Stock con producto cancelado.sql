ALTER TRIGGER ActualizarStockPedidoCancelado
ON Pedidos
AFTER UPDATE
AS
BEGIN
	--Ejecuta si algun pedido paso a ser cancelado
    IF EXISTS(
        SELECT 1
        FROM inserted AS i
        INNER JOIN deleted AS d
        ON i.PedidoID = d.PedidoID
        WHERE i.Estado = 'Cancelado' AND d.Estado <> 'Cancelado'
    )
    BEGIN
	--Devuelve el stock
        UPDATE p
        SET Stock = Stock + dp.Cantidad
        FROM Productos AS p
        INNER JOIN DetallePedido AS dp
        ON p.ProductoID = dp.ProductoID
        INNER JOIN inserted AS i
        ON dp.PedidoID = i.PedidoID
        WHERE i.Estado = 'Cancelado';
        --Si al cancelar el pedido el producto tenia stock 0,
		--se le cambia el estado a activo despues de devolverle al stock
        UPDATE p
        SET Estado = 'Activo'
        FROM Productos AS p
        INNER JOIN DetallePedido AS dp
        ON p.ProductoID = dp.ProductoID
        INNER JOIN inserted AS i
        ON dp.PedidoID = i.PedidoID
        INNER JOIN deleted AS d
        ON i.PedidoID = d.PedidoID
        WHERE i.Estado = 'Cancelado'
        AND p.Stock - dp.Cantidad = 0 
        AND p.Stock > 0
    END
END