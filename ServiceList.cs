using System;
using System.Management;
using System.Collections.Generic;
namespace ServiciosServidores

//Esa clase permite obtener los serviciios que corren dentro de un servidor 
    //se hace uso de la WMI para obtener los servivios 
{
    class ServiceList
    {
        public void Servicios(string nameServer)
        {

            try
            {


                //Asigna el nombre de la cuenta que quieres buscar 

                String logon = "transnetwork";

                ConnectionOptions connectoptions = new ConnectionOptions();

                //connectoptions.Impersonation = ImpersonationLevel.Impersonate;  

                //Se agrega a un usuario o cuenta que tenga permisos de adminiostrador sobre los servidores . 

                connectoptions.Username = @"DOMINIO\usuario";

                connectoptions.Password = "*******";

                Console.WriteLine("\n\n\nNOMBRE DEL SERVIDOR " + nameServer); //For purposes debug 
                 ManagementScope scope = new ManagementScope(@"\\" + nameServer + @"\root\cimv2");

                scope.Options = connectoptions;

                //Define el query WMI para buscar dentro de los servidores 

                SelectQuery query = new SelectQuery("select name,State,StartMode,Description,startname from Win32_Service where startname like " + "'%" + logon + "%'");



                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {

                    ManagementObjectCollection collection = searcher.Get();
                    // Se recorren los objetos que se recuperaron de el metodo searcher.get 
                    foreach (ManagementObject service in collection)
                    {
                       // mediante la variable service , se accede a las propiedades de el objeto 

                        SqlConnections sql = new SqlConnections();
                        int exist = sql.consulta("SELECT COUNT (*) FROM DevServices WHERE nombreservidor='" + nameServer + "' AND nombreServicio='" + service["Name"] + "'");
                        
                        if (exist==0)
                        {
                            if (service["State"].ToString() == "Running")
                            {

                                string services = "INSERT INTO DevServices (nombreServidor,nombreServicio,estatusServicio,modoInicio,descripcionServicio,logonServicio,tipoServidor)"
                                + "VALUES ('" + nameServer + "','" + service["Name"] + "','" + service["State"] + "','" + service["StartMode"] + "','" + service["Description"] + "','" + service["startname"] + "','"+"Operativo"+"')";
                                sql.SqlInsert(services);
                            }
                            else
                            {

                                string services = "INSERT INTO DevServices (nombreServidor,nombreServicio,estatusServicio,modoInicio,descripcionServicio,logonServicio,tipoServidor)"
                                           + "VALUES ('" + nameServer + "','" + service["Name"] + "','" + service["State"] + "','" + service["StartMode"] + "','" + service["Description"] + "','" + service["startname"] + "','" + "Respaldo" + "')";
                                sql.SqlInsert(services);    
                            }
                        
                        }
                        else
                        {
                            if (service["State"].ToString() == "Running")
                            {
                                string update = "UPDATE DevServices SET estatusServicio='" + service["State"] + "',modoInicio='" + service["StartMode"] + "',tipoServidor='Operativo'"
                                + " WHERE nombreservidor = '" + nameServer + "' AND nombreServicio='" + service["Name"] + "'";
                                sql.ExecuteUpdate(update);
                            }

                            else
                            {
                                string update = "UPDATE DevServices SET estatusServicio='" + service["State"] + "',modoInicio='" + service["StartMode"] + "',tipoServidor='Respaldo'"
                               + " WHERE nombreservidor = '" + nameServer + "' AND nombreServicio='" + service["Name"] + "'";
                                sql.ExecuteUpdate(update);
                            }
                            

                        }
                        /*SqlConnections sql = new SqlConnections();//Se llama a la clase sqlConnections para ingresar un query de inster 
                         string services = "INSERT INTO DevServices (nombreServidor,nombreServicio,estatusServicio,modoInicio,descripcionServicio,LogonServicio)"
                          + "VALUES ('" + nameServer + "','" + service["Name"] + "','" + service["State"] + "','" + service["StartMode"] + "','" + service["Description"] + "','" + service["startname"] + "')";
                         sql.SqlInsert(services);
                        */
                       
                            /* string servicelogondetails = string.Format("Name: {0} , description:{1}  Logon : {2} ", service["Name"].ToString(), service["Description"].ToString(), service["startname"].ToString());
                        Console.WriteLine(servicelogondetails);*/
                        

                    }
                   
                }

            }//fin del TRY 

            catch (Exception ex)
            {

                //Log exception in exception log.

                //Logger.WriteEntry(ex.StackTrace);

                Console.WriteLine("Exception with the conecction to the server ");
                Console.WriteLine(ex);


            }

        }//FIN DE METODO SERVICIOS 
    }//FIN DE CLASE 
}//FIN DE NAMESPACE 
