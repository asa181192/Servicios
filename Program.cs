using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

//@autor Alfredo Santiago 
//27/07/2015 
//Allows to get services from serves in develop . 

//UPDATES 
/*
 * 31/07/15 save the information about services in database 
 */
namespace ServiciosServidores
{       
    class Program 
    {
        static void Main(string[] args)
        {          
           /*List<string> servers = ServersDev.GetComputers();
           foreach (string name in servers) {
               ServiceList myserv = new ServiceList();
               myserv.Servicios(name);
            }*/

            SqlConnections prueba = new SqlConnections();
            prueba.ServerRespaldo();
           Console.ReadKey();
            }
    }
}
