using Azure.Storage.Blobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;

namespace ImageFunctions
{
    public class DownloadImageFromUrl
    {
        public static readonly string BLOB_STORAGE_CONNECTION_STRING = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        [FunctionName("DownloadImageFromUrl")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, string url, ILogger log)
        {
            try
            {
                var urlComponents = url.Split('.');
                var extension = urlComponents[urlComponents.Length - 1];

                // Check length is that of an extension (jpeg, jpg, png, bmp, etc)
                if(extension.Length <= 4 && extension.Length >= 3)
                {
                    using(var wc = new WebClient())
                    {
                        await wc.DownloadFileTaskAsync(url, $"test.{extension}");
                    }
                }
                else
                {
                    log.LogInformation("The url did not contain an extension");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}
