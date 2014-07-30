using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using System.Text.RegularExpressions;
using System.Globalization;
using Windows.Storage;
using System.ComponentModel;


// ColorPickerApplication FOR WP 8.1
// Publisher: Lukács Péter Alex
//            https://github.com/LukacsP
//
//USAGE:
//
//          if you want to put this on your application, use the following code:
//          ColorPicker newCpicker = new ColorPicker();
                            //you can put here a simple (), with no starting color,
                            //HTML hex formatted Color code or RGB color code for starting color.
//          
//
//          Use SetNewSize = value to resize, it's the Width property of the new size what you want.
//
//          the ColorChanged Event notify you when the color is Changed, you can bind your own functions to handle it.       

namespace ColorPicker
{

    public partial class ColorPicker : UserControl
    {
        private int CD = 255;
        private bool NotOnChanging = false;
        private int OldR, OldG, OldB;
        //initialize
        public ColorPicker()
        {

            this.InitializeComponent();
            Red = 0.ToString();
            Green = 0.ToString();
            Blue = 0.ToString();
            SetNewSize = 800;
            NotOnChanging = true;
        }
        public ColorPicker(String Color)
        {
            this.InitializeComponent();
            HTMLColor = Color;
            SetNewSize = 800;
            NotOnChanging = true;
        }
        public ColorPicker(int R, int G, int B)
        {
            this.InitializeComponent();
            if (R >= 0 & R < 256) Red = R.ToString();
            if (G >= 0 & G < 256) Green = G.ToString();
            if (B >= 0 & B < 256) Blue = B.ToString();
            SetNewSize = 800;
            NotOnChanging = true;
        }
        //Resize
        public double SetNewSize
        {
            get { return Width; }
            set
            {
                // Calculate new Seizes
                Width = value;
                int Height = (int)(value / 2);
                parentbox.Width = value;
                parentbox.Height = Height;

                ColorImage.Width = Height;
                ColorImage.Height = Height;

                SLider.Width = Height;
                Canvas.SetTop(SLider, Height * 0.45);
                Canvas.SetLeft(SLider, Height * 0.05);

                Intensit_BG.Height = Height;
                Canvas.SetLeft(Intensit_BG, Height + 1);

                RedText.FontSize = Width / 20;
                textboxR.MinHeight = 0;
                textboxR.Height = Width / 18;
                textboxR.FontSize = Width / 33;
                textboxR.Width = Width / 8;
                Canvas.SetLeft(RedText, Width * 5 / 8);
                Canvas.SetTop(RedText, Width / 32);
                Canvas.SetLeft(textboxR, Width * 6.5 / 8);
                Canvas.SetTop(textboxR, Width / 40);

                GreenText.FontSize = Width / 20;
                textboxG.MinHeight = 0;
                textboxG.Height = Width / 18;
                textboxG.FontSize = Width / 33;
                textboxG.Width = Width / 8;
                Canvas.SetLeft(GreenText, Width * 5 / 8);
                Canvas.SetTop(GreenText, Width / 10);
                Canvas.SetLeft(textboxG, Width * 6.5 / 8);
                Canvas.SetTop(textboxG, Width / 10.666);

                BlueText.FontSize = Width / 20;
                textboxB.MinHeight = 0;
                textboxB.Height = Width / 18;
                textboxB.FontSize = Width / 33;
                textboxB.Width = Width / 8;
                Canvas.SetLeft(BlueText, Width * 5 / 8);
                Canvas.SetTop(BlueText, Width / 5.925925);
                Canvas.SetLeft(textboxB, Width * 6.5 / 8);
                Canvas.SetTop(textboxB, Width / 6.1538461);

                HexText.FontSize = Width / 20;
                Canvas.SetLeft(HexText, Width * 5 / 8);
                Canvas.SetTop(HexText, Width * 0.24375);
                TextboxH.MinHeight = 0;
                TextboxH.Height = Width / 18;
                TextboxH.FontSize = Width / 33;
                TextboxH.Height = Width * 0.05625;
                TextboxH.Width = Width * 0.188;
                Canvas.SetLeft(TextboxH, Width * 0.755);
                Canvas.SetTop(TextboxH, Width / 4.324324);

                ColorShowBox.Width = Width * 0.375;
                ColorShowBox.Height = Width * 0.15;
                Canvas.SetLeft(ColorShowBox, Width * 0.615);
                Canvas.SetTop(ColorShowBox, Width * 0.325);
            }

        }

        //Get/Set Shoulder
        private double ShoulderState
        {
            get { return SLider.Value; }
            set { SLider.Value = value; }
        }
        //get/Set Colors
        public string Red
        {
            get
            {
                return textboxR.Text;
            }
            set
            {
                textboxR.Text = Convert.ToInt32(value).ToString();
                this.setColorBox();
            }
        }
        public string Green
        {
            get
            {
                return textboxG.Text;
            }
            set
            {
                textboxG.Text = Convert.ToInt32(value).ToString();
                this.setColorBox();
            }
        }
        public string Blue
        {
            get
            {
                return textboxB.Text;
            }
            set
            {
                textboxB.Text = Convert.ToInt32(value).ToString();
                this.setColorBox();
            }
        }
        public string HTMLColor
        {
            get { return HexColor; }
            set
            {
                if (Regex.IsMatch(HTMLColor, @"^#(?:[0-9A-Fa-f]){3}$")) this.HexColor = "#" + value.Substring(1, 1) + value.Substring(1, 1) + value.Substring(2, 1) + value.Substring(2, 1) + value.Substring(3, 1) + value.Substring(3, 1);
                if (Regex.IsMatch(HTMLColor, @"^#(?:[0-9A-Fa-f]){6}$")) this.HexColor = value;
            }
        }
        private string HexColor
        {
            get { return TextboxH.Text; }
            set
            {
                TextboxH.Text = value;
            }
        }
        //SetDisplayedColor
        private void setColorBox()
        {
            this.ColorShowBox.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)Convert.ToInt32(Red), (byte)Convert.ToInt32(Green), (byte)Convert.ToInt32(Blue)));
            Color_Changed(HexColor);
        }


        protected void ColorImage_Tapped(object sender, TappedRoutedEventArgs e)
        {


            //precorrigate
            Point coords = e.GetPosition(this.ColorImage);
            int x = (int)(coords.X - this.ColorImage.Height / 2);
            int y = (int)(this.ColorImage.Height / 2 - coords.Y);



            //preset
            int r = (int)(this.ColorImage.ActualHeight / 2) - 1;
            float c = (float)Math.Sqrt((x * x + y * y));
            //R;G;B; points on the field
            double[] R = new double[2];
            double[] G = new double[2];
            double[] B = new double[2];
            int Rvalue; int Gvalue; int Bvalue;

            R[0] = -r / 2;
            R[1] = -(Math.Sqrt(3) * r / 2);

            G[0] = r;
            G[1] = 0;

            B[0] = -r / 2;
            B[1] = (Math.Sqrt(3) * r / 2);


            if (c >= r)
            {
                //out of circle sight
                return;
            }
            else
            {
                //in circle sight
                if (CaseNullHandler(x, c) == 1)
                {
                    //Green baseline                                    
                    Bvalue = GetIncrementValue(new double[2] { -G[0], -B[1] }, x, y, r);
                    Gvalue = GetValue(G, R, x, y);
                    Rvalue = GetIncrementValue(new double[2] { -G[0], -B[1] }, x, y, r);

                }
                else if (CaseNullHandler(x, c) == -0.5)
                {
                    if (CaseNullHandler(y, c) > 0)
                    {
                        //Blue baseline
                        Bvalue = GetValue(B, R, x, y);
                        Gvalue = GetIncrementValue(new double[2] { -B[0], -B[1] }, x, y, r);
                        Rvalue = GetIncrementValue(new double[2] { -B[0], -B[1] }, x, y, r);
                    }
                    else
                    {
                        //Red baseline                                
                        Bvalue = GetIncrementValue(new double[2] { -R[0], -R[1] }, x, y, r);
                        Gvalue = GetIncrementValue(new double[2] { -R[0], -R[1] }, x, y, r);
                        Rvalue = GetValue(R, G, x, y);

                    }
                }
                else
                {
                    if (CaseNullHandler(x, c) > -0.5)
                    {
                        if (CaseNullHandler(y, c) > 0)
                        {
                            //R increment
                            Bvalue = GetValue(B, G, x, y);
                            Gvalue = GetValue(G, B, x, y);
                            Rvalue = GetIncrementValue(R, x, y, r);
                        }
                        else
                        {
                            //B increment
                            Bvalue = GetIncrementValue(B, x, y, r);
                            Gvalue = GetValue(G, R, x, y);
                            Rvalue = GetValue(R, G, x, y);
                        }
                    }
                    else
                    {
                        //G increment
                        Bvalue = GetValue(B, R, x, y);
                        Gvalue = GetIncrementValue(G, x, y, r);
                        Rvalue = GetValue(R, B, x, y);
                    }
                }

            }
            OldR = Rvalue;
            OldG = Gvalue;
            OldB = Bvalue;

            NotOnChanging = false;
            this.Red = Convert.ToInt32(Rvalue * GetShoulderInt(Rvalue, sender, this.ShoulderState)).ToString();
            this.Green = Convert.ToInt32(Gvalue * GetShoulderInt(Gvalue, sender, this.ShoulderState)).ToString();
            this.Blue = Convert.ToInt32(Bvalue * GetShoulderInt(Bvalue, sender, this.ShoulderState)).ToString();
            NotOnChanging = true;
        }
        protected void ShoulderValue_Changed(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (NotOnChanging)
            {

                NotOnChanging = false;

                this.Red = (Convert.ToInt32(OldR * GetShoulderInt(OldR, sender, e.NewValue))).ToString();
                this.Green = (Convert.ToInt32(OldG * GetShoulderInt(OldG, sender, e.NewValue))).ToString();
                this.Blue = (Convert.ToInt32(OldB * GetShoulderInt(OldB, sender, e.NewValue))).ToString();
                this.HexColor = "#" + Convert.ToInt32(this.Red).ToString("X2") + Convert.ToInt32(this.Green).ToString("X2") + Convert.ToInt32(this.Blue).ToString("X2");
                NotOnChanging = true;
            }
        }
        protected void R_changed(object sender, TextChangedEventArgs e)
        {
            if (NotOnChanging)
            {
                NotOnChanging = false;


                if (this.Red != "")
                {
                    this.Red = IsValidHexint(this.Red, getFromHex(this.HexColor, 0)).ToString();
                    this.HexColor = "#" + Convert.ToInt32(this.Red).ToString("X2") + Convert.ToInt32(this.Green).ToString("X2") + Convert.ToInt32(this.Blue).ToString("X2");
                    this.ShoulderState = 100;
                }
                NotOnChanging = true;
            }
        }
        protected void G_changed(object sender, TextChangedEventArgs e)
        {
            if (NotOnChanging)
            {
                NotOnChanging = false;

                if (this.Green != "")
                {
                    this.Green = IsValidHexint(this.Green, getFromHex(this.HexColor, 2)).ToString();
                    this.HexColor = "#" + Convert.ToInt32(this.Red).ToString("X2") + Convert.ToInt32(this.Green).ToString("X2") + Convert.ToInt32(this.Blue).ToString("X2");
                    this.ShoulderState = 100;
                }
                NotOnChanging = true;
            }
        }
        protected void B_changed(object sender, TextChangedEventArgs e)
        {
            if (NotOnChanging)
            {
                NotOnChanging = false;

                if (this.Blue != "")
                {
                    this.Blue = IsValidHexint(this.Blue, getFromHex(this.HexColor, 4)).ToString();
                    this.HexColor = "#" + Convert.ToInt32(this.Red).ToString("X2") + Convert.ToInt32(this.Green).ToString("X2") + Convert.ToInt32(this.Blue).ToString("X2");
                    this.ShoulderState = 100;
                }
                NotOnChanging = true;
            }
        }
        protected void HEX_changed(object sender, TextChangedEventArgs e)
        {
            if (NotOnChanging)
            {
                NotOnChanging = false;

                if (Regex.IsMatch(this.HexColor, @"^#(?:[0-9A-Fa-f]){6}$"))
                {
                    this.Red = int.Parse(this.HexColor.Substring(1, 2), NumberStyles.AllowHexSpecifier).ToString();
                    this.Green = int.Parse(this.HexColor.Substring(3, 2), NumberStyles.AllowHexSpecifier).ToString();
                    this.Blue = int.Parse(this.HexColor.Substring(5, 2), NumberStyles.AllowHexSpecifier).ToString();
                }
                this.ShoulderState = 100;
                NotOnChanging = true;
            }
        }

        //Output Event
        public PropertyChangedEventHandler ColorChanged;
        protected void Color_Changed(string color)
        {
            if (this.ColorChanged != null) this.ColorChanged(this, new PropertyChangedEventArgs(color));
        }



        //get selected sequence from hex int
        public int getFromHex(string HexString, int start)
        {
            if (HexString.Substring(0, 1) == "#") { start += 1; }
            return int.Parse(HexString.Substring(start, 2), NumberStyles.AllowHexSpecifier);
        }
        //Get Shoulder modofier int
        private float GetShoulderInt(int c, object sender, double State)
        {

            if (State > 100)
            {
                return CaseNullHandler((int)(c + (CD - c) * (State - 100) / 100), (float)c);
            }
            else
            {
                return (float)State / 100;
            }
        }
        //check hex int validation, if invalid, return the old int
        private int IsValidHexint(string text, int Basevalue)
        {
            int C = new int();
            if (int.TryParse(text, out C))
            {
                if (C < 256 && C >= 0)
                {
                    return C;
                }
            }
            return Basevalue;
        }

        //math epressions
        private float CaseNullHandler(int a, float b)
        {
            if (b == 0) { return 0; }
            return (float)(a / b);

        }
        private int GetValue(double[] p1, double[] p2, int Xp, int Yp)
        {

            //get color value of p point, p1 component; P(x;y) between p1 and p2;
            return (int)(CD / ((Math.Sqrt(
                                    Math.Pow(p1[0] - Xp, 2)
                                  + Math.Pow(p1[1] - Yp, 2)
                                  )
                             + Math.Sqrt(
                                    Math.Pow(p2[0] - Xp, 2)
                                  + Math.Pow(p2[1] - Yp, 2)
                                  )
                        )
                       / Math.Sqrt(
                                    Math.Pow(p2[0] - Xp, 2)
                                  + Math.Pow(p2[1] - Yp, 2)
                                  )
                         ));
        }
        private int GetIncrementValue(double[] p1, int x, int y, int r)
        {
            double[] intersec;
            float dx, dy, A, B, C, det, t;

            dx = (float)(x - p1[0]);
            dy = (float)(y - p1[1]);

            A = dx * dx + dy * dy;
            B = (float)(2 * (dx * p1[0] + dy * p1[1]));
            C = (float)(p1[0] * p1[0] + p1[1] * p1[1] - r * r);

            det = B * B - 4 * A * C;

            t = (float)((-B + Math.Sqrt(det)) / (2 * A));
            intersec = new double[2] { (int)(p1[0] + t * dx), (int)(p1[1] + t * dy) };
            if (intersec[0] == p1[0] && intersec[1] == p1[1])
            {
                t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                intersec = new double[2] { (int)(p1[0] + t * dx), (int)(p1[1] + t * dy) };
            }
            return GetValue(p1, intersec, x, y);
        }
    }
}
