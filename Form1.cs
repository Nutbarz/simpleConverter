using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //private void btnConvert_Click(object sender, EventArgs e)
        //{
        //    string input = txtInput.Text;
        //    txtOutput.Text = "";
        //    var matches = new List<string>();
        //    var pattern = @"[XY][^\s]*";
        //    foreach (Match match in Regex.Matches(input, pattern))
        //    {
        //        matches.Add(match.Value);
        //        txtOutput.Text += match.Value.ToString();
        //        txtOutput.Text += "\r\n";
        //    }
        //}
        private void btnConvert_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text;
            txtOutput.Text = "";
            txtOutput.Text += "WPS(60,33,35,12,0.40,750) ;1MM SETTINGS\r\n";
            txtOutput.Text += "SERVO(575,216,220,100,100)\r\n";
            txtOutput.Text += "G0G54G90\r\n";
            txtOutput.Text += "R70=0.025 ;M50 RETRACT\r\n";
            txtOutput.Text += "R71=-3.75 ;M50 ZDRILL\r\n";
            txtOutput.Text += "R72=60    ;M50 POWER SETTING\r\n";
            txtOutput.Text += ";R73=1    ;M49 RETRACT\r\n";
            txtOutput.Text += ";R74=1    ;M49 ZDRILL\r\n";
            txtOutput.Text += ";R75=1    ;M49 POWER SETTING\r\n";
            txtOutput.Text += "/M140\r\n";
            txtOutput.Text += "/M141\r\n";
            txtOutput.Text += "/M142\r\n";
            txtOutput.Text += "/M143\r\n";
            txtOutput.Text += "X0Y0\r\n";
            txtOutput.Text += "W0\r\n";
            List<string> occurrences = new List<string>();
            int captureCount = 1;
            int ignoreNext = 2;
            bool captured = false;
            bool isCapturing = false;
            string currentCapture = "";

            foreach (char c in input)
            {
                if (c == '\n')
                {
                    //end step
                    if (captured == true){
                        txtOutput.Text += " ;";
                        txtOutput.Text += captureCount.ToString();
                        txtOutput.Text += "\r\n";
                        txtOutput.Text += "M01\r\n";
                        txtOutput.Text += "/M70\r\n";
                        txtOutput.Text += "/M142\r\n";
                        txtOutput.Text += "/M50(R70,R71,R72)\r\n";
                        txtOutput.Text += ";/M49(R73,R74,R75)\r\n";
                        captureCount++;
                        captured = false;
                    }
                }

                if ((c == 'X' || c == 'Y') && !isCapturing)
                {
                    if (ignoreNext > 0)
                    {
                        ignoreNext--;
                        continue;
                    }
                    isCapturing = true;
                    currentCapture += c;
                }
                else if (isCapturing)
                {
                    if (char.IsWhiteSpace(c))
                    {
                        occurrences.Add(currentCapture);
                        txtOutput.Text += currentCapture;
                        txtOutput.Text += " ";
                        currentCapture = "";
                        isCapturing = false;
                        captured = true;
                    }
                    else
                    {
                        currentCapture += c;
                    }
                }
            }
            captureCount--;
            string lastStep = "X0 Y0  ;" + captureCount.ToString() + "\r\nM01\r\n/M70\r\n/M142\r\n/M50(R70,R71,R72)\r\n;/M49(R73,R74,R75)";
            txtOutput.Text = txtOutput.Text.Replace(lastStep, "M30");
            lbCount.Text = (captureCount-1).ToString();
            Console.WriteLine(lastStep);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                txtInput.Text = Clipboard.GetText();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
        }
    }
}

