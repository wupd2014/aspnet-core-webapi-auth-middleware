﻿using MiddlewareAuth.Config.Claims.ExtractionConfigs.Valid;
using System;

namespace MiddlewareAuth.Config.Claims.ExtractionConfigs
{
    public abstract class ClaimsExtractionConfig
    {
        protected ClaimsExtractionConfig(string claimName)
        {
            if (string.IsNullOrEmpty(claimName))
            {
                throw new ArgumentNullException($"{nameof(claimName)} can't be empty or null.");
            }
        }
        public ClaimLocation Location { get; protected set; }
        protected ExtractionType ExtractionType { get; set; }
        public string ClaimName { get; protected set; }
        public abstract IValidClaimsExtractionConfig Build();
    }
}
