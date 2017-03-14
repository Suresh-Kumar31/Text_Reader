using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Text_To_Speech
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var filePath = ConfigurationManager.AppSettings["FilePath"];
                var extention = System.IO.Path.GetExtension(filePath);
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();

                synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
                synthesizer.Volume = 100;  // (0 - 100)
                synthesizer.Rate = -1;     // (-10 - 10)
                switch (extention)
                {
                    case ".pdf":
                        {
                            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                            using (PdfReader pdfReader = new PdfReader(filePath))
                            {
                                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                                {
                                    string thePage = PdfTextExtractor.GetTextFromPage(pdfReader, i, its);
                                    synthesizer.Speak(thePage);
                                }
                            }
                            break;
                        }
                    case ".txt":
                        {
                            string text = System.IO.File.ReadAllText(filePath);
                            synthesizer.Speak(text);
                        }
                        break;
                    default:
                        Console.WriteLine("Error!!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceptoion!!\n" + ex.Message);
            }
        }
    }
}
