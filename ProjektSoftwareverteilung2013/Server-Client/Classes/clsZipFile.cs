using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;

namespace Server_Client.Classes
{
     
public static class clsZipFile
{
    public static void Unzip(string zipPath, string baseFolder)
    {
        using (FileStream fileStream = new FileStream(zipPath, FileMode.Open))
        {
            clsZipFile.UnzipFilesFromStream(fileStream, baseFolder);
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

    public static void CopyToFileStream(Stream input, FileStream output)
    {
        byte[] buffer = new byte[32768];
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, read);
        }
    }

    private const string PackageRelationshipType = @"http://schemas.microsoft.com/opc/2006/sample/document";
    private const string ResourceRelationshipType = @"http://schemas.microsoft.com/opc/2006/ample/required-resource";

    /// <summary>
    ///   Creates a package zip file containing specified
    ///   content and resource files.</summary>
    public static void CreatePackage(string packagePath, string documentPath)
    {

        Uri partUriDocument = PackUriHelper.CreatePartUri(
                                  new Uri(documentPath, UriKind.Relative));

        using (Package package =
            Package.Open(packagePath, FileMode.Create))
        {
            PackagePart packagePartDocument =
              package.CreatePart(partUriDocument,
                             System.Net.Mime.MediaTypeNames.Text.Xml);

            using (FileStream fileStream = new FileStream(
                  documentPath, FileMode.Open, FileAccess.Read))
            {
                CopyStream(fileStream, packagePartDocument.GetStream());
            }
        }
    }

    /// <summary>
    ///   Copies data from a source stream to a target stream.</summary>
    /// <param name="source">
    ///   The source stream to copy from.</param>
    /// <param name="target">
    ///   The destination stream to copy to.</param>
    private static void CopyStream(Stream source, Stream target)
    {
        const int bufSize = 0x1000;
        byte[] buf = new byte[bufSize];
        int bytesRead = 0;
        while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            target.Write(buf, 0, bytesRead);
    }

}
}
