using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        String[] request_line; 
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

           request_line = this.requestString.Split(new[] { "\r\n" }, StringSplitOptions.None);
            if (request_line.Length < 3)
                return false;
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line
          
            // Validate blank line exists
            
            // Load header lines into HeaderLines dictionary
            
            if (ParseRequestLine() && ValidateBlankLine() && LoadHeaderLines())
                return true;  
            return false ; 
        }

        private bool ParseRequestLine()
        {
            
            //throw new NotImplementedException();
            string[] method = request_line[0].Split(' ');

            if (method[2] == "HTTP/1.1")
            {
                this.httpVersion = HTTPVersion.HTTP11;
            }
            else if (method[2] == "HTTP/1.0")
            {
                this.httpVersion = HTTPVersion.HTTP10;
            }
            else if (method[2] == "HTTP/0.9")
                this.httpVersion = HTTPVersion.HTTP09; 

            if (method[0] == "GET")
                this.method = RequestMethod.GET;
            else if (method[0] == "POST")
                this.method = RequestMethod.POST;
            else if (method[0] == "HEAD")
                this.method = RequestMethod.HEAD;
            else
                return false;

            this.relativeURI = method[1].Remove(0,1);

                return (true && ValidateIsURI(this.relativeURI)); 
             
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
             string [] header;
             headerLines = new Dictionary<string, string>(); 
           //throw new NotImplementedException();
             for (int i = 1; i < request_line.Length; i++)
             {
                 if (request_line[i] == "")
                     continue; 
                 header = request_line[i].Split(':');
                 headerLines.Add(header[0], header[1]);
             }
            return true; 
        }

        private bool ValidateBlankLine()
        {
           // throw new NotImplementedException();
            foreach (string s in request_line)
            {
                if (s == "")
                    return true;
            }
            return false; 
        
        }

    }
}
