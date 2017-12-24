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
        string[] request_lines;
        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

           request_lines = this.requestString.Split(new[] { "\r\n" }, StringSplitOptions.None);
            if (request_lines.Length < 3)
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
            string[] method = request_lines[0].Split(' ');

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

            return ValidateIsURI(this.relativeURI); 
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
             string [] header;
            string value;
            headerLines = new Dictionary<string, string>();
            bool is_hosted = false;
             for (int i = 1; i < request_lines.Length; i++)
             {
                 if (request_lines[i] == "")
                     break;
                 header = request_lines[i].Split(':');
                if (header[0] == "Host")
                    is_hosted = true;
                value = "";
                for(int j=1;j<header.Length;j++)
                {
                    if (j != 1)
                        value += ":";
                    value += header[j];
                }
                 headerLines.Add(header[0], value);
             }
            return (is_hosted || this.httpVersion==HTTPVersion.HTTP10); 
        }

        private bool ValidateBlankLine()
        {
           // throw new NotImplementedException();
            foreach (string s in request_lines)
            {
                if (s == "")
                    return true;
            }
            return false; 
        
        }

    }
}
