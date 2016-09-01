using System.Web.Mvc;
using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using BEYON.Web.Extension.Filters;

namespace BEYON.Web.Areas.Common.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [LayoutAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public void Proxy()
        {
            string ERR = "";
            string MIME = "";

            HttpResponseBase HS = Response;

            //set access-control
            HS.AddHeader("Access-Control-Allow-Origin", "*");
            HS.AddHeader("Access-Control-Request-Method", "*");
            HS.AddHeader("Access-Control-Allow-Methods", "OPTIONS, GET");
            HS.AddHeader("Access-Control-Allow-Headers", "*");

            HS.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HS.Cache.SetValidUntilExpires(false);
            HS.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HS.Cache.SetCacheability(HttpCacheability.NoCache);
            HS.Cache.SetNoStore();

            //GET
            string geturl = Request.Url.Query.Substring(1);
            //string geturl = Request.QueryString["url"];

            if (geturl != "" && geturl != null)
            {
                if (ERR == "")
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(geturl);
                        ((HttpWebRequest)request).UserAgent = Request.UserAgent;

                        // If required by the server, set the credentials.
                        request.Credentials = CredentialCache.DefaultCredentials;

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (!(response.StatusCode == HttpStatusCode.OK && response.ContentLength > 0))
                        {
                            ERR = response.StatusCode.ToString();
                        }
                        else
                        {
                            MIME = response.ContentType.ToLower().Trim();
                            //if ("|image/jpeg|image/jpg|image/png|image/gif|text/html|application/xhtml|application/json|".IndexOf("|" + MIME + "|") == -1)
                            //{
                            //    ERR = MIME + " mime is invalid";
                            //}
                            //else
                            {
                                HS.ContentType = MIME;

                                Stream receiveStream = response.GetResponseStream();

                                byte[] buffer = new byte[32768];
                                int read = 0;

                                int chunk;
                                while ((chunk = receiveStream.Read(buffer, read, buffer.Length - read)) > 0)
                                {
                                    read += chunk;
                                    if (read != buffer.Length) { continue; }
                                    int nextByte = receiveStream.ReadByte();
                                    if (nextByte == -1) { break; }

                                    // Resize the buffer
                                    byte[] newBuffer = new byte[buffer.Length * 2];
                                    Array.Copy(buffer, newBuffer, buffer.Length);
                                    newBuffer[read] = (byte)nextByte;
                                    buffer = newBuffer;
                                    read++;
                                }

                                // Buffer is now too big. Shrink it.
                                byte[] ret = new byte[read];
                                Array.Copy(buffer, ret, read);

                                HS.OutputStream.Write(ret, 0, ret.Length);
                                response.Close();
                                receiveStream.Close();
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        ERR = e.ToString();
                    }
                }
            }
            else
            {
                ERR = "url var not defined";
            }
        }
    }
}
