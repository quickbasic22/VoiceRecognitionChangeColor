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
using System.Globalization;
using System.Collections;

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
        int buttonClickCount = 0;
        int lineClick = 0;
        Point lineStart;
        Point lineEnd;

        public Form1()
        {
            InitializeComponent();
            getColors(); 
            SpeechStartup();
            
        }

        
        private void speakAll()
        {
            synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            CultureInfo culture = new CultureInfo("en-US");
            synthesizer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female, System.Speech.Synthesis.VoiceAge.Teen, 1, culture);
          
            List<string> lis = new List<string>(174);
            foreach (var it in colorsArray)
            {
                lis.Add(colorsArray.ToString());
            }
            // System.Threading.Thread.Sleep(20000);
            Random random = new Random();
            int item = 0;
            int count = lis.Count;
            for (int i = 0; i <= count; i++)
            {
                item = random.Next(0, lis.Count);
                synthesizer.Speak(lis[item]);
                lis.Remove(lis[item]);
            }
            
            

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

            speech.EmulateRecognize("Blue");

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
                        lblStatus.Text += string.Concat(ex.ToString(), colorFailure);
                    }
                   
                }
                listBox1.Items.Add(kc);
                
                
            }
            
        }

        private void btnSpeechStartup_Click(object sender, EventArgs e)
        {
            SpeechStartup();
            btnSpeechStartup.BackColor = Color.Green;

            buttonClickCount++;
            if (buttonClickCount % 2 == 0)
            {
                btnSpeechStartup.BackColor = Color.Blue;
            }
            else
            {
                btnSpeechStartup.BackColor = Color.Green;
            }
                            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
          
            

            Pen p = new Pen(Color.Purple);
            p.Width = 5;

            Point[] points = new Point[]
            {
                new Point(1040, 373),
                new Point(1041, 197),
                new Point(1040, 373),    
                new Point(1047, 552),
                new Point(1040, 373),
                new Point(861, 373),
                new Point(1040, 373),
                new Point(1339, 373),
                new Point(1040, 373),
                new Point(1500, 373)
            };

            //g.DrawLine(p, points[0], points[1]);
            //g.DrawLine(p, points[2], points[3]);
            //g.DrawLine(p, points[4], points[5]);
            //g.DrawLine(p, points[6], points[7]);
            //g.DrawLine(p, points[8], points[9]);


            //if (drawingPoint != null)
            //{
            //    g.DrawPie(p, drawingPoint.X, drawingPoint.Y, 50, 50, 0, 360);
            //}


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


            //g.FillPie(Brushes.Violet, e.X - 20, e.Y - 20, 20, 20, 0, 360);

            if (lineClick % 2 == 1)
            {
                
                lineEnd = DrawLineWithDegrees(double.Parse(textBoxDegrees.Text.ToString()), double.Parse(textBoxLength.Text.ToString()), lineStart);              
                g.DrawLine(pen, lineStart, lineEnd);
                lineClick = 0;
            }
            else
            {
                lineStart = new Point(e.X, e.Y);
                lineClick++;
            }
            



        }

        private Point DrawLineWithDegrees(double lineLength, double degrees, Point point)
        {
            lineLength = 5.00d;
            int lineX = point.X;
            int lineY = point.Y;
            double linePointX = double.Parse(lineX.ToString());
            double linePointY = double.Parse(lineY.ToString());
            double calcX = Math.Round((lineLength * Math.Cos(degrees)) + lineX, 0);
            double calcY = Math.Round((lineLength * Math.Sin(degrees)) + lineY, 0);
            
            Int32 calcIntX = Int32.Parse(calcX.ToString());
            Int32 calcIntY = Int32.Parse(calcY.ToString());

            lineEnd = new Point(calcIntX, calcIntY);

            return lineEnd;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            CultureInfo culture = new CultureInfo("en-US");
            synthesizer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female, System.Speech.Synthesis.VoiceAge.Teen, 1, culture);
            synthesizer.Speak(listBox1.SelectedItem.ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (listBox2.Items.Count - 1);
            listBox2.SelectedIndex = selected;
            listBox2.SelectionMode = SelectionMode.One;

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (listBox2.Items.Count - 1);
            listBox2.SelectedIndex = selected;
        }

        private void btnClearScreen_Click(object sender, EventArgs e)
        {
            Graphics g = Form1.ActiveForm.CreateGraphics();

            g.Clear(Form1.ActiveForm.BackColor);
        }

        private void textBoxDegrees_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
