﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client {
    public class ChargebacksClient : BaseMollieClient, IChargebacksClient {
        public ChargebacksClient(string apiKey) : base(apiKey) {
        }

        public async Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId) {
            return await this.GetAsync<ChargebackResponse>($"payments/{paymentId}/chargebacks/{chargebackId}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string paymentId,
            int? offset = null, int? count = null) {
            return await this
                .GetListAsync<ListResponse<ChargebackResponse>>($"payments/{paymentId}/chargebacks", offset, count)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(int? offset = null,
            int? count = null, string oathProfileId = null, bool? oauthTestmode = null) {
            if (oathProfileId != null || oauthTestmode != null) {
                this.ValidateApiKeyIsOauthAccesstoken();
            }

            // Build parameters
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(oathProfileId)) {
                parameters.Add("profileId", oathProfileId);
            }

            if (oauthTestmode.HasValue) {
                parameters.Add("testmode", oauthTestmode.Value.ToString().ToLower());
            }

            return await this
                .GetListAsync<ListResponse<ChargebackResponse>>($"chargebacks", offset, count, parameters).ConfigureAwait(false);
        }
    }
}