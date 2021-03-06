﻿using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Request;
using Mollie.WebApplicationExample.Infrastructure;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client.Abstract;

namespace Mollie.WebApplicationExample.Controllers {
    public class MandateController : Controller {
        private readonly IMandateClient _mandateClient;
        private readonly IPaymentClient _paymentClient;

        public MandateController() {
            this._mandateClient = new MandateClient(AppSettings.MollieApiKey);
            this._paymentClient = new PaymentClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index(string customerId) {
            ViewBag.CustomerId = customerId;

            ListResponse<MandateResponse> mandateList = await this._mandateClient.GetMandateListAsync(customerId);
            return this.View(mandateList.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string customerId, string mandateId) {
            MandateResponse mandate = await this._mandateClient.GetMandateAsync(customerId, mandateId);
            return this.View(mandate);
        }

        [HttpGet]
        public async Task<ActionResult> Create(string customerId) {
            // You need at least 1 payment to make a new mandate
            var result = await this._paymentClient.CreatePaymentAsync(new PaymentRequest() {
                Amount = 1,
                Description = "First payment",
                Locale = Api.Models.Payment.Locale.NL,
                CustomerId = customerId,
                RecurringType = Api.Models.Payment.RecurringType.First,
                RedirectUrl = @"http://www.google.nl"
            });


            return this.Redirect(result.Links.PaymentUrl);
        }
    }
}