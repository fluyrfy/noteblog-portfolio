using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace noteblog.Controllers
{
    [RoutePrefix("api/verify")]
    public class VerifyController : ApiController
    {
        [HttpPost]
        [Route("turnstile")]
        public async Task<HttpResponseMessage> turnstile()
        {
            try
            {
                string SECRET_KEY = ConfigurationManager.AppSettings["TurnstileSecretKey"];
                dynamic obj = await Request.Content.ReadAsAsync<JObject>();
                var token = obj.token.ToString();
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", SECRET_KEY),
                    new KeyValuePair<string, string>("response", token)
                });
                var client = new HttpClient();
                var response = await client.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", formData);
                string responseBody;
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseBody = "";
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseBody);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(InternalServerError(ex));
            }
        }
    }
}
