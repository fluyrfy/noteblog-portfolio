using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
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

    [HttpPost]
    [Route("url/safeBrowsing")]
    public async Task<HttpResponseMessage> safeBrowsing()
    {
      try
      {
        string apiKey = ConfigurationManager.AppSettings["SafeBrowsingApiKey"];
        string apiUrl = $"https://safebrowsing.googleapis.com/v4/threatMatches:find?key={apiKey}";
        string requestBody = await Request.Content.ReadAsStringAsync();

        using (HttpClient client = new HttpClient())
        {
          HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
               
          string responseBody = await response.Content.ReadAsStringAsync();
          return Request.CreateResponse(HttpStatusCode.OK, responseBody);
        }
      }
      catch (Exception ex)
      {
        return Request.CreateResponse(InternalServerError(ex));
      }
    }

    [HttpPost]
    [Route("url/OPSWAT")]
    public async Task<HttpResponseMessage> opswat()
    {
      try
      {
        string apiKey = ConfigurationManager.AppSettings["OPSWATApiKey"];
        string requestBody = await Request.Content.ReadAsStringAsync();
        dynamic requestData = JsonConvert.DeserializeObject(requestBody);
        string url = requestData.url;
        url = $"https://api.metadefender.com/v4/url/{url}";

        using (HttpClient client = new HttpClient())
        {
          client.DefaultRequestHeaders.Add("apikey", apiKey);
          HttpResponseMessage response = await client.GetAsync(url);
          string responseBody = await response.Content.ReadAsStringAsync();
          return Request.CreateResponse(HttpStatusCode.OK, responseBody);
        }
      }
      catch (Exception ex)
      {
        return Request.CreateResponse(InternalServerError(ex));
      }
    }

  }
}
