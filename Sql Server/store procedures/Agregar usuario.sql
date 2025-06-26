ALTER PROCEDURE RegistrarUsuario
  @Nombre NVARCHAR(100),
  @Apellido NVARCHAR(100),
  @Email NVARCHAR(100),
  @Contrasena NVARCHAR(255),
  @UsuarioID INT OUTPUT
AS
BEGIN
  BEGIN TRY
    IF EXISTS (
      SELECT 1 FROM Usuarios WHERE Email = @Email
    )
    BEGIN
      RAISERROR('El correo ya está registrado', 16, 1)
      RETURN
    END

    INSERT INTO Usuarios (Nombre, Apellido, Email, Contrasena, FechaRegistro)
    VALUES (@Nombre, @Apellido, @Email, @Contrasena, GETDATE())

    SET @UsuarioID = SCOPE_IDENTITY()
  END TRY
  BEGIN CATCH
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    RAISERROR('Error al registrar el usuario: %s', 16, 1, @ErrorMessage)
    RETURN
  END CATCH
END

DECLARE @ID INT;

EXEC RegistrarUsuario 
  @Nombre = 'Prueba', 
  @Apellido = 'Usuario', 
  @Email = 'prueba@correo.com', 
  @Contrasena = '12345', 
  @UsuarioID = @ID OUTPUT;

SELECT @ID AS Resultado;