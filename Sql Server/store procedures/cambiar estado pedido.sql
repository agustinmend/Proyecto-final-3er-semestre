ALTER PROCEDURE cambiarestadopedido
    @PedidoID INT,
    @Estadonuevo NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
		--Verifica que existe el PedidoID
        IF NOT EXISTS (SELECT 1 FROM Pedidos WHERE PedidoID = @PedidoID)
        BEGIN
            RAISERROR('Error: El pedido con ID %d no existe', 16, 1, @PedidoID)
            RETURN
        END
        --Verifica que solo se ingrese uno de los 3 estados Pendiente, Entregado, Cancelado
        IF @Estadonuevo NOT IN ('Pendiente', 'Entregado', 'Cancelado')
        BEGIN
            RAISERROR('Error: Estado "%s" no válido. Use: Pendiente, Entregado o Cancelado', 16, 1, @Estadonuevo)
            RETURN
        END
        
        UPDATE Pedidos
        SET Estado = @Estadonuevo
        WHERE PedidoID = @PedidoID
        
        PRINT 'Estado actualizado correctamente'
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al cambiar estado: %s', 16, 1, @ErrorMessage)
    END CATCH
END