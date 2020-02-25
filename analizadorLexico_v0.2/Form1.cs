using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace analizadorLexico_v0._2
{
    public partial class Form1 : Form
    {
        int idx; //Indice de la palabra a evaluar
        string estado; //Estado actual del parse
        string token; //palabra acumulada
        int retornar;
        string[] palabrasReservadas = { "if", "while", "return", "else" };
        string[] tiposDatos = {"int","float", "string", "char", "double" };

        int[,] reglasTabla = new int[100,2];
        int sizeReglas = 0;

        int[,] matriz = new int[100, 100];
        int xMatriz = 0;
        int yMatriz = 0;

        public Form1()
        {
            InitializeComponent();
            cargarTabla();
            cargarReglas();
            
        }

        

        private void buttonLoading_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            String input = richTextBoxInput.Text; //Igualar lo que hay a la izquierda a una cadena
            richTextBoxOutput.Text = input; //La cadena de la izquierda se pasa al campo derecho
            richTextBoxOutput.Refresh();

            String pattern = @"\s"; //expresiones regulares para tomar como separaciones
            String[] elements = System.Text.RegularExpressions.Regex.Split(input, pattern); //Todo se guardara en un arreglo
            List<string> lista = new List<string>();

            int tam = elements.Length;
            int idx = 0;
            int column_size = 4;
            int raw_size = tam/column_size;

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
                
            dataGridView1.Columns.Add("cadena","Cadena");
            dataGridView1.Columns.Add("valor", "Valor");
            
            string[] fila = new string[4];
            foreach (var element in elements)
            {
                if (idx == 4)
                {
                    idx = 0;
                    //dataGridView1.Columns.Add("adfds", element);
                }
                int a = automataPrincipal(element);
                //MessageBox.Show(a.ToString());
                fila[idx] = element;
                fila[idx+1] = a.ToString();
                if (a == 20)
                {
                    MessageBox.Show("Error: " + a.ToString());
                    return;
                }
                    
                dataGridView1.Rows.Add(fila);
                //dataGridView1.Columns.GetFirstColumn().DataBindings = element;
                
                
                
            }
            // dataGridView1.AccessibilityObject
            dataGridView1.AutoGenerateColumns = true;
            
            
            
            dataGridView1.Refresh();
            
        }
        public int automataPrincipal(string cadena)
        {
            idx = 0; //Indice del caracter del string a
            estado = "0" ; // Estado actual en el parse
            retornar = -1;
            token = null; 
            while (idx!= cadena.Length) //Hasta que no hayamos terminado de evaluar toda la palabra
            {
                token = null;
                while (estado != "20") // Y que no haya habido un error 20
                {
                    token += cadena[idx]; //Agregamos a token el caracter a evaluar

                    if (estado == "0")
                        estado0(cadena);
                    else if (estado == "1")
                        estado1(cadena);
                    else if (estado == "2")
                        estado2(cadena);
                    else if (estado == "3")
                        estado3(cadena);
                    else if (estado == "4")
                        estado4(cadena);
                    else if (estado == "5")
                        estado5(cadena);
                    else if (estado == "6")
                        estado6(cadena);
                    else if (estado == "7")
                        estado7(cadena);
                    else if (estado == "8")
                        estado8(cadena);
                    else if (estado == "9")     //Palabra reservada
                        estado9(cadena);                         
                    else if (estado == "13")
                        estado13(cadena);
                    else if (estado == "13_2")
                        estado13_2(cadena);
                    else if (estado == "14")
                        estado14(cadena);
                    else if (estado == "14_2")
                        estado14_2(cadena);
                    else if (estado == "15")
                        estado15(cadena);                    
                    else if (estado == "15_3")
                        estado15_3(cadena);                    
                    else if (estado == "16")
                        estado16(cadena);
                    else if (estado == "16_2")
                        estado16_2(cadena);
                    else if (estado == "17")
                        estado17(cadena);                    
                    else if (estado == "17_3")
                        estado17_3(cadena);                    
                    else if (estado == "18")        //Fin cadena
                        estado18(cadena);
                    else if (estado == "20")        //Error
                        estado20(cadena);
                                        
                    idx++;
                    if (cadena.Length == idx)
                    {
                        if (estado == "1")
                        {
                            estado9(cadena);
                            estado0_1(cadena);
                        }

                        break;
                    }
                }
                return retornar;

            }


            return 20;
        }

        string estado0(string cadena)
        {
            if (Char.IsLetter(cadena[idx]) || cadena[idx]=='_'){         //Estado 1                                
                estado1(cadena);                                
            }                   
            else if (cadena[idx] == ';')                                  //estado 2
            {                               
                estado2(cadena);
                
            }
            else if (cadena[idx] == ',')                                  //estado 3
            {                                
                estado3(cadena);

            }
            else if (cadena[idx] == '(')                                //estado 4
            {
                estado4(cadena);
            }
            else if (cadena[idx] == ')')                                   //estado 5
            {
                estado5(cadena);
            }
            else if (cadena[idx] == '{')                                 //estado 6
            {                
                estado6(cadena);
            }
            else if (cadena[idx] == '}')                                 //estado 7
            {
                estado7(cadena);
            }
            else if (cadena[idx] == '=')                                  //estado 8
            {                
                estado8(cadena);
            }
                        
            else if (char.IsNumber(cadena[idx]))                         //Estado 13
            {
                estado13(cadena);
            }
            else if (cadena[idx] == '.')                                  //estado 13_2
            {
                estado13_2(cadena);

            }
            else if (cadena[idx] == '+')                                  //estado 14
            {                
                estado14(cadena);
            }
            else if (cadena[idx] == '-')                                  //estado 14_2
            {
                estado14_2(cadena);
            }
            else if (cadena[idx] == '&')                                  //estado 15
            {                
                estado15(cadena);
            }
            else if (cadena[idx] == '|')                                  //estado 15_3
            {
                estado15_3(cadena);
            }
            else if (cadena[idx] == '*')                                  //estado 16
            {                
                estado16(cadena);
            }
            else if (cadena[idx] == '/')                                  //estado 16_2
            {                
                estado16_2(cadena);
            }
            else if (cadena[idx] == '<')                                  //estado 17
            {                
                estado17(cadena);
            }
            else if (cadena[idx] == '>')                                  //estado 17_2
            {
                estado17_3(cadena);
            }
            else if (cadena[idx] == '$')                                  //estado 18
            {                
                estado18(cadena);
            }
            else
                return "0";
            return estado;
            
        }
        int estado0_1(string cadena)                    //Identificador
        {                       
            foreach (string x in tiposDatos)
            {
                if (cadena.Contains(x))
                {
                    retornar = 0;

                }
            }
            return 9;            
        }

        int estado1(string cadena)                                                              //Estado1
        {
            if (Char.IsLetterOrDigit(cadena[idx]) || cadena[idx] == '_')
            {
                estado = "1";
                retornar = 1;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }           
            
            return retornar;
        }


        int estado2(string cadena)
        {   
            if (cadena[idx] == ';')                          //Estado2
            {
                retornar = 2;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado3(string cadena)
        {
            if (cadena[idx] == ',')                          //Estado3
            {

                retornar = 3;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado4(string cadena)
        {

            if (cadena[idx] == '(')                          //Estado4
            {
                retornar = 4;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado5(string cadena)
        {

            if (cadena[idx] == ')')                          //Estado5
            {
                retornar = 5;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado6(string cadena)
        {
            if (cadena[idx] == '{')                          //Estado6
            {

                retornar = 6;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado7(string cadena)
        {
            if (cadena[idx] == '}')                          //Estado7
            {
                retornar = 7;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado8(string cadena)
        {
            if (token == "=")                           //Estado8
            {
                retornar = 8;
            }                                
            else if (token == "==")                          
            {
                retornar = 22;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado9(string cadena)       //Estado9 - Palabra Reservada
        {
            foreach (string x in palabrasReservadas)
            {
                if (cadena.Contains(x))
                {
                    if (x == "if")
                    {
                        retornar = 9;
                    }
                    else if (x == "while")
                    {
                        retornar = 10;
                    }
                    else if (x == "return")
                    {
                        retornar = 11;
                    }
                    else if (x == "else")
                    {
                        retornar = 12;
                    }

                }
            }
            return 9;

        }
   
        int estado13(string cadena)
        {
            if (char.IsNumber(cadena[idx]))         //Estado13
            {
                estado = "13";
                retornar = 13;
            }
            if (cadena[idx] == '.')
            {
                estado = "13_2";
                retornar = 13;
            }
            return retornar;

        }
        int estado13_2(string cadena)
        {
            if (cadena[idx] == '.' || char.IsNumber(cadena[idx]))                 //Estado13_2
            {
                estado = "13_2";
                retornar = 13;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado14(string cadena)
        {
            if (cadena[idx] == '+')          //Estado14
            {
                retornar = 14;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;

 
        }
        int estado14_2(string cadena)
        {
            if (cadena[idx] == '-')          //Estado14
            {
                retornar = 14;
            }
            else if (cadena[idx] == '.' || char.IsNumber(cadena[idx]))          //Estado14_2
            {
                estado = "13";
                retornar = 13;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }

        int estado15(string cadena)
        {
            if (token == "&")         //Estado15
            {
                estado = "15";
                retornar = 20;
            }
            else if(token=="&&")
            {
                estado = "20";
                retornar = 15;
            }
            return retornar;
        }
        int estado15_3(string cadena)
        {
            if (token == "|")                    //Estado  15_3
            {
                estado = "15_3";
                retornar = 20;
            }
            else if (token == "||")     
            {
                retornar = 15;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado16(string cadena)
        {

            if (cadena[idx] == '*')                          //Estado16
            {
                retornar = 16;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado16_2(string cadena)
        {

            if (cadena[idx] == '/')                          //Estado16_2
            {
                retornar = 16;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado17(string cadena)
        {

            if (token == "<")                          //Estado17
            {
                estado = "  17";
                retornar = 17;
            }
            else if(token =="<=")
            {
                retornar = 17;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado17_3(string cadena)
        {

            if (token == ">")                          //Estado17_3
            {
                estado = "17_3";
                retornar = 17;
            }
            else if (token == ">=")
            {
                retornar = 17;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado18(string cadena)
        {

            if (token == "$")                          //Estado18
            {
                retornar = 18;
            }
            else
            {
                estado = "20";
                retornar = 20;
            }
            return retornar;
        }
        int estado20(string cadena)             //Estado 20 error
        {

                estado = "20";
                retornar = 20;

            return retornar;
        }

        /******Lectura Escritura archivos********/
        public void cargarTabla()
        {
            string fileName = "GR2slrTable.txt";

            string[] lineas = System.IO.File.ReadAllLines(@fileName);
            foreach (string linea in lineas)
            {
                string[] sep = linea.Split('\t');
                string[] fila = new string[100];
                int i = 0;
                
                foreach (string c in sep)
                {
                    //MessageBox.Show(c.ToString());
                    fila[i] = c;
                    i++;
                    richTextBox1.Text += c;
                    richTextBox1.Text += "**";
                }
                richTextBox1.Text += '\n';

                int numColumnas = 0;
                for(; numColumnas < sep.Count();)
                {
                    numColumnas++;
                    
                }
                
            }
        }
        public void cargarReglas()
        {
            
            string fileName = "GR2slrRulesId.txt";

            string[] lineas = System.IO.File.ReadAllLines(@fileName);
            foreach (string linea in lineas)
            {
                string[] sep = linea.Split('\t');
                string[] fila = new string[100];
                int i = 0;

                foreach (string c in sep)
                {
                    //MessageBox.Show(c.ToString());
                    fila[i] = c;
                    i++;
                    richTextBox2.Text += c;
                    richTextBox2.Text += "------";
                }
                richTextBox2.Text += '\n';

                int numColumnas = 0;
                for (; numColumnas < sep.Count();)
                {
                    numColumnas++;

                }

            }
           
            
        }
    }

}
