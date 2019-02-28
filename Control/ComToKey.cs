using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace BarcodeScannerSerialPort.Control
{
    class ComToKey : IDisposable
    {
        private readonly SerialPort _port;

        public ComToKey(SerialPort port)
        {
            _port = port;
            _port.DataReceived += PortOnDataReceived;
        }

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            while (_port.BytesToRead > 0)
            {
                // PostKeys
                if (!_port.IsOpen)
                    return;
                var original = _port.ReadExisting();
                // Reformat string to fit SendKeys()
                var reformattedString = DefaultFormatter.Reformat(original);
                try
                {
                    SendKeys.SendWait(reformattedString);
                }
                // Handle exception caused if keys are sent to an application
                // not handling keys
                catch(InvalidOperationException)    
                {
                }
            }
        }

        public void Start()
        {
            if (!_port.IsOpen)
                _port.Open();
        }

        public void Stop()
        {
            if (_port.IsOpen)
                _port.Close();
        }

        public void Dispose()
        {
            if (_port.IsOpen)
                _port.Close();
            _port.DataReceived -= PortOnDataReceived;
        }
    }
}
