using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;
using BarcodeScannerSerialPort.Control;
using System.IO;


namespace BarcodeScannerSerialPort
{


    public partial class FrmMain : Form
    {
        private bool _isListening;
        private readonly int[] _bauds = new[] { 4800, 9600, 19200, 38400, 57600, 115200 };
        private ComToKey _transfer;
        
        public string[] portas = SerialPort.GetPortNames();

        


        public FrmMain()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += OnKeyDown;
            //comboBox1.Items.AddRange(SerialPort.GetPortNames()); da erro porque nao tem porta serial
            //if (comboBox1.Items.Count > 0)
            //{
            //    comboBox1.SelectedIndex = 0;
            //}
            


            //chamando o metodo quando a janela fecha
            this.FormClosing += this.Form_Closing;
            //LendoIni();
            // NOTA :  melhorar o metodo CriandoIni()
            //         para escrever Mais Tags 

        }
        // metodo para fechar a janela
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {

                CriandoIniTeste();
            }
            catch (NullReferenceException ex11)
            {
                MessageBox.Show(ex11.Message,"");
            }

        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Return)
            {
                Debug.Print("Enter received");
            }
            //keyEventArgs.SuppressKeyPress = true;
            //keyEventArgs.Handled = true;
        }

        private void FrmMainLoad(object sender, EventArgs e)
        {

            
                FillPortList();
                FillBaudList();
            

                
        }

        private void FillBaudList()
        {
            

                foreach (var baud in _bauds)
                {
                    cmbBaud.Items.Add(baud);
                }
                if (cmbBaud.Items.Count > 0)
                    cmbBaud.SelectedItem = 9600;
            
            
         

        }

        private void FillPortList()
        {
            
            
                cmbPort.Sorted = true;
                var s = SerialPort.GetPortNames();

                foreach (var s1 in s)
                {
                    cmbPort.Items.Add(s1);
                }
                if (cmbPort.Items.Count > 0)
                {
                    cmbPort.SelectedIndex = 0;

                    if (!_isListening)
                    {
                        _isListening = true;
                        btnTransfer.Text = "Desativar";
                        StartListening();
                    }
                    else
                    {
                        _isListening = false;
                        btnTransfer.Text = "Ativar";
                        StopListening();
                    }

                }
                else
                {
                    cmbPort.Enabled = false;
                }
                

             
            
        }

        private void BtnTransferClick(object sender, EventArgs e)
        {
            if (cmbPort.Items.Count > 0)
            {
                if (e is MouseEventArgs)
                {
                    if (!_isListening)
                    {
                        _isListening = true;
                        btnTransfer.Text = "Desativar";
                        StartListening();
                    }
                    else
                    {
                        _isListening = false;
                        btnTransfer.Text = "Ativar";
                        StopListening();
                    }
                }
            }
            else
            {
                MessageBox.Show("Não existe porta serial \noperação cancelada","Porta Serial");
                
                Application.Exit();
            }
        }

        private void StopListening()
        {
            _transfer.Stop();
            _transfer.Dispose();
            SetInterfaceEnable(true);
        }

        private void StartListening()
        {
            
                if (_transfer != null)
                    _transfer.Dispose();

                SetInterfaceEnable(false);
                var pName = cmbPort.SelectedItem.ToString();
                int pBaud;
                int.TryParse(cmbBaud.SelectedItem.ToString(), out pBaud);
                _transfer = new ComToKey(new SerialPort(pName, pBaud, Parity.None, 8, StopBits.One));
                _transfer.Start();
            
        }

        private void SetInterfaceEnable(bool b)
        {
            cmbBaud.Enabled = b;
            cmbPort.Enabled = b;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (!serialPort1.IsOpen)
            {

                serialPort1.PortName = cmbPort.SelectedItem.ToString();
                serialPort1.Open();
                serialPort1.WriteLine(label1.Text);
                serialPort1.Close();
            }

            
            //START HERE !!

            
            // ENDS HERE !!
        
        
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        public void CriandoIniTeste()
        {
            if (cmbPort.Items.Count > 0)
            {

                string port = "Porta=";
                string Baud = "Baud=";
                string portValue = portas[cmbPort.SelectedIndex];
                string baudValue = cmbBaud.SelectedItem.ToString();
                string[] Option_ini = { port + cmbPort.SelectedIndex, Baud + cmbBaud.SelectedItem };

                FileInfo SerialSCE = new FileInfo("SerialSCE.ini");
                using (FileStream file = SerialSCE.Create())
                {

                }
                File.WriteAllLines("SerialSCE.ini", Option_ini);
            }
            else
            {
                MessageBox.Show("Não foi possivel criar o Arquivo de Configurações", "Porta Serial");
            }

        }

        /*
        public void CriandoIni()
        {

            string[] Option_ini = { "Porta=" + portas[cmbPort.SelectedIndex], "Baud=" + cmbBaud.SelectedItem };
            //if(!File.Exists("SerialSCE.ini"))
            //{
            
            FileInfo SerialSCE = new FileInfo("SerialSCE.ini");
            using (FileStream fs = SerialSCE.Create())
            {
            }
            File.WriteAllLines("SerialSCE.ini", Option_ini);
            //}
            
        }
        
         */
        private void LendoIni()
        {
            
            string[] Option_value = {"Porta=","Baud=" }; 
            FileStream fs = File.Open("SerialSCE.ini", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string valueLine = sr.ReadLine();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("SerialSCE.ini"))
                {
                    foreach (string line in Option_value)
                    {
                        file.WriteLine(line);
                        
                    }
                }
            }
            //Console.Read();
            sr.Close();
            fs.Close();

        }

        private void cmbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        }

}

