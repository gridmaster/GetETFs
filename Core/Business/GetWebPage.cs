using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Core.Business
{
    public class GetWebPage
    {
        protected string _url;
        protected string _result;

        /// <summary>
        /// Generates a website for the given URL
        /// </summary>
        /// <param name="url">Address of website from which to generate the thumbnail</param>

        /// <returns></returns>
        public static string GetPage(string url)
        {
            var page = new GetWebPage(url);
            return page.GetWebsite();
        }

        /// <summary>
        /// Protected constructor
        /// </summary>
        protected GetWebPage(string url)
        {
            _url = url;
        }

        /// <summary>
        /// Returns a thumbnail for the current member values
        /// </summary>
        /// <returns>Thumbnail bitmap</returns>
        protected string GetWebsite()
        {
            // WebBrowser is an ActiveX control that must be run in a
            // single-threaded apartment so create a thread to create the
            // control and generate the thumbnail
            Thread thread = new Thread(new ThreadStart(GetWebWorker));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return _result;
        }

        /// <summary>
        /// Creates a WebBrowser control to generate the thumbnail image
        /// Must be called from a single-threaded apartment
        /// </summary>
        protected void GetWebWorker()
        {
            try
            {
                using (WebBrowser browser = new WebBrowser())
                {
                    browser.ScrollBarsEnabled = false;
                    browser.ScriptErrorsSuppressed = true;

                    browser.Navigate(_url);
                    
                    // Wait for control to load page
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();

                    Stream stream = browser.DocumentStream;
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();

                    _result = text; // browser.DocumentText;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
