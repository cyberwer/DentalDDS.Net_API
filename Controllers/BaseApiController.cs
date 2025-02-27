﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DentalDDS_Api.Controllers
{
    //THis is base class controller, it is NOT called from client.
    [Authorize]
    public class BaseApiController : ApiController, IDisposable
    {

        //private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public BaseApiController()
        {
        }
        

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _AppUserManager != null)
            {
                _AppUserManager.Dispose();
                _AppUserManager = null;
            }
            if (disposing && _AppRoleManager != null)
            {
                _AppRoleManager.Dispose();
                _AppRoleManager = null;
            }

            base.Dispose(disposing);
        }
    }
}