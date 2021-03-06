﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACME.MicrosoftCA.Gateway.Exceptions ;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;

namespace ACME.MicrosoftCA.Gateway.Services
{
    public interface IReplayNonceRegistry
    {
        Task<Models.Data.ReplayNonce> NewNonceAsync();
        Task<bool> VerifyNonceAsync(string nonce_);
        Task SetNonceUsedAsync(string nonce_);

    }

    public class ReplayNocneRegistry : IReplayNonceRegistry
    {
        public Services.DataBase Db { get; private set; }

        public ReplayNocneRegistry(Services.DataBase db_)
        {
            Db = db_;
        }

        public async Task<Models.Data.ReplayNonce> NewNonceAsync()
        {
            var n = new Models.Data.ReplayNonce
            {
                Nonce = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()),
                IssedAt = DateTimeOffset.Now
            };
            await Db.IssuedNonces.AddAsync(n);

            await Db.SaveChangesAsync();

            return n;
        }

        public async Task SetNonceUsedAsync(string nonce_)
        {
            var l = await
                (from nonce in Db.IssuedNonces where nonce.Nonce == nonce_ select nonce).ToListAsync().ConfigureAwait(false);

            var n = l.FirstOrDefault();
            if (null == n)
            {
                throw new BadReplayNonceException();
            }

            Db.Entry(n).State = EntityState.Deleted;
            await Db.SaveChangesAsync();
        }

        public async Task<bool> VerifyNonceAsync(string nonce_)
        {
            if (null == nonce_)
            {
                return false;
            }
            var l = await (from
                                nonce
                           in
                               Db.IssuedNonces
                           where
                               nonce.Nonce == nonce_
                           select
                                nonce).ToListAsync();

            return l.Any();
        }
    }
}
