using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Project_Fichier_DXF.Form3;
/**
 *  else if (entity is Circle)
                   {
                       Circle circle = (Circle)entity;
                       // Add code to export Circle entity
                   }
                   else if (entity is Arc)
                   {
                       Arc arc = (Arc)entity;
                       // Add code to export Arc entity
                   }
                    **/

namespace Project_Fichier_DXF
{
   public partial class Form2 : Form
   {
       private List<Circle> circles = new List<Circle>(); // List to hold circles
        private List<Arc> arcs = new List<Arc>(); // List to hold circles

        // un booleen qui indique si l'utilisateur est en train de dessiner une linge
        private bool drawing = false;
       // Represente le point de depart
       private Point startPoint;
       // Represente le nom du fichier DXF a exporter
       public string fileName = "C:\\Test.dxf";
       public Form2()
       {
           InitializeComponent();
           textBox2.Text = fileName;
           pic.MouseDown += new MouseEventHandler(pic_MouseDown_1);
           pic.MouseMove += new MouseEventHandler(pic_MouseMove_1);
           pic.MouseUp += new MouseEventHandler(pic_MouseUp_1);
           pic.Paint += new PaintEventHandler(pic_Paint_1);
       }

       private void pictureBox1_Click(object sender, EventArgs e)
       {

       }
       private void ExportToDXF(string fileName)
       {
           using (StreamWriter writer = new StreamWriter(fileName))
           {
               // Write DXF header
               writer.WriteLine("0");
               writer.WriteLine("SECTION");
               writer.WriteLine("2");
               writer.WriteLine("HEADER");
               writer.WriteLine("0");
               writer.WriteLine("ENDSEC");
               writer.WriteLine("0");
               writer.WriteLine("SECTION");
               writer.WriteLine("2");
               writer.WriteLine("TABLES");
               writer.WriteLine("0");
               writer.WriteLine("TABLE");
               writer.WriteLine("2");
               writer.WriteLine("LAYER");
               writer.WriteLine("70");
               writer.WriteLine("1");
               writer.WriteLine("0");
               writer.WriteLine("LAYER");
               writer.WriteLine("2");
               writer.WriteLine("Layer1");
               writer.WriteLine("70");
               writer.WriteLine("0");
               writer.WriteLine("62");
               writer.WriteLine("7");
               writer.WriteLine("6");
               writer.WriteLine("CONTINUOUS");
               writer.WriteLine("0");
               writer.WriteLine("ENDTAB");
               writer.WriteLine("0");
               writer.WriteLine("ENDSEC");
               writer.WriteLine("0");
               writer.WriteLine("SECTION");
               writer.WriteLine("2");
               writer.WriteLine("ENTITIES");

               // Write circles
               foreach (var circle in circles)
               { 
                       writer.WriteLine("0");
                       writer.WriteLine("CIRCLE");
                       writer.WriteLine("8");
                       writer.WriteLine(circle.Layer);
                       writer.WriteLine("10");
                       writer.WriteLine(circle.Center.X);
                       writer.WriteLine("20");
                       writer.WriteLine(circle.Center.Y);
                       writer.WriteLine("30");
                       writer.WriteLine(circle.ZCenter);
                       writer.WriteLine("40");
                       writer.WriteLine(circle.Radius); 
               }
               // Write Arc
                foreach (var arc in arcs)
                {
                    writer.WriteLine("0");
                    writer.WriteLine("ARC");
                    writer.WriteLine("8");
                    writer.WriteLine(arc.Layer);
                    writer.WriteLine("10");
                    writer.WriteLine(arc.Center.X);
                    writer.WriteLine("20");
                    writer.WriteLine(arc.Center.Y);
                    writer.WriteLine("30");
                    writer.WriteLine(arc.ZCenter);
                    writer.WriteLine("40");
                    writer.WriteLine(arc.Radius);
                    writer.WriteLine("50");
                    writer.WriteLine(arc.StartAngle);
                    writer.WriteLine("51");
                    writer.WriteLine(arc.EndAngle);
                }

                // End entities section
                writer.WriteLine("0");
               writer.WriteLine("ENDSEC"); //La fin de section
               writer.WriteLine("0");
               writer.WriteLine("EOF"); //La fin du fichier DXF
           }
       }


       // Circle class
       public class Circle
       {
           public PointF Center { get; set; }
           public float ZCenter { get; set; }
           public float Radius { get; set; }
           public string Layer { get; set; }
           public int Color { get; set; }
           public string LineType { get; set; }
           public float LineTypeScale { get; set; }

           public Circle(PointF center, float radius, string layer = "Layer1", int color = 0, string lineType = "", float lineTypeScale = 1.0f)
           {
               Center = center;
               Radius = radius;
               ZCenter = 0; // Default Z coordinate
               Layer = layer;
               Color = color;
               LineType = lineType;
               LineTypeScale = lineTypeScale;
           }
       }

       private void pic_MouseDown_1(object sender, MouseEventArgs e)
       {
           if (e.Button == MouseButtons.Left)
           {
               drawing = true;
               startPoint = e.Location; // Capture the starting point of the circle
           }
       }

       private void pic_MouseMove_1(object sender, MouseEventArgs e)
       {
           if (drawing)
           {
               pic.Invalidate(); // Refresh the PictureBox to redraw
           }

       }

       private void pic_Paint_1(object sender, PaintEventArgs e)
       {
            // Draw existing circles
            foreach (var circle in circles)
            {
                e.Graphics.DrawEllipse(Pens.Black, circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
            }

            // Draw existing arcs
            foreach (var arc in arcs)
            {
                using (Pen pen = new Pen(Color.Black))
                {
                    e.Graphics.DrawArc(pen, arc.Center.X - arc.Radius, arc.Center.Y - arc.Radius, arc.Radius * 2, arc.Radius * 2, arc.StartAngle, arc.EndAngle - arc.StartAngle);
                }
            }
            // Draw the currently being created shape
            if (drawing)
            {
                float radius = (float)Math.Sqrt(Math.Pow(pic.PointToClient(MousePosition).X - startPoint.X, 2) +
                                                 Math.Pow(pic.PointToClient(MousePosition).Y - startPoint.Y, 2));
                e.Graphics.DrawEllipse(Pens.Red, startPoint.X - radius, startPoint.Y - radius, radius * 2, radius * 2);
            }
        }
        // Arc class
    public class Arc
        {
            public PointF Center { get; set; }
            public float ZCenter { get; set; }
            public float Radius { get; set; }
            public float StartAngle { get; set; }
            public float EndAngle { get; set; }
            public string Layer { get; set; }

            public Arc(PointF center, float radius, float startAngle, float endAngle, string layer = "Layer1")
            {
                Center = center;
                Radius = radius;
                StartAngle = startAngle;
                EndAngle = endAngle;
                ZCenter = 0; // Default Z coordinate
                Layer = layer;
            }
        }

        private void pic_MouseUp_1(object sender, MouseEventArgs e)
       {
            if (drawing)
            {
                drawing = false;
                float radius = (float)Math.Sqrt(Math.Pow(e.X - startPoint.X, 2) + Math.Pow(e.Y - startPoint.Y, 2));

                // Ask the user to choose the type of entity
                var result = MessageBox.Show("Do you want to add this as a Circle? (Click 'No' to add as an Arc)", "Choose Entity Type", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Add the new circle to the list
                    circles.Add(new Circle(startPoint, radius));
                }
                else
                {
                    // For simplicity, we’ll assume an arc from 0 to 180 degrees
                    arcs.Add(new Arc(startPoint, radius, 0, 180));
                }

                pic.Invalidate(); // Refresh to show the new shape
            }

        }

       private void button5_Click(object sender, EventArgs e)
       {
           // Clear Pour supprimer tout les lignes
           circles.Clear();
           // Invalidate pour faire mise a jour de pictureBox
           pic.Invalidate();
       }

       private void button4_Click(object sender, EventArgs e)
       {
           DialogResult result = MessageBox.Show("Are you sure you want to exit  ?", "Exit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

           if (result == DialogResult.Yes)
           {
               Application.Exit();
           }
       }

       private void buttonExport_Click(object sender, EventArgs e)
       {
           if (circles.Count == 0)
           {
               MessageBox.Show("No circles to export.");
               return;
           }

           SaveFileDialog saveFileDialog = new SaveFileDialog
           {
               //Type de fichier
               Filter = "DXF files (*.dxf)|*.dxf",
               //Titre de fenter
               Title = "Save DXF File",
               FileName = textBox2.Text
           };

           if (saveFileDialog.ShowDialog() == DialogResult.OK)
           {
               fileName = saveFileDialog.FileName;
               ExportToDXF(fileName);
               MessageBox.Show("Export completed.");
           }
       }

       private void Form2_Load(object sender, EventArgs e)
       {

       }

       private void button1_Click(object sender, EventArgs e)
       {
            drawing = true;
            MessageBox.Show("Click on the PictureBox to start drawing circles.");
       }
   }
}

