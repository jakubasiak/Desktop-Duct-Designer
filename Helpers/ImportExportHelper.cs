using HVAC.Elements;
using HVACDesigner.ViewModels;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace HVACDesigner.Helpers
{
    public static class ImportExportHelper
    {
        public static bool SaveDuctSystemToXML(string filePath, DuctSystem ductSystem)
        {
            try
            {
                string xml = "";
                using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(DuctSystem), new Type[] { typeof(BaseDuct), typeof(RoundDuct), typeof(RectangularDuct), typeof(LocalLoss), typeof(RoundDuctViewModel), typeof(RectangularDuctViewModel) });
                    xmlSerializer.Serialize(stringWriter, ductSystem);

                    xml = stringWriter.ToString();
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.Write(xml);
                }

                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show("Saving to file has been failed", "Saving failed", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError("Błąd przy zapisie do pliku XML", e);
                return false;
            }
        }
        public static DuctSystem ReadDuctSystemFromXML(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(DuctSystem), new Type[] { typeof(BaseDuct), typeof(RoundDuct), typeof(RectangularDuct), typeof(LocalLoss), typeof(RoundDuctViewModel), typeof(RectangularDuctViewModel) });
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    StreamReader stream = new StreamReader(fs, Encoding.UTF8);
                    var ductSystem = (DuctSystem)serializer.Deserialize(new XmlTextReader(stream));
                    return ductSystem as DuctSystem;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Loading from file has been failed", "Loading failed", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError("Błąd przy odczycie zapisanego pliku XML", e);
                return null;
            }

        }
        public static bool ExportDuctSystemToCSV(string filePath, DuctSystem ductSystem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Size [mm]");
            sb.Append("\t");
            sb.Append("Hydraulic Diameter [mm]");
            sb.Append("\t");
            sb.Append("Length [m]");
            sb.Append("\t");
            sb.Append("Relative Roughness [mm]");
            sb.Append("\t");
            sb.Append("Air Flow [m3/h]");
            sb.Append("\t");
            sb.Append("Velocity [m/s]");
            sb.Append("\t");
            sb.Append("Dynamic Pressure [Pa]");
            sb.Append("\t");
            sb.Append("Reynolds Number [-]");
            sb.Append("\t");
            sb.Append("Approximation type");
            sb.Append("\t");
            sb.Append("Friction Factor [-]");
            sb.Append("\t");
            sb.Append("Friction Loss [Pa/m]");
            sb.Append("\t");
            sb.Append("Local Pressure Drop [Pa]");
            sb.Append("\t");
            sb.Append("Linear Pressure Drop [Pa]");
            sb.Append("\t");
            sb.Append("Total Pressure Drop [Pa]");
            sb.AppendLine();

            foreach (var item in ductSystem)
            {
                sb.Append(item.Size);
                sb.Append("\t");
                sb.Append(item.HydraulicDiameter);
                sb.Append("\t");
                sb.Append(item.Length);
                sb.Append("\t");
                sb.Append(string.Format("{0:F8}",item.RelativeRoughness));
                sb.Append("\t");
                sb.Append(item.AirFlow);
                sb.Append("\t");
                sb.Append(item.Velocity);
                sb.Append("\t");
                sb.Append(item.VelocityPressure);
                sb.Append("\t");
                sb.Append(item.ReynoldsNumber);
                sb.Append("\t");
                sb.Append(item.Approximation);
                sb.Append("\t");
                sb.Append(item.FrictionFactor);
                sb.Append("\t");
                sb.Append(item.FrictionLoss);
                sb.Append("\t");
                sb.Append(item.LocalPressureDrop);
                sb.Append("\t");
                sb.Append(item.LinearPressureDrop);
                sb.Append("\t");
                sb.Append(item.PressureDrop);
                sb.AppendLine();

            }

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.Write(sb.ToString());
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Saving to file has been failed", "Saving failed", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError("Błąd przy zapisie do pliku CSV", e);
                return false;
            }
        }

        public static bool CreateDuctSystemRaport(string filePath, DuctSystem ductSystem)
        {
            try
            {

                StringBuilder sb1 = new StringBuilder();

                sb1.AppendLine();
                sb1.AppendLine();
                sb1.Append("Total pressure drop: ").Append(string.Format("{0:0.####}", ductSystem.TotalPressureDrop)).Append(" Pa");
                sb1.AppendLine();
                sb1.Append("Local pressure drop: ").Append(string.Format("{0:0.####}", ductSystem.LocalPressureDrop)).Append(" Pa");
                sb1.AppendLine();
                sb1.Append("Linear pressure drop: ").Append(string.Format("{0:0.####}", ductSystem.LinearPressureDrop)).Append(" Pa");
                sb1.AppendLine();
                sb1.Append("Total system lenght: ").Append(string.Format("{0:0.####}", ductSystem.TotalSystemLength)).Append(" m");
                sb1.AppendLine();
                sb1.Append("Average air velocity: ").Append(string.Format("{0:0.####}", ductSystem.AverageAirVelocity)).Append(" m/s");
                sb1.AppendLine();
                sb1.Append("Average friction loss: ").Append(string.Format("{0:0.####}", ductSystem.AverageFrictionLoss)).Append(" Pa/m");
                sb1.AppendLine();
                sb1.Append("System element count: ").Append(string.Format("{0:0.####}", ductSystem.Count));
                sb1.AppendLine();
                sb1.AppendLine();
                sb1.AppendLine();

                StringBuilder sb2 = new StringBuilder();

                foreach (var item in ductSystem.DuctCollection)
                {
                    sb2.AppendLine();
                    sb2.Append(string.Format("Size: {0:0.##} mm", item.Size));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Hydraulic Diameter: {0:0.####} mm", item.HydraulicDiameter));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Length: {0:0.##} m", item.Length));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Relative Roughness: {0:0.####} mm", item.RelativeRoughness*1000.0));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Air Flow: {0:0.##} m3/h", item.AirFlow.Flow));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Velocity: {0:0.####} m/s", item.Velocity));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Dynamic Pressure: {0:0.####} Pa", item.VelocityPressure));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Reynolds Number: {0:0.####}", item.ReynoldsNumber));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Approximation type: {0}", item.Approximation));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Friction Factor: {0:0.####}", item.FrictionFactor));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Friction Loss: {0:0.####} Pa/m", item.FrictionLoss));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Local Pressure Drop: {0:0.####} Pa", item.LocalPressureDrop));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Linear Pressure Drop: {0:0.####} Pa", item.LinearPressureDrop));
                    sb2.AppendLine();
                    sb2.Append(string.Format("Total Pressure Drop: {0:0.####} Pa", item.PressureDrop));
                    sb2.AppendLine();
                    if (item.LocalLosses.Count > 0)
                    {
                        sb2.AppendLine();
                        sb2.AppendLine("Local Losses:");
                        sb2.AppendLine();
                        foreach (var locLose in item.LocalLosses)
                        {
                            sb2.Append(string.Format("     - {0}: {1}", locLose.Description, locLose.LocalLossCoefficient));
                            sb2.AppendLine();
                        }
                    }
                    sb2.AppendLine();
                    sb2.AppendLine("************************************************************************************************");
                }
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Rectangle pageSize = new Rectangle(PageSize.A4);
                    Document doc = new Document(pageSize);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                    doc.Open();

                    doc.AddHeader("Duct system", "Duct System");

                    Font fHeader = new Font();
                    fHeader.Size = 16;

                    Font fParagraph = new Font();
                    fParagraph.Size = 14;

                    Paragraph p1 = new Paragraph("Duct system summary", fHeader);
                    p1.Alignment = Element.ALIGN_CENTER;
                    doc.Add(p1);
                    
                    Paragraph p2 = new Paragraph(sb1.ToString(), fParagraph);
                    doc.Add(p2);

                    Paragraph p3 = new Paragraph("Duct system elements summary", fHeader);
                    p3.Alignment = Element.ALIGN_CENTER;
                    doc.Add(p3);

                    Paragraph p4 = new Paragraph(sb2.ToString(), fParagraph);
                    doc.Add(p4);

                    doc.Close();
                }

                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show("Saving to file has been failed", "Saving failed", MessageBoxButton.OK,MessageBoxImage.Error);
                LogError("Błąd przy zapisie do pliku PDF", e);
                return false;
            }

        }
        public static void LogError(string errorMessag, Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("My message: " + errorMessag);
            if (e != null)
            {
                sb.AppendLine("Data: " + e.Data.ToString());
                sb.AppendLine("Message: " + e.Message.ToString());
                sb.AppendLine("Source: " + e.Source.ToString());
                sb.AppendLine("StackTrace: " + e.StackTrace.ToString());
            }

        }
    }
}
