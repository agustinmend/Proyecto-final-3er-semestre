alter procedure inactivarproducto
	@ProductoID int,
	@Estado nvarchar(25)
as
begin
    BEGIN TRY
		--verificar que exista el producto
        IF NOT EXISTS (SELECT 1 FROM Productos WHERE ProductoID = @ProductoID)
        BEGIN
            RAISERROR('Error: No existe ningún producto con el ID %d', 16, 1, @ProductoID)
            RETURN
        END
        UPDATE Productos
        SET Estado = @Estado
        WHERE ProductoID = @ProductoID
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al inactivar el producto: %s', 16, 1, @ErrorMessage)
    END CATCH
end