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
using System.IO.Ports;
namespace cnc_v0
{
    public partial class Form1 : Form
    {
        string filename;
        string filename1;
        string line;
        int j = 0;
        int i = 0;
        float[] x;
        float[] y;
        float[] z;
        SerialPort serialport1 = new SerialPort();
        public Form1()
        {
            InitializeComponent();
            Getports();
        }
        void Getports()
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }
        private void open_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Opening ...";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {

                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                toolStripProgressBar1.MarqueeAnimationSpeed = 100;
                filename = openFile.FileName;
                commands.Clear();
                commands.Text = File.ReadAllText(filename);
            }
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.Value = 100;
            toolStripStatusLabel1.Text = "file opened ";
            operate.Enabled = true;
            save.Enabled = true;
        }

        private void operate_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Operating ...";
            i = 0;
            using (StreamReader reader = new StreamReader(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    j++;
                }
                x = new float[j];
                y = new float[j];
                z = new float[j];
            }
            
            using (StreamReader reader = new StreamReader(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(new char[] { ' ' });
                    x[i] = float.Parse(values[2]);
                    y[i] = float.Parse(values[4]);
                    z[i] = float.Parse(values[6]);
                    i++;
                }
            }
            for (int t = x.Length - 1; t > 0; t--)
            {
                x[t] -= x[t - 1];
                y[t] -= y[t - 1];
                z[t] -= z[t - 1];
            }
            commands.Clear();
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel1.Enabled = true;
            for (int k = 0; k <x.Length; k++)
            {
               
                commands.AppendText((k+1)+") "+"x = "+x[k]+"\t"+ "y = " + y[k] + "\t"+ "z= " + z[k]+"\r\n");
                toolStripProgressBar1.Value  = (int)((k /(float)x.Length)*100);
            }
            toolStripProgressBar1.Value= 100;
            //toolStripStatusLabel1.Text = "file operated ";
            operate.Enabled = false;
        }
        private void save_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "saving ...";
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                toolStripProgressBar1.MarqueeAnimationSpeed = 100;
                filename1 = saveFile.FileName;
                File.WriteAllText(filename1, commands.Text);
                
            }
            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            toolStripProgressBar1.Value = 100;
            //toolStripStatusLabel1.Text = "saved";
            connect.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;

        }
        private void connect_Click(object sender, EventArgs e)
        {
            serialport1.PortName = comboBox1.Text;
            serialport1.BaudRate = Convert.ToInt32(comboBox2.Text);
            serialport1.Open();
        }

        private void send_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "sending..";
            float PitchSize = (float)0.03;
            int[] steps = new int[x.Length*3];
            int a = 0;
            for(int k=0;k<x.Length*3;k+=3)
            {
                steps[k] = Convert.ToInt32(x[a] / PitchSize);
                steps[k+1] = Convert.ToInt32(y[a] / PitchSize);
                steps[k+2] = Convert.ToInt32(z[a] / PitchSize);
                a++;
                
            }
            int[] TotalSteps = new int[steps.Length + 3];
            int b = 0;
            for(int k=0; k<steps.Length+3;k+=2)
            {
                TotalSteps[k] = Math.Abs(steps[b]);
                if (steps[b] < 0)
                {
                    TotalSteps[k + 1] = 0;
                }
                else

                {
                    TotalSteps[k + 1] = 1;
                }
                b++;
            }
            






        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void commands_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            serialport1.Close();
        }
        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click_2(object sender, EventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
