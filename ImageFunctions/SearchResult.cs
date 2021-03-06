using Azure.Storage.Blobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageFunctions
{
    public static class SearchResult
    {
        [FunctionName("SearchResult")]
        public static async Task Run(
            [EventGridTrigger]EventGridEvent eventGridEvent,
            [Blob("{data.url}", FileAccess.Read)] Stream input,
            ILogger log)
        {
            try
            {
                var width = Convert.ToInt32(Environment.GetEnvironmentVariable("SEARCH_RESULT_WIDTH"));
                var containerName = Environment.GetEnvironmentVariable("SEARCH_RESULT_CONTAINER_NAME");

                var functions = new Functions(width, containerName);

                await functions.ResizeImage(eventGridEvent, input, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw;
            }
}
    }
}
