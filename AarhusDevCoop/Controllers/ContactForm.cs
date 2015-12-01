using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using mariyaUmbracoMA.ViewModels;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models;

namespace mariyaUmbracoMA.Controllers
{

    public class ContactFormSurfaceController : SurfaceController
    {
        //get : Default
        public ActionResult Index ()
        {
          //  TempData["success"] = true;
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model) {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

        //if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
        //// Read data from model and send mail
        TempData["success"] = true;
   
            MailMessage message = new MailMessage();
            message.To.Add("eferwe@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

           

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("eferwe@gmail.com", "da3barzabarza");
                smtp.EnableSsl = true;
                // send mail
                smtp.Send(message);
            }
          //  TempData["success"] = true;

            //// Parameters – name, parentId, contentTypeAlias
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "ContactPage");
            // assign values
            comment.SetValue("name", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);
            // save to Umbraco
            Services.ContentService.Save(comment);
            //// Services.ContentService.SaveAndPublishWithStatus(comment);


        return RedirectToCurrentUmbracoPage();
        }
       

    }
}