using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pruebaDigitalSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            CaptureScreenshots("C:\\Users\\Enzo Gonzalez\\source\\repos\\pruebaDigitalSolutions\\pruebaDigitalSolutions\\VideoTest\\Test.mp4");

            DetectAndSaveFacialExpressions("C:\\Users\\Enzo Gonzalez\\source\\repos\\pruebaDigitalSolutions\\pruebaDigitalSolutions\\VideoTest\\Test2.mp4");
        }

        static void CaptureScreenshots(string videoPath)
        {
            var capture = new VideoCapture(videoPath);

            if (!capture.IsOpened())
            {
                Console.WriteLine("No se pudo abrir el video.");
                return;
            }

            Directory.CreateDirectory("Screenshots");

            using (Mat frame = new Mat())
            {
                int frameIndex = 0;

                while (true)
                {
                    capture.Read(frame);

                    if (frame.Empty())
                        break;

                    frame.SaveImage($"Screenshots/screenshot_{frameIndex}.png");

                    frameIndex++;
                }

                Console.WriteLine($"Se capturaron {frameIndex} screenshots del video.");
            }
        }

        static void DetectAndSaveFacialExpressions(string videoPath)
        {
            var capture = new VideoCapture(videoPath);

            if (!capture.IsOpened())
            {
                Console.WriteLine("No se pudo abrir el video.");
                return;
            }

            Directory.CreateDirectory("FacialExpressions");

            var faceCascade = new CascadeClassifier("C:\\Users\\Enzo Gonzalez\\source\\repos\\pruebaDigitalSolutions\\pruebaDigitalSolutions\\path\\haarcascade_frontalface_default.xml");

            int frameIndex = 0;

            using (Mat frame = new Mat())
            {
                while (true)
                {
                    capture.Read(frame);

                    if (frame.Empty())
                        break;

                    var faces = faceCascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

                    if (faces.Length > 0)
                    {
                        Console.WriteLine($"Expresión facial detectada en el frame {frameIndex}.");

                        foreach (var face in faces)
                        {
                            Cv2.Rectangle(frame, face, Scalar.Red, 2);
                        }

                        frame.SaveImage($"FacialExpressions/expression_{frameIndex}.png");
                    }

                    frameIndex++;
                }

                Console.WriteLine($"Se detectaron expresiones faciales en {frameIndex} frames del video.");
            }
        }
    }
}
