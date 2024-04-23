// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.FeatureManagement.FeatureFilters;
using System.Security.Claims;

namespace FeatureFlagDemo
{
    /// <summary>
    /// Provides an implementation of <see cref="ITargetingContextAccessor"/> that creates a targeting context using info from the current HTTP request.
    /// </summary>
    public class HttpContextTargetingContextAccessor : ITargetingContextAccessor
    {
        private const string TargetingContextLookup = "HttpContextTargetingContextAccessor.TargetingContext";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<TargetingContext> GetContextAsync()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                return new ValueTask<TargetingContext>(new TargetingContext());
            }

            //
            // Try cache lookup
            if (httpContext.Items.TryGetValue(TargetingContextLookup, out object? value) && value != null)
            {
                TargetingContext? cachedContext = value as TargetingContext;

                if (cachedContext != null) {
                    return new ValueTask<TargetingContext>(cachedContext);
                }
            }

            ClaimsPrincipal user = httpContext.User;

            List<string> groups = new();

            //
            // This application expects groups to be specified in the user's claims
            foreach (Claim claim in user.Claims)
            {
                if (claim.Type == ClaimTypes.GroupName)
                {
                    groups.Add(claim.Value);
                }
            }

            //
            // Build targeting context based off user info
            TargetingContext targetingContext = new TargetingContext
            {
                UserId = user.Identity?.Name,
                Groups = groups
            };

            //
            // Cache for subsequent lookup
            httpContext.Items[TargetingContextLookup] = targetingContext;

            return new ValueTask<TargetingContext>(targetingContext);
        }
    }
}
