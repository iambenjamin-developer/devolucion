using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;

namespace Benjamin.PracticoMVC.AccesoDatos
{
   public class Test
    {


        string cadenaConexion = Conexiones.ObtenerCadenaConexion();

        public List<Entidades.Join_UsuariosRoles> ListarUsuariosRoles()
        {
            List<Entidades.Join_UsuariosRoles> listaUsuariosRoles = new List<Entidades.Join_UsuariosRoles>();

            StringBuilder consultaSQL = new StringBuilder();

            /*
             
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


             */

            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Usuarios.Id AS ID, ");
            consultaSQL.Append("Usuario AS USUARIO, ");
            consultaSQL.Append("Roles.Descripcion AS ROL, ");
            consultaSQL.Append("Nombre AS NOMBRES, ");
            consultaSQL.Append("Apellido AS APELLIDOS, ");
            consultaSQL.Append("FechaCreacion AS FECHA_ALTA, ");
            consultaSQL.Append("CASE ");
            consultaSQL.Append("WHEN Activo = 1 THEN 'ACTIVO' ");
            consultaSQL.Append("WHEN Activo = 0 THEN 'BAJA' ");
            consultaSQL.Append("ELSE 'DESCONOCIDO' ");
            consultaSQL.Append("END AS ESTADO ");
            consultaSQL.Append("FROM Usuarios ");
            consultaSQL.Append("INNER JOIN Roles ON  ");
            consultaSQL.Append("Usuarios.IdRol = Roles.Id ");

            using (var connection = new SqlConnection(cadenaConexion))
            {
                listaUsuariosRoles = connection.Query<Entidades.Join_UsuariosRoles>(consultaSQL.ToString()).ToList();
            }

            return listaUsuariosRoles;
        }


        public List<Entidades.Roles> ListaDeRoles()
        {
            List<Entidades.Roles> lista = new List<Entidades.Roles>();

            StringBuilder consultaSQL = new StringBuilder();

            /*
           
SELECT
Id,
Descripcion 
FROM ROLES
             */

            consultaSQL.Append("SELECT ");
            consultaSQL.Append("Id, ");
            consultaSQL.Append("Descripcion ");
            consultaSQL.Append("FROM ROLES ");



            using (var connection = new SqlConnection(cadenaConexion))
            {
                lista = connection.Query<Entidades.Roles>(consultaSQL.ToString()).ToList();
            }

            return lista;
        }




    }
}
