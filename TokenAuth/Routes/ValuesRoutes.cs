﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using TokenAuth.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using System.IO;
using MisturTee.Config;
using MisturTee.Config.Claims;
using MisturTee.Config.Claims.ExtractionConfigs;
using MisturTee.Config.Claims.ExtractionConfigs.Valid;
using MisturTee.Config.Routing;

namespace TokenAuth.Routes
{
    public class ValuesRoutes : IRouteDefinitions
    {
        public const string Prefix = "values";
        public const string Get = "";
        public const string GetById = "{id}";
        public const string GetByIdGuid = "{id:guid}/yolo/{manafort}/sup/{manaforts:guid}";
        public const string Post = "";
        public const string Put = "{id}";
        public const string Delete = "{id}";
        public const string PostMoralValues = "moralValues/{subscriberId}/{kiwiChant}";
        private const string PathSeparator = "/";

        public List<RouteDefinition> GetRouteDefinitions()
        {
            return new List<RouteDefinition>
            {
                new PostRouteDefinition(typeof(string))
                {
                    RouteTemplate = $"{Prefix}{PathSeparator}{Post}",
                },
                new GetRouteDefinition
                {
                    RouteTemplate = $"{Prefix}{PathSeparator}{GetByIdGuid}"
                },
                new PostRouteDefinition(typeof(MoralValues))
                {
                    RouteTemplate = $"{Prefix}{PathSeparator}{PostMoralValues}",
                    ClaimsConfig = new RouteClaimsConfig()
                    {
                        ExtractionConfigs = new List<IValidClaimsExtractionConfig>()
                        {
                            new JsonPathClaimExtractionConfig("AltruisticAlignment")
                                .ConfigureExtraction(ExtractionFunctions.JsonPathFunc, "$.AltruisticAptitude.AltruisticAlignment").Build(),
                            new JsonPathClaimExtractionConfig(JwtRegisteredClaimNames.Email)
                                .ConfigureExtraction(ExtractionFunctions.JsonPathFunc, "$.ReportViolationEmail").Build(),
                            new KeyValueClaimExtractionConfig("hukaChaka1", ClaimLocation.QueryParameters).ConfigureExtraction(ExtractionFunctions.KeyValueFunc, "chandKara").Build(),
                            new KeyValueClaimExtractionConfig("hukaChaka2", ClaimLocation.Uri).ConfigureExtraction(ExtractionFunctions.KeyValueFunc, "kiwiChant").Build(),
                            new KeyValueClaimExtractionConfig("hukaChaka3", ClaimLocation.Headers).ConfigureExtraction(ExtractionFunctions.KeyValueFunc, "hukaChaka").Build(),
                            //make regex for the path only, not including the query parameters. For query params, use KeyValueClaimExtractionConfig instead
                            new RegexClaimExtractionConfig("hookaRegex", ClaimLocation.Uri).ConfigureExtraction(ExtractionFunctions.RegexFunc,
                                new System.Text.RegularExpressions.Regex("/values/moralvalues/ca413986-f096-11e7-8c3f-9a214cf093ae/(.*)")).Build()
                        },
                        ValidationConfigs = new List<ClaimValidationConfig>()
                        {
                            new ClaimValidationConfig()
                            {
                                ClaimName = "AltruisticAlignment",
                                IsRequired = true
                            },
                            new ClaimValidationConfig()
                            {
                                ClaimName = JwtRegisteredClaimNames.Email,
                                IsRequired = true,
                                AllowNullOrEmpty = false
                            }
                        },
                        BadRequestResponse = new BadRequestResponse
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.Forbidden,
                            Response = GetMissingClaimsResponse(),
                            BadRequestResponseOverride = (missingClaims, expectedClaims) =>
                            {
                                return Task.FromResult(GetSampleResponse());
                            }
                        }
                    }
                }
            };
        }

        private dynamic GetMissingClaimsResponse()
        {
            dynamic result = new ExpandoObject();
            result.ErrorCode = 2500;
            result.Message = "Somebody gonna get hurt real bad";
            return result;
        }

        private HttpResponse GetSampleResponse()
        {
            var response = new DefaultHttpResponse(new DefaultHttpContext())
            {
                Body = new MemoryStream()
            };
            var responseBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Yolo = "nolo" }));
            response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
            response.Headers.Add(new KeyValuePair<string, StringValues>("yolo", new StringValues("solo")));
            response.StatusCode = StatusCodes.Status502BadGateway;
            return response;
        }
    }
}
