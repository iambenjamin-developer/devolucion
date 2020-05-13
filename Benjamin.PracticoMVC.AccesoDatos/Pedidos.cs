using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using Dapper;

namespace Benjamin.PracticoMVC.AccesoDatos
{
    public class Pedidos
    {
        string cadenaConexion = Conexiones.ObtenerCadenaConexion();

        public List<Entidades.DetallesPedidos> ListaDetallePedido(int idPedido)
        {

            var lista = new List<Entidades.DetallesPedidos>();


            /*
SELECT 
DetallesPedidos.NumeroItem AS ITEM,
Marcas.Nombre AS MARCA,
Productos.Codigo AS ID_PRODUCTO,
Productos.Nombre AS PRODUCTO,
DetallesPedidos.PrecioUnitario AS PRECIO_UNITARIO,
DetallesPedidos.Cantidad AS CANTIDAD,
(DetallesPedidos.PrecioUnitario * DetallesPedidos.Cantidad) AS SUBTOTAL
FROM DetallesPedidos
INNER JOIN Pedidos ON
DetallesPedidos.NumeroPedido = Pedidos.NumeroPedido
INNER JOIN Productos ON
DetallesPedidos.CodigoProducto = Productos.Codigo
INNER JOIN Marcas ON
Productos.IdMarca = Marcas.Id 
WHERE Pedidos.NumeroPedido = 1
ORDER BY DetallesPedidos.NumeroItem ASC
             */
            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT ");
            consultaSQL.Append("DetallesPedidos.NumeroItem AS ITEM, ");
            consultaSQL.Append("Marcas.Nombre AS MARCA, ");
            consultaSQL.Append("Productos.Codigo AS ID_PRODUCTO, ");
            consultaSQL.Append("Productos.Nombre AS PRODUCTO, ");
            consultaSQL.Append("DetallesPedidos.PrecioUnitario AS PRECIO_UNITARIO, ");
            consultaSQL.Append("DetallesPedidos.Cantidad AS CANTIDAD, ");
            consultaSQL.Append("(DetallesPedidos.PrecioUnitario * DetallesPedidos.Cantidad) AS SUBTOTAL ");
            consultaSQL.Append("FROM DetallesPedidos ");
            consultaSQL.Append("INNER JOIN Pedidos ON ");
            consultaSQL.Append("DetallesPedidos.NumeroPedido = Pedidos.NumeroPedido ");
            consultaSQL.Append("INNER JOIN Productos ON ");
            consultaSQL.Append("DetallesPedidos.CodigoProducto = Productos.Codigo ");
            consultaSQL.Append("INNER JOIN Marcas ON ");
            consultaSQL.Append("Productos.IdMarca = Marcas.Id  ");
            consultaSQL.Append("WHERE Pedidos.NumeroPedido = @idPedidoParametro ");
            consultaSQL.Append("ORDER BY DetallesPedidos.NumeroItem ASC ");




            using (var connection = new SqlConnection(cadenaConexion))
            {
                lista = connection.Query<Entidades.DetallesPedidos>(consultaSQL.ToString(),

                     new
                     {
                         idPedidoParametro = idPedido

                     }).ToList();
            }

            return lista;

        }


        public int CalcularPrecioSegunCantidad(int idPedido, int nroItem, int cantidad)
        {
            int filasAfectadas = 0;
            /*
UPDATE DetallesPedidos
SET Cantidad = 3
WHERE NumeroPedido = 1
AND NumeroItem = 4

            */

            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("UPDATE DetallesPedidos ");
            consultaSQL.Append("SET Cantidad = @cantidadParametro ");
            consultaSQL.Append("WHERE NumeroPedido = @idPedidoParametro ");
            consultaSQL.Append("AND NumeroItem = @nroItemParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                filasAfectadas = connection.Execute(consultaSQL.ToString(),
                   new
                   {
                       idPedidoParametro = idPedido,
                       nroItemParametro = nroItem,
                       cantidadParametro = cantidad
                   });


            }

            return filasAfectadas;
        }

        public int EliminarItemPedido(int idPedido, int codProducto)
        {
            int filasAfectadas = 0;
            /*
DELETE FROM DetallesPedidos 
WHERE NumeroPedido = 1
AND CodigoProducto = 1003;

            */

            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("DELETE FROM DetallesPedidos ");
            consultaSQL.Append("WHERE NumeroPedido = @numeroPedidoParametro ");
            consultaSQL.Append("AND CodigoProducto = @codigoProductoParametro ");



            using (var connection = new SqlConnection(cadenaConexion))
            {
                filasAfectadas = connection.Execute(consultaSQL.ToString(),
                   new
                   {
                       numeroPedidoParametro = idPedido,
                       codigoProductoParametro = codProducto
                   });


            }

            if (filasAfectadas == 1)
            {

                ReordenarItemsPedido(idPedido);
            }

            return filasAfectadas;
        }

        public void ReordenarItemsPedido(int idPedido)
        {

            SqlConnection conexion = new SqlConnection(cadenaConexion);

            conexion.Open();

            //como vamos a realizar dos inserciones debemos hacerlo con una transaccion
            var transaccion = conexion.BeginTransaction();


            try
            {
                /*
SELECT CodigoProducto FROM DetallesPedidos
WHERE NumeroPedido  = 1
                 */
                StringBuilder consultaSQL1 = new StringBuilder();
                consultaSQL1.Append("SELECT CodigoProducto FROM DetallesPedidos ");
                consultaSQL1.Append("WHERE NumeroPedido  = @idPedidoParametro ");

                List<int> lista = new List<int>();

                using (var connection = new SqlConnection(cadenaConexion))
                {
                    lista = connection.Query<int>(consultaSQL1.ToString(),
                        new { idPedidoParametro = idPedido }).ToList();
                }



                /*
UPDATE DetallesPedidos
SET NumeroItem = 1
WHERE NumeroPedido = 1
AND CodigoProducto = 1006
                 */

                StringBuilder consultaSQL2 = new StringBuilder();
                int filasAfectadas = 0;

                for (int i = 0; i < lista.Count; i++)
                {
                    consultaSQL2.Clear();

                    consultaSQL2.Append("UPDATE DetallesPedidos ");
                    consultaSQL2.Append("SET NumeroItem = @nroItemParametro ");
                    consultaSQL2.Append("WHERE NumeroPedido = @idPedidoParametro ");
                    consultaSQL2.Append("AND CodigoProducto = @idProductoParametro ");


                    filasAfectadas = conexion.Execute(consultaSQL2.ToString(),
                           new
                           {//lo multiplicamos por menos uno para que haya restriccion de clave primaria
                               nroItemParametro = (i + 1) * (-1),
                               idPedidoParametro = idPedido,
                               idProductoParametro = lista[i]
                           }
                           , transaction: transaccion);

                }

                //se enumera la lista para actualizarla nuevo positivamente
                List<int> listaEnumerada = new List<int>();

                for (int i = 0; i < lista.Count; i++)
                {
                    listaEnumerada.Add(i + 1);
                }

                for (int i = 0; i < lista.Count; i++)
                {
                    consultaSQL2.Clear();

                    consultaSQL2.Append("UPDATE DetallesPedidos ");
                    consultaSQL2.Append("SET NumeroItem = @nroItemParametro ");
                    consultaSQL2.Append("WHERE NumeroPedido = @idPedidoParametro ");
                    consultaSQL2.Append("AND CodigoProducto = @idProductoParametro ");


                    filasAfectadas = conexion.Execute(consultaSQL2.ToString(),
                           new
                           {//lo multiplicamos por menos uno para que haya restriccion de clave primaria
                               nroItemParametro = listaEnumerada[i],
                               idPedidoParametro = idPedido,
                               idProductoParametro = lista[i]
                           }
                           , transaction: transaccion);

                }


                // si las operaciones relacionadas salieron bien, se realiza un commit
                transaccion.Commit();


            }
            catch (Exception ex)
            {

                transaccion.Rollback();

            }
            finally
            {
                //si el procedimiento salio bien o mal, siempre se debe cerrar la conexion
                conexion.Close();
            }

        }

        public List<Entidades.Pedidos> MisPedidos(int idCliente)
        {

            var lista = new List<Entidades.Pedidos>();

            /*
SELECT 
NumeroPedido AS ID_PEDIDO,
CodigoCliente AS ID_CLIENTE,
Fecha AS FECHA_PEDIDO, 
SUBSTRING( Observacion, 4, 90 ) AS OBSERVACIONES,
SUBSTRING( Observacion, 0, 4 ) AS ESTADO_PEDIDO --PSEUDO COLUMNA CONTENIDA DENTRO DE LA COLUMNA OBSERVACION
FROM Pedidos
--WHERE SUBSTRING( Observacion, 0, 4 ) = '(P)'
WHERE CodigoCliente = 1000
ORDER BY FECHA_PEDIDO DESC

           
             */
            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT ");
            consultaSQL.Append("NumeroPedido AS ID_PEDIDO, ");
            consultaSQL.Append("CodigoCliente AS ID_CLIENTE, ");
            consultaSQL.Append("Fecha AS FECHA_PEDIDO, ");
            consultaSQL.Append("SUBSTRING( Observacion, 4, 90 ) AS OBSERVACIONES, ");
            consultaSQL.Append("SUBSTRING( Observacion, 0, 4 ) AS ESTADO_PEDIDO  ");
            consultaSQL.Append("FROM Pedidos ");
            consultaSQL.Append("WHERE CodigoCliente = @idClienteParametro ");
            consultaSQL.Append("ORDER BY FECHA_PEDIDO DESC ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                lista = connection.Query<Entidades.Pedidos>(consultaSQL.ToString(),

                     new
                     {
                         idClienteParametro = idCliente

                     }).ToList();
            }

            return lista;


        }





        public int VerCantidadProductosEnCarrito(int idPedido)
        {
            /*
   --CANTIDAD PUESTA EN EL CARRITO SEGUN ID PEDIDO
    
 SELECT COUNT(*) 
 FROM DetallesPedidos
 WHERE NumeroPedido = 1
                 */
            int cantidadProductosEnCarrito = 0;

            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT COUNT(*) ");
            consultaSQL.Append("FROM DetallesPedidos ");
            consultaSQL.Append("WHERE NumeroPedido = @idPedidoParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                cantidadProductosEnCarrito = connection.ExecuteScalar<int>(consultaSQL.ToString(),
                   new
                   {
                       idPedidoParametro = idPedido
                   });
            }

            return cantidadProductosEnCarrito;
        }

        public int AgregarAlCarrito(int idCliente, int idProducto)
        {
            /*
             SELECT COUNT (*) FROM Pedidos
             WHERE CodigoCliente  = 1000
             */


            /*
--VERIFICAR SI EL CLIENTE TIENE UN PEDIDO PENDIENTE
SELECT COUNT(*)
FROM Pedidos
WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1001


--VER NUMERO DE PEDIDO DEL PEDIDO PENDIENTE
SELECT 
NumeroPedido AS ID_PEDIDO,
CodigoCliente AS ID_CLIENTE,
SUBSTRING( Pedidos.Observacion, 0, 4 ) AS ESTADO_PEDIDO
FROM Pedidos
WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1001
             
             */
            int pedidosPendientes = 0;
            int idPedidoPendiente = 0;

            //VERIFICAR SI EL CLIENTE TIENE UN PEDIDO PENDIENTE
            StringBuilder consultaSQL1 = new StringBuilder();
            consultaSQL1.Append("SELECT COUNT(*) ");
            consultaSQL1.Append("FROM Pedidos ");
            consultaSQL1.Append("WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' ");
            consultaSQL1.Append("AND CodigoCliente = @idClienteParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                pedidosPendientes = connection.ExecuteScalar<int>(consultaSQL1.ToString(),
                   new
                   {
                       idClienteParametro = idCliente
                   });


            }

            if (pedidosPendientes > 0)
            {
                /*

                --VER NUMERO DE PEDIDO DEL PEDIDO PENDIENTE
                 SELECT NumeroPedido 
                 FROM Pedidos
                 WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
                 AND CodigoCliente = 1003
                                 */
                StringBuilder consultaSQL2 = new StringBuilder();
                consultaSQL2.Append("SELECT NumeroPedido  ");
                consultaSQL2.Append("FROM Pedidos ");
                consultaSQL2.Append("WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' ");
                consultaSQL2.Append("AND CodigoCliente = @idClienteParametro ");
           

                using (var connection = new SqlConnection(cadenaConexion))
                {
                    idPedidoPendiente = connection.ExecuteScalar<int>(consultaSQL2.ToString(),
                       new
                       {
                           idClienteParametro = idCliente
                       });


                }

            }

            int cantidadItemsCarrito;

            if (idPedidoPendiente == 0)
            {
                cantidadItemsCarrito = CrearPedido(idCliente, idProducto);
            }
            else if (idPedidoPendiente > 0)
            {
                cantidadItemsCarrito = EditarPedido(idCliente, idProducto, idPedidoPendiente);
            }
            else
            {

                cantidadItemsCarrito = -1;
            }


            return cantidadItemsCarrito;
        }



        public int CrearPedido(int idCliente, int idProducto)
        {

            /*
             
-- SI NO TIENE NINGUN PEDIDO ASIGNADO, CREAR UNO Y EL DETALLE DE ESE PEDIDO
INSERT INTO Pedidos(CodigoCliente, Fecha, Observacion)
VALUES (@idClienteParametro , GETDATE(), '')

--CREAR DETALLE DE PEDIDO DE ESE PEDIDO

INSERT INTO DetallesPedidos(NumeroPedido, 
							NumeroItem, 
							CodigoProducto, 
							Cantidad, 
							PrecioUnitario)
VALUES 						((SELECT MAX(NumeroPedido) FROM Pedidos),
							1, 
							@idProductoParametro, 
							1, 
							(SELECT PrecioUnitario FROM PRODUCTOS WHERE Codigo = @idProductoParametro));

             
             */

            int filasAfectadas = 0;
            int cantidadProductosEnCarrito = 0;
            SqlConnection conexion = new SqlConnection(cadenaConexion);


            conexion.Open();

            //como vamos a realizar dos inserciones debemos hacerlo con una transaccion
            var transaccion = conexion.BeginTransaction();


            try
            {
                /*
                 * 
                 * --AGREGAR UN PEDIDO NUEVO PARA EL CLIENTE CONECTADO
                 * 
                 INSERT INTO Pedidos(CodigoCliente, Fecha, Observacion)
                VALUES (@idClienteParametro , GETDATE(), '')
                                 */

                StringBuilder consultaSQL1 = new StringBuilder();
                consultaSQL1.Append("INSERT INTO Pedidos(CodigoCliente, Fecha, Observacion) ");
                consultaSQL1.Append("VALUES (@idClienteParametro , @fechaParametro, @observacionParametro) ");



                filasAfectadas = conexion.Execute(consultaSQL1.ToString(),
                       new
                       {
                           idClienteParametro = idCliente,
                           fechaParametro = DateTime.Now,
                           observacionParametro = "(P)"  

                       }
                       , transaction: transaccion);



                /*
                 INSERT INTO DetallesPedidos(NumeroPedido, 
							NumeroItem, 
							CodigoProducto, 
							Cantidad, 
							PrecioUnitario)
VALUES 						((SELECT MAX(NumeroPedido) FROM Pedidos),
							1, 
							@idProductoParametro, 
							1, 
							(SELECT PrecioUnitario FROM PRODUCTOS WHERE Codigo = @idProductoParametro));
                 */
                StringBuilder consultaSQL2 = new StringBuilder();
                consultaSQL2.Append("INSERT INTO DetallesPedidos(NumeroPedido, NumeroItem, CodigoProducto, Cantidad, PrecioUnitario) ");
                consultaSQL2.Append("VALUES( ");
                consultaSQL2.Append("(SELECT MAX(NumeroPedido) FROM Pedidos), ");
                consultaSQL2.Append("1, ");
                consultaSQL2.Append("@idProductoParametro,  ");
                consultaSQL2.Append("1, ");
                consultaSQL2.Append("(SELECT PrecioUnitario FROM PRODUCTOS WHERE Codigo = @idProductoParametro)); ");


                filasAfectadas = conexion.Execute(consultaSQL2.ToString(),
                       new
                       {
                           idProductoParametro = idProducto
                       },
                       transaction: transaccion);

                // si las operaciones relacionadas salieron bien, se realiza un commit
                transaccion.Commit();

                /*
                --VERIFICAR CUAL ES EL ULTIMO ID PEDIDO
                SELECT MAX(NumeroPedido)
                FROM Pedidos
                                */

                int idPedido = 0;

                StringBuilder consultaSQL3 = new StringBuilder();
                consultaSQL3.Append("SELECT MAX(NumeroPedido) ");
                consultaSQL3.Append("FROM Pedidos ");
 


                using (var connection = new SqlConnection(cadenaConexion))
                {
                    idPedido = connection.ExecuteScalar<int>(consultaSQL3.ToString());
                }


                cantidadProductosEnCarrito = VerCantidadProductosEnCarrito(idPedido);

            }
            catch (Exception ex)
            {
                // en caso que haya un error en el medio de la funcion
                //lanzamos codigo de error 0 y realizamos un rollback para que los datos
                //no se reflejen en la base de datos
                filasAfectadas = 0;
                transaccion.Rollback();

            }
            finally
            {
                //si el procedimiento salio bien o mal, siempre se debe cerrar la conexion
                conexion.Close();
            }

            // si el resultado de filasafectadas es 1 es porque salio OK
            return cantidadProductosEnCarrito;

        }

        public int EditarPedido(int idCliente, int idProducto, int idPedidoPendiente)
        {
            int proximoNroItem = 0;
           
            int filasAfectadas = 0;
            int cantidadProductosEnCarrito = 0;





            SqlConnection conexion = new SqlConnection(cadenaConexion);


            conexion.Open();

            //como vamos a realizar dos inserciones debemos hacerlo con una transaccion
            var transaccion = conexion.BeginTransaction();


            try
            {
              
                /*
           --ULTIMO NUMERO DE ITEM DEL PEDIDO DEL CLIENTE
                SELECT MAX(NumeroItem) 
                FROM DetallesPedidos
                WHERE NumeroPedido = 1
            */

                StringBuilder consultaSQL2 = new StringBuilder();
                consultaSQL2.Append("SELECT MAX(NumeroItem)  ");
                consultaSQL2.Append("FROM DetallesPedidos ");
                consultaSQL2.Append("WHERE NumeroPedido = @idPedidoPendienteParametro ");
  

                proximoNroItem = conexion.ExecuteScalar<int>(consultaSQL2.ToString(),
                    new { idPedidoPendienteParametro = idPedidoPendiente },
                    transaction: transaccion);

                proximoNroItem = proximoNroItem + 1;

                /*
            INSERT INTO DetallesPedidos
                           (NumeroPedido, 
                           NumeroItem, 
                           CodigoProducto, 
                           Cantidad, 
                           PrecioUnitario)
                   VALUES( @idPedidoParametro,
                           @proximoNroItem, 
                           @idProductoParametro, 
                           1, 
                           (SELECT PrecioUnitario FROM PRODUCTOS WHERE Codigo = @idProductoParametro));

            */
                StringBuilder consultaSQL3 = new StringBuilder();
                consultaSQL3.Append("INSERT INTO DetallesPedidos ");
                consultaSQL3.Append("(NumeroPedido, ");
                consultaSQL3.Append("NumeroItem,  ");
                consultaSQL3.Append("CodigoProducto, ");
                consultaSQL3.Append("Cantidad,  ");
                consultaSQL3.Append("PrecioUnitario) ");
                consultaSQL3.Append("VALUES( ");
                consultaSQL3.Append("@idPedidoPendienteParametro, ");
                consultaSQL3.Append("@proximoNroItem,  ");
                consultaSQL3.Append("@idProductoParametro,  ");
                consultaSQL3.Append("1, ");
                consultaSQL3.Append(" (SELECT PrecioUnitario FROM PRODUCTOS WHERE Codigo = @idProductoParametro)); ");



                filasAfectadas = conexion.Execute(consultaSQL3.ToString(),
                       new
                       {
                           idPedidoPendienteParametro = idPedidoPendiente,
                           proximoNroItem = proximoNroItem,
                           idProductoParametro = idProducto
                       }
                       , transaction: transaccion);


                // si las operaciones relacionadas salieron bien, se realiza un commit
                transaccion.Commit();

                cantidadProductosEnCarrito = VerCantidadProductosEnCarrito(idPedidoPendiente);

            }
            catch (Exception ex)
            {
                // en caso que haya un error en el medio de la funcion
                //lanzamos codigo de error 0 y realizamos un rollback para que los datos
                //no se reflejen en la base de datos
                filasAfectadas = 0;
                transaccion.Rollback();

            }
            finally
            {
                //si el procedimiento salio bien o mal, siempre se debe cerrar la conexion
                conexion.Close();
            }

            // si el resultado de filasafectadas es 1 es porque salio OK
            return cantidadProductosEnCarrito;
        }



        public List<Entidades.Join_PedidosClientes> ListarPedidosClientes()
        {

            var lista = new List<Entidades.Join_PedidosClientes>();

            /*
SELECT 
Pedidos.NumeroPedido AS ID_PEDIDO,
Pedidos.CodigoCliente AS ID_CLIENTE,
Clientes.RazonSocial AS RAZON_SOCIAL,
Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE,
Pedidos.Fecha AS FECHA_PEDIDO, 
--SUBSTRING( Pedidos.Observacion, 0, 4 ) AS ESTADO_PEDIDO,
CASE
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO'
    ELSE 'DESCONOCIDO'
END AS ESTADO_PEDIDO,
SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES
FROM Pedidos
INNER JOIN Clientes ON
Pedidos.CodigoCliente = Clientes.Codigo
INNER JOIN Usuarios ON
Clientes.IdUsuario = Usuarios.Id
ORDER BY FECHA_PEDIDO DESC
           
             */
            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Pedidos.NumeroPedido AS ID_PEDIDO, ");
            consultaSQL.Append("Pedidos.CodigoCliente AS ID_CLIENTE, ");
            consultaSQL.Append("Clientes.RazonSocial AS RAZON_SOCIAL, ");
            consultaSQL.Append("Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE, ");
            consultaSQL.Append("Pedidos.Fecha AS FECHA_PEDIDO, ");
            consultaSQL.Append("CASE ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE' ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO' ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO' ");
            consultaSQL.Append("ELSE 'DESCONOCIDO' ");
            consultaSQL.Append("END AS ESTADO_PEDIDO, ");
            consultaSQL.Append("SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES ");
            consultaSQL.Append("FROM Pedidos ");
            consultaSQL.Append("INNER JOIN Clientes ON ");
            consultaSQL.Append("Pedidos.CodigoCliente = Clientes.Codigo ");
            consultaSQL.Append("INNER JOIN Usuarios ON ");
            consultaSQL.Append("Clientes.IdUsuario = Usuarios.Id ");
            consultaSQL.Append("ORDER BY FECHA_PEDIDO DESC ");



            using (var connection = new SqlConnection(cadenaConexion))
            {
                lista = connection.Query<Entidades.Join_PedidosClientes>(consultaSQL.ToString()).ToList();
            }

            return lista;


        }

        public List<Entidades.DetallesPedidos> MostrarDetallePedidoDelCliente(int idPedido, int idCliente)
        {

            var lista = new List<Entidades.DetallesPedidos>();

            /*
SELECT 
Pedidos.NumeroPedido AS ID_PEDIDO,
Pedidos.CodigoCliente AS ID_CLIENTE,
Clientes.RazonSocial AS RAZON_SOCIAL,
Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE,
Pedidos.Fecha AS FECHA_PEDIDO, 
--SUBSTRING( Pedidos.Observacion, 0, 4 ) AS ESTADO_PEDIDO,
CASE
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO'
    ELSE 'DESCONOCIDO'
END AS ESTADO_PEDIDO,
SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES
FROM Pedidos
INNER JOIN Clientes ON
Pedidos.CodigoCliente = Clientes.Codigo
INNER JOIN Usuarios ON
Clientes.IdUsuario = Usuarios.Id
ORDER BY FECHA_PEDIDO DESC
           
             */
            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                lista = connection.Query<Entidades.DetallesPedidos>(consultaSQL.ToString()).ToList();
            }

            return lista;


        }


        public Entidades.Join_PedidosClientes ObtenerDetallePedidoCliente(int idPedido, int idCliente)
        {
            /*
SELECT 
Pedidos.NumeroPedido AS ID_PEDIDO,
Pedidos.CodigoCliente AS ID_CLIENTE,
Clientes.RazonSocial AS RAZON_SOCIAL,
Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE,
Pedidos.Fecha AS FECHA_PEDIDO, 
CASE
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO'
    WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO'
    ELSE 'DESCONOCIDO'
END AS ESTADO_PEDIDO,
SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES
FROM Pedidos
INNER JOIN Clientes ON
Pedidos.CodigoCliente = Clientes.Codigo
INNER JOIN Usuarios ON
Clientes.IdUsuario = Usuarios.Id
WHERE Pedidos.CodigoCliente = 1000
AND Pedidos.NumeroPedido = 1
             */
            StringBuilder consultaSQL = new StringBuilder();
            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Pedidos.NumeroPedido AS ID_PEDIDO, ");
            consultaSQL.Append("Pedidos.CodigoCliente AS ID_CLIENTE, ");
            consultaSQL.Append("Clientes.RazonSocial AS RAZON_SOCIAL, ");
            consultaSQL.Append("Usuarios.Apellido + ' ' + Usuarios.Nombre AS APELLIDO_NOMBRE, ");
            consultaSQL.Append("Pedidos.Fecha AS FECHA_PEDIDO,  ");
            consultaSQL.Append("CASE ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' THEN 'PENDIENTE' ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(C)' THEN 'CANCELADO' ");
            consultaSQL.Append("WHEN SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(F)' THEN 'FINALIZADO' ");
            consultaSQL.Append("ELSE 'DESCONOCIDO' ");
            consultaSQL.Append("END AS ESTADO_PEDIDO, ");
            consultaSQL.Append("SUBSTRING( Pedidos.Observacion, 4, 90 ) AS OBSERVACIONES ");
            consultaSQL.Append("FROM Pedidos ");
            consultaSQL.Append("INNER JOIN Clientes ON ");
            consultaSQL.Append("Pedidos.CodigoCliente = Clientes.Codigo ");
            consultaSQL.Append("INNER JOIN Usuarios ON ");
            consultaSQL.Append("Clientes.IdUsuario = Usuarios.Id ");
            consultaSQL.Append("WHERE Pedidos.CodigoCliente = @idClienteParametro ");
            consultaSQL.Append("AND Pedidos.NumeroPedido = @idPedidoParametro ");



            using (var connection = new SqlConnection(cadenaConexion))
            {
                var obj = connection.QuerySingleOrDefault<Entidades.Join_PedidosClientes>(consultaSQL.ToString(),
                    new { idClienteParametro = idCliente, idPedidoParametro = idPedido });


                return obj;
            }


        }

        public int FinalizarPedido(Entidades.Pedidos obj)
        {

            /*
UPDATE Pedidos
SET Fecha = GETDATE(), 
Observacion = Observacion
WHERE NumeroPedido = 1
             */
            int filasAfectadas = 0;

            StringBuilder consultaSQL = new StringBuilder();

            consultaSQL.Append("UPDATE Pedidos ");
            consultaSQL.Append("SET Fecha = GETDATE(), ");
            consultaSQL.Append("Observacion = ");
            consultaSQL.Append("@estadoPedidoParametro + @observacionesParametro ");
            consultaSQL.Append("WHERE NumeroPedido = @idPedidoParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                filasAfectadas = connection.Execute(consultaSQL.ToString(),
                   new
                   {
                       estadoPedidoParametro = obj.ESTADO_PEDIDO,
                       observacionesParametro = obj.OBSERVACIONES,
                       idPedidoParametro = obj.ID_PEDIDO
                   });


            }

            return filasAfectadas;
        }



        public int BuscarPedidoPendientePorCliente(int idCliente)
        {

            /*
--VERIFICAR SI EL CLIENTE TIENE UN PEDIDO PENDIENTE
SELECT COUNT(*)
FROM Pedidos
WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1001


--VER NUMERO DE PEDIDO DEL PEDIDO PENDIENTE
SELECT 
NumeroPedido AS ID_PEDIDO,
CodigoCliente AS ID_CLIENTE,
SUBSTRING( Pedidos.Observacion, 0, 4 ) AS ESTADO_PEDIDO
FROM Pedidos
WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1001
             
             */
            int pedidosPendientes = 0;
            int idPedidoPendiente = 0;

            //VERIFICAR SI EL CLIENTE TIENE UN PEDIDO PENDIENTE
            StringBuilder consultaSQL1 = new StringBuilder();
            consultaSQL1.Append("SELECT COUNT(*) ");
            consultaSQL1.Append("FROM Pedidos ");
            consultaSQL1.Append("WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' ");
            consultaSQL1.Append("AND CodigoCliente = @idClienteParametro ");


            using (var connection = new SqlConnection(cadenaConexion))
            {
                pedidosPendientes = connection.ExecuteScalar<int>(consultaSQL1.ToString(),
                   new
                   {
                       idClienteParametro = idCliente
                   });


            }

            if (pedidosPendientes > 0)
            {
                /*

                --VER NUMERO DE PEDIDO DEL PEDIDO PENDIENTE
                 SELECT NumeroPedido 
                 FROM Pedidos
                 WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)'
                 AND CodigoCliente = 1003
                                 */
                StringBuilder consultaSQL2 = new StringBuilder();
                consultaSQL2.Append("SELECT NumeroPedido  ");
                consultaSQL2.Append("FROM Pedidos ");
                consultaSQL2.Append("WHERE SUBSTRING( Pedidos.Observacion, 0, 4 ) = '(P)' ");
                consultaSQL2.Append("AND CodigoCliente = @idClienteParametro ");


                using (var connection = new SqlConnection(cadenaConexion))
                {
                    idPedidoPendiente = connection.ExecuteScalar<int>(consultaSQL2.ToString(),
                       new
                       {
                           idClienteParametro = idCliente
                       });


                }

            }


            return idPedidoPendiente;
        }

    }
}
