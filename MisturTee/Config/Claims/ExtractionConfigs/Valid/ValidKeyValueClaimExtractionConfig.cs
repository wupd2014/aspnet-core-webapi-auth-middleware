﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MisturTee.Config.Claims.ExtractionConfigs.Valid
{
    /// <inheritdoc />
    internal class ValidKeyValueClaimExtractionConfig : IValidClaimsExtractionConfig
    {
        private readonly KeyValueClaimExtractionConfig.KeyValueExtractionAsync _extract;
        private readonly string _keyName;
        private readonly string _claimName;

        /// <inheritdoc />
        internal ValidKeyValueClaimExtractionConfig(KeyValueClaimExtractionConfig.KeyValueExtractionAsync func, string key, ClaimLocation location, string claimName)
        {
            _extract = func;
            _keyName = key;
            ClaimLocation = location;
            _claimName = claimName;
        }
        
        /// <inheritdoc />
        public ExtractionType ExtractionType => ExtractionType.KeyValue;
        
        /// <inheritdoc />
        public ClaimLocation ClaimLocation { get; }

        /// <inheritdoc />
        public async Task<Claim> GetClaimAsync(string content)
        {
            var contentDict = JsonConvert.DeserializeObject<List<KeyValuePair<string, List<object>>>>(content);
            var value = await _extract(contentDict, _keyName)
                .ConfigureAwait(false);
            return new Claim(_claimName, value);
        }
    }
}
