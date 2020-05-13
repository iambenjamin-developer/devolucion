using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benjamin.PracticoMVC.Entidades
{
    public class Pedidos
    {

     
        public int ID_PEDIDO { get; set; }

        public int ID_CLIENTE { get; set; }

        public DateTime FECHA_PEDIDO { get; set; }

        public string OBSERVACIONES { get; set; }

        public string ESTADO_PEDIDO { get; set; }
        /*
         SUBSTRING( Observacion, 0, 5 ) ESTADO_PEDIDO
         CON ESTA FUNCION SQL EN LAS PRIMERAS 4 LETRAS DE LA COLUMNA OBSERVACION
         PODES CHEQUEAR EL ESTADO DEL PEDIDO

        ESTADOS DISPONIBLES:

        (P)  --> PENDIENTE
        (F)  --> FINALIZADO
        (C)  --> CANCELADO

        BUSCAR EN SQL SERVER

----------------------SELECT PEDIDOS----------------------------------------------------
SELECT 
NumeroPedido AS ID_PEDIDO,
CodigoCliente AS ID_CLIENTE,
Fecha AS FECHA_PEDIDO, 
SUBSTRING( Observacion, 4, 90 ) AS OBSERVACIONES,
SUBSTRING( Observacion, 0, 4 ) AS ESTADO_PEDIDO --PSEUDO COLUMNA CONTENIDA DENTRO DE LA COLUMNA OBSERVACION
FROM Pedidos
WHERE SUBSTRING( Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1000
ORDER BY FECHA_PEDIDO DESC
---------------------------------------------------------------

SELECT 
NumeroPedido,
CodigoCliente,
Fecha, 
SUBSTRING( Observacion, 4, 90 ) AS OBSERVACIONES,
SUBSTRING( Observacion, 0, 4 ) AS ESTADO_PEDIDO --PSEUDO COLUMNA CONTENIDA DENTRO DE LA COLUMNA OBSERVACION
FROM Pedidos
WHERE SUBSTRING( Observacion, 0, 4 ) = '(P)'
AND CodigoCliente = 1000



SELECT 
NumeroPedido,
CodigoCliente,
Fecha,
Observacion,
SUBSTRING( Observacion, 0, 5 ) ESTADO_PEDIDO
FROM Pedidos
WHERE CodigoCliente = 1000
AND  SUBSTRING( Observacion, 0, 5 ) = '(P)'

         */
    }
}
