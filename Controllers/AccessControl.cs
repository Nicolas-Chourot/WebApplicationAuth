using Antlr.Runtime.Misc;
using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{

    public class AccessControl
    {
        

        public class UserAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                try
                {
                    if (User.ConnectedUser == null)
                    {
                        httpContext.Response.Redirect("/Accounts/Login?message=Accès non autorisé!&success=false");
                        return false;
                    }
                    else
                    {
                        if (User.ConnectedUser.Blocked)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public class SuperUserAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                try
                {
                    if (User.ConnectedUser == null)
                    {
                        httpContext.Response.Redirect("/Accounts/Login?message=Accès en écriture non autorisé!&success=false");
                        return false;
                    }
                    else
                    {
                        if (User.ConnectedUser.Access < Models.Access.Write || User.ConnectedUser.Blocked)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public class AdminAccess : AuthorizeAttribute
        {
            // todo refactor users rights encoding
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                try
                {
                    if (User.ConnectedUser == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (User.ConnectedUser.Access < Models.Access.Admin || User.ConnectedUser.Blocked)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
}