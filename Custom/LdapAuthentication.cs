using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
namespace Ldapform
{
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