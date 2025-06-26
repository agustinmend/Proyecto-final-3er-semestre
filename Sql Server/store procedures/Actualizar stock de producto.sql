CREATE PROCEDURE actualizarstockproducto
    @ProductoID INT,
    @Stock INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Productos WHERE ProductoID = @ProductoID)
        BEGIN
            RAISERROR('El producto no existe', 16, 1)
            RETURN
        END
        
        IF @Stock < 0
        BEGIN
            RAISERROR('El stock debe ser mayor o igual a 0', 16, 1)
            RETURN
        END

        -- Actualizar el stock
        UPDATE Productos
        SET Stock = @Stock
        WHERE ProductoID = @ProductoID
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al actualizar el stock: %s', 16, 1, @ErrorMessage)
        RETURN
    END CATCH
END