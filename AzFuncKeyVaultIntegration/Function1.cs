
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Services;
using System.Threading.Tasks;
using System;

namespace AzFuncKeyVaultIntegration
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            KeyVaultService _service = new KeyVaultService();

            //secret name applicationSecret2
            string secretValue = await _service.GetSecretValue("applicationSecret2");

            log.Info($"The value of secret value is {secretValue}");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name} using keyvault Syntax from app settings {Environment.GetEnvironmentVariable("secret2")}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
