using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class RolesSettings
    {
        // DOBIJANJE PERMISIJA SVIH ZA TU ROLU
        public static bool GetPermissions(string rolename, out string[] permissions)
        {
            permissions = new string[10];
            string permissionString = string.Empty;

            permissionString = (string)RolesSettingsFile.ResourceManager.GetObject(rolename);
            if (permissionString != null)
            {
                permissions = permissionString.Split(',');
                return true;
            }
            return false;

        }

        //PROVERIMO DA LI JE U ROLI TA PERMISIJA
        public static bool IsInRole(string group, string permission)
        {
            string[] permissions;
            GetPermissions(group, out permissions);
            foreach (string perm in permissions)
            {
                if (perm.Equals(permission))
                    return true;
            }
            return false;
        }
    }
}
