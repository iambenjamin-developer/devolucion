using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benjamin.PracticoMVC.Entidades
{
   public class Join_PedidosClientes
    {

        public int ID_PEDIDO { get; set; }

        public int ID_CLIENTE { get; set; }

        public string RAZON_SOCIAL { get; set; }

        public string APELLIDO_NOMBRE { get; set; }

        public DateTime FECHA_PEDIDO { get; set; }

        public string ESTADO_PEDIDO { get; set; }

        public string OBSERVACIONES { get; set; }

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


    }
}
