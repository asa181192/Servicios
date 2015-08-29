using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

//Esta clase permite obtener un listado de todos los servidores que se encuentran en el Acrtive directory 
//Mediante comando LDAP se necesita especificar los OU para filtrar la informacioin de las carpetas 
namespace ServiciosServidores
{
    class ServersDev
    {
      
        public static List<string> GetComputers()
        {
            List<string> ComputerNames = new List<string>();

            DirectoryEntry entry = new DirectoryEntry("LDAP://transnetwork.local/OU=Phoenix-DC,DC=transnetwork,DC=local");
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = ("(objectClass=computer)"); //se buscan solamente objetos de ltipo computadora / server 
            mySearcher.SizeLimit = int.MaxValue;
            mySearcher.PageSize = int.MaxValue;
            
            foreach (SearchResult resEnt in mySearcher.FindAll())
            {
                
                //"CN=SGSVG007DC"
                string ComputerName = resEnt.GetDirectoryEntry().Name;
                if (ComputerName.StartsWith("CN="))
                    ComputerName = ComputerName.Remove(0, "CN=".Length);
                ComputerNames.Add(ComputerName);
            }

          
            mySearcher.Dispose();
            entry.Dispose();
         // Console.ReadLine();
            return ComputerNames;
        }
    }

  
}
