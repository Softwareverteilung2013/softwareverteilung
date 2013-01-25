using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Packaging;

namespace Client.Classes
{
    class clsZipFile
    {
        public static void ZipFiles(string path, IEnumerable<string> files, CompressionOption compressionLevel)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                clsZipFile.ZipFilesToStream(fileStream, files, compressionLevel);
            }
        }

        public static void Unzip(string zipPath, string baseFolder)
        {
            using (FileStream fileStream = new FileStream(zipPath, FileMode.Open))
            {
                clsZipFile.UnzipFilesFromStream(fileStream, baseFolder);
            }
        }

        private static void ZipFilesToStream(FileStream destination, IEnumerable<string> files, CompressionOption compressionLevel)
        {
            using (Package package = Package.Open(destination, FileMode.Create))
            {
                foreach (string path in files)
                {
                    Uri fileUri = new Uri(@"/" + Path.GetFileName(path), UriKind.Relative);
                    string contentType = @"data/" + clsZipFile.GetFileExtentionName(path);

                    using (Stream zipStream = package.CreatePart(fileUri, contentType, compressionLevel).GetStream())
                    {

                        CopyToStream(destination, zipStream);

                    }
                }
            }
        }

        private static void UnzipFilesFromStream(Stream source, string baseFolder)
        {
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }

            using (Package package = Package.Open(source, FileMode.Open))
            {
                foreach (PackagePart zipPart in package.GetParts())
                {
                    string path = Path.Combine(baseFolder, zipPart.Uri.ToString().Substring(1));

                    using (Stream zipStream = zipPart.GetStream())
                    {
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            CopyToFileStream(zipStream, fileStream);
                        }
                    }
                }
            }
        }

        private static string GetFileExtentionName(string path)
        {
            string extention = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(extention) && extention.StartsWith("."))
            {
                extention = extention.Substring(1);
            }

            return extention;
        }

        public static void CopyToStream(FileStream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static void CopyToFileStream(Stream input, FileStream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
