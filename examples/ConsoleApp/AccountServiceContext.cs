// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
class AccountServiceContext : IAccountContext
{
    public string AccountId { get; set; }

    public AccountServiceContext(string accountId)
    {
        AccountId = accountId;
    }
}
