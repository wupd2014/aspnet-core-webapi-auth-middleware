﻿using System.Security.Claims;
using System.Threading.Tasks;
using MisturTee.Config.Claims;
using MisturTee.Config.Claims.ExtractionConfigs;
using MisturTee.Config.Claims.ExtractionConfigs.Valid;

namespace TokenAuth.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Validated version of <see cref="JsonPathClaimExtractionConfig" /> used to actually extract the claim
    /// </summary>
    public class ValidatedJsonPathExtractor : IValidClaimsExtractionConfig
    {
        private readonly JsonPathClaimExtractionConfig.ExtractValueByJsonPathAsync _extract;
        private readonly string _path;
        private readonly string _claimName;

        /// <summary>
        /// creates a new instance
        /// </summary>
        /// <param name="path">jsonPath</param>
        /// <param name="jsonPathExtraction"><see cref="JsonPathClaimExtractionConfig.ExtractValueByJsonPathAsync"/></param>
        /// <param name="claimName">name of the claim to extract</param>
        /// <param name="location"><see cref="ClaimLocation"/> location of the claim value</param>
        public ValidatedJsonPathExtractor(string path,
            JsonPathClaimExtractionConfig.ExtractValueByJsonPathAsync jsonPathExtraction, string claimName,
            ClaimLocation location)
        {
            _path = path;
            _extract = jsonPathExtraction;
            _claimName = claimName;
            ClaimLocation = location;
        }

        public ExtractionType ExtractionType => ExtractionType.JsonPath;
        public ClaimLocation ClaimLocation { get; }

        /// <inheritdoc />
        /// <summary>
        /// Extracts claim using the json and jsonPath
        /// </summary>
        /// <param name="json">JSON</param>
        /// <returns></returns>
        public async Task<Claim> GetClaimAsync(string json)
        {
            if (json == null)
            {
                return null;
            }
            var claimValue = await _extract(json, _path).ConfigureAwait(false);
            return new Claim(_claimName, claimValue);
        }
    }
}


