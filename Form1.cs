using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Drawing.Drawing2D;

namespace SpeechReco
{
    public partial class Form1 : Form
    {
        System.Speech.Recognition.SpeechRecognitionEngine speech;
        System.Speech.Synthesis.SpeechSynthesizer synthesizer;
        System.Array colorsArray;
        KnownColor[] allColors;
        Choices colors;
        Point drawingPoint;

        public Form1()
        {
            InitializeComponent();
            getColors();
        }

        private void getColors()
        {
            colorsArray = Enum.GetValues(typeof(KnownColor));
            allColors = new KnownColor[colorsArray.Length];
            colors = new Choices();
        }

        private void SpeechStartup()
        {
            var CultureInfo = new System.Globalization.CultureInfo("en-US");
            speech = new SpeechRecognitionEngine(CultureInfo);

            Choices colors = new Choices();
            foreach (KnownColor item in colorsArray)
            {
                colors.Add(new string[] { item.ToString() });
            }
            

            speech.SetInputToDefaultAudioDevice();

            GrammarBuilder gb = new GrammarBuilder();
           
            gb.Append(colors);

            Grammar g = new Grammar(gb);

            speech.LoadGrammar(g);

            speech.SpeechRecognized +=
        new EventHandler<SpeechRecognizedEventArgs>(speech_SpeechRecognized);

            speech.SpeechRecognitionRejected += Speech_SpeechRecognitionRejected;

            speech.RecognizeAsync(RecognizeMode.Multiple);

        }

        private void Speech_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            foreach (var item in e.Result.Alternates.ToList())
            {
                lblStatus.Text += item.Homophones.ToString();
            }
        }

        private void speech_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int colorFailure = 0;
            
            Array.Copy(colorsArray, allColors, colorsArray.Length);
            listBox2.Items.Add(e.Result.Text);
            foreach(KnownColor kc in allColors)
            {
                if (kc.ToString().ToLower() == e.Result.Text.ToLower())
                {
                    try
                    {
                        Form1.ActiveForm.BackColor = Color.FromName(e.Result.Text.ToLowerInvariant());
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text += ex.ToString();
                    }
                   
                }
                listBox1.Items.Add(kc);
                
                
            }
            
        }

        private void btnSpeechStartup_Click(object sender, EventArgs e)
        {
            SpeechStartup();
            btnSpeechStartup.BackColor = Color.Green;

            if (btnSpeechStartup.BackColor == Color.Green)
            {
                btnSpeechStartup.BackColor = Color.Blue;
            }
           
                
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen p = new Pen(Color.Fuchsia);

            Point[] points = new Point[]
            {
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150),
                new Point(100, 150)
            };

            if (drawingPoint != null)
            {
                g.DrawPie(p, drawingPoint.X, drawingPoint.Y, 50, 50, 0, 360);
            }

            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox1.Text = e.X.ToString();
            textBox2.Text = e.Y.ToString();

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Form1.ActiveForm.CreateGraphics();
            Pen pen = new Pen(Color.Blue);
            pen.Width = 15;


            g.FillPie(Brushes.Violet, e.X - 20, e.Y - 20, 20, 20, 0, 360);
            
            Point point1 = new Point(1050, 150);
            Point point2 = new Point(1000, 400);
            g.DrawLine(pen, point1, point2);

            Point point3 = new Point(980, 171);
            Point point4 = new Point(845, 382);
            g.DrawLine(pen, point1, point2);


        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            synthesizer.Speak(listBox1.SelectedItem.ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    listBox2.SelectedIndex++;
                }
                catch (Exception ex)
                {
                    lblStatus.Text += ex.ToString();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text += ex.ToString();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    listBox2.SelectedIndex++;
                }
                catch (Exception ex)
                {
                    lblStatus.Text += ex.ToString();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text += ex.ToString();
            }
        }
    }
}
