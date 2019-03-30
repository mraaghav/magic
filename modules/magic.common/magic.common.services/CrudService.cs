﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using NHibernate;
using magic.common.model;
using magic.common.contracts;

namespace magic.common.services
{
    public abstract class CrudService<DbModel> : ICrudService<DbModel> where DbModel : Model
    {
        readonly protected ISession Session;
        readonly protected ILog Logger;

        public CrudService(ISession session, ILog logger)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual Guid Create(DbModel model)
        {
            Session.Save(model);
            Session.Flush();

            Logger.Info($"Created {typeof(DbModel).Name} with id of '{model.Id}'");

            return model.Id;
        }

        public virtual void Delete(Guid id)
        {
            var query = Session.CreateQuery($"delete from {typeof(DbModel).Name} where Id = :id");
            query.SetParameter("id", id);
            var result = query.ExecuteUpdate();

            if (result != 1)
                throw new ArgumentNullException($"{typeof(DbModel).Name} with the id of '{id}' was not found");

            Logger.Info($"Deleted {typeof(DbModel)} with id of '{id}'");
        }

        public virtual DbModel Get(Guid id)
        {
            var model = Session.Load<DbModel>(id);

            if (model == null)
                throw new ArgumentOutOfRangeException($"{typeof(DbModel).Name} with the id of '{id}' was not found");

            return model;
        }

        public virtual IEnumerable<DbModel> List(int offset, int limit)
        {
            return Session.Query<DbModel>()
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public virtual void Update(DbModel model)
        {
            if (model.Id == Guid.Empty)
                throw new ArgumentException("Model doesn't exist in database, and hence cannot be updated");

            Session.Merge(model);
            Session.Flush();

            Logger.Info($"Updated {typeof(DbModel)} with id of '{model.Id}'");
        }

        public virtual long Count()
        {
            return Session.Query<DbModel>()
                .LongCount();
        }
    }
}