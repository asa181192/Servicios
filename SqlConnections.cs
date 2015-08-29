using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

//Esta clase permite  generar Inserts para guardar la informacion de servicios . 
namespace ServiciosServidores
{
    class SqlConnections
    {
        private string connect ;
        SqlConnection myConnection;
        SqlCommand cmd;
        private int added = 0;
        public SqlConnections() //Constructor genera variables para la coneccion 
        {
            connect = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
             myConnection = new SqlConnection(connect);
             cmd = new SqlCommand();
          
        }


        public void ServerRespaldo()
        {
            string update;
            List<string> operativos = new List<string> ();
            List<string> respaldo = new List<string> ();
            List<string> servoperativo = new List<string>();
            List<string> servrespaldo = new List<string>();
            cmd.Connection = myConnection;
            cmd.CommandText = "Query"; 
          
            try
            {
                myConnection.Open();
                SqlDataReader reader;
                 reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["estatusServicio"].ToString() == "Stopped")
                    {
                        respaldo.Add(reader["nombreServidor"].ToString());
                        servrespaldo.Add(reader["nombreServicio"].ToString());
                    }
                    else
                    {
                        operativos.Add(reader["nombreServidor"].ToString());
                        servoperativo.Add(reader["nombreServicio"].ToString());
                    }

                }
                myConnection.Close();


                try
                {
                    myConnection.Open();
                    for (int i = 0; i < (operativos.Count ); i++)
                    {
                        update = null;

                        for (int j = 1; j < (respaldo.Count); j++)
                        {
                            Console.WriteLine(j);
                            if (servoperativo[i] == servrespaldo[j])
                            {
                                update = update +"/"+ respaldo[j].ToString();

                                cmd.CommandText = "UPDATE DevServices SET servidorRespaldo='" + update + "' WHERE  nombreServidor='" + operativos[i] + "' AND nombreServicio='" + servoperativo[i] + "'";
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("update completo");
                            }



                        }//fin de for 

                    }//fin de for 
                    myConnection.Close();
                }//fin de try
                catch (Exception ex ) {
                    Console.WriteLine(ex);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
         
        }


        public void SqlInsert(string query) //Recive un parametro el cual es un query para ejecutar la insctruccion insert .
        {
            cmd.Connection = myConnection;
            cmd.CommandText = query;
            myConnection.Open();
            if(query!=null){ // ser verifica que el query ingresado no sea una variable nula 
                try
                {
                   
                    added = cmd.ExecuteNonQuery();

                    if (added > 0)
                    {
                        Console.WriteLine("Agregado");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
           
        }

        public void ExecuteUpdate (string query)
        {
            cmd.Connection = myConnection;
            cmd.CommandText = query;
            try
            {
                myConnection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar");
            }
            finally
            {
                myConnection.Close();
            }
            
         }

        public int consulta( string query)
        {
            cmd.Connection = myConnection;
            cmd.CommandText = query;
            myConnection.Open();
            int exist = (int)cmd.ExecuteScalar();
            if (exist>0)
            {
                myConnection.Close();
                return 1;
            }
            else
            {  
                myConnection.Close();
                return 0; 
            }
          

        }//fin de metodo consulta query 



    }
}
