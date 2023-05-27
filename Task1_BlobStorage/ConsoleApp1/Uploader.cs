using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1
{
    internal class Uploader
    {
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=shagilstorageaccount;AccountKey=UosgMOfIr16chAHJve7Um4sBQIigoycK83Wc3Y9Q8+/O7oYqa1uUHaRL7EIG70A9HDrrWAqopTHc+AStJbZWQg==;EndpointSuffix=core.windows.net";
        private readonly string containerName = "shagil-blob-container";
        private readonly string folderPath = "E:\\GTP-Batch-04\\Task1_BlobStorage\\ConsoleApp1\\filestoupload\\1.jpg";

        public void Upload(int size)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            client.CreateIfNotExists();
            var blockBlobClient = client.GetBlockBlobClient("1.jpg");
            int blockCounterId = 0;
            List<string> blockIds = new List<string>();

            using (var fs = File.OpenRead(folderPath))
            {
                long bytesRemaining = fs.Length;
                while (bytesRemaining > 0)
                {
                    var dataToRead = (int)Math.Min(bytesRemaining, size);
                    byte[] data = new byte[dataToRead];
                    int dataRead = fs.Read(data, 0, dataToRead);
                    bytesRemaining -= dataRead;

                    if (dataRead > 0)
                    {
                        string blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(blockCounterId.ToString()));
                        blockBlobClient.StageBlock(blockId, new MemoryStream(data));
                        Console.WriteLine($"Block {blockCounterId} uploaded.");
                        blockIds.Add(blockId);
                        blockCounterId++;
                    }
                }
            }

            Console.WriteLine("All blocks uploaded.");
            blockBlobClient.CommitBlockList(blockIds);
            Console.WriteLine("Blob uploaded.");
            Console.ReadLine();
        }
    }

}
