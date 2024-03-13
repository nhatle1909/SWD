using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Services.Tools
{
    public class VNPayUtil
    {

        public const string VERSION = "2.1.0";
        private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
        private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());
        public void ClearRequestData() 
        {
            _requestData.Clear();
        }
        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Request

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }



        #endregion

        #region Response process

        public bool ValidateSignature(string baseQuery, string inputHash, string secretKey)
        {
            string rspRaw = baseQuery;
            string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion
    }

    public class Utils
    {

        public static String HmacSHA512(string key, String inputData)
        {

            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static string GetIpAddress()
        {


            string ipAddress;
            try
            {
                HttpContextAccessor ipget = new HttpContextAccessor();
                ipAddress = ipget.HttpContext.Connection.RemoteIpAddress.ToString();
                //  _conte

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                    ipAddress = ipget.HttpContext.GetServerVariable("REMOTE_ADDR");
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
        public static string ExtractUrlParam(string url, string paramName)
        {
            var queryString = url.Split('?').Skip(1).FirstOrDefault();
            if (queryString == null)
            {
                return null; // Return null if no query string
            }

            var keyValuePairs = queryString.Split('&');

            foreach (var pair in keyValuePairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2 && parts[0] == paramName)
                {
                    return parts[1];
                }
            }

            return null; // Return null if parameter not found
        }
        public static string GetQueryString(string url)
        {
            // Find the first question mark (?), which marks the beginning of the query string
            int questionMarkIndex = url.IndexOf('?');

            // Check if a question mark exists
            if (questionMarkIndex < 0)
            {
                return null; // No query string found
            }

            // Extract the query string
            string queryString = url.Substring(questionMarkIndex + 1);

            // Find the last ampersand (&)
            int lastAmpersandIndex = queryString.LastIndexOf('&');

            // Handle single parameter or no ampersand
            if (lastAmpersandIndex < 0)
            {
                return queryString; // Only one parameter or no ampersand
            }

            // Return the query string without the last parameter
            return queryString.Substring(0, lastAmpersandIndex);
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
