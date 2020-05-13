SELECT  
Usuarios.Id AS ID, 
Usuario AS USUARIO, 
Roles.Descripcion AS ROL, 
Nombre AS NOMBRES, 
Apellido AS APELLIDOS, 
FechaCreacion AS FECHA_ALTA,
CASE
    WHEN Activo = 1 THEN 'ACTIVO'
    WHEN Activo = 0 THEN 'BAJA'
    ELSE 'DESCONOCIDO'
END AS ESTADO
FROM Usuarios
INNER JOIN Roles ON 
Usuarios.IdRol = Roles.Id

 --SELECT MAX(NumeroItem) 
 --               FROM DetallesPedidos
 --               WHERE NumeroPedido = 3
--         SELECT 
--Pedidos.NumeroPedido AS ID_PEDIDO,
--Pedidos.CodigoCliente AS ID_CLIENTE,
--Clientes.RazonSocial AS RAZON_SOCIAL,
--Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE,
--Pedidos.Fecha AS FECHA_PEDIDO, 
--CASE
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE'
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO'
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO'
--    ELSE 'DESCONOCIDO'
--END AS ESTADO_PEDIDO,
--SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES
--FROM Pedidos
--INNER JOIN Clientes ON
--Pedidos.CodigoCliente = Clientes.Codigo
--INNER JOIN Usuarios ON
--Clientes.IdUsuario = Usuarios.Id
--WHERE Pedidos.CodigoCliente = 1000
--AND Pedidos.NumeroPedido = 1




--SELECT 
--Pedidos.NumeroPedido AS ID_PEDIDO,
--Pedidos.CodigoCliente AS ID_CLIENTE,
--Clientes.RazonSocial AS RAZON_SOCIAL,
--Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE,
--Pedidos.Fecha AS FECHA_PEDIDO, 
----SUBSTRING( Pedidos.Observacion, 0, 4 ) AS ESTADO_PEDIDO,
--CASE
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE'
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO'
--    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO'
--    ELSE 'DESCONOCIDO'
--END AS ESTADO_PEDIDO,
--SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES
--FROM Pedidos
--INNER JOIN Clientes ON
--Pedidos.CodigoCliente = Clientes.Codigo
--INNER JOIN Usuarios ON
--Clientes.IdUsuario = Usuarios.Id
--WHERE Pedidos.NumeroPedido = 1
--AND Clientes.Codigo = 1000

--INSERT INTO DetallesPedidos (NumeroPedido, NumeroItem, CodigoProducto, Cantidad, PrecioUnitario )
--VALUES (1, 10, 1007, 5, 152.67);

/*

SELECT column1, column2, ...
FROM table_name;

INSERT INTO table_name (column1, column2, column3, ...)
VALUES (value1, value2, value3, ...);

UPDATE table_name
SET column1 = value1, column2 = value2, ...
WHERE condition;

DELETE FROM table_name 
WHERE condition;



*/