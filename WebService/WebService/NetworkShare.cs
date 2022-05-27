using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web;

namespace WebService
{
    public static class NetworkShare
    {
        /// <summary>
        /// Connects to the remote share
        /// </summary>
        /// <returns>Null if successful, otherwise error message.</returns>
        public static string ConnectToShare(string uri, string username, string password)
        {
            //Create netresource and point it at the share
            NETRESOURCE nr = new NETRESOURCE();
            nr.dwType = RESOURCETYPE_DISK;
            nr.lpRemoteName = uri;

            //Create the share
            int ret = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);

            //Check for errors
            if (ret == NO_ERROR)
                return null;
            else
                return GetError(ret);
        }

        /// <summary>
        /// Remove the share from cache.
        /// </summary>
        /// <returns>Null if successful, otherwise error message.</returns>
        public static string DisconnectFromShare(string uri, bool force)
        {
            //remove the share
            int ret = WNetCancelConnection(uri, force);

            //Check for errors
            if (ret == NO_ERROR)
                return null;
            else
                return GetError(ret);
        }

        #region P/Invoke Stuff
        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult
            );

        [DllImport("Mpr.dll")]
        private static extern int WNetCancelConnection(
            string lpName,
            bool fForce
            );

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 0;
            public int dwDisplayType = 0;
            public int dwUsage = 0;
            public string lpLocalName = "";
            public string lpRemoteName = "";
            public string lpComment = "";
            public string lpProvider = "";
        }

        #region Consts
        const int RESOURCETYPE_DISK = 0x00000001;
        const int CONNECT_UPDATE_PROFILE = 0x00000001;
        #endregion

        #region Errors
        const int NO_ERROR = 0;

        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_ALREADY_ASSIGNED = 85;
        const int ERROR_BAD_DEVICE = 1200;
        const int ERROR_BAD_NET_NAME = 67;
        const int ERROR_BAD_PROVIDER = 1204;
        const int ERROR_CANCELLED = 1223;
        const int ERROR_EXTENDED_ERROR = 1208;
        const int ERROR_INVALID_ADDRESS = 487;
        const int ERROR_INVALID_PARAMETER = 87;
        const int ERROR_INVALID_PASSWORD = 1216;
        const int ERROR_MORE_DATA = 234;
        const int ERROR_NO_MORE_ITEMS = 259;
        const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        const int ERROR_NO_NETWORK = 1222;
        const int ERROR_SESSION_CREDENTIAL_CONFLICT = 1219;

        const int ERROR_BAD_PROFILE = 1206;
        const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        const int ERROR_DEVICE_IN_USE = 2404;
        const int ERROR_NOT_CONNECTED = 2250;
        const int ERROR_OPEN_FILES = 2401;

        private struct ErrorClass
        {
            public int num;
            public string message;
            public ErrorClass(int num, string message)
            {
                this.num = num;
                this.message = message;
            }
        }

        private static ErrorClass[] ERROR_LIST = new ErrorClass[] {
        new ErrorClass(ERROR_ACCESS_DENIED, "Error: Access Denied"),
        new ErrorClass(ERROR_ALREADY_ASSIGNED, "Error: Already Assigned"),
        new ErrorClass(ERROR_BAD_DEVICE, "Error: Bad Device"),
        new ErrorClass(ERROR_BAD_NET_NAME, "Error: Bad Net Name"),
        new ErrorClass(ERROR_BAD_PROVIDER, "Error: Bad Provider"),
        new ErrorClass(ERROR_CANCELLED, "Error: Cancelled"),
        new ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"),
        new ErrorClass(ERROR_INVALID_ADDRESS, "Error: Invalid Address"),
        new ErrorClass(ERROR_INVALID_PARAMETER, "Error: Invalid Parameter"),
        new ErrorClass(ERROR_INVALID_PASSWORD, "Error: Invalid Password"),
        new ErrorClass(ERROR_MORE_DATA, "Error: More Data"),
        new ErrorClass(ERROR_NO_MORE_ITEMS, "Error: No More Items"),
        new ErrorClass(ERROR_NO_NET_OR_BAD_PATH, "Error: No Net Or Bad Path"),
        new ErrorClass(ERROR_NO_NETWORK, "Error: No Network"),
        new ErrorClass(ERROR_BAD_PROFILE, "Error: Bad Profile"),
        new ErrorClass(ERROR_CANNOT_OPEN_PROFILE, "Error: Cannot Open Profile"),
        new ErrorClass(ERROR_DEVICE_IN_USE, "Error: Device In Use"),
        new ErrorClass(ERROR_EXTENDED_ERROR, "Error: Extended Error"),
        new ErrorClass(ERROR_NOT_CONNECTED, "Error: Not Connected"),
        new ErrorClass(ERROR_OPEN_FILES, "Error: Open Files"),
        new ErrorClass(ERROR_SESSION_CREDENTIAL_CONFLICT, "Error: Credential Conflict"),
    };

        private static string GetError(int errNum)
        {
            foreach (ErrorClass er in ERROR_LIST)
            {
                if (er.num == errNum) return er.message;
            }
            return "Error: Unknown, " + errNum;
        }
        #endregion

        #endregion

        //generate img path
        public static string todayFilePath(string fileName)
        {
            Class1 c = new Class1();
            string rs = "", sPath = "";

            string[] iPath = c.ImgPathTabletLoanImageGet();
            sPath = iPath[0];

            string todayDate = DateTime.Now.ToString("yyyyMMdd");
            rs = sPath + @"\Done\" + todayDate;

            if (!Directory.Exists(rs))
            {
                Directory.CreateDirectory(rs);
            }

            rs = sPath + @"\Done\" + todayDate + @"\" + fileName;

            return rs;
        }
        //move file
        public static string MoveFile(string FileNameForLog, string ControllerName, string fromPath, string fileName)
        {
            string rs = "", sPath = "";
            Class1 c = new Class1();
            try
            {
                //string[] iPath=c.ImgPathTabletLoanImageGet();
                //sPath = iPath[0];
                //string sUsername = iPath[1];
                //string sPwd = iPath[2];

                //DisconnectFromShare(sPath, true);//Disconnect in case we are currently connected with our credentials;
                //string nRSERRORCode = ConnectToShare(sPath, sUsername, sPwd);
                //if (nRSERRORCode == null)
                //{                    
                //    string toPath = todayFilePath(fileName);
                //    if (File.Exists(toPath)) {
                //        File.Delete(toPath);
                //    }
                //    File.Move(fromPath, toPath);
                //    rs = toPath;
                //}
                //else
                //{
                //    //invalid auth
                //    rs = "";
                //    c.T24_AddLog(FileNameForLog, "MoveFile", "Unable to access share path: "+ sPath, ControllerName);
                //}
                //DisconnectFromShare(sPath, false);//Disconnect from the server.

                string toPath = todayFilePath(fileName);
                if (File.Exists(toPath))
                {
                    File.Delete(toPath);
                }
                File.Move(fromPath, toPath);
                rs = toPath;

            }
            catch (Exception ex)
            {
                rs = "";
                c.T24_AddLog(FileNameForLog, "MoveFile", "Unable to access share path: " + sPath + " -> " + ex, ControllerName);
            }
            return rs;
        }
        //get file
        public static Object[] GetFile(string FileNameForLog, string ControllerName, HttpResponseMessage response, string filePath)
        {
            Object[] rs = new Object[3];
            string sPath = "";
            Class1 c = new Class1();
            try
            {
                string[] iPath = c.ImgPathTabletLoanImageGet();
                sPath = iPath[0];
                string sUsername = iPath[1];
                string sPwd = iPath[2];

                //DisconnectFromShare(sPath, true);//Disconnect in case we are currently connected with our credentials;
                //string nRSERRORCode = ConnectToShare(sPath, sUsername, sPwd);
                //if (nRSERRORCode == null)
                //{
                //    if (!File.Exists(filePath))
                //    {
                //        rs[0] = "Error";
                //        rs[1] = "File is not existed";
                //    }
                //    else
                //    {
                //        string Ext = Path.GetExtension(filePath);
                //        response = new HttpResponseMessage(HttpStatusCode.OK);
                //        response.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                //        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //        response.Content.Headers.ContentDisposition.FileName = filePath;
                //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + Ext);

                //        rs[0] = "Succeed";
                //        rs[1] = "Succeed";
                //        rs[2] = response;
                //        c.T24_AddLog(FileNameForLog, "GetFile", "Move file by SVC", ControllerName);
                //    }
                //}
                //else
                //{
                //    ////invalid auth
                //    //rs[0] = "Error";
                //    //rs[1] = "Unable to access share path";
                //    //c.T24_AddLog(FileNameForLog, "GetFile", "Unable to access share path: " + sPath, ControllerName);
                //    if (!File.Exists(filePath))
                //    {
                //        rs[0] = "Error";
                //        rs[1] = "File is not existed";
                //    }
                //    else
                //    {
                //        string Ext = Path.GetExtension(filePath);
                //        response = new HttpResponseMessage(HttpStatusCode.OK);
                //        response.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                //        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //        response.Content.Headers.ContentDisposition.FileName = filePath;
                //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + Ext);

                //        rs[0] = "Succeed";
                //        rs[1] = "Succeed";
                //        rs[2] = response;
                //        c.T24_AddLog(FileNameForLog, "GetFile", "Move file by Everyone", ControllerName);
                //    }
                //}

                if (!File.Exists(filePath))
                {
                    rs[0] = "Error";
                    rs[1] = "File is not existed";
                    c.T24_AddLog(FileNameForLog, "GetFile", "NotFound: " + filePath, ControllerName);
                }
                else
                {
                    string Ext = Path.GetExtension(filePath);
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filePath;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + Ext);

                    rs[0] = "Succeed";
                    rs[1] = "Succeed";
                    rs[2] = response;
                    c.T24_AddLog(FileNameForLog, "GetFile", "Move file by Everyone", ControllerName);
                }



            }
            catch (Exception ex)
            {
                rs[0] = "Error";
                rs[1] = "Unable to access share path";
                c.T24_AddLog(FileNameForLog, "GetFile", "Unable to access share path: " + sPath + " -> " + ex, ControllerName);
            }
            return rs;
        }
    }
}