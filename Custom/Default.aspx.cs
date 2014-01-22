using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.DirectoryServices;
using System.Configuration;

namespace Ldapform
{
    public partial class Default : System.Web.UI.Page
    {//Firsly Set up a few properties
        static string host = ConfigurationManager.AppSettings["host"].ToString();
        static string domain = ConfigurationManager.AppSettings["domain"].ToString();
        static string path = "LDAP://" + host+ @":389/" + domain;



        static string QlikViewServerURL = @"http://localhost/QVAJAXZFC/getwebticket.aspx";  //address of the QlikView server
        static string ticketinguser = ConfigurationManager.AppSettings["qv_admin"].ToString();  //Service account used to ask for a ticket (QV Administrator), this is not the end user
        static string ticketingpassword = ConfigurationManager.AppSettings["qv_pass"].ToString();
        static string document = ConfigurationManager.AppSettings["document"].ToString();  // specify a single QVW or leave blank for access point
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GO_Button_Click(object sender, EventArgs e)
        {
            
            //Grab the details the user provides
            string username = txtUser.Text;
            string password = txtPassword.Text;
            string groups = txtGroups.Text;

            // Test if the user is valid
            bool loginOK = ValidateUser(username, password);

            //If the user is valid get a ticket and log them in
			string ticket = "";
            if (loginOK)
            {
				//Get the Ticket
                    ticket = getTicket(username, groups, ticketinguser, ticketingpassword); // add groups into the empty string if required
                
				//Build a redirect link to either access point or to a single document
					string RedirectLink = "";
                
					if (document.Length > 0)
					{//Send to a single document
						RedirectLink = "http://localhost/qvajaxzfc/authenticate.aspx?type=html&try=/qvajaxzfc/opendoc.htm?document=" + document + "&back=/LoginPage.htm&webticket=" + ticket;
					}
					else
					{//Send to a Access Point
                        RedirectLink = "http://localhost/qvajaxzfc/authenticate.aspx?type=html&try=/qlikview&back=/LoginPage.htm&webticket=" + ticket;
					}
				//Redirect the user
					Response.Redirect(RedirectLink);
			
            }
        }
            
        
        // This function is a place holder for code to test the user id and password against a database or LDAP
        // For the purpose of this example we are not going to test it at all, just return "true" for logged in
        // Of course this means anyone would be let in, so this needs to be added
        private bool ValidateUser(string User, string pass)
        {
            LdapAuthentication ldap = new LdapAuthentication(path);
            if (true == ldap.IsAuthenticated(User, pass))
            {
              //user is authenticated!
                return true;
            }
            else
            {
                result.Text = "Invalid UserName or Password!";
                //Invalid UserName or Password!!                
                return false;
            }
        }
               

        
		// This function is going to take the username and groups and return a web ticket from QV
		// User and groups relate to the user you want to reuqest a ticket for
		// ticketinguser and password are the credentials used to ask for the ticket and needs to be a QV admin
        private string getTicket(string user,string usergroups,string ticketinguser, string ticketingpassword) 
        {
            //do not change anything here...Arbaaz
            StringBuilder groups = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(usergroups))
            {
                groups.Append("<GroupList>");
                foreach (string group in usergroups.Split(new char[] { ',' }))
                {
                    groups.Append("<string>");
                    groups.Append(group);
                    groups.Append("</string>");
                }
                groups.Append("</GroupList>");
                groups.Append("<GroupsIsNames>");
                groups.Append("true");
                groups.Append("</GroupsIsNames>");
            }
            string webTicketXml = string.Format("<Global method=\"GetWebTicket\"><UserId>{0}</UserId></Global>", user);

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create(new Uri(QlikViewServerURL));
            client.PreAuthenticate = true;
            client.Method = "POST";
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            client.Credentials = new NetworkCredential(ticketinguser,ticketingpassword);

            using (StreamWriter sw = new StreamWriter(client.GetRequestStream()))
                sw.WriteLine(webTicketXml);
            StreamReader sr = new StreamReader(client.GetResponse().GetResponseStream());
            string result = sr.ReadToEnd();

            XDocument doc = XDocument.Parse(result);
            return doc.Root.Element("_retval_").Value;
        }
		

        }


    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;
        public LdapAuthentication(string path)
        {
            _path = path;
        }
        public bool IsAuthenticated(string username, string pwd)
        {
            //DirectoryEntry entry = new DirectoryEntry(_path, username, pwd);
            DirectoryEntry entry = new DirectoryEntry(_path);
            entry.Username = "uid=" + username + ",ou=Users,dc=example,dc=local";
            entry.Password = pwd;
            entry.AuthenticationType = AuthenticationTypes.None;

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(cn=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];

            }
            catch (Exception ex)
            {
                //                throw new exception("error authenticating user. " + ex.message);
                return false;

            }
            return true;
        }

    }

    }