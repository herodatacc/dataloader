/** 
 * HERODATA.CC DataLoader v1.0
 * visited & get the key
 * https://www.herodata.cc  
 * herodata
 * **/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace dataloader
{
    class Program
    {
        static void Main(string[] args)
        {
            //string symbol = args[0]; // command with argument input
            string symbol = "GC"; //Gold
            string key = "this_is_demo_key";
            string url = string.Format("https://www.herodata.cc/api/download", symbol);
            download(url, symbol, key);

            Console.Read();
        }

        static async void download(string url, string symbol, string key, bool isoverwrite = true)
        {
            var client = new HttpClient();

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] { 
                new KeyValuePair<string, string>("symbol", symbol), 
                new KeyValuePair<string, string>("key", key) });

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(url, requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            string csv = string.Format("{0}.csv", symbol);
            if (isoverwrite)
            {
                if (File.Exists(csv))
                {
                    File.Delete(csv);
                }
            }

            StreamWriter file = new StreamWriter(csv, true);
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                string line = await reader.ReadToEndAsync();
                file.WriteLine(line);
                Console.WriteLine(line);
            }
            file.Close();
        }
    }
}
