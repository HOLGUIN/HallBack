using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Hallearn.Models;
using Hallearn.Providers;
using Hallearn.Results;

namespace Hallearn.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
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

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion






        private void Page_Load(object sender, EventArgs e)
        {


            //Link donde explica como unirse  a paypal
            /*https://www.codeproject.com/articles/19184/use-of-the-paypal-payment-system-in-asp-net

            string requestUriString;
            CultureInfo provider = new CultureInfo("en-us");
            string requestsFile = this.Server.MapPath(
                "~/App_Data/PaymentRequests.xml");
            requests.Clear();
            if (System.IO.File.Exists(requestsFile))
            {
                requests.ReadXml(requestsFile);
            }
            else
            {
                Carts.CreateXml(requestsFile, "Requests");
                requests.ReadXml(requestsFile);
            }
            string responseFile = this.Server.MapPath(
                "~/App_Data/PaymentResponses.xml");
            responses.Clear();
            if (System.IO.File.Exists(responseFile))
            {
                responses.ReadXml(responseFile);
            }
            else
            {
                Carts.CreateXml(responseFile, "Responses");
                responses.ReadXml(responseFile);
            }
            string strFormValues = Encoding.ASCII.GetString(
                this.Request.BinaryRead(this.Request.ContentLength));

            // getting the URL to work with
            if (String.Compare(
                ConfigurationManager.AppSettings["UseSandbox"].ToString(),
                "true", false) == 0)
            {
                requestUriString =
                    "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                requestUriString = "https://www.paypal.com/cgi-bin/webscr";
            }

            // Create the request back
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(requestUriString);

            // Set values for the request back
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string obj2 = strFormValues + "&cmd=_notify-validate";
            request.ContentLength = obj2.Length;

            // Write the request back IPN strings
            StreamWriter writer =
                new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(RuntimeHelpers.GetObjectValue(obj2));
            writer.Close();

            //send the request, read the response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            StreamReader reader = new StreamReader(responseStream, encoding);

            // Reads 256 characters at a time.

            char[] buffer = new char[0x101];
            int length = reader.Read(buffer, 0, 0x100);
            while (length > 0)
            {
                // Dumps the 256 characters to a string

                string requestPrice;
                string IPNResponse = new string(buffer, 0, length);
                length = reader.Read(buffer, 0, 0x100);
                try
                {
                    // getting the total cost of the goods in
                    // cart for an identifier
                    // of the request stored in the "custom" variable
                    requestPrice =
                        GetRequestPrice(this.Request["custom"].ToString());
                    if (String.Compare(requestPrice, "", false) == 0)
                    {
                        Carts.WriteFile("Error in IPNHandler: amount = \");
    
                        reader.Close();
                        response.Close();
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Carts.WriteFile("Error in IPNHandler: " + exception.Message);
                    reader.Close();
                    response.Close();
                    return;
                }

                NumberFormatInfo info2 = new NumberFormatInfo();
                info2.NumberDecimalSeparator = ".";
                info2.NumberGroupSeparator = ",";
                info2.NumberGroupSizes = new int[] { 3 };

                // if the request is verified
                if (String.Compare(IPNResponse, "VERIFIED", false) == 0)
                {
                    // check the receiver's e-mail (login is user's
                    // identifier in PayPal)
                    // and the transaction type
                    if ((String.Compare(this.Request["receiver_email"],
                        this.business, false) != 0) ||
                        (String.Compare(this.Request["txn_type"],
                        "web_accept", false) != 0))
                    {
                        try
                        {
                            // parameters are not correct. Write a
                            // response from PayPal
                            // and create a record in the Log file.
                            this.CreatePaymentResponses(this.Request["txn_id"],
                                Convert.ToDecimal(
                                this.Request["mc_gross"], info2),
                                this.Request["payer_email"],
                                this.Request["first_name"],
                                this.Request["last_name"],
                                this.Request["address_street"],
                                this.Request["address_city"],
                                this.Request["address_state"],
                                this.Request["address_zip"],
                                this.Request["address_country"],
                                Convert.ToInt32(this.Request["custom"]), false,
                                "INVALID payment's parameters" +
                                "(receiver_email or txn_type)");
                            Carts.WriteFile(
                                "Error in IPNHandler: INVALID payment's" +
                                " parameters(receiver_email or txn_type)");
                        }
                        catch (Exception exception)
                        {
                            Carts.WriteFile("Error in IPNHandler: " +
                                exception.Message);
                        }
                        reader.Close();
                        response.Close();
                        return;
                    }

                    // check whether this request was performed
                    // earlier for its identifier
                    if (this.IsDuplicateID(this.Request["txn_id"]))
                    {
                        // the current request is processed. Write
                        // a response from PayPal
                        // and create a record in the Log file.
                        this.CreatePaymentResponses(this.Request["txn_id"],
                            Convert.ToDecimal(this.Request["mc_gross"], info2),
                            this.Request["payer_email"],
                            this.Request["first_name"],
                            this.Request["last_name"],
                            this.Request["address_street"],
                            this.Request["address_city"],
                            this.Request["address_state"],
                            this.Request["address_zip"],
                            this.Request["address_country"],
                            Convert.ToInt32(this.Request["custom"]), false,
                            "Duplicate txn_id found");
                        Carts.WriteFile(
                            "Error in IPNHandler: Duplicate txn_id found");
                        reader.Close();
                        response.Close();
                        return;
                    }

                    // the amount of payment, the status of the
                    // payment, and a possible reason of delay
                    // The fact that Getting txn_type=web_accept or
                    // txn_type=subscr_payment are got odes not mean that
                    // seller will receive the payment.
                    // That's why we check payment_status=completed. The
                    // single exception is when the seller's account in
                    // not American and pending_reason=intl
                    if (((String.Compare(
                        this.Request["mc_gross"].ToString(provider),
                        requestPrice, false) != 0) ||
                        (String.Compare(this.Request["mc_currency"],
                        this.currency_code, false) != 0)) ||
                        ((String.Compare(this.Request["payment_status"],
                        "Completed", false) != 0) &&
                        (String.Compare(this.Request["pending_reason"],
                        "intl", false) != 0)))
                    {
                        // parameters are incorrect or the payment
                        // was delayed. A response from PayPal should not be
                        // written to DB of an XML file
                        // because it may lead to a failure of
                        // uniqueness check of the request identifier.
                        // Create a record in the Log file with information
                        // about the request.
                        Carts.WriteFile(
                            "Error in IPNHandler: INVALID payment's parameters." +
                            "Request: " + strFormValues);
                        reader.Close();
                        response.Close();
                        return;
                    }
                    try
                    {
                        // write a response from PayPal
                        this.CreatePaymentResponses(this.Request["txn_id"],
                            Convert.ToDecimal(this.Request["mc_gross"], info2),
                            this.Request["payer_email"],
                            this.Request["first_name"],
                            this.Request["last_name"],
                            this.Request["address_street"],
                            this.Request["address_city"],
                            this.Request["address_state"],
                            this.Request["address_zip"],
                            this.Request["address_country"],
                            Convert.ToInt32(this.Request["custom"]), true, "");
                        Carts.WriteFile(
                            "Success in IPNHandler: PaymentResponses created");

                        ///////////////////////////////////////////////////
                        // Here we notify the person responsible for
                        // goods delivery that
                        // the payment was performed and providing
                        // him with all needed information about
                        // the payment. Some flags informing that
                        // user paid for a services can be also set here.
                        // For example, if user paid for registration
                        // on the site, then the flag should be set
                        // allowing the user who paid to access the site
                        //////////////////////////////////////////////////
                    }
                    catch (Exception exception)
                    {
                        Carts.WriteFile(
                            "Error in IPNHandler: " + exception.Message);
                    }
                }
                else
                {
                    Carts.WriteFile(
                        "Error in IPNHandler. IPNResponse = 'INVALID'");
                }
            }
            reader.Close();
            response.Close();

           */
        }



















    }
}
